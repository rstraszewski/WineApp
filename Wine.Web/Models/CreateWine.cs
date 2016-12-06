using System.Web;
using Wine.Core.Entities;

namespace Wine.Web.Models
{
    public class CreateWine
    {
        public string Region { get; set; }

        public HttpPostedFileBase Thumbnail { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public Varietal Varietal { get; set; }

        public string Description { get; set; }

        public int Vintage { get; set; }
    }
}