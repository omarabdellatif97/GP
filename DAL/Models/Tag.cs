using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    //[Owned]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CaseId { get; set; }
        virtual public Case Case { get; set; }
    }
}
