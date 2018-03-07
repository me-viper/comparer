using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparerService.App.Models;
using ComparerService.App.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ComparerService.App.Controllers
{
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class DiffController : Controller
    {
        private readonly IDiffService _diffService;
        private readonly IComparisonContentRepository _contentRepository;
        private readonly ILogger<DiffController> _logger;

        public DiffController(
            IDiffService diffService, 
            IComparisonContentRepository contentRepository,
            ILogger<DiffController> logger)
        {
            if (diffService == null)
                throw new ArgumentNullException(nameof(diffService));

            if (contentRepository == null)
                throw new ArgumentNullException(nameof(contentRepository));

            _diffService = diffService;
            _contentRepository = contentRepository;
            _logger = logger ?? NullLogger<DiffController>.Instance;
        }

        /// <summary>
        /// Sets left side of comparison.
        /// </summary>
        /// <param name="id">Comparison ID</param>
        /// <param name="content">Base64 encoded content to compare</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /999/left
        ///     {
        ///         "content": "dGhpcyBpcyBzYW1wbGUgdGV4dA=="
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("{id}/left")]
        public async Task<IActionResult> SetLeft([FromRoute] string id, [FromBody] string content)
        {
            return await SetComparisonContent(id, content, ComparisonSide.Left).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets right side of comparison.
        /// </summary>
        /// <param name="id">Comparison ID</param>
        /// <param name="content">Base64 encoded content to compare</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /999/right
        ///     {
        ///         "content": "dGhpcyBpcyBzYW1wbGUgdGV4dA=="
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("{id}/right")]
        public async Task<IActionResult> SetRight([FromRoute] string id, [FromBody] string content)
        {
            return await SetComparisonContent(id, content, ComparisonSide.Right).ConfigureAwait(false);
        }

        /// <summary>
        /// Compares left and right.
        /// </summary>
        /// <param name="id">Copmarison ID</param>
        /// <returns>Comparison reuslt</returns>
        /// <response code = "200">Returns diff result</response>
        /// <response code = "204">No contents to compare for specified ComparisonID</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DiffResultDto), 200)]
        public async Task<IActionResult> GetDiff([FromRoute] string id)
        {
            IDisposable loggingScope = null;

            try
            {
                loggingScope = _logger.BeginScope("ComparisonID {0}", id);
                _logger.LogInformation("Comparing contents.");

                var content = await _contentRepository.GetContent(id).ConfigureAwait(false);

                if (content == null)
                {
                    _logger.LogInformation("Nothing to compare.");
                    return NoContent();
                }

                var diffResult = _diffService.SimpleDiff(content.Left, content.Right);

                _logger.LogDebug("Comparision result: {0}", diffResult.ToString());
                
                var result = new DiffResultDto { Id = id, DiffType = diffResult.Type, Diffs = diffResult.Diffs };

                _logger.LogInformation("Comparison completed successfully");

                return new ObjectResult(result);
            }
            finally
            {
                loggingScope?.Dispose();
            }
        }

        private async Task<IActionResult> SetComparisonContent(string id, string content, ComparisonSide side)
        {
            if (content == null)
                return BadRequest();

            IDisposable loggingScope = null;

            try
            {
                loggingScope = _logger.BeginScope("ComparisonID: {0}; Side: {2}", id, side);
                _logger.LogInformation("Setting comparison content");

                byte[] buffer;

                try
                {
                    buffer = Convert.FromBase64String(content);
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, "Failed to parse content. Content: {0}", content);
                    return BadRequest("Invalid content format. Expected base64 encoded string.");
                }

                var decodedContent = Encoding.UTF8.GetString(buffer);

                await _contentRepository.SetContent(id, decodedContent, side).ConfigureAwait(false);

                _logger.LogInformation("Content set");

                return Ok();
            }
            finally
            {
                loggingScope?.Dispose();
            }
        }
    }
}
