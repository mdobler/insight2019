using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsightAPISample.WebApp.Helpers;


namespace InsightAPISample.WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly Repositories.IRepository _repository;

        //initializes class and gets the repository (dependency injection)
        public InvoiceController(Repositories.IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new Models.InvoiceViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(Models.InvoiceViewModel model)
        {
            var authClient = await _repository.GetAuthorizedVantagePointClientAsync();

            string requestUri = $"utilities/invokecustom/getinvoiceinfo";
            Dictionary<string, object> spParams = new Dictionary<string, object>();
            spParams.Add("Invoice", model.RequestInvoice);

            //returns a structure in xml
            string invoiceInfo = await _repository.PostAsync<string>(authClient, requestUri, spParams);

            //turn structure into json
            var retvalJson = Helpers.XMLHelpers.StoredProcXMLToJObject(invoiceInfo);

            //turn structure into dicts
            var retvalDict = Helpers.XMLHelpers.StoredProcXMLToDictionary(invoiceInfo);

            //populating the model the hard way
            //with this code you have to know exactly what the stored procedure returns...
            model.Invoice = retvalDict["Table"][0]["Invoice"].ToString();
            model.MainWBS1 = retvalDict["Table"][0]["MainWBS1"].ToString();
            model.InvoiceDate = DateTime.Parse(retvalDict["Table"][0]["InvoiceDate"].ToString());
            model.MainName = retvalDict["Table"][0]["MainName"].ToString();
            model.Description = retvalDict["Table"][0]["Description"].ToString();
            model.ProjectName = retvalDict["Table"][0]["ProjectName"].ToString();
            model.ClientName = retvalDict["Table"][0]["ClientName"].ToString();

            foreach (var item in retvalDict["Table1"])
            {
                Models.InvoiceSectionViewModel section = new Models.InvoiceSectionViewModel();
                section.Section = item["section"].ToString();
                section.BaseAmount = Decimal.Parse(item["BaseAmount"].ToString());
                section.FinalAmount = Decimal.Parse(item["FinalAmount"].ToString());
                model.Sections.Add(section);
            }

            model.TotalInvoiceAmount = model.Sections.Sum(x => x.FinalAmount);

            return View(model);
        }
    }
}