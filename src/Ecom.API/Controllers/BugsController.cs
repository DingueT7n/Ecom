using Ecom.API.Errors;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BugsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Not-Found")]
        public ActionResult GetNotFound()
        {
            var product = _context.Products.Find(50);
            if (product is null)
            {
                return NotFound(new BaseCommuneResponse(404));
            }
            return Ok(product);
        }


        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(50);
            product.Name="";
            return Ok(product);
        }



        [HttpGet("Bad-Request/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }


        [HttpGet("Bad-Request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new BaseCommuneResponse(400));
        }



    }
}
