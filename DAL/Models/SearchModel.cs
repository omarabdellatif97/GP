using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SearchModel
    {
        public ICollection<String> Tags { get; set; }
        public ICollection<String> Applications { get; set; }
        public string Title { get; set; }
        public int? PageNum { get; set; }
        public int? PageCnt { get; set; }
    }
}
