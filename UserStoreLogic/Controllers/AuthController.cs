using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using DTOs;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
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
        private readonly IConfiguration? _configuration;
        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;


        public AuthController(IConfiguration? configuration, UserManager<IdentityUser> userManager,
            VoucherContext voucherContext, UserDbContext context, IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _unitOfWork = new UnitOfWork(voucherContext);
            _userDbContext = context;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<IdentityResult?>> Register([FromBody] IdentityUserDto request)
        {
            var response = new AuthResponseDto();
            var user = new IdentityUser
            {
                UserName = request.UserName,
            };

            var newUser = await _userManager.CreateAsync(user, request.Password);
            if (newUser.Succeeded)
            {
                var currentIdentityUser = _userManager.FindByNameAsync(request.UserName).Result;
                await _userManager.AddToRoleAsync(currentIdentityUser, Role.User.ToString());
                
                var currentUser = new User()
                {
                    IdentityUserId = currentIdentityUser.Id,
                };

                await _unitOfWork.User.AddAsync(currentUser);

                await _unitOfWork.Complete();

                return Ok(newUser);
            }

            response.Message = newUser.Errors.First().Description;
            return BadRequest(response);


        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleSearchByName([FromBody] ChangeRoleBodyDto body)
        {


            var currentUser = await _userManager.FindByNameAsync(body.UserName);
            if (currentUser != null)
            {
                var currentRole = _userManager.GetRolesAsync(currentUser).Result[0];
                if (currentRole == Role.SuperAdmin.ToString())
                {
                    return BadRequest("SuperAdmin is unchangeable");
                }
                if (body.Role.ToString() != currentRole)
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, currentRole);
                    await _userManager.AddToRoleAsync(currentUser, body.Role.ToString());
                    return Ok($"Assigned to role {body.Role}");
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

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleSearchById([FromBody] ChangeRoleBodyDto body)
        {


            var currentUser = await _userManager.FindByIdAsync(body.UserId);
            if (currentUser != null)
            {
                var currentRole = _userManager.GetRolesAsync(currentUser).Result[0];
                if (currentRole == Role.SuperAdmin.ToString())
                {
                    return BadRequest("SuperAdmin is unchangeable");
                }
                if (body.Role.ToString() != currentRole)
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, currentRole);
                    await _userManager.AddToRoleAsync(currentUser, body.Role.ToString());
                    return Ok($"Assigned to role {body.Role}");
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

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllUsersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllUsersResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = new GetAllUsersResponseDto();
            var superAdmins = await _userManager.GetUsersInRoleAsync(Role.SuperAdmin.ToString());
            var getUsersAsQueryable = () => _userDbContext.Users
                .Where(u => !superAdmins.Contains(u))
                .Select(u => u);
            try
            {
                var identityUsersQuery = await Task.Run(getUsersAsQueryable);
                response.Users = _mapper.ProjectTo<UserDto>(identityUsersQuery);
                response.Message = "Done";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var response = new AuthResponseDto();
            var currentUser = HttpContext.User.Identity;
            if (currentUser == null)
            {
                response.Message = "No user identified in current context";
                return BadRequest(response);
            }
            if (currentUser.IsAuthenticated)
            {
                var userIdentity = _userManager.FindByNameAsync(currentUser.Name).Result;
                if (userIdentity == null)
                {
                    response.Message = "User not found";
                    return BadRequest(response);
                }
                await _userManager.ChangePasswordAsync(userIdentity, currentPassword, newPassword);
                response.Message = "Password changed successfully";
                return Ok(response);
            }
            else
            {
                response.Message = "User not authenticated";
                return BadRequest(response);
            }

        }

    }
}
