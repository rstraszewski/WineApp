using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Wine.Core.Entities
{
    public class Credentials
    {
        private Credentials()
        {
            
        }

        public Credentials(string password)
        {
            Salt = GenerateSalt();
            PasswordHash = GenerateHash(password);
        }

        public int Id { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        private static string GenerateSalt()
        {
            var salt = new byte[16];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return string.Join("", salt.Select(x => x.ToString("X2")));
        }

        public bool ValidatePassword(string password)
        {
            var hash = GenerateHash(password);

            return hash == PasswordHash;
        }

        private string GenerateHash(string password)
        {
            var hash = string.Join("", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password + Salt)).Select(x => x.ToString("X2")));
            return hash;
        }
    }
}