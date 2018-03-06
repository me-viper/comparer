using System.Collections.Concurrent;
using System.Threading.Tasks;

using ComparerService.App.Models;

namespace ComparerService.App.Services
{
    internal class InMemoryRepository : IComparisonContentRepository
    {
        private ConcurrentDictionary<string, ComparisonContent> _store = new ConcurrentDictionary<string, ComparisonContent>();

        public async Task SetContent(string id, string content, ComparisonSide side)
        {
            var comparisonContent = new ComparisonContent { Id = id };
            Set(comparisonContent, content, side);

            _store.AddOrUpdate(id, comparisonContent, (key, val) => Set(val, content, side));
        }

        public async Task<ComparisonContent> GetContent(string id)
        {
            if (_store.TryGetValue(id, out var comparisonContent))
                return comparisonContent;

            return null;
        }

        private static ComparisonContent Set(ComparisonContent comparisonContent, string content, ComparisonSide side)
        {
            if (side == ComparisonSide.Left)
                comparisonContent.Left = content;

            if (side == ComparisonSide.Right)
                comparisonContent.Right = content;

            return comparisonContent;
        }
    }
}