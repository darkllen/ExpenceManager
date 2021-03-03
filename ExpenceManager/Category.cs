using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    public class Category
    {
        private string name { get; set; }

        private string description { get; set; }

        private string icon { get; set; }

        public Category(string name, string description, string icon)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
        }
    }
}
