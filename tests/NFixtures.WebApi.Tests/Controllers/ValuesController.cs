using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NFixtures.WebApi.Tests.Controllers
{
    public class ValuesController : ControllerBase
    {
        [Authorize]
        [HttpGet("/api/values")]
        public IActionResult Get() => Ok();
    }
}
