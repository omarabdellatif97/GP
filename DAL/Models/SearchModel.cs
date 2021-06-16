using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SearchModel
    {
        public ICollection<string> Tags { get; set; }
        public string Name { get; set; }
        public string Application { get; set; }
    }
}
