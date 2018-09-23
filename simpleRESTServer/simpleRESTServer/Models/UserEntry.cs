using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simpleRESTServer
{
    public class UserEntry
    {
        public int Id { get; set; }
        public string Category { get; set; }        
        public string Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}