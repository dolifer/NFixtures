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
            var testCase = new LabeledTestCase<int>(42);

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
            var testCase = new LabeledTestCase<object>();

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
            var testCase = new LabeledTestCase<object>();

            // act && assert
            Assert.Throws<ArgumentException>("name", () => testCase.WithName(name));
        }

        [Fact]
        public void Default_Naming_Convention_NoValue()
        {
            // arrange
            var testCase = new LabeledTestCase<object>();

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("Object", testCase.Name);
            Assert.Equal("Object", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Int()
        {
            // arrange
            var testCase = new LabeledTestCase<int>(1);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("1", testCase.Name);
            Assert.Equal("1", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Double()
        {
            // arrange
            var testCase = new LabeledTestCase<double>(1.2d);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("1.2", testCase.Name);
            Assert.Equal("1.2", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Decimal()
        {
            // arrange
            var testCase = new LabeledTestCase<decimal>(1.2m);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("1.2", testCase.Name);
            Assert.Equal("1.2", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Long()
        {
            // arrange
            var testCase = new LabeledTestCase<long>(123L);

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("123", testCase.Name);
            Assert.Equal("123", testCase.ToString());
        }

        [Fact]
        public void Default_Naming_Convention_Value()
        {
            // arrange
            var testCase = new LabeledTestCase<object>(null, "name");

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("name", testCase.Name);
            Assert.Equal("name", testCase.ToString());
        }

        [Fact]
        public void WithName_Sets_Value()
        {
            // arrange
            var testCase = new LabeledTestCase<object>().WithName("name");

            // assert
            Assert.NotNull(testCase.Name);

            Assert.Equal("name", testCase.Name);
            Assert.Equal("name", testCase.ToString());
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData("abc", "abc")]
        [InlineData(null, "<null>")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void Default_Naming_Value_Conventions(object value, string expectedName)
        {
            // arrange
            var testCase = new LabeledTestCase<object>(value);

            // assert
            Assert.Equal(expectedName, testCase.Name);
            Assert.Equal(expectedName, testCase.ToString());

            Assert.Equal(value, testCase.Parameters);
        }

        [Theory]
        [MemberData(nameof(SingleValueTestCases))]
        public void Single_Value_Test_Cases(LabeledTestCase<int> t)
        {
            Assert.NotNull(t);
            Assert.NotEmpty($"{t.Parameters}");
        }

        private class TestFormattable : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
                => "TestFormattable";
        }

        [Fact]
        public void TestCase_Uses_IFormattableAsString()
        {
            // arrange
            var testCase = new LabeledTestCase<object>(new TestFormattable());

            // act
            var result = testCase.ToString();

            // assert
            Assert.Equal("TestFormattable", result);
        }

        public static IEnumerable<object[]> SingleValueTestCases()
        {
            static LabeledTestCase<int> GetTestCase(int value, string name)
            {
                return new LabeledTestCase<int>(value)
                    .WithName(name);
            }

            yield return GetTestCase(1, "One");
            yield return GetTestCase(2, "Two");
            yield return GetTestCase(3, "Three");
        }
    }
}
