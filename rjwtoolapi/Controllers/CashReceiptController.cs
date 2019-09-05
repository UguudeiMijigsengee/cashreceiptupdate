using DataAccess.Services;
using Business.CashReceipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Model.CashReceipt.Resource;

namespace rjwtoolapi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CashReceiptController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ICashReceiptService cashReceiptService;

        public CashReceiptController(IUserService userService,
                                     ICashReceiptService cashReceiptService)
        {
            this.userService = userService;
            this.cashReceiptService = cashReceiptService;
        }

        [HttpPost("getmbinvoices")]
        public async Task<IActionResult> GetMbInvoices([FromBody] CashReceiptRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cashReceiptResponse = await cashReceiptService.getMbInvoices(request, userService.getUserName(User.Claims));

            if (!cashReceiptResponse.isInvoiced)
                return BadRequest(cashReceiptResponse);

            return Ok(cashReceiptResponse);
        }

        [HttpPost("apply2invoices")]
        public async Task<IActionResult> Apply2Invoices([FromBody] CashReceiptApply2InvoiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cashReceiptApply2InvoiceResponse = await cashReceiptService.apply2Invoice(request, userService.getUserName(User.Claims));

            if (!cashReceiptApply2InvoiceResponse.isApplied)
                return BadRequest(cashReceiptApply2InvoiceResponse);

            return Ok(cashReceiptApply2InvoiceResponse);
        }
    }
}
