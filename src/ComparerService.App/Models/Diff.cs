using System;

namespace ComparerService.App.Models
{
    public class Diff : IEquatable<Diff>
    {
        public int Offset { get; set; }
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