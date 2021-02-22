using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOVAPI.Models
{
    public static class Security
    {
        public static string GetHash(string str)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
            return BCrypt.Net.BCrypt.HashPassword(str, salt);
        }
        public static bool VerifyHash(string str, string str2)
        {
            return BCrypt.Net.BCrypt.Verify(str, str2);
        }
    }
}
