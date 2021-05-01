using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Service.SharedModel.Helpers
{
    public class AuthOptions
    {
        public const string ISSUER = "MobileServer";
        public const string AUDIENCE = "9DD28AA7-96B6-46A3-9E22-E7A1CB84350D";
        const string KEY = "861255E6-05E0-45B4-94F6-7E8DC330BADF";
        public const int TokenLIFETIME = 60 * 24;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
