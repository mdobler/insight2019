using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightAPISample.WebApp.Models
{
    public class InvoiceViewModel
    {
        public string RequestInvoice { get; set; }

        public string Invoice { get; set; }
        public string MainWBS1 { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string MainName { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public decimal TotalInvoiceAmount { get; set; }

        public List<InvoiceSectionViewModel> Sections = new List<InvoiceSectionViewModel>();
    }

    public class InvoiceSectionViewModel
    {
        public string Section { get; set; }
        public decimal BaseAmount {get;set;}
        public decimal FinalAmount { get; set; }

    }
}
