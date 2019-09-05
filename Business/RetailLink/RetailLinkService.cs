using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Model.RetailLink;
using Model.Resource.RetailLink;
using DataAccess.Persistence.RetailLink;
using DataAccess.Persistence.UnitOfWork;
using DataAccess.Services;
using Model.Model;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Linq;
using AutoMapper;
using Model.Model.RetailLink.Resource;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;

namespace Business.RetailLink
{
    public class RetailLinkService : IRetailLinkService
    {
        private readonly Config _config;
        private readonly IMapper mapper;
        private readonly IRetailLinkRepository retailLinkRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;

        public RetailLinkService(IMapper mapper,
                                 IOptions<Config> config,
                                 IRetailLinkRepository retailLinkRepository, 
                                 IUnitOfWork unitOfWork,
                                 IUserService userService)
        {
            _config = config.Value;
            this.mapper = mapper;
            this.retailLinkRepository = retailLinkRepository;
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }
        public Task<IEnumerable<RJWLoadsRetailApptMultipleResource>> GetSchedules()
        {
            throw new NotImplementedException();
        }

        public async Task<RetailLinkResponse2> ScheduleAppt(RetailLinkRequest2 request, string userName)
        {
            var retailLinkResponse2 = new RetailLinkResponse2();

            if (!userService.isUserMemberOfGroup(_config.UserServiceConfig.UserRoutes[1].group, userName))
            {
                retailLinkResponse2.isLoggedIn = false;
                retailLinkResponse2.message.Add(String.Format(_config.UserServiceConfig.accountMessages[2], userName, _config.UserServiceConfig.UserRoutes[1].header));
            }

            if (retailLinkResponse2.isLoggedIn)
            {
                if (request.loads == null || request.loads.Count == 0)
                {
                    retailLinkResponse2.isLoggedIn = false;
                    retailLinkResponse2.message.Add(_config.ConfigRetailLink.messages[11]);
                }

                if (retailLinkResponse2.isLoggedIn)
                {
                    var _formData = _config.ConfigRetailLink.formData;
                    _formData[_config.ConfigRetailLink.headers[7]] = request.username;
                    _formData[_config.ConfigRetailLink.headers[8]] = request.password;

                    var cookie_RLESSION = getCookieWithPost(_config.ConfigRetailLink.URLs[0],
                                                        _config.ConfigRetailLink.Cookies,
                                                        _formData);
                    var cookiesBuilder = getCookieWithGet(_config.ConfigRetailLink.URLs[1], cookie_RLESSION);

                    var schedulerToken = getCookieWithGetToken(_config.ConfigRetailLink.URLs[2], cookiesBuilder);

                    if (schedulerToken == null)
                    {
                        // token is not retrieved
                        retailLinkResponse2.isLoggedIn = false;
                        retailLinkResponse2.message.Add(_config.ConfigRetailLink.messages[13]);
                    }

                    if (retailLinkResponse2.isLoggedIn)
                    {
                        cookiesBuilder.AddRange(schedulerToken.cookies);

                        var userResponse = getCookieWithGetUser(String.Format(_config.ConfigRetailLink.URLs[6], schedulerToken.userId), cookiesBuilder, schedulerToken);

                        if (userResponse == null)
                        {
                            // Not able to login
                            retailLinkResponse2.isLoggedIn = false;
                            retailLinkResponse2.message.Add(_config.ConfigRetailLink.messages[5]);
                        }
                        else
                        {
                            // Logged in successfully
                            foreach (var req in request.loads)
                            {
                                var retailLinkResponse = new RetailLinkResponse()
                                {
                                    loads = req.loads
                                };
                                var readytoSchedule = false;
                                //var isPendingLoad = false;
                                //var isAlreadyScheduledLoad = false;
                                var loadsFromSameDc = new List<string>();
                                var numberOfOrdersInLoad = new List<int>();

                                foreach (var load in req.loads)
                                {
                                    // Check if load is in system
                                    var loadsAtSystem = await retailLinkRepository.GetLoadsAsync(String.Format(_config.ConfigRetailLink.loadExistQuery, load));
                                    if (loadsAtSystem != null && loadsAtSystem.Count > 0)
                                    {
                                        readytoSchedule = true;
                                        loadsFromSameDc.Add(Regex.Replace(loadsAtSystem[0].SHIPTOCODE, _config.ConfigRetailLink.fromSameDCPattern, ""));
                                        numberOfOrdersInLoad.Add(loadsAtSystem[0].CNT);
                                    }
                                    else
                                        readytoSchedule = false;

                                    if (!readytoSchedule)
                                    {
                                        // Not ready to be scheduled
                                        retailLinkResponse.isScheduled = readytoSchedule;
                                        retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[0], load));
                                    }
                                }

                                if (!FromSameDC(loadsFromSameDc))
                                {
                                    // Not from the same DC
                                    retailLinkResponse.isScheduled = false;
                                    retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[1], String.Join(",", loadsFromSameDc)));
                                }

                                if (retailLinkResponse.isScheduled)
                                {

                                    foreach (string load in req.loads)
                                    {
                                        var loadIsPending = await retailLinkRepository.GetLoadIsPending(load);
                                        if (loadIsPending != null)
                                        {
                                            // load is pending
                                            retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[3], load));
                                            if (retailLinkResponse.isScheduled)
                                                retailLinkResponse.isScheduled = false;
                                        }
                                    }

                                    if (retailLinkResponse.isScheduled)
                                    {
                                        // Load is already scheduled
                                        foreach (string load in req.loads)
                                        {
                                            var loadAlreadySchduled = await retailLinkRepository.GetLoadAlreadyScheduled(load, _config.ConfigRetailLink.queueStatuses[1]);
                                            if (loadAlreadySchduled != null && req.overrideExisting)
                                            {
                                                // Override existing schedule
                                                loadAlreadySchduled.Status = _config.ConfigRetailLink.queueStatuses[0];
                                                await unitOfWork.CompleteAsync();
                                            }
                                            else if (loadAlreadySchduled != null && !req.overrideExisting)
                                            {
                                                // Schedule exists already
                                                if (retailLinkResponse.isScheduled)
                                                    retailLinkResponse.isScheduled = false;
                                                retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[2], load, loadAlreadySchduled.DeliveryId, loadAlreadySchduled.WalmartEnforcedDate.ToString(_config.ConfigRetailLink.dropDateFormat)));
                                            }
                                        }

                                        if (retailLinkResponse.isScheduled)
                                        {
                                            // Put a request in scheduler                                                                                  
                                            for (var i = 0; i < req.loads.Count; i++)
                                            {                                     
                                                retailLinkRepository.insertLoadToQueueMultipleTranzNumber(req.loads[i], 
                                                                                                          userName, 
                                                                                                          req.loads[0], 
                                                                                                          req.dropDate.ToString(),
                                                                                                          _config.ConfigRetailLink.queueStatuses[3], 
                                                                                                          numberOfOrdersInLoad[i]);
                                                await unitOfWork.CompleteAsync();                                                                                                                                                               
                                            }

                                            // Send a delivery request to Retail Link Scheduler 2.0
                                            retailLinkResponse = await GetScheduleAppt(Int32.Parse(loadsFromSameDc[0]),
                                                                                                req,
                                                                                                retailLinkResponse,
                                                                                                cookiesBuilder,
                                                                                                schedulerToken,
                                                                                                userResponse, 
                                                                                                request.username);
                                            // update queue status
                                            await retailLinkRepository.UpdateLoads(req.loads[req.loads.Count - 1],
                                                                             (retailLinkResponse.isScheduled ? _config.ConfigRetailLink.queueStatuses[1] : _config.ConfigRetailLink.queueStatuses[2]),
                                                                             retailLinkResponse.deliveryId,
                                                                             retailLinkResponse.walmartEnforcedDate,
                                                                             _config.ConfigRetailLink.queueStatuses[3]);
                                            await unitOfWork.CompleteAsync();

                                            // update Synapse if scheduled successfully
                                            if (retailLinkResponse.isScheduled)
                                            {
                                                try
                                                {
                                                    foreach (string load in req.loads)
                                                    {
                                                        retailLinkRepository.executeSP(String.Format(_config.ConfigRetailLink.synapseQuery, load, retailLinkResponse.deliveryId, retailLinkResponse.walmartEnforcedDate, userName));
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[12], retailLinkResponse.deliveryId));
                                                }
                                            }
                                        }
                                    }
                                }

                                // Add individual load result to the main response
                                retailLinkResponse2.loads.Add(retailLinkResponse);
                            }
                        }
                        // Log off main retail link app
                        await getCookieWithLoggOff(_config.ConfigRetailLink.URLs[10], 
                                                   cookiesBuilder);
                        // log scheduler 2.0
                        await getCookieWithLogoutPut(_config.ConfigRetailLink.URLs[11],
                                                     cookiesBuilder,
                                                     schedulerToken,
                                                     userResponse);
                        await getCookieWithLogoutGet(_config.ConfigRetailLink.URLs[12],
                                                     cookiesBuilder,
                                                     schedulerToken,
                                                     userResponse);

                    }

                }
            }
            
            return retailLinkResponse2;
        }

        private bool FromSameDC(List<string> loadsFromSameDc)
        {
            for (int i = 0; i < loadsFromSameDc.Count; i++)
            {
                if (!loadsFromSameDc[0].Equals(loadsFromSameDc[i]))
                    return false;
            }
            return true;
        }

        public async Task<RetailLinkResponse> GetScheduleAppt(int DC, 
                                                              RetailLinkRequest request, 
                                                              RetailLinkResponse retailLinkResponse,
                                                              List<Cookie> cookiesBuilder, 
                                                              RetailLinkSchedulerToken schedulerToken,
                                                              RetailLinkUserResponse userResponse,
                                                              string userID)
        {
            var nodeId = 0;
            if (retailLinkResponse.isScheduled)
            {
                var isDCSupported = false;
                foreach (var buList in userResponse.userbuList)
                {
                    if (buList.buId.ToString().Trim().Equals(_config.ConfigRetailLink.headers[9]))
                    {
                        foreach (var buListNode in buList.nodes)
                        {
                            if (buListNode.nodeName.Trim().Equals(DC.ToString()))
                            {
                                nodeId = Int32.Parse(buListNode.nodeId);
                                isDCSupported = true;
                                break;
                            }
                        }
                    }
                }

                if (!isDCSupported)
                {
                    retailLinkResponse.isScheduled = false;
                    retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[6], DC));
                }
            }

            if (retailLinkResponse.isScheduled)
            {

                List<RJWLoad> POs = new List<RJWLoad>();

                foreach (string load in request.loads)
                {
                    var _POs = await retailLinkRepository.GetPOsAsync(String.Format(_config.ConfigRetailLink.mainQuery, load));
                    // Check if POs are in the existing delivery
                    //if (_POs.Count > 0)
                    //{
                    //    var retailLinkSearchDeliveryRequest = new RetailLinkSearchDeliveryRequest()
                    //    {
                    //        organization = _config.ConfigRetailLink.headers[17],
                    //        userTypeName = schedulerToken.userType,
                    //        poNbr = _POs.ElementAt(_POs.Count - 1).PO
                    //    };
                    //    var retailLinkSearchDeliveryResponses = await getCookieWithSearchDelivery(_config.ConfigRetailLink.URLs[9],
                    //                                                                        cookiesBuilder,
                    //                                                                        retailLinkSearchDeliveryRequest,
                    //                                                                        schedulerToken,
                    //                                                                        userResponse);
                    //    if (retailLinkSearchDeliveryResponses != null && retailLinkSearchDeliveryResponses.Count > 0)
                    //    {
                    //        // Delete pre-existing appointment

                    //    }
                    //}

                    POs.AddRange(_POs);
                }
            
                var POsToValidate = mapper.Map<List<RJWLoad>, List<RetailLinkLoadPOValidateRequest>>(POs);
                foreach(RetailLinkLoadPOValidateRequest po in POsToValidate)
                {
                    po.buId = Int32.Parse(_config.ConfigRetailLink.headers[9]);
                    po.nodeId = DC;
                }
                var retailLinkLoadPOValidateResponses = getCookieWithValidatePO(_config.ConfigRetailLink.URLs[4],
                                                                                cookiesBuilder,
                                                                                POsToValidate,
                                                                                schedulerToken,
                                                                                userResponse);
                var theLoads = String.Join(",", request.loads);
                if (retailLinkLoadPOValidateResponses == null)
                {
                    retailLinkResponse.isScheduled = false;
                    retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[10], theLoads));
                }

                if (retailLinkResponse.isScheduled)
                {
                    var availableWindowsShipments = mapper.Map<List<RetailLinkLoadPOValidateResponse>, List<RetailLinkAvailableAppWindowsRequestShipment>>(retailLinkLoadPOValidateResponses);
                    var availableWindowsRequest = new RetailLinkAvailableAppWindowsRequest
                    {
                        buId = Int32.Parse(_config.ConfigRetailLink.headers[9]),
                        carrierId = _config.ConfigRetailLink.headers[12],
                        deliveryTypeId = Int32.Parse(_config.ConfigRetailLink.headers[10]),
                        nodeId = nodeId,
                        schedulingFlag = _config.ConfigRetailLink.headers[13],
                        shipment = availableWindowsShipments,
                        subnodeId = "",
                        timezoneId = Int32.Parse(_config.ConfigRetailLink.headers[11])
                    };
                    var availableWindowsResponse = getCookieWithAvailableApptWindows(_config.ConfigRetailLink.URLs[7],
                                                                                    cookiesBuilder,
                                                                                    availableWindowsRequest,
                                                                                    schedulerToken,
                                                                                    userResponse);

                    if (availableWindowsResponse == null)
                    {
                        retailLinkResponse.isScheduled = false;
                        retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[9], theLoads));
                    }

                    if (retailLinkResponse.isScheduled)
                    {
                        var theDeliveryDate = AdjustDropDate(request.dropDate, availableWindowsResponse);

                        var deliveryRequestShipment = mapper.Map<List<RetailLinkLoadPOValidateResponse>, List< RetailLinkApptDeliveryRequestShipment>>(retailLinkLoadPOValidateResponses);
                        for (int i = 0; i < deliveryRequestShipment.Count; i++)
                        {
                            deliveryRequestShipment[i].poLoadSequenceNbr = i + 1;
                        }

                        var deliveryRequest = new RetailLinkApptDeliveryRequest();

                        try
                        {                           
                            deliveryRequest = new RetailLinkApptDeliveryRequest
                            {
                                appointmentDate = theDeliveryDate,
                                buId = Int32.Parse(_config.ConfigRetailLink.headers[9]),
                                capacityConfigId = Int32.Parse(_config.ConfigRetailLink.headers[15]),
                                carrierId = _config.ConfigRetailLink.headers[12],
                                delReturns = _config.ConfigRetailLink.headers[14],
                                deliveryStatusId ="",
                                deliveryTypeDets = new RetailLinkApptDeliveryRequestDeliveryTypeDets { schFactorId = Int32.Parse(_config.ConfigRetailLink.headers[10]) },
                                deliveryTypeId = Int32.Parse(_config.ConfigRetailLink.headers[10]),
                                desNodeId = nodeId,
                                destinationNodeDets = new RetailLinkApptDeliveryRequestDestinationNodeDets { nodeName = DC.ToString(), nodeId = nodeId},
                                destinationSubNodeDets = new RetailLinkApptDeliveryRequestDestinationSubNodeDets { },
                                expectedDeliveryDate = DateTime.Parse(availableWindowsResponse.Keys.ElementAt(0)).ToString(_config.ConfigRetailLink.deliveryRequestDateFormat),
                                inventoryTransfer = _config.ConfigRetailLink.headers[14],
                                inventoryTypeId = _config.ConfigRetailLink.headers[16],
                                manageWindowDets = new RetailLinkApptDeliveryRequestManageWindowDets
                                {
                                    apptWindowId = Int32.Parse(_config.ConfigRetailLink.headers[21]),//Int32.Parse(availableWindowsResponse[availableWindowsResponse.Keys.ElementAt(1)][0].apptWindowId),
                                    windowEndTime = _config.ConfigRetailLink.headers[23],//availableWindowsResponse[availableWindowsResponse.Keys.ElementAt(1)][0].apptWindow.Substring(availableWindowsResponse[availableWindowsResponse.Keys.ElementAt(1)][0].apptWindow.IndexOf(_config.ConfigRetailLink.headers[18]) + 2).Trim(),
                                    windowStartTime = _config.ConfigRetailLink.headers[22]//availableWindowsResponse[availableWindowsResponse.Keys.ElementAt(1)][0].apptWindow.Substring(0, availableWindowsResponse[availableWindowsResponse.Keys.ElementAt(1)][0].apptWindow.IndexOf(_config.ConfigRetailLink.headers[18])).Trim()
                                },
                                paymentTermId = "",
                                poCutOff = DateTime.Parse(availableWindowsResponse.Keys.ElementAt(0)).ToString(_config.ConfigRetailLink.deliveryRequestDateFormat),
                                recurringDeliveryIndicator = _config.ConfigRetailLink.headers[14],
                                scacCarrierID = _config.ConfigRetailLink.headers[17],
                                shipment = deliveryRequestShipment
                            };
                        }
                        catch (Exception ex)
                        {
                            
                        }

                        var deliveryResponse = getCookieWithDelivery(_config.ConfigRetailLink.URLs[8],
                                                                                        cookiesBuilder,
                                                                                        deliveryRequest,
                                                                                        schedulerToken,
                                                                                        userResponse);
                        if (deliveryResponse == null)
                        {
                            retailLinkResponse.isScheduled = false;
                            retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[7], theLoads));
                        }
                        else
                        {
                            // Successfully scheduled
                            retailLinkResponse.deliveryId = deliveryResponse.deliveryNumber;
                            retailLinkResponse.walmartEnforcedDate = DateTime.Parse(deliveryResponse.appointment).ToString(_config.ConfigRetailLink.dropDateFormat);
                            retailLinkResponse.message.Add(String.Format(_config.ConfigRetailLink.messages[8], theLoads, deliveryResponse.deliveryNumber, DateTime.Parse(deliveryResponse.appointment).ToString(_config.ConfigRetailLink.dropDateFormat)));
                        }
                    }
                }
            }               
            return retailLinkResponse;
        }
    
        private RetailLinkApptDeliveryResponse getCookieWithDelivery(string url,
                                                                               List<Cookie> Cookies,
                                                                               RetailLinkApptDeliveryRequest retailLinkDeliveryRequest,
                                                                               RetailLinkSchedulerToken schedulerToken,
                                                                               RetailLinkUserResponse userResponse)
        {
            //var userResponse = new RetailLinkUserResponse();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], userResponse.organization);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);               
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                var result = client.PostAsync(baseAddress,
                                              new StringContent(JsonConvert.SerializeObject(retailLinkDeliveryRequest),
                                                                Encoding.UTF8, _config.ConfigRetailLink.headers[6])
                                              ).Result;
                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var retailLinkApptDeliveryResponse = JsonConvert.DeserializeObject<RetailLinkApptDeliveryResponse>(result.Content.ReadAsStringAsync().Result);
                        return retailLinkApptDeliveryResponse;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return null;
            }
        }

        private async Task <List<RetailLinkSearchDeliveryResponse>> getCookieWithSearchDelivery(string url,
                                                                               List<Cookie> Cookies,
                                                                               RetailLinkSearchDeliveryRequest retailLinkSearchDeliveryRequest,
                                                                               RetailLinkSchedulerToken schedulerToken,
                                                                               RetailLinkUserResponse userResponse)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://retaillink2.wal-mart.com/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
            client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);            
            client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
            var jsonContent = JsonConvert.SerializeObject(retailLinkSearchDeliveryRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "ILP2/core-api/rest/delivery/searchDeliveries");
            request.Content = new StringContent(jsonContent,
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            HttpResponseMessage myResult;

            try
            {

            await client.SendAsync(request)
                  .ContinueWith(responseTask =>
                  {
                      var result = responseTask.Result;
                      myResult = result;
                  });
            }
            catch (Exception ex)
            {

                
            }

            //return null;
            //var userResponse = new RetailLinkUserResponse();

            //using (var client = new HttpClient())
            //{              

            //    var myContent = JsonConvert.SerializeObject(retailLinkSearchDeliveryRequest);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);

            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    var builder = new UriBuilder(new Uri(url));

            //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);
            //    request.Content = new StringContent(myContent);//CONTENT-TYPE header
            //    request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            //    HttpResponseMessage response = client.SendAsync(request).Result;

            //}

            //try
            //{
            //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //    httpWebRequest.ContentType = "application/json";
            //    httpWebRequest.Method = "POST";
            //    httpWebRequest.Headers.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
            //    httpWebRequest.Headers.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
            //    httpWebRequest.Headers.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);

            //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //    {
            //        var myContent = JsonConvert.SerializeObject(retailLinkSearchDeliveryRequest);

            //        streamWriter.Write(myContent);
            //    }

            //    using (Stream s = httpWebRequest.GetResponse().GetResponseStream())
            //    {
            //        using (StreamReader sr = new StreamReader(s))
            //        {
            //            var jsonResponse = sr.ReadToEnd();                        
            //        }
            //    }                   

            //}
            //catch (Exception ex)
            //{

            //}

            return null;
            //var baseAddress = new Uri(url);
            //var cookieContainer = new CookieContainer();
            //using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            //using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            //{
            //    //client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[19], _config.ConfigRetailLink.headers[20]);
            //    //client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[20], _config.ConfigRetailLink.headers[6]);
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_config.ConfigRetailLink.headers[6]));
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], userResponse.organization);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
            //    client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);
            //    //client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            //    //foreach (Cookie cookie in Cookies)
            //    //{
            //    //    cookieContainer.Add(baseAddress, cookie);
            //    //}

            //    var jsonContent = JsonConvert.SerializeObject(retailLinkSearchDeliveryRequest);
            //    using (var content = new StringContent(jsonContent,Encoding.UTF8, _config.ConfigRetailLink.headers[6]))
            //    {
            //        //content.Headers.ContentType.CharSet = "";
            //        var result = client.PostAsync(baseAddress,content).Result;

            //        if (result.IsSuccessStatusCode)
            //        {
            //            try
            //            {
            //                var retailLinkSearchDeliveryResponse = JsonConvert.DeserializeObject<List<RetailLinkSearchDeliveryResponse>>(result.Content.ReadAsStringAsync().Result);
            //                return retailLinkSearchDeliveryResponse;
            //            }
            //            catch (Exception ex)
            //            {

            //            }
            //        }

            //    }

            //    return null;
            //}


        }

        private Dictionary<string, List<RetailLinkAvailableAppWindowsResponseDetail>> getCookieWithAvailableApptWindows(string url,
                                                                               List<Cookie> Cookies,
                                                                               RetailLinkAvailableAppWindowsRequest retailLinkAvailableAppWindowsRequests,
                                                                               RetailLinkSchedulerToken schedulerToken,
                                                                               RetailLinkUserResponse userResponse)
        {
            //var userResponse = new RetailLinkUserResponse();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], userResponse.organization);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                var result = client.PostAsync(baseAddress,
                                              new StringContent(JsonConvert.SerializeObject(retailLinkAvailableAppWindowsRequests),
                                                                Encoding.UTF8, _config.ConfigRetailLink.headers[6])
                                              ).Result;
                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var retailLinkAvailableAppWindowsResponse = JsonConvert.DeserializeObject<Dictionary<string, List<RetailLinkAvailableAppWindowsResponseDetail>>>(result.Content.ReadAsStringAsync().Result);                
                        return retailLinkAvailableAppWindowsResponse;
                    }
                    catch (Exception ex)
                    {
                     
                    }
                }
                return null;
            }
        }

        private string AdjustDropDate(DateTime theDate, Dictionary<string, List<RetailLinkAvailableAppWindowsResponseDetail>> dates)
        {
            var pickedDate = theDate;
            var dateFound = false;

            foreach (string date in dates.Keys)
            {
                if (DateTime.Parse(date) > theDate)
                {
                    pickedDate = DateTime.Parse(date);
                    dateFound = true;
                    break;
                }

                if (DateTime.Parse(date) == theDate)
                {
                    pickedDate = DateTime.Parse(date);
                    dateFound = true;
                    break;
                }
            }

            if (!dateFound)
                pickedDate = DateTime.Parse(dates.Keys.ElementAt(dates.Count - 1));

            return pickedDate.ToString(_config.ConfigRetailLink.deliveryRequestDateFormat);
        }
        private List<RetailLinkLoadPOValidateResponse> getCookieWithValidatePO(string url, 
                                                                               List<Cookie> Cookies, 
                                                                               List<RetailLinkLoadPOValidateRequest> retailLinkLoadPOValidateRequests, 
                                                                               RetailLinkSchedulerToken schedulerToken, 
                                                                               RetailLinkUserResponse userResponse)
        {
            //var userResponse = new RetailLinkUserResponse();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], userResponse.organization);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                var result = client.PostAsync(baseAddress,
                                              new StringContent(JsonConvert.SerializeObject(retailLinkLoadPOValidateRequests), 
                                                                Encoding.UTF8, _config.ConfigRetailLink.headers[6])
                                              ).Result;
                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var retailLinkLoadPOValidateResponses = JsonConvert.DeserializeObject<List<RetailLinkLoadPOValidateResponse>>(result.Content.ReadAsStringAsync().Result);
                        return retailLinkLoadPOValidateResponses;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                return null;
            }
        }

        private RetailLinkUserResponse getCookieWithGetUser(string url, List<Cookie> Cookies, RetailLinkSchedulerToken schedulerToken)
        {
            //var userResponse = new RetailLinkUserResponse();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], "");
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                var result = client.GetAsync(baseAddress).Result;
                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var userResponse = JsonConvert.DeserializeObject<RetailLinkUserResponse>(result.Content.ReadAsStringAsync().Result);
                        return userResponse;
                    }
                    catch (Exception ex)
                    {

                    }
                }    
                return null;
            }
        }

        private RetailLinkSchedulerToken getCookieWithGetToken(string url, List<Cookie> Cookies)
        {
            var schedulerToken = new RetailLinkSchedulerToken();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                // var uri = new Uri(result.RequestMessage.RequestUri.AbsoluteUri);
                try
                {
                    var result = client.GetAsync(baseAddress).Result;
                    var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(result.RequestMessage.RequestUri.AbsoluteUri);
                    schedulerToken.token = queryDictionary[_config.ConfigRetailLink.tokenParams[0]];
                    schedulerToken.securityID = queryDictionary[_config.ConfigRetailLink.tokenParams[1]];
                    schedulerToken.userId = queryDictionary[_config.ConfigRetailLink.tokenParams[2]];
                    schedulerToken.userFirstName = queryDictionary[_config.ConfigRetailLink.tokenParams[3]];
                    schedulerToken.userLastName = queryDictionary[_config.ConfigRetailLink.tokenParams[4]];
                    schedulerToken.userType = queryDictionary[_config.ConfigRetailLink.tokenParams[5]];
                    schedulerToken.loginId = queryDictionary[_config.ConfigRetailLink.tokenParams[6]];
                    schedulerToken.langCode = queryDictionary[_config.ConfigRetailLink.tokenParams[7]];
                    schedulerToken.cookies = cookieContainer.GetCookies(baseAddress).Cast<Cookie>().ToList();
                    return schedulerToken;
                }
                catch (Exception ex)
                {
                 
                }
                return null;
            }
        }

        private List<Cookie> getCookieWithGet(string url, List<Cookie> Cookies)
        {
            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {                
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                var result = client.GetAsync(baseAddress).Result;

                var cookies = cookieContainer.GetCookies(baseAddress).Cast<Cookie>().ToList();
                return cookies;
            }
        }

        private List<Cookie> getCookieWithPost(string url, 
                                               Dictionary<string, string> Cookies,
                                               Dictionary<string, string> formData)
        {
            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var content = new FormUrlEncodedContent(formData);
                foreach (string key in Cookies.Keys)
                {
                    cookieContainer.Add(baseAddress, new Cookie(key, Cookies[key]));
                }

                var result = client.PostAsync(baseAddress, content).Result;

                var cookies = cookieContainer.GetCookies(baseAddress).Cast<Cookie>().ToList();
                return cookies;
            }
        }

        private async Task getCookieWithLoggOff(string url, List<Cookie> Cookies)
        {
            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }
                try
                {
                    var result = await client.GetAsync(baseAddress);
                }
                catch (Exception)
                {
                    
                }                             
            }
        }

        private async Task getCookieWithLogoutPut(string url,
                                                  List<Cookie> Cookies,                                                                          
                                                  RetailLinkSchedulerToken schedulerToken,
                                                  RetailLinkUserResponse userResponse)
        {
            //var userResponse = new RetailLinkUserResponse();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], userResponse.organization);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                try
                {
                    var result = await client.PutAsync(baseAddress, null);                
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private async Task getCookieWithLogoutGet(string url,
                                                  List<Cookie> Cookies,
                                                  RetailLinkSchedulerToken schedulerToken,
                                                  RetailLinkUserResponse userResponse)
        {
            //var userResponse = new RetailLinkUserResponse();

            var baseAddress = new Uri(url);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[0], schedulerToken.langCode);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[1], userResponse.organization);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[2], schedulerToken.loginId);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[3], schedulerToken.securityID);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[4], schedulerToken.token);
                client.DefaultRequestHeaders.Add(_config.ConfigRetailLink.headers[5], schedulerToken.userType);
                foreach (Cookie cookie in Cookies)
                {
                    cookieContainer.Add(baseAddress, cookie);
                }

                try
                {
                    var result = await client.GetAsync(baseAddress);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
