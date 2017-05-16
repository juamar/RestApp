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
    }
}