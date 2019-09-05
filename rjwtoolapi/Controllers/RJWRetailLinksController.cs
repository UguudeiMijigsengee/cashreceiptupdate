using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Model.Model.RetailLink;
using Microsoft.Extensions.Options;
using DataAccess.Services;
using Business.RetailLink;

namespace rjwtoolapi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class RJWRetailLinksController : ControllerBase
    {        
        private readonly IUserService userService;
        private readonly IRetailLinkService retailLinkService;

        public RJWRetailLinksController(IUserService userService, 
                                        IRetailLinkService retailLinkService)
        {        
            this.userService = userService;
            this.retailLinkService = retailLinkService;
        }

        //[HttpPost("schedule")]
        //public async Task<IActionResult> ScheduleService([FromBody] RetailLinkRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var retailLinkResponse = await retailLinkService.ScheduleAppt(request, userService.getUserName(User.Claims));

        //    if (!retailLinkResponse.isScheduled)
        //        return BadRequest(retailLinkResponse);

        //    return Ok(retailLinkResponse);
        //}

        [HttpPost("schedule2")]
        public async Task<IActionResult> ScheduleService2([FromBody] RetailLinkRequest2 request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var retailLinkResponse = await retailLinkService.ScheduleAppt(request, userService.getUserName(User.Claims));

            if (!retailLinkResponse.isLoggedIn)
                return BadRequest(retailLinkResponse);

            return Ok(retailLinkResponse);
        }
    }
}
