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
using RestApp.Dtos;
using RestApp.Models;
using AutoMapper;

namespace RestApp.Controllers.Api
{
    public class FriendshipsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Friendships
        public IEnumerable<FriendshipDto> GetFriendshipDtoes()
        {
            return db.Friendships.ToList().Select(Mapper.Map<Friendship, FriendshipDto>);
        }

        // GET: api/Friendships/5
        [ResponseType(typeof(FriendshipDto))]
        public IHttpActionResult GetFriendshipDto(int id)
        {
            Friendship friendship = db.Friendships.SingleOrDefault(c => c.Id == id);
            if (friendship == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Friendship, FriendshipDto>(friendship));
        }

        // PUT: api/Friendships/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFriendshipDto(int id, FriendshipDto friendshipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Friendship friendship = Mapper.Map<FriendshipDto, Friendship>(friendshipDto);

            if (id != friendshipDto.Id)
            {
                return BadRequest();
            }

            db.Entry(friendship).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendshipDtoExists(id))
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

        // POST: api/Friendships
        [ResponseType(typeof(FriendshipDto))]
        public IHttpActionResult PostFriendshipDto(FriendshipDto friendshipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Friendship friendship = Mapper.Map<FriendshipDto, Friendship>(friendshipDto);
            db.Friendships.Add(friendship);
            db.SaveChanges();

            Mapper.Map(friendship, friendshipDto);

            return CreatedAtRoute("DefaultApi", new { id = friendshipDto.Id }, friendshipDto);
        }

        // DELETE: api/Friendships/5
        [ResponseType(typeof(FriendshipDto))]
        public IHttpActionResult DeleteFriendshipDto(int id)
        {
            Friendship friendship = db.Friendships.Find(id);
            if (friendship == null)
            {
                return NotFound();
            }

            db.Friendships.Remove(friendship);
            db.SaveChanges();

            return Ok(Mapper.Map<Friendship, FriendshipDto>(friendship));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FriendshipDtoExists(int id)
        {
            return db.Friendships.Count(e => e.Id == id) > 0;
        }
    }
}