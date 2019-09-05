using Business.Consolidate;
using DataAccess.Services;
using Model.Model.Consolidate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rjwtoolapi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ConsolidateController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IConsolidateService consolidateService;

        public ConsolidateController(IUserService userService,
                                     IConsolidateService consolidateService)
        {
            this.userService = userService;
            this.consolidateService = consolidateService;
        }

        [HttpPost("consolidate")]
        public async Task<IActionResult> Consolidate([FromBody] ConsolidateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var consolidateResponse = await consolidateService.ConsolidateLoad(request, userService.getUserName(User.Claims));

            if (!consolidateResponse.isConsolidated)
                return BadRequest(consolidateResponse);

            return Ok(consolidateResponse);
        }

        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromBody] ConsolidateMergeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mergeResponse = await consolidateService.MergeLoads(request, userService.getUserName(User.Claims));

            if (!mergeResponse.isMerged)
                return BadRequest(mergeResponse);

            return Ok(mergeResponse);
        }
    }
}
