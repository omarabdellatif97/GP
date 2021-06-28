using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Settings
{
    public class DirectoryPerCaseServiceSettings
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool EnableService { get; set; }
    }
}
