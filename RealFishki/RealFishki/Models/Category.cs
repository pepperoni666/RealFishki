using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace RealFishki.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Subject { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Item> CatItems { get; set; }

        public Category()
        {
            CatItems = new List<Item>();
            Subject = " ";
        }
    }
}
