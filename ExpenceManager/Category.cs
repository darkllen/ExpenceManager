using System;

namespace ExpenceManagerModels
{

    public class Category
    {
        public static Category DefaultCategory = new("", ""); 
        public string Name { get; set; }
        public string Description { get; set; }

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Category()
        {
        }

        protected bool Equals(Category other)
        {
            return Name == other.Name && Description == other.Description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Category) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description);
        }
    }
}
