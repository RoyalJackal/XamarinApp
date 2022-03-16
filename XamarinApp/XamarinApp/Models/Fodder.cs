using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Models
{
    [Table("Fodders")]
    public class Fodder
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [OneToMany]
        public List<Feed> Feeds { get; set; }
    }
}
