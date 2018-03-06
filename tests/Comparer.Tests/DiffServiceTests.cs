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
        public void DiffLeftArgIsNullTest()
        {
            var diffService = new DiffService();

            Assert.Throws<ArgumentNullException>(() => diffService.SimpleDiff(null, "aaaa"));
        }

        [Test]
        public void DiffRightArgIsNullTest()
        {
            var diffService = new DiffService();

            Assert.Throws<ArgumentNullException>(() => diffService.SimpleDiff("xxx", null));
        }

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
                yield return new TestCaseData("aaa", "a")
                    .Returns(new DiffResult(DiffType.LeftGreaterThanRight));

                yield return new TestCaseData("a", "aaa")
                    .Returns(new DiffResult(DiffType.RightGreateThanLeft));

                yield return new TestCaseData(string.Empty, string.Empty)
                    .Returns(new DiffResult(DiffType.Equal));

                yield return new TestCaseData("aaa bbb ccc", "aaa bbb ccc")
                    .Returns(new DiffResult(DiffType.Equal));

                yield return new TestCaseData("aaa", "bbb")
                    .Returns(new DiffResult(DiffType.Diff, new[] { new Diff { Offset = 1, Length = 3 } }));

                yield return new TestCaseData("aaa", "baa")
                    .Returns(new DiffResult(DiffType.Diff, new[] { new Diff { Offset = 1, Length = 1 } }));

                yield return new TestCaseData("aaa bbb ccc", "aaa xxx ccc")
                    .Returns(new DiffResult(DiffType.Diff, new[] { new Diff { Offset = 5, Length = 3 } }));

                yield return new TestCaseData("aaa bbb cccq", "aaa xxx cccz")
                    .Returns(
                        new DiffResult(
                            DiffType.Diff, 
                            new[]
                            {
                                new Diff { Offset = 5, Length = 3 },
                                new Diff { Offset = 12, Length = 1 },
                            })
                        );
            }
        }
    }
}
