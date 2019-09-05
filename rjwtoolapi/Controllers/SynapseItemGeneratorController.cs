using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.SynapseItemGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DataAccess.Services;

namespace rjwtoolapi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class SynapseItemGeneratorController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ISynapseItemGeneratorService synapseItemGeneratorService;

        public SynapseItemGeneratorController(IUserService userService, 
                                              ISynapseItemGeneratorService synapseItemGeneratorService)
        {
            this.userService = userService;
            this.synapseItemGeneratorService = synapseItemGeneratorService;
        }

        [HttpPost("itemgenerator")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var synapseItemGeneratorResponse = await synapseItemGeneratorService.GenerateItem(file, userService.getUserName(User.Claims));

            if (!synapseItemGeneratorResponse.isUploaded)
                return BadRequest(synapseItemGeneratorResponse);

            return Ok(synapseItemGeneratorResponse);
        }
    }
}
