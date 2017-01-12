using System;

namespace Wine.Core.Entities
{
    public class Review
    {
        private Review()
        {
        }

        public Review(string body, User user)
        {
            UserName = user.Username;
            Body = body;
            UserId = user.Id;
            Created = DateTime.Now;
        }

        public int WineId { get; set; }

        public int Id { get; set; }

        public string Body { get; set; }

        public string UserName { get; set; }

        public DateTime Created { get; set; }

        public int UserId { get; set; }
    }
}