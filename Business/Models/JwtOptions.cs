using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Business.Models
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int Expire { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => 
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}
