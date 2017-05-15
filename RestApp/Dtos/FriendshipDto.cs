using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestApp.Dtos
{
    public class FriendshipDto
    {
        public int Id { get; set; }

        public int UserRequesterId { get; set; }

        public int UserId { get; set; }
    }
}