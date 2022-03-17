using System;

namespace Data.Models
{
    public class Feed
    {
        public int Id { get; set; }

        public Fodder Fodder { get; set; }

        public int Amount { get; set; }

        public TimeSpan Time { get; set; }

        public Pet Pet { get; set; }
    }
}
