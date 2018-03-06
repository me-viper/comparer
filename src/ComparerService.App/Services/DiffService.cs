using System;
using System.Collections;
using System.Collections.Generic;

using ComparerService.App.Models;

namespace ComparerService.App.Services
{
    internal class DiffService : IDiffService
    {
        public DiffResult SimpleDiff(string left, string right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.Length > right.Length)
                return new DiffResult(DiffType.LeftGreaterThanRight);
            
            if (right.Length > left.Length)
                return new DiffResult(DiffType.RightGreateThanLeft);
            
            // Equal.
            var diffs = CalculateSimpleDiff(left, right);
            var outcome = diffs.Count == 0 ? DiffType.Equal : DiffType.Diff;

            return new DiffResult(outcome, diffs);
        }

        private static IReadOnlyCollection<Diff> CalculateSimpleDiff(string left, string right)
        {
            var result = new List<Diff>();

            Diff currentDiff = null;

            for (var i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    if (currentDiff == null)
                        currentDiff = new Diff {Offset = i + 1};

                    continue;
                }

                if (currentDiff != null)
                {
                    currentDiff.Length = i - currentDiff.Offset + 1;
                    result.Add(currentDiff);
                    currentDiff = null;
                }
            }

            // Handling case when strings differ at the end.
            if (currentDiff != null)
            {
                currentDiff.Length = left.Length - currentDiff.Offset + 1;
                result.Add(currentDiff);
            }

            return result;
        }
    }
}