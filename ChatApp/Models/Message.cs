using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Models
{
    public class Message
    {
        public Message()
        {
            Id = Guid.NewGuid().ToString();
            TimeStamp = DateTime.Now;
            Content = string.Empty;
        }

        [MaxLength(36)]
        public string Id { get; private set; }

        public DateTime TimeStamp { get; set; }

        [ForeignKey(nameof(Owner))]
        public string? OwnerId { get; set; }
        public IdentityUser Owner { get; set; }

        [MaxLength(256)]
        public string Content { get; set; }
    }
}

