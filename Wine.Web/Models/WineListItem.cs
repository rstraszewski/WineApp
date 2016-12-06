namespace Wine.Web.Models
{
    public class WineListItem
    {
        public int Id { get; set; }

        public string Region { get; set; }

        public string ThumbnailBase64 { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Varietal { get; set; }

        public string Description { get; set; }
    }
}