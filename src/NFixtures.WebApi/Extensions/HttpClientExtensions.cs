using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NFixtures.WebApi.Configuration;
using NFixtures.WebApi.Constants;
using NFixtures.WebApi.Helpers;

namespace NFixtures.WebApi.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithBasicAuth(this HttpClient client, BasicAuthOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return client.WithBasicAuth(options.Username, options.Password);
        }

        public static HttpClient WithBasicAuth(this HttpClient client, string userName, string password)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            }

            string basicAuthCredentials = BasicAuthOptions.GetHeaderValue(userName, password);

            return client.WithFormattedHeader(FormatStrings.Authorization.Basic, basicAuthCredentials);
        }

        public static HttpClient WithJwtBearer(this HttpClient client, SecurityTokenDescriptor securityTokenDescriptor)
        {
            var securityToken = JwtTokenHelper.SecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return client.WithJwtBearer(securityToken);
        }

        public static HttpClient WithJwtBearer(this HttpClient client, SecurityToken securityToken)
        {
            string jwtToken = JwtTokenHelper.SecurityTokenHandler.WriteToken(securityToken);

            return client.WithJwtBearer(jwtToken);
        }

        public static HttpClient WithJwtBearer(this HttpClient client, string bearerToken)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new ArgumentNullException(nameof(bearerToken));
            }

            return client.WithFormattedHeader(FormatStrings.Authorization.Bearer, bearerToken);
        }

        public static HttpClient WithHeader(this HttpClient client, string name, string value)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            client.DefaultRequestHeaders.Add(name, value);

            return client;
        }

        private static HttpClient WithFormattedHeader(this HttpClient client, string formatString, string value)
            => client.WithHeader(HeaderNames.Authorization, string.Format(CultureInfo.InvariantCulture, formatString, value));
    }
}