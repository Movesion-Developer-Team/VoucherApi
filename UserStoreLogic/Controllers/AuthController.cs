using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Domain;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult<IdentityUser>> Register(IdentityUserDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.UserName
            };
            var newUser = await _userManager.CreateAsync(user, request.Password);
            return Ok(newUser);
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(IdentityUserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong password");
            }

            var token = await _userManager.CreateSecurityTokenAsync(user);

            var tokenString = token.ToString();

            return Ok(tokenString);

        }

        [HttpGet("Get Current User"), Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {

            var identity = this.HttpContext.User.Identity;
            return Ok();
        }


    
        
    }
}
