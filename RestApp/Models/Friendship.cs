using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestApp.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public int UserRequesterId { get; set; }

        public User UserRequester { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}