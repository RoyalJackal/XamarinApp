using System;

namespace Shared.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public DateTimeOffset DateTime { get; set; }
        public string Text { get; set; }
    }
}