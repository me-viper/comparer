using System;
using System.Collections.Generic;
using System.Linq;

using ComparerService.App.Utility;

using JetBrains.Annotations;

namespace ComparerService.App.Models
{
    public class DiffResult : IEquatable<DiffResult>
    {
        /// <summary>
        /// Comparison outcome.
        /// </summary>
        public DiffType Type { get; }

        /// <summary>
        /// Enumeration of differences.
        /// </summary>
        public IEnumerable<DiffSpan> Diffs { get; }

        private DiffResult(DiffType type, IEnumerable<DiffSpan> diffs = null)
        {
            Type = type;
            Diffs = diffs ?? Enumerable.Empty<DiffSpan>();
        }

        public static DiffResult Equal()
        {
            return new DiffResult(DiffType.Equal);
        }

        public static DiffResult SizeDoesNotMatch()
        {
            return new DiffResult(DiffType.SizeDoesNotMatch);
        }

        public static DiffResult Diff([ItemNotNull] IEnumerable<DiffSpan> diffs)
        {
            if (diffs == null)
                throw new ArgumentNullException(nameof(diffs));

            var tmpDiffs = diffs.ToArray();

            if (!tmpDiffs.Any())
                throw new ArgumentException("Diffs collection is empty", nameof(diffs));

            return new DiffResult(DiffType.Diff, tmpDiffs);
        }

        public bool Equals(DiffResult other)
        {
            if (other == null)
                return false;

            return other.Type == Type && other.Diffs.SequenceEqual(Diffs, new DiffEqualityComparer());
        }

        public override string ToString()
        {
            return $"Type: {Type}; {Environment.NewLine} Diffs: {string.Join(Environment.NewLine, Diffs)}";
        }
    }
}