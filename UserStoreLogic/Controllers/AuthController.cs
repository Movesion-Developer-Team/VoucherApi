﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Domain;
using DTOs;
using Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            //_signInManager = signInManager;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<IdentityResult?>> Register([FromQuery]IdentityUserDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.UserName,
            };

            var newUser = await _userManager.CreateAsync(user, request.Password);
            if (newUser.Succeeded == true)
            {
                await _userManager.AddToRoleAsync(_userManager.FindByNameAsync(request.UserName).Result, Role.User.ToString());
            }

            return Ok(newUser);
        }

        [HttpPost]
        [Route("[action]")]
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

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromQuery]IdentityUserDto request)
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

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(jwt);

        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var currentUser = HttpContext.User.Identity;
            if (currentUser.IsAuthenticated)
            {
                var userIdentity = _userManager.FindByNameAsync(currentUser.Name).Result;
                if (userIdentity == null)
                {
                    return BadRequest("User not found");
                }
                await _userManager.ChangePasswordAsync(userIdentity, currentPassword, newPassword);
                return Ok("Password changed successfully");
            }
            else
            {
                return BadRequest("User not authenticated");
            }

        }




    }
}