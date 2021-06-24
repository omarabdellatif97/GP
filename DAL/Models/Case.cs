using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Case
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        
        [ForeignKey("PostedBy")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        virtual public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        virtual public ICollection<Step> Steps { get; set; } = new HashSet<Step>();
        virtual public ICollection<CaseFile> CaseFiles { get; set; } = new HashSet<CaseFile>();
        virtual public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        
    }
}
