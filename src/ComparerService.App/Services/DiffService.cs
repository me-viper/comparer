﻿using System;
using System.Collections;
using System.Collections.Generic;

using ComparerService.App.Models;

namespace ComparerService.App.Services
{
    internal class DiffService : IDiffService
    {
        public DiffResult SimpleDiff(string left, string right)
        {
            // Considering if both strings are null they are equal.
            if (left == null && right == null)
                return DiffResult.Equal();

            if (left == null || right == null)
                return DiffResult.SizeDoesNotMatch();

            if (left.Length != right.Length)
                return DiffResult.SizeDoesNotMatch();
            
            // Left and right have equal size. Calculating diff.
            var diffs = CalculateSimpleDiff(left, right);

            return diffs.Count == 0 ? DiffResult.Equal() : DiffResult.Diff(diffs);
        }

        private static IReadOnlyCollection<DiffSpan> CalculateSimpleDiff(string left, string right)
        {
            var result = new List<DiffSpan>();

            DiffSpan currentDiffSpan = null;

            for (var i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    // For this point strings are different.
                    if (currentDiffSpan == null)
                        currentDiffSpan = new DiffSpan {Offset = i + 1};

                    continue;
                }

                // Strings are different up to this point.
                if (currentDiffSpan != null)
                {
                    currentDiffSpan.Length = i - currentDiffSpan.Offset + 1;
                    result.Add(currentDiffSpan);
                    currentDiffSpan = null;
                }
            }

            // Handling case when strings differ at the end.
            if (currentDiffSpan != null)
            {
                currentDiffSpan.Length = left.Length - currentDiffSpan.Offset + 1;
                result.Add(currentDiffSpan);
            }

            return result;
        }
    }
}