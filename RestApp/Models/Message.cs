using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string MessageText { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string Attachment { get; set; }

        public int ConversationId { get; set; }

        public Conversation Conversation { get; set; }

        public bool IsReaded { get; set; }
    }
}