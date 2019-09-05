using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Model.Model.RetailLink;
using Model.Model.RetailLink.Resource;
using Model.Resource.RetailLink;

namespace Business.RetailLink
{
    public interface IRetailLinkService
    {
        Task<RetailLinkResponse2> ScheduleAppt(RetailLinkRequest2 request, string userName);
        Task<IEnumerable<RJWLoadsRetailApptMultipleResource>> GetSchedules();
        Task<RetailLinkResponse> GetScheduleAppt(int DC,
                                                 RetailLinkRequest request,
                                                 RetailLinkResponse retailLinkResponse,
                                                 List<Cookie> cookiesBuilder,
                                                 RetailLinkSchedulerToken schedulerToken,
                                                 RetailLinkUserResponse userResponse,
                                                 string userID);
    }
}
