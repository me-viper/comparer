using ComparerService.App.Models;

using JetBrains.Annotations;

namespace ComparerService.App.Services
{
    public interface IDiffService
    {
        /// <summary>
        /// Performs string diff.
        /// </summary>
        /// <param name="left">Left side of diff</param>
        /// <param name="right">Right side of diff</param>
        /// <returns>
        /// <see cref="DiffResult"/>
        /// </returns>
        DiffResult SimpleDiff([CanBeNull] string left, [CanBeNull] string right);
    }
}