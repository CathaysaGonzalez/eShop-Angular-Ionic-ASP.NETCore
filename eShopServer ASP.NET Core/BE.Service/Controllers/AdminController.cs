using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BE.Models.Entities;
using BE.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BE.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private IUserValidator<AppUser> _userValidator;
        private IPasswordValidator<AppUser> _passwordValidator;
        private IPasswordHasher<AppUser> _passwordHasher;

        public AdminController(UserManager<AppUser> usrMgr,
            IUserValidator<AppUser> userValid,
            IPasswordValidator<AppUser> passValid,
            IPasswordHasher<AppUser> passwordHash)
        {
            _userManager = usrMgr;
            _userValidator = userValid;
            _passwordValidator = passValid;
            _passwordHasher = passwordHash;
        }

        [Authorize(Roles = "Admins")]
        [HttpGet]
        public async Task<List<AppUser>> ViewUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ViewUser(string id)
        {
            AppUser user = await _userManager.FindByNameAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Users")]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> ViewCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = model.Name;
                user.Email = model.Email;
                //Property for Back End
                user.Role = "Users";
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    IdentityResult result2 = await _userManager.AddToRoleAsync(user, "Users");
                    if (result2.Succeeded)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        foreach (IdentityError error in result2.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return BadRequest();
                    }
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("new")]
        public async Task<IActionResult> CreateAdmin([FromBody]CreateUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = model.Name;
                user.Email = model.Email;
                //Property for Back End
                user.Role = "Admins";
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    IdentityResult result2 = await _userManager.AddToRoleAsync(user, "Admins");
                    if (result2.Succeeded)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        foreach (IdentityError error in result2.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return BadRequest();
                    }
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] EditUser model)
        {
            AppUser user = await _userManager.FindByNameAsync(id);
            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                    return BadRequest("Email not valid");
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                        return BadRequest("Password not valid");
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && model.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok("true");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                        return BadRequest("Error in IdentiyResult");
                    }
                }
                else
                {
                    return BadRequest("Password or email invalid");
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
                return BadRequest("User not found");
            }
        }

        [Authorize(Roles = "Users")]
        [HttpPost("CurrentUser")]
        public async Task<IActionResult> EditCurrentUser([FromBody] EditUser model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                    return BadRequest("Email not valid");
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                        return BadRequest("Password not valid");
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                    || (validEmail.Succeeded
                        && model.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok("true");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                        return BadRequest("Error in Identiy Result");
                    }
                }
                else
                {
                    return BadRequest("Password or email invalid");
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
                return BadRequest("User not found");
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("search/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)] //Returns single user
        [ProducesResponseType(404)] //Returned when user with specific username doesn't exist
        public async Task<IActionResult> SearchUser(string id)
        {
            AppUser user = await _userManager.FindByNameAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
                return NotFound();
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    return Ok("true");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admins")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            AppUser user = await _userManager.FindByNameAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok("true");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return Ok("true");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}