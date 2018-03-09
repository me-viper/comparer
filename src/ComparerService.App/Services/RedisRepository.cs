using System;
using System.Threading.Tasks;

using ComparerService.App.Models;

using ServiceStack.Redis;

namespace ComparerService.App.Services
{
    public class RedisRepository : IComparisonContentRepository
    {
        private IRedisClientsManager _clientManager;

        public int MaxLength => 1024 * 8;

        public RedisRepository(IRedisClientsManager clientManager)
        {
            if (clientManager == null)
                throw new ArgumentNullException(nameof(clientManager));

            _clientManager = clientManager;
        }

        public Task<ComparisonContent> GetContent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value can't be null or empty string", nameof(id));

            using (var client = _clientManager.GetClient())
            {
                var left = client.Get<string>($"{id}:{ComparisonSide.Left}");
                var right = client.Get<string>($"{id}:{ComparisonSide.Right}");

                return Task.FromResult(new ComparisonContent {Id = id, Left = left, Right = right});
            }
        }

        public Task SetContent(string id, string content, ComparisonSide side)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value can't be null or empty string", nameof(id));

            if (content?.Length > MaxLength)
                throw new NotSupportedException("Content is to large");

            using (var client = _clientManager.GetClient())
            {
                client.Set($"{id}:{side.ToString()}", content);
            }

            return Task.CompletedTask;
        }
    }
}