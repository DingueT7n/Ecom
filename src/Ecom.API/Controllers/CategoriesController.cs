
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet("Get-All-Categories")]
        public async Task<IActionResult> Get()
        {
            var AllCategories = await UnitOfWork.CategoryRepository.GetAllAsync();
            if (AllCategories is not null)
            {
                var result = AllCategories.Select(x => new ListingCategoryDtos
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                }).ToList();
                return Ok(result);
            }

            return BadRequest("Not Found");
        }

        [HttpGet("Get-Category-By-Id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Category = await UnitOfWork.CategoryRepository.GetAsync(id);
            if (Category is not null)
            {
                ListingCategoryDtos result = new ListingCategoryDtos
                { 
                    Id  = Category.Id,
                    Description = Category.Description,
                    Name = Category.Name
                };
                return Ok(result);
            }
            return BadRequest($"Not Found This Id {id}");
        }


        [HttpPost("Add-New-Category")]
        public async Task<IActionResult> AddNewCategory(CategoryDtos CatDtos)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var NewCategory = new Category { 
                        Name = CatDtos.Name,
                        Description = CatDtos.Description 

                    };
                    await UnitOfWork.CategoryRepository.AddAsync(NewCategory);
                    return Ok(NewCategory);
                }
                return BadRequest(CatDtos);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

                
            }

        }
        [HttpPut("Update-Exiting-Category-ById/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryDtos CatDtos)
        {
            try
            {
                if (ModelState.IsValid) { 
                var exitingCategory = await UnitOfWork.CategoryRepository.GetAsync(id);
                    if (exitingCategory is not null)
                    {
                        exitingCategory.Description = CatDtos.Description;
                        exitingCategory.Name = CatDtos.Name;
                        await UnitOfWork.CategoryRepository.UpdateAsync(id, exitingCategory);
                        return Ok(CatDtos);

                    }

                }
                return BadRequest($"Category Not Found , Id [{id}] Incorrect");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }


        }
        [HttpDelete("Delete-Category-ById/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var exitingCategory = await UnitOfWork.CategoryRepository.GetAsync(id);
                if (exitingCategory is not null)
                {
                    await UnitOfWork.CategoryRepository.DeleteAsync(id);
                
                    return Ok($"This Category [{exitingCategory.Name}] is Deleted Successfully");
                
                }
                return BadRequest($"Category Not Found , Id [{id}] Incorrect");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
         

        }




    }
}
