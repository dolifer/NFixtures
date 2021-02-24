using System;
using System.Text;

namespace NFixtures.WebApi.Configuration
{
    public class BasicAuthOptions
    {
        public static readonly Encoding Encoding = Encoding.GetEncoding("iso-8859-1");

        public string Username { get; set; }

        public string Password { get; set; }

        public static string GetHeaderValue(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(password));
            }

            var credentials = $"{userName}:{password}";
            return Convert.ToBase64String(Encoding.GetBytes(credentials));
        }
    }
}
