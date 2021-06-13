using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
    public class FTPServerSettings : IFTPServerSettings
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password {get; set;}
    }

    
}
