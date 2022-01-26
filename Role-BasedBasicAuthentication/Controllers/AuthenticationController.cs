using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Role_BasedBasicAuthentication.Authentication;
using Role_BasedBasicAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Role_BasedBasicAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Add(Register register)
        {
            var userExist = await _userManager.FindByNameAsync(register.UserName);
            if (userExist!=null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new Response { 
                Status="Duplicate Data",
                Message="Already registered as "+register.UserName,
                });
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName=register.UserName,
                Email=register.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "Password Related Error"
                }); 
            }
            return Ok(new Response { 
                Status = "Success",
                Message = "Hello " + register.UserName + " Welcome to ....."
            });
        }
    }
}
