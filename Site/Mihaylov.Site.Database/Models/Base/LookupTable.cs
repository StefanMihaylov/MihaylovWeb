namespace Mihaylov.Site.Database.Models.Base
{
    public abstract class LookupTable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
