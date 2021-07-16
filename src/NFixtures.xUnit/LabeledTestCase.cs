using System;
using System.Globalization;
using NFixtures.xUnit.Infrastructure;
using Xunit.Abstractions;

namespace NFixtures.xUnit
{
    /// <summary>
    /// Labeled test case with typed parameters.
    /// </summary>
    /// <typeparam name="TData">Typed test case input.</typeparam>
    /// <typeparam name="TExpected">Typed test case expected result.</typeparam>
    public class LabeledTestCase<TData, TExpected> : LabeledTestCase<(TData Data, TExpected Expected)>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledTestCase{TData, TExpected}"/> class.
        /// </summary>
        /// <param name="data">Test case input.</param>
        /// <param name="expected">Test case expected result.</param>
        /// <param name="name">Test case name.</param>
        public LabeledTestCase(TData data, TExpected expected, string name = null)
            : base((data, expected), name)
        {
        }
    }

    /// <summary>
    /// Labeled test case with typed parameters.
    /// </summary>
    /// <typeparam name="TParameters">Typed test case parameters.</typeparam>
    public class LabeledTestCase<TParameters> : IXunitSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledTestCase{TParameters}"/> class.
        /// </summary>
        public LabeledTestCase()
        {
            Name = typeof(TParameters).Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledTestCase{TParameters}"/> class.
        /// </summary>
        /// <param name="parameters">Typed parameters.</param>
        /// <param name="name">The name of test case.</param>
        public LabeledTestCase(TParameters parameters, string name = null)
        {
            Parameters = parameters;

            Name = !string.IsNullOrWhiteSpace(name) ? name : parameters switch
            {
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                _ => Parameters?.ToString() ?? "<null>"
            };
        }

        /// <summary>
        /// Gets the name of test case.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the typed parameters.
        /// </summary>
        public TParameters Parameters { get; private set; }

        /// <summary>
        /// Sets the test case name for current instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A test case instance with updated name.</returns>
        public LabeledTestCase<TParameters> WithName(string name)
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

        public static implicit operator object[](LabeledTestCase<TParameters> src) => src?.FromNamedTestCase();

        /// <summary>
        /// Creates an array of objects from current instance.
        /// </summary>
        /// <returns>XUnit test case array of objects.</returns>
        private object[] FromNamedTestCase() => new object[] {this};
    }
}
