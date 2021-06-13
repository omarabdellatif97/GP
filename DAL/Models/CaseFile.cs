﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CaseFile
    {
        public int Id { get; set; }
        public string FileURL { get; set; }
        public string ContentType { get; set; }
        public long? FileSize { get; set; }
        public int CaseId { get; set; }
        virtual public Case Case { get; set; }
    }
}
