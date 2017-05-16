using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestApp.Dtos
{
    public class UserConversationDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConversationId { get; set; }
    }
}