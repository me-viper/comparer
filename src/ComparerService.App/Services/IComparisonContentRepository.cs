using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ComparerService.App.Models;

namespace ComparerService.App.Services
{
    public interface IComparisonContentRepository
    {
        Task<ComparisonContent> GetContent(string id);

        Task SetContent(string id, string content, ComparisonSide side);
    }
}