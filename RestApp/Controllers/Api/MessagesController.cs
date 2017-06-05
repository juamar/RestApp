using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestApp.Models;
using RestApp.Dtos;
using AutoMapper;
using RestApp.App_Start;
using System.IO;
using System.Web.Script.Serialization;

namespace RestApp.Controllers.Api
{
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Messages
        public IEnumerable<MessageDto> GetMessages()
        {
            return db.Messages.ToList().Select(Mapper.Map<Message, MessageDto>);
        }

        // GET: api/UserConversations?userid=3
        public IEnumerable<MessageDto> GetMessagesByConversation(int conversationId)
        {
            return db.Messages.Where(c => c.ConversationId == conversationId).Select(Mapper.Map<Message, MessageDto>);
        }

        // GET: api/Messages/5
        [ResponseType(typeof(MessageDto))]
        public IHttpActionResult GetMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Message, MessageDto>(message));
        }

        // PUT: api/Messages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMessage(int id, MessageDto messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Message message = Mapper.Map<MessageDto, Message>(messageDto);

            if (id != messageDto.Id)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Messages
        [ResponseType(typeof(MessageDto))]
        public IHttpActionResult PostMessage(MessageDto messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Message message = Mapper.Map<MessageDto, Message>(messageDto);
            db.Messages.Add(message);
            db.SaveChanges();

            //obtener el otro user de la conversacion
            User friend = db.Users.Single(u => u.Id == db.UserConversations.FirstOrDefault(uc => uc.ConversationId == message.ConversationId && uc.UserId != message.UserId).UserId);

            if (friend.Token != null)
            {
                SendNotification(message, friend);
            }
            

            Mapper.Map(message, messageDto);

            return CreatedAtRoute("DefaultApi", new { id = messageDto.Id }, messageDto);
        }

        // DELETE: api/Messages/5
        [ResponseType(typeof(MessageDto))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            db.SaveChanges();

            return Ok(Mapper.Map<Message, MessageDto>(message));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.Id == id) > 0;
        }

        private void SendNotification(Message message, User friend)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://gcm-http.googleapis.com/gcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "key=AIzaSyCNt2yq3fkpi7PskbWDgoAAUaVF3UQb7hI");

            User user = db.Users.Single(u => u.Id == message.UserId);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    to = friend.Token,
                    notification = new{
                        title = user.Name + " " + user.LastName,
                        body = message.MessageText,
                        icon = "go",
                        sound = "default"
                    }
                });
    
                streamWriter.Write(json);
            }

            /*var httpResponse = (HttpWebResponse)*/httpWebRequest.GetResponse();
            /*using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }*/
        }
    }
}