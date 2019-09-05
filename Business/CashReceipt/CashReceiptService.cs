using AutoMapper;
using Microsoft.Extensions.Options;
using DataAccess.Persistence.CashReceipt;
using DataAccess.Services;
using Model.Model;
using Model.Model.CashReceipt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Model.CashReceipt.Resource;
using System.Net.Http;
using Newtonsoft.Json;

namespace Business.CashReceipt
{
    public class CashReceiptService : ICashReceiptService
    {
        private readonly Config _config;
        private readonly IMapper mapper;
        private readonly ICashReceiptRepository cashReceiptRepository;
        private readonly IUserService userService;

        public CashReceiptService(IMapper mapper,
                                  IOptions<Config> config,
                                  ICashReceiptRepository cashReceiptRepository,
                                  IUserService userService)
        {
            _config = config.Value;
            this.mapper = mapper;
            this.cashReceiptRepository = cashReceiptRepository;
            this.userService = userService;
        }

        public async Task<CashReceiptResponse> getMbInvoices(CashReceiptRequest request, string userName)
        {
            var cashReceiptResponse = new CashReceiptResponse();

            if (!userService.isUserMemberOfGroup(_config.UserServiceConfig.UserRoutes[4].group, userName))
            {
                cashReceiptResponse.isInvoiced = false;
                cashReceiptResponse.message.Add(String.Format(_config.UserServiceConfig.accountMessages[2], userName, _config.UserServiceConfig.UserRoutes[4].header));
            }

            if (cashReceiptResponse.isInvoiced)
            {
                List<CashReceiptApplyCheck> invoices = null;
                try
                {
                    invoices = await cashReceiptRepository.getMbInvoices(String.Format(_config.CashReceiptConfig.storedProcedures[0], request.Company, request.MbNumber));
                    if (invoices.Count > 0)
                    {                        
                        var invoicesResponse = mapper.Map<List<CashReceiptApplyCheck>, List<CashReceiptApplyCheckResponse>>(invoices);
                        cashReceiptResponse.CashReceiptCustomerResponse.CashReceiptApplyCheckResponses = invoicesResponse;
                        cashReceiptResponse.CashReceiptCustomerResponse.custID = invoices[0].Custid.Trim();                        
                    }
                    else
                    {
                        cashReceiptResponse.isInvoiced = false;
                        cashReceiptResponse.message.Add(_config.CashReceiptConfig.messages[0]);
                    }
                }
                catch (Exception ex)
                {
                    cashReceiptResponse.isInvoiced = false;
                    cashReceiptResponse.message.Add(_config.CashReceiptConfig.messages[0]);
                }
            }

            return cashReceiptResponse;
        }

        public async Task<CashReceiptApply2InvoiceResponse> apply2Invoice(CashReceiptApply2InvoiceRequest request, string userName)
        {
            var cashReceiptApply2InvoiceResponse = new CashReceiptApply2InvoiceResponse();

            if (!userService.isUserMemberOfGroup(_config.UserServiceConfig.UserRoutes[4].group, userName))
            {
                cashReceiptApply2InvoiceResponse.isApplied = false;
                cashReceiptApply2InvoiceResponse.message.Add(String.Format(_config.UserServiceConfig.accountMessages[2], userName, _config.UserServiceConfig.UserRoutes[4].header));
            }

            if (cashReceiptApply2InvoiceResponse.isApplied)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(_config.CashReceiptConfig.GPServiceURLs[0],
                                                              new StringContent(JsonConvert.SerializeObject(request), 
                                                                                Encoding.UTF8, 
                                                                                _config.CashReceiptConfig.contentType));
                        if (response.IsSuccessStatusCode)
                        {
                            cashReceiptApply2InvoiceResponse.isApplied = true;
                            cashReceiptApply2InvoiceResponse.message.Add(_config.CashReceiptConfig.messages[2]);
                        }
                        else
                        {
                            cashReceiptApply2InvoiceResponse.isApplied = false;
                            var cashReceiptGPResponses = JsonConvert.DeserializeObject<List<CashReceiptGPResponse>>(response.Content.ReadAsStringAsync().Result);
                            foreach (CashReceiptGPResponse gPResponse in cashReceiptGPResponses)
                            {
                                cashReceiptApply2InvoiceResponse.message.Add(String.Format(_config.CashReceiptConfig.messages[1], gPResponse.ErrorNumber, gPResponse.Description));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    cashReceiptApply2InvoiceResponse.isApplied = false;
                    cashReceiptApply2InvoiceResponse.message.Add(_config.CashReceiptConfig.messages[1]);
                }
            }

            return cashReceiptApply2InvoiceResponse;
        }
    }
}
