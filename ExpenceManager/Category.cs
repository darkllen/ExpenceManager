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
    }
}
