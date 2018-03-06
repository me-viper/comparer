using System;
using System.Collections.Generic;
using System.Linq;

using ComparerService.App.Services;
using ComparerService.App.Utility;

namespace ComparerService.App.Models
{
    public class DiffResult : IEquatable<DiffResult>
    {
        public DiffType Type { get; }
        public IEnumerable<Diff> Diffs { get; }

        public DiffResult(DiffType type)
        {
            Type = type;
            Diffs = Enumerable.Empty<Diff>();
        }

        public DiffResult(DiffType type, IEnumerable<Diff> diffs)
        {
            if (diffs == null)
                throw new ArgumentNullException(nameof(diffs));

            Type = type;
            Diffs = diffs;
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