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
        [HttpPost]
        [Route("{id}/left")]
        public Task<IActionResult> SetLeft([FromRoute] string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{id}/right")]
        public Task<IActionResult> SetRight([FromRoute] string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<IActionResult> GetDiff([FromRoute] string id)
        {
            throw new NotImplementedException();
        }
    }
}
