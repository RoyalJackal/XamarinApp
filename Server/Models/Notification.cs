using System;
using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public IdentityUser User { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Text { get; set; }
        public bool IsSend { get; set; }
    }
}