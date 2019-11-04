using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightAPISample.WebApp.Models
{
    public class ExportViewModel
    {
        public string ExportDefinitionName { get; set; } = "";

        public List<MailChimpMemberModel> Contacts { get; set; } = new List<MailChimpMemberModel>();
    }
}
