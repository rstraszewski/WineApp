using System.Collections.Generic;

namespace Wine.Core.Entities
{
    public class User
    {
        private User()
        {
        }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Credentials = new Credentials(password);
            Roles = new List<Role>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int CredentialsId { get; set; }

        public virtual Credentials Credentials { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
