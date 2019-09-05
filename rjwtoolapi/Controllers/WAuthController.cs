using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Model;
using Model.Model.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Services;
//using Microsoft.AspNetCore.Cors;

namespace rjwtoolapi.Controllers
{   
    [Authorize]    
    [Route("[controller]")]
    public class WAuthController : ControllerBase
    {
        private readonly Config config;
        private readonly IUserService userService;

        public WAuthController(IOptions<Config> configAccessor, 
                               IUserService userService
                               )
        {
            config = configAccessor.Value;
            this.userService = userService;            
        }

        [AllowAnonymous]
        [HttpPost("auth")]        
        public async Task<IActionResult> LoginService([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginResponse = await userService.AuthenticateAsync(loginModel);

            if (!loginResponse.loggedIn)
                return BadRequest(new { message = loginResponse.token });
           
            return Ok(loginResponse);
        }

        [HttpGet]
        public async Task<IActionResult> getGroupsAsync()
        {
            var userRoutes = await userService.checkIfUserMemberOfAsync(userService.getUserName(User.Claims));
            
            return Ok(new { userRoutes = userRoutes});
        }
    }
}
