using System.Collections.Generic;
using Xunit;

namespace NFixtures.xUnit.Tests
{
    public class NamedTestCaseTests
    {
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
