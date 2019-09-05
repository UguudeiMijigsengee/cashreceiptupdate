using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Model.RetailLink;

namespace DataAccess.Persistence.RetailLink
{
    public interface IRetailLinkRepository
    {
        Task<RJWLoadsRetailApptMultipleRJWTool> GetRetailLink(int id);
        void Add(RJWLoadsRetailApptMultipleRJWTool retailLink);
        void Remove(RJWLoadsRetailApptMultipleRJWTool retailLink);
        Task<List<RetailLinkLoad>> GetLoadsAsync(string query);
        Task<List<RJWLoad>> GetPOsAsync(string query);
        Task<RJWLoadsRetailApptMultipleRJWTool> GetLoadIsPending(string load);
        Task<RJWLoadsRetailApptMultipleRJWTool> GetLoadAlreadyScheduled(string load, string status);
        Task<bool> UpdateLoads(string refLoadNo,
                                            string status,
                                            string deliveryId,
                                            string walmartEnforcedDate,
                                            string existingStatus);        
        void insertLoadToQueueMultipleTranzNumber(string loadNo,
                                                         string windowsUser,
                                                         string refLoadNo,
                                                         string dropDate,
                                                         string Status,
                                                         int numberOfOrdersInLoad);
        void executeSP(string query);
    }
}
