using System;

namespace NFixtures.Primitives
{
    /// <summary>
    /// Represents the test user.
    /// </summary>
    public class TestUser
    {
        /// <summary>
        /// Gets the name of test user identifier (Guid) claim name.
        /// </summary>
        public const string IdClaim = nameof(IdClaim);

        /// <summary>
        /// Gets the user name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the user email
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the user identifier
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUser"/> class.
        /// </summary>
        /// <param name="name">user name.</param>
        /// <param name="email">user email.</param>
        public TestUser(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(name))
                : name;

            Email = string.IsNullOrWhiteSpace(email)
                ? throw new ArgumentException(FormatStrings.ValueCanNotBeNull, nameof(email))
                : email;
        }
    }
}
