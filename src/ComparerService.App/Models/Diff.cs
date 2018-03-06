using System;

namespace ComparerService.App.Models
{
    /// <summary>
    /// Represents single instance of contents difference.
    /// </summary>
    public class Diff : IEquatable<Diff>
    {
        /// <summary>
        /// Position startng from which contents are different.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Length of different contents.
        /// </summary>
        public int Length { get; set; }

        public bool Equals(Diff other)
        {
            if (other == null)
                return false;

            return other.Offset == Offset && other.Length == Length;
        }

        public override string ToString()
        {
            return $"Offset: {Offset}; Length: {Length}";
        }
    }
}