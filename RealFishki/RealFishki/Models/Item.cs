using SQLite;
using System;

namespace RealFishki.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("[Item: ID={0}, Text={1}, Description={2}]", Id, Text, Description);
        }

        public Item()
        {
            Text = " ";
            Description = " ";
        }
    }
}