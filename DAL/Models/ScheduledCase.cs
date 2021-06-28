using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ScheduledCase
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public Case Case { get; set; }
    }
}
