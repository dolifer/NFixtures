using System;
using System.Collections.Generic;
using Xunit;

namespace NFixtures.xUnit.Tests
{
    public class NamedTestCaseTests
    {
        [Fact]
        public void Operator_Test()
        {
            // arrange
            var testCase = new NamedTestCase<int>(42);

            // act
            object[] values = testCase;

            // assert
            Assert.NotNull(values);

            Assert.Equal("42", values[0].ToString());
        }

        [Fact]
        public void Serialization_Validation()
        {
            // arrange
            var testCase = new NamedTestCase<object>();

            // act & assert
            Assert.Throws<ArgumentNullException>("info", () => testCase.Serialize(null));
            Assert.Throws<ArgumentNullException>("info", () => testCase.Deserialize(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void WithName_Throws_ArgumentException(string name)
        {
            // arrange
            var testCase = new NamedTestCase<object>();

            // act && assert
            Assert.Throws<ArgumentException>("name", () => testCase.WithName(name));
        }

        [Fact]
        public void Default_Naming_Convention_NoValue()
        {
            // arrange
            var testCase = new NamedTestCase<object>();

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("Object", testCase.Name);
            Assert.Equal("Object", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Int()
        {
            // arrange
            var testCase = new NamedTestCase<int>(1);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("1", testCase.Name);
            Assert.Equal("1", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Double()
        {
            // arrange
            var testCase = new NamedTestCase<double>(1.2d);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("1.2", testCase.Name);
            Assert.Equal("1.2", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Decimal()
        {
            // arrange
            var testCase = new NamedTestCase<decimal>(1.2m);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("1.2", testCase.Name);
            Assert.Equal("1.2", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Long()
        {
            // arrange
            var testCase = new NamedTestCase<long>(123L);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("123", testCase.Name);
            Assert.Equal("123", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Value()
        {
            // arrange
            var testCase = new NamedTestCase<object>(null, "name");

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("name", testCase.Name);
            Assert.Equal("name", testCase.ToString());
        }

        [Fact]
        public void WithName_Sets_Value()
        {
            // arrange
            NamedTestCase<object> testCase = new NamedTestCase<object>().WithName("name");

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("name", testCase.Name);
            Assert.Equal("name", testCase.ToString());
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData("abc", "abc")]
        [InlineData(null, "null")]
        public void Default_Naming_Value_Conventions(object value, string expectedName)
        {
            // arrange
            var testCase = new NamedTestCase<object>(value);

            // assert
            Assert.Equal(expectedName, testCase.Name);
            Assert.Equal(expectedName, testCase.ToString());

            Assert.Equal(value, testCase.Parameters);
        }

        [Theory]
        [MemberData(nameof(SingleValueTestCases))]
        public void Single_Value_Test_Cases(NamedTestCase<int> t)
        {
            Assert.NotNull(t);
            Assert.NotEmpty($"{t.Parameters}");
        }

        public static IEnumerable<object[]> SingleValueTestCases()
        {
            static NamedTestCase<int> GetTestCase(int value, string name)
            {
                return new NamedTestCase<int>(value)
                    .WithName(name);
            }

            yield return GetTestCase(1, "One");
            yield return GetTestCase(2, "Two");
            yield return GetTestCase(3, "Three");
        }
    }
}
