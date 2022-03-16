using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Models
{
    [Table("Pets")]
    public class Pet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Breed { get; set; }

        [OneToMany]
        public List<Feed> Feeds { get; set; }
    }
}
