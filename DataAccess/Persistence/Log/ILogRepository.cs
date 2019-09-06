using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model.Model.Log;

namespace DataAccess.Persistence.Log
{
    public interface ILogRepository
    {
        Task<tblProcessErrorLog> GetLog(int id);
        void Add(tblProcessErrorLog log);
        void Remove(tblProcessErrorLog log);
    }
}
