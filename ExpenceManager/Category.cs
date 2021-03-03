using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    public class Category
    {
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

        public Category(string name, string description, string icon)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
        }
    }
}
