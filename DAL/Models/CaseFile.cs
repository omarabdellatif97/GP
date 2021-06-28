using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CaseFile
    {
        public int Id { get; set; }
        
        [JsonIgnore]
        public string FileURL { get; set; }

        [NotMapped]
        public string URL { get; set; }
        
        public string ContentType { get; set; }
        
        // name with extension
        public string FileName { get; set; }
        
        // extension
        public string Extension { get; set; }

        public long? FileSize { get; set; }


        public bool IsDescriptionFile { get; set; }

        public DateTime PublishDate { get; set; }

        public int? CaseId { get; set; }//nullable
        
        virtual public Case Case { get; set; }
    }
}
