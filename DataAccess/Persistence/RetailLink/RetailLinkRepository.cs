using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Model.RetailLink;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Persistence.RetailLink
{
    public class RetailLinkRepository : IRetailLinkRepository
    {
        private readonly RJWDataDbContext context;

        public RetailLinkRepository(RJWDataDbContext context)
        {
            this.context = context;
        }
        public void Add(RJWLoadsRetailApptMultipleRJWTool retailLink)
        {
            context.RJWLoadsRetailApptMultipleRJWTools.Add(retailLink);
        }

        public async Task<List<RetailLinkLoad>> GetLoadsAsync(string query)
        {
            var loads = await context.retailLinkLoads.FromSql(query).ToListAsync();
            return loads;          
        }

        public async Task<List<RJWLoad>> GetPOsAsync(string query)
        {
            try
            {
                var POs = await context.rjwLoads.FromSql(query).ToListAsync();
                return POs;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<RJWLoadsRetailApptMultipleRJWTool> GetLoadIsPending(string load)
        {
            var loadIsPending = await context.RJWLoadsRetailApptMultipleRJWTools
                                        .Where(l => l.LOADNO.ToString().Equals(load) && (String.IsNullOrEmpty(l.Status) || l.Status.Equals("P")))
                                        .OrderByDescending(x => x.EntryDate).ToListAsync();
            return loadIsPending.Count > 0 ? loadIsPending[0] : null;
        }

        public async Task<RJWLoadsRetailApptMultipleRJWTool> GetLoadAlreadyScheduled(string load, string status)
        {
            try
            {
                var loadAlreadyScheduled = await context.RJWLoadsRetailApptMultipleRJWTools
                                                    .Where(l => l.LOADNO == Convert.ToDecimal(load) && l.Status == status)
                                                    .OrderByDescending(x => x.EntryDate).ToListAsync();
                return loadAlreadyScheduled.Count > 0 ? loadAlreadyScheduled[0]: null;

            }
            catch (Exception ex)
            {

                throw;
            }            
        }

        public async Task<bool> UpdateLoads(string refLoadNo, 
                                            string status, 
                                            string deliveryId,
                                            string walmartEnforcedDate,
                                            string existingStatus)
        {
            bool result = false;
            try
            {
                var latestLoads = await context.RJWLoadsRetailApptMultipleRJWTools
                  .Where(l => (l.Status == existingStatus || l.Status == null)&& l.RefLOADNO == Convert.ToDecimal(refLoadNo))
                  .OrderBy(x => x.LOADNO).ToListAsync();

                foreach (var load in latestLoads)
                {
                    load.Status = status;
                    if(walmartEnforcedDate.Length > 0)
                    load.WalmartEnforcedDate = DateTime.Parse(walmartEnforcedDate);
                    if (deliveryId.Length > 0)
                        load.DeliveryId = deliveryId;
                }

                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;                             
        }       

        public void insertLoadToQueueMultipleTranzNumber(string loadNo, 
                                                         string windowsUser,                                                                
                                                         string refLoadNo, 
                                                         string dropDate, 
                                                         string Status, 
                                                         int numberOfOrdersInLoad)
        {
            var rjwLoadsRetailApptMultiple = new RJWLoadsRetailApptMultipleRJWTool
            {
                USERID = windowsUser,
                LOADNO = Convert.ToDecimal(loadNo),
                DeliveryDate = DateTime.Parse(dropDate),
                RefLOADNO = Convert.ToDecimal(refLoadNo),
                EntryDate = DateTime.Now,
                NumberofOrdersByUser = Convert.ToDecimal(numberOfOrdersInLoad),
                Status = Status
            };
            
            context.RJWLoadsRetailApptMultipleRJWTools.Add(rjwLoadsRetailApptMultiple);
        }


        public async Task<RJWLoadsRetailApptMultipleRJWTool> GetRetailLink(int id)
        {
            return await context.RJWLoadsRetailApptMultipleRJWTools.FindAsync(id);
        }

        public void Remove(RJWLoadsRetailApptMultipleRJWTool retailLink)
        {
            context.Remove(retailLink);
        }

        public void executeSP(string query)
        {
            context.Database.ExecuteSqlCommand(query);
        }
    }
}
