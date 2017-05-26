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
    public class UserConversationsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserConversations
        public IEnumerable<UserConversationDto> GetUserConversations()
        {
            return db.UserConversations.ToList().Select(Mapper.Map<UserConversation, UserConversationDto>);
        }

        // GET: api/UserConversations?userid=3
        public IEnumerable<UserConversationDto> GetUserConversations(int userId)
        {
            return db.UserConversations.Where(c => c.UserId == userId).Select(Mapper.Map<UserConversation, UserConversationDto>);
        }

        // GET: api/UserConversations/5
        [ResponseType(typeof(UserConversationDto))]
        public IHttpActionResult GetUserConversation(int id)
        {
            UserConversation userConversation = db.UserConversations.Find(id);
            if (userConversation == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<UserConversation, UserConversationDto>(userConversation));
        }

        // PUT: api/UserConversations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserConversation(int id, UserConversationDto userConversationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserConversation userConversation = Mapper.Map<UserConversationDto, UserConversation>(userConversationDto);

            if (id != userConversationDto.Id)
            {
                return BadRequest();
            }

            db.Entry(userConversation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserConversationExists(id))
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

        // POST: api/UserConversations
        [ResponseType(typeof(UserConversationDto))]
        public IHttpActionResult PostUserConversation(UserConversationDto userConversationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserConversation userConversation = Mapper.Map<UserConversationDto, UserConversation>(userConversationDto);
            db.UserConversations.Add(userConversation);
            db.SaveChanges();

            Mapper.Map(userConversation, userConversationDto);

            return CreatedAtRoute("DefaultApi", new { id = userConversationDto.Id }, userConversationDto);
        }

        // DELETE: api/UserConversations/5
        [ResponseType(typeof(UserConversationDto))]
        public IHttpActionResult DeleteUserConversation(int id)
        {
            UserConversation userConversation = db.UserConversations.Find(id);
            if (userConversation == null)
            {
                return NotFound();
            }

            db.UserConversations.Remove(userConversation);
            db.SaveChanges();

            return Ok(Mapper.Map<UserConversation, UserConversationDto>(userConversation));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserConversationExists(int id)
        {
            return db.UserConversations.Count(e => e.Id == id) > 0;
        }
    }
}