using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateConfig
    {
        public List<string> mainStoredProcedures { get; set; }
        public List<string> errorLogTables { get; set; }
        public List<string> successLogTables { get; set; }
        public List<string> successLogTableMessages { get; set; }
        public List<string> mergeHelper { get; set; }

    }
}
