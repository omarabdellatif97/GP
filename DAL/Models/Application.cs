using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        virtual public ICollection<Case> Cases { get; set; } = new HashSet<Case>();
        
        [NotMapped]
        [JsonIgnore]
        public bool Serialize { get; set; } = false;
        public bool ShouldSerializeCases()
        {
            return Serialize;
        }
    }
}
