using System;
using System.Text;

namespace NFixtures.WebApi.Configuration
{
    /// <summary>
    /// Represents Basic authentication options.
    /// </summary>
    public class BasicAuthOptions
    {
        /// <summary>
        /// Gets the default encoding for Basic authentication header value.
        /// </summary>
        public static readonly Encoding Encoding = Encoding.GetEncoding("iso-8859-1");

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Build a Basic auth header value.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Basic authentication header string.</returns>
        /// <exception cref="ArgumentException">username or password is null or whitespace string.</exception>
        public static string GetHeaderValue(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(password));
            }

            var credentials = $"{username}:{password}";
            return Convert.ToBase64String(Encoding.GetBytes(credentials));
        }
    }
}
