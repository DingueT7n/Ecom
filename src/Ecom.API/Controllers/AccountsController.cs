using Ecom.API.Errors;
using Ecom.API.Extensions;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _sinInManager;
        private readonly ITokenServices _tokenServices;
     


        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> sinInManager,ITokenServices tokenServices)
        {
            _userManager = userManager;
            _sinInManager = sinInManager;
            _tokenServices = tokenServices;
        }

        [Authorize]
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "hi";
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDtos dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return Unauthorized(new BaseCommuneResponse(401));
            }
            var result = await _sinInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (result is null || result.Succeeded == false)
            {

                return Unauthorized(new BaseCommuneResponse(401));
            }

            return Ok(new UserDtos
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenServices.CreateToken(user)
            });



        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDtos dto)
        {
            if (CheckEmailExist(dto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {"This Email Is Already Taken" }

                });
            }
            var user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email,

            };
            var result = await _userManager.CreateAsync(user,dto.Password);
            if (result.Succeeded == false)
            {
                return BadRequest(new BaseCommuneResponse(400));
            }
            return Ok(
                new UserDtos
                {
                    DisplayName = dto.DisplayName,
                    Email = dto.Email,
                    Token = _tokenServices.CreateToken(user)
                }
                );

        }

        [Authorize]
        [HttpGet("Get-Current-User")]
        public async Task<IActionResult> GetCurrentUser()
        {
            //var email = HttpContext.User?.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.Email)?.Value;
            //var user =await _userManager.FindByEmailAsync(email);
            var user =await _userManager.FindEmailByClaimPrincipal(HttpContext.User);
            return Ok(new UserDtos
            {
                Email = user?.Email,
                DisplayName = user.DisplayName,
                Token = _tokenServices.CreateToken(user)
            });
        }

        [HttpGet("Check-Email-Exist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result is not null)
            {
                return true;
            }
            return false;
        }


        [Authorize]
        [HttpGet("Get-User-Address")]
        public async Task<IActionResult> GetUserAddress()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            //var user = await _userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);

            var _result = new AddressDtos
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street,

                City = user.Address.City,
                State = user.Address.State,
                ZipCode = user.Address.ZipCode,
               
            };

            //var _result = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(_result);
        }

        [Authorize]
        [HttpPut("Update-User-Address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDtos dto)
        {
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            user.Address.FirstName = dto.FirstName;
            user.Address.LastName = dto.LastName;   
            user.Address.ZipCode = dto.ZipCode;
            user.Address.State=dto.State;
            user.Address.City = dto.City;
            user.Address.Street = dto.Street;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(new AddressDtos
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street,

                City = user.Address.City,
                State = user.Address.State,
                ZipCode = user.Address.ZipCode,

            });

            return BadRequest($"problem while updating this user {HttpContext.User}");
        }




    }
}
