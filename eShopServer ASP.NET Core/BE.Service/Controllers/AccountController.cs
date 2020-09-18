using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using BE.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BE.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IConfiguration configuration;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr, IConfiguration config ) { 
            _userManager = userMgr;
            _signInManager = signInMgr;
            configuration = config;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]Credentials creds)
        {
            var user = await _userManager.FindByNameAsync(creds.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, creds.Password))
            {
                //Get role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                byte[] secret = Encoding.ASCII.GetBytes(configuration["jwtSecret"]);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, creds.Username),
                        new Claim(ClaimTypes.Role,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secret),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken token = handler.CreateToken(descriptor);
                user.Role = role.FirstOrDefault();
                return Ok(new
                {
                    user.Id,
                    user.UserName,
                    user.NormalizedUserName,
                    user.Email,
                    user.NormalizedEmail,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.ConcurrencyStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEnd,
                    user.LockoutEnabled,
                    user.AccessFailedCount,
                    user.Role,
                    user.Orders,
                    token = handler.WriteToken(token),
                    tokenExpirationDate = handler.TokenLifetimeInMinutes
                });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel obj)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                //Property for Back End
                user.Role = "Users";
                IdentityResult result = await _userManager.CreateAsync(user, obj.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Users");
                    return Ok(user);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details");
                    return BadRequest("Invalid user details");
                }

            }
            return BadRequest("Invalid data introduced");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel creds)
        {
            if (ModelState.IsValid && await DoLogin(creds))
            {
                return Ok("true");
            }
            return BadRequest("Invalid data");
        }

		[Authorize]
		[HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private async Task<bool> DoLogin(LoginViewModel creds)
        {
            AppUser user = await _userManager.FindByNameAsync(creds.Name);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, creds.Password, false, false);

                return result.Succeeded;
            }
            return false;
        }

        public class Credentials
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
        }
    }
}