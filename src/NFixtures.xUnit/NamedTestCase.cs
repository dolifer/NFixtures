using System;
using NFixtures.xUnit.Infrastructure;
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
            Name = typeof(TParameters).Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTestCase{TParameters}"/> class.
        /// </summary>
        /// <param name="parameters">Typed parameters.</param>
        /// <param name="name">The name of test case.</param>
        public NamedTestCase(TParameters parameters, string name = null)
        {
            Parameters = parameters;
            Name = string.IsNullOrWhiteSpace(name)
                ? Parameters?.ToString() ?? "null"
                : name;
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

            Parameters = ValueSerializer.FromJson<TParameters>(info.GetValue<string>(nameof(Parameters)));
            Name = ValueSerializer.FromJson<string>(info.GetValue<string>(nameof(Name)));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Parameters), ValueSerializer.ToJson(Parameters));
            info.AddValue(nameof(Name), ValueSerializer.ToJson(Name));
        }

        /// <inheritdoc/>
        public override string ToString() => Name;

        public static implicit operator object[](NamedTestCase<TParameters> src) => src?.FromNamedTestCase();

        /// <summary>
        /// Creates an array of objects from current instance.
        /// </summary>
        /// <returns>XUnit test case array of objects.</returns>
        public object[] FromNamedTestCase() => new object[] {this};
    }
}
