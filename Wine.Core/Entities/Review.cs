using System;

namespace Wine.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }

        private Review()
        {
        }

        public Review(string body, User user)
        {
            Body = body;
            User = user;
            Created = DateTime.Now;
        }

        public int WineId { get; set; }

        public virtual User User { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }

        public int UserId { get; set; }
    }
}