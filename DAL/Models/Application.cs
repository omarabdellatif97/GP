using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Case> Cases { get; set; }
    }
}
