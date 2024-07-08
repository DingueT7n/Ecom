using Ecom.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        //[HttpGet("Error")]
        public IActionResult Error( int statusCode)
        {
            return new ObjectResult(new BaseCommuneResponse(statusCode));
        }
    }
}
