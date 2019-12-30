using System;
using System.Text.Json;
using Xunit.Abstractions;

namespace NFixtures.xUnit
{
    /// <summary>
    /// Named test case with typed parameters.
    /// </summary>
    /// <typeparam name="TParameters">Typed test case parameters.</typeparam>
    public class NamedTestCase<TParameters> : IXunitSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTestCase{TParameters}"/> class.
        /// </summary>
        public NamedTestCase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTestCase{TParameters}"/> class.
        /// </summary>
        /// <param name="parameters">Typed parameters.</param>
        /// <param name="name">The name of test case.</param>
        public NamedTestCase(TParameters parameters, string name = null)
            : this()
        {
            Parameters = parameters;
            Name = name;
        }

        /// <summary>
        /// Gets the name of test case.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets he typed parameters.
        /// </summary>
        public TParameters Parameters { get; private set; }

        /// <summary>
        /// Sets the test case name for current instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A test case instance with updated name.</returns>
        public NamedTestCase<TParameters> WithName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            Name = name;
            return this;
        }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Parameters = JsonSerializer.Deserialize<TParameters>(info.GetValue<string>(nameof(Parameters)));
            Name = JsonSerializer.Deserialize<string>(info.GetValue<string>(nameof(Name)));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Parameters), JsonSerializer.Serialize(Parameters));
            info.AddValue(nameof(Name), JsonSerializer.Serialize(Name));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Parameters?.ToString() ?? "null";
            }

            return Name;
        }

        public static implicit operator object[](NamedTestCase<TParameters> src) => src?.FromNamedTestCase();

        /// <summary>
        /// Creates an array of objects from current instance.
        /// </summary>
        /// <returns>XUnit test case array of objects.</returns>
        public object[] FromNamedTestCase() => new object[] {this};
    }
}
