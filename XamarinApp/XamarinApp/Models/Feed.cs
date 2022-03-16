using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace XamarinApp.Models
{
    [Table("Feeds")]
    public class Feed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ManyToOne]
        public Fodder Fodder { get; set; }

        public int Amount { get; set; }

        public TimeSpan Time { get; set; }

        [ForeignKey(typeof(Pet))]
        public int PetId { get; set; }
    }
}
