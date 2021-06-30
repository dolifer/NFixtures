using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NFixtures.WebApi.Configuration;
using NFixtures.WebApi.Helpers;

namespace NFixtures.WebApi.Extensions
{
    /// <summary>
    /// Set of extensions that allows to configure a <see cref="HttpClient"/> to use Basic or JWT authentication.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Configures http client to use Basic auth.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="options">The basic auth options.</param>
        /// <returns>Configured http client with a Basic authentication header.</returns>
        /// <exception cref="ArgumentNullException">client or options is null.</exception>
        public static HttpClient WithBasicAuth(this HttpClient client, BasicAuthOptions options)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return client.WithBasicAuth(options.Username, options.Password);
        }

        /// <summary>
        /// Configures http client to use Basic auth.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="username">Basic auth username.</param>
        /// <param name="password">Basic auth password.</param>
        /// <returns>Configured http client with a Basic authentication header.</returns>
        /// <exception cref="ArgumentNullException">client or options is null.</exception>
        /// <exception cref="ArgumentException">username or password token is null or whitespace.</exception>
        public static HttpClient WithBasicAuth(this HttpClient client, string username, string password)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(password));
            }

            var basicAuthCredentials = BasicAuthOptions.GetHeaderValue(username, password);

            return client.WithFormattedHeader(FormatStrings.Authorization_Basic, basicAuthCredentials);
        }

        /// <summary>
        /// Configures http client to use JWT auth.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="securityTokenDescriptor">The JWT token descriptor.</param>
        /// <returns>Configured http client with a Basic authentication header.</returns>
        /// <exception cref="ArgumentNullException">client or securityTokenDescriptor is null.</exception>
        public static HttpClient WithJwtBearer(this HttpClient client, SecurityTokenDescriptor securityTokenDescriptor)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (securityTokenDescriptor is null)
            {
                throw new ArgumentNullException(nameof(securityTokenDescriptor));
            }

            var securityToken = JwtTokenHelper.GetToken(securityTokenDescriptor);

            return client.WithJwtBearer(securityToken);
        }

        /// <summary>
        /// Configures http client to use JWT auth.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="identity">The test identity.</param>
        /// <returns>Configured http client with a Basic authentication header.</returns>
        /// <exception cref="ArgumentNullException">client or securityToken is null.</exception>
        public static HttpClient WithJwtBearer(this HttpClient client, ClaimsIdentity identity)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (identity is null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var jwtToken = JwtTokenHelper.GetToken(identity);

            return client.WithJwtBearer(jwtToken);
        }

        /// <summary>
        /// Configures http client to use JWT auth.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="token">The JWT token.</param>
        /// <returns>Configured http client with a Basic authentication header.</returns>
        /// <exception cref="ArgumentNullException">client is null.</exception>
        /// <exception cref="ArgumentException">token is null or whitespace.</exception>
        public static HttpClient WithJwtBearer(this HttpClient client, string token)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(token));
            }

            return client.WithFormattedHeader(FormatStrings.Authorization_Bearer, token);
        }

        /// <summary>
        /// Configures http client to use a given header.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        /// <returns>Configured http client with a Basic authentication header.</returns>
        /// <exception cref="ArgumentNullException">client is null.</exception>
        /// <exception cref="ArgumentException">name or value is null or whitespace.</exception>
        public static HttpClient WithHeader(this HttpClient client, string name, string value)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(name));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(value));
            }

            client.DefaultRequestHeaders.Add(name, value);

            return client;
        }

        private static HttpClient WithFormattedHeader(this HttpClient client, string formatString, string value)
            => client.WithHeader(HeaderNames.Authorization, string.Format(CultureInfo.InvariantCulture, formatString, value));
    }
}
