using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using ComparerService.App.Models;

namespace ComparerService.App.Services
{
    internal class InMemoryRepository : IComparisonContentRepository
    {
        private ConcurrentDictionary<string, ComparisonContent> _store = new ConcurrentDictionary<string, ComparisonContent>();

        public int MaxLength => 1024 * 8;

        public Task SetContent(string id, string content, ComparisonSide side)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value can't be null or empty string", nameof(id));

            if (content?.Length > MaxLength)
                throw new NotSupportedException("Content is to large");

            var comparisonContent = new ComparisonContent { Id = id };
            Set(comparisonContent, content, side);

            _store.AddOrUpdate(id, comparisonContent, (key, val) => Set(val, content, side));

            return Task.CompletedTask;
        }

        public Task<ComparisonContent> GetContent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value can't be null or empty string", nameof(id));

            if (_store.TryGetValue(id, out var comparisonContent))
                return Task.FromResult(comparisonContent);

            return Task.FromResult<ComparisonContent>(null);
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