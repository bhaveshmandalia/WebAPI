using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokensApp.Models
{
    public class tsRequest
    {
        public credentials credentials { get; set; }
        public site site { get; set; }
    }

    public class credentials
    {
        public string name { get; set; }
        public string password { get; set; }
    }

    public class site
    {
        public string contentUrl { get; set; }
    }
}