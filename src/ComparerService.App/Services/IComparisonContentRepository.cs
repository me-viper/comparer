using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ComparerService.App.Models;

using JetBrains.Annotations;

namespace ComparerService.App.Services
{
    public interface IComparisonContentRepository
    {
        [CanBeNull]
        Task<ComparisonContent> GetContent([NotNull] string id);

        Task SetContent([NotNull] string id, [CanBeNull] string content, ComparisonSide side);
    }
}