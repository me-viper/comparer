using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace ComparerService.App.Controllers
{
    [Route("v1/[controller]")]
    public class DiffController : Controller
    {
        /// <summary>
        /// Sets left side of comparison.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/left")]
        public Task<IActionResult> SetLeft([FromRoute] string id)
        {
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost]
        [Route("{id}/right")]
        public Task<IActionResult> SetRight([FromRoute] string id)
        {
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpGet]
        [Route("{id}")]
        public Task<IActionResult> GetDiff([FromRoute] string id)
        {
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}
