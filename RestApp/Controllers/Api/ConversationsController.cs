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
    public class ConversationsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Conversations
        public IEnumerable<ConversationDto> GetConversations()
        {
            return db.Conversations.ToList().Select(Mapper.Map<Conversation, ConversationDto>);
        }

        // GET: api/Conversations/5
        [ResponseType(typeof(ConversationDto))]
        public IHttpActionResult GetConversation(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Conversation, ConversationDto>(conversation));
        }

        // POST: api/Conversations
        [ResponseType(typeof(ConversationDto))]
        public IHttpActionResult PostConversation(ConversationDto conversationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Conversation conversation = Mapper.Map<ConversationDto, Conversation>(conversationDto);
            db.Conversations.Add(conversation);
            db.SaveChanges();

            Mapper.Map(conversation, conversationDto);

            return CreatedAtRoute("DefaultApi", new { id = conversationDto.Id }, conversationDto);
        }

        // DELETE: api/Conversations/5
        [ResponseType(typeof(ConversationDto))]
        public IHttpActionResult DeleteConversation(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return NotFound();
            }

            db.Conversations.Remove(conversation);
            db.SaveChanges();

            return Ok(Mapper.Map<Conversation, ConversationDto>(conversation));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConversationExists(int id)
        {
            return db.Conversations.Count(e => e.Id == id) > 0;
        }
    }
}