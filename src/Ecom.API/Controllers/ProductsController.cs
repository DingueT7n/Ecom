
using Ecom.API.Errors;
using Ecom.API.Helper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Ecom.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IConfiguration config;

        public ProductsController(IUnitOfWork unitofwrok, IConfiguration config)
        {
            this.UnitOfWork = unitofwrok;
            this.config = config;
        }

        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> Get([FromQuery] ProductParams ProdParam)
        {
            
            //var AllProducts = await UnitOfWork.ProductRepository.GetAllAsync(x => x.Category);
            var AllProducts = await UnitOfWork.ProductRepository.GetAllAsync(ProdParam);
            var Count = await UnitOfWork.ProductRepository.CountProductsAsync(ProdParam);
            if (AllProducts is not null)
            {
                //return Ok(AllProducts);
                return Ok(new Pagination<ProductDtos>(ProdParam.Pagenumber,ProdParam.PageSiz,Count,AllProducts));

            }

            return BadRequest("Not Found");
        }


        [HttpGet("Get-Product-By-Id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommuneResponse),StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(int id)
        {
            var Product = await UnitOfWork.ProductRepository.GetByIdAsync(id,x=>x.Category);
            if (Product is null)
                return NotFound(new BaseCommuneResponse(404));
            if (Product is not null)
            {
                ProductDtos result = new ProductDtos
                {
                    Id = Product.Id,
                    Description = Product.Description,
                    Name = Product.Name,
                    Price = Product.Price,
                    CategoryName = Product.Category.Name,
                    ProductPicture = string.IsNullOrEmpty(Product.ProductPicture) ? null : config["ApiURL"] + Product.ProductPicture,
                };
                return Ok(result);
            }
            return BadRequest($"Not Found This Id {id}");
        }

        [HttpPost("Add-New-Product")]
        public async Task<IActionResult> AddNewProduct(AddProductDtos ProdDtos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  var res =  await UnitOfWork.ProductRepository.AddAsync(ProdDtos);
                    return res ? Ok(ProdDtos) :BadRequest();
                }
                return BadRequest(ProdDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("Update-Product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id ,[FromForm]UpdateProductDtos ProdDtos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await UnitOfWork.ProductRepository.UpdateAsync(id,ProdDtos);
                    return res ? Ok(ProdDtos) : BadRequest();
                }
                return BadRequest(ProdDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await UnitOfWork.ProductRepository.DeleteAsyncWithPicture(id);
                    return res ? Ok(res) : BadRequest();
                }
                return NotFound($"This is {id} Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
