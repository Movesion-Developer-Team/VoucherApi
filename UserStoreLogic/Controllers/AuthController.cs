using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Domain;
using DTOs;
using Enum;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using UserStoreLogic.DTOs.BodyDtos;
using UserStoreLogic.DTOs.ResponseDtos;

namespace UserStoreLogic.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("/[controller]/[action]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly UnitOfWork _unitOfWork;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AuthResponseDto _response = new();

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, VoucherContext voucherContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _unitOfWork = new UnitOfWork(voucherContext);
            //_signInManager = signInManager;
        }


        [HttpPost]
        public async Task<ActionResult<IdentityResult?>> Register([FromBody] IdentityUserDto request)
        {
            Company currentCompany;
            var user = new IdentityUser
            {
                UserName = request.UserName,
            };


            try
            {
                currentCompany = await _unitOfWork.Company.Find(c => c.Id == request.CompanyId).FirstAsync();
            }
            
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }


            var newUser = await _userManager.CreateAsync(user, request.Password);
            if (newUser.Succeeded)
            {
                var currentIdentityUser = _userManager.FindByNameAsync(request.UserName).Result;
                await _userManager.AddToRoleAsync(currentIdentityUser, Role.User.ToString());
                _unitOfWork.Company.Update(currentCompany);

                var currentUser = new User()
                {
                    IdentityUserId = currentIdentityUser.Id,
                    CompanyId = request.CompanyId
                };

                await _unitOfWork.Company.AddWorkerToCompany(currentUser, currentCompany.Id);

                await _unitOfWork.Complete();

                return Ok(newUser);
            }
            else
            {
                _response.Message = newUser.Errors.First().Description;
                return BadRequest(_response);
            }


        }
        
        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> ChangeRole(string name, BasicRole role)
        {


            var currentUser = await _userManager.FindByNameAsync(name);
            if (currentUser != null)
            {
                var currentRole = _userManager.GetRolesAsync(currentUser).Result[0];
                if (currentRole == Role.SuperAdmin.ToString())
                {
                    return BadRequest("SuperAdmin is unchangeable");
                }
                if (role.ToString() != currentRole)
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, currentRole);
                    await _userManager.AddToRoleAsync(currentUser, role.ToString());
                    return Ok($"Assigned to role {role}");
                }
                else
                {
                    return BadRequest("User is already in role");
                }


            }
            else
            {
                return NotFound("User Not Found");
            }




        }

        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginBodyDto login)
        {
            var authResponse = new AuthResponseDto();

            login.UserName = StringExtensions.RemoveSpacesForLogin(login.UserName);

            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null)
            {
                authResponse.Message = "User not found";
                return BadRequest(authResponse);
            }

            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password) ==
                PasswordVerificationResult.Failed)
            {
                authResponse.Message = "Wrong password";
                return BadRequest(authResponse);
            }

            authResponse.IsAuthenticated = true;
            List<Claim> claims = new()

            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, string.Join(", ",
                    _userManager.GetRolesAsync(user).Result))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AuthSettings:Token")));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
            );

            authResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(authResponse);


        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var currentUser = HttpContext.User.Identity;
            if (currentUser == null)
            {
                _response.Message = "No user identified in current context";
                return BadRequest(_response);
            }
            if (currentUser.IsAuthenticated)
            {
                var userIdentity = _userManager.FindByNameAsync(currentUser.Name).Result;
                if (userIdentity == null)
                {
                    _response.Message = "User not found";
                    return BadRequest(_response);
                }
                await _userManager.ChangePasswordAsync(userIdentity, currentPassword, newPassword);
                _response.Message = "Password changed successfully";
                return Ok(_response);
            }
            else
            {
                _response.Message = "User not authenticated";
                return BadRequest(_response);
            }

        }


    }
}
