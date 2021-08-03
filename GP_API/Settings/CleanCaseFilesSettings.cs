using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Settings
{
    public class CleanCaseFilesSettings
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool EnableService { get; set; }
        
        /// <summary>
        /// the number of hours that case file can be deleted after
        /// </summary>
        public int MaxCaseFilesHours { get; set; }
    }
}
