using System;
using System.Collections.Generic;

using ComparerService.App.Models;
using ComparerService.App.Services;

using NUnit.Framework;

namespace Comparer.Tests
{
    [TestFixture]
    public class DiffServiceTests
    {
        [Test]
        [TestCaseSource(typeof(DiffServiceTests), nameof(DiffTestCases))]
        public DiffResult DiffTest(string left, string right)
        {
            var diffService = new DiffService();
            return diffService.SimpleDiff(left, right);
        }

        public static IEnumerable<TestCaseData> DiffTestCases
        {
            get
            {
                yield return new TestCaseData(null, null)
                    .Returns(DiffResult.Equal());

                yield return new TestCaseData("aaa", null)
                    .Returns(DiffResult.SizeDoesNotMatch());

                yield return new TestCaseData(null, "aaa")
                    .Returns(DiffResult.SizeDoesNotMatch());

                yield return new TestCaseData("aaa", "a")
                    .Returns(DiffResult.SizeDoesNotMatch());

                yield return new TestCaseData("a", "aaa")
                    .Returns(DiffResult.SizeDoesNotMatch());

                yield return new TestCaseData(string.Empty, string.Empty)
                    .Returns(DiffResult.Equal());

                yield return new TestCaseData("aaa bbb ccc", "aaa bbb ccc")
                    .Returns(DiffResult.Equal());

                yield return new TestCaseData("aaa", "bbb")
                    .Returns(DiffResult.Diff(new[] { new DiffSpan { Offset = 1, Length = 3 } }));

                yield return new TestCaseData("aaa", "baa")
                    .Returns(DiffResult.Diff(new[] { new DiffSpan { Offset = 1, Length = 1 } }));

                yield return new TestCaseData("aaa bbb ccc", "aaa xxx ccc")
                    .Returns(DiffResult.Diff(new[] { new DiffSpan { Offset = 5, Length = 3 } }));

                yield return new TestCaseData("aaa bbb cccq", "aaa xxx cccz")
                    .Returns(
                        DiffResult.Diff(
                            new[]
                            {
                                new DiffSpan { Offset = 5, Length = 3 },
                                new DiffSpan { Offset = 12, Length = 1 },
                            })
                        );
            }
        }
    }
}
