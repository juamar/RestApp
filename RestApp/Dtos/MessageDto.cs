using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RestApp.Models;

namespace RestApp.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }

        [Required]
        public string MessageText { get; set; }

        public int UserId { get; set; }

        public string AttachmentName { get; set; }

        public byte[] AttachmentBynary { get; set; }

        public int ConversationId { get; set; }

        public bool IsReaded { get; set; }
    }
}