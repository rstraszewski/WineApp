using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wine.Core.Entities
{
    public class Wine
    {
        public Wine()
        {
            Reviews = new List<Review>();
        }

        public int Id { get; set; }

        public string Region { get; set; }

        public byte[] Thumbnail { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public Varietal Varietal { get; set; }

        public string Description { get; set; }

        public int Vintage { get; set; }

        public string Search { get; private set; }

        public virtual IList<Review> Reviews { get; private set; }

        public void AddReview(string body, User user)
        {
            Reviews.Add(new Review(body, user));
        }
    }
}
