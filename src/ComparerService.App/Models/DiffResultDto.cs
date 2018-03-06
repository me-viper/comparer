using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ComparerService.App.Models
{
    /// <summary>
    /// Represent diff result.
    /// </summary>
    public class DiffResultDto
    {
        /// <summary>
        /// Comparison ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Comparison outcome.
        /// </summary>
        public DiffType DiffType { get; set; }

        /// <summary>
        /// Detected diffs.
        /// </summary>
        public IEnumerable<Diff> Diffs { get; set; }
    }
}