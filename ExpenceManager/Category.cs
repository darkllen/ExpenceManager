using System;

namespace ExpenceManager
{
    public class Category
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //path to icon
        public string Icon { get; set; }

        public Category(string name, string description, string icon)
        {
            Name = name;
            Description = description;
            Icon = icon;
        }

        public Category()
        {
        }

        protected bool Equals(Category other)
        {
            return Name == other.Name && Description == other.Description && Icon == other.Icon;
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
            return HashCode.Combine(Name, Description, Icon);
        }
    }
}
