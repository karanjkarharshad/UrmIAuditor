using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrmIAuditor.Models
{
   

        public class AuditSearch
        {
            public int count { get; set; }
            public int total { get; set; }
            public Audit[] audits { get; set; }
        }

        public class Audit
        {
            public string audit_id { get; set; }
            public DateTime modified_at { get; set; }
            public string template_id { get; set; }
        }

    
}