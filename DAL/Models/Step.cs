using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string StepText { get; set; }
        public int CaseId { get; set; }
        virtual public Case Case { get; set; }
    }
}
