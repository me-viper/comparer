namespace ComparerService.App.Models
{
    /// <summary>
    /// Represents comparison outcome.
    /// </summary>
    public enum DiffType
    {
        /// <summary>
        /// Strings are equal size and equal contents.
        /// </summary>
        Equal,

        /// <summary>
        /// String are not equal size.
        /// </summary>
        SizeDoesNotMatch,

        /// <summary>
        /// Strings are equal size but different contents.
        /// </summary>
        Diff
    }
}