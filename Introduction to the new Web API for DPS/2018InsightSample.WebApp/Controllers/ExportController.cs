using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InsightAPISample.WebApp.Controllers
{
    public class ExportController : Controller
    {
        private readonly Repositories.IRepository _repository;

        public ExportController(Repositories.IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new Models.ExportViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(Models.ExportViewModel model)
        {
            if (string.IsNullOrEmpty(model?.ExportDefinitionName))
            { 
                //return error
            }

            var authClient = await _repository.GetAuthorizedVantagePointClientAsync();
            string requestUri = $"exportutility/exportdefinitions/{model.ExportDefinitionName}";

            var contacts = await _repository.GetAsync(authClient, requestUri);

            var mailchimpmembersTyped = await _repository.GetAsync<List<Models.MailChimpMemberModel>>(authClient, requestUri);

            //remove contacts without email
            mailchimpmembersTyped.RemoveAll(x => string.IsNullOrEmpty(x.EMailAddress));

            model.Contacts = mailchimpmembersTyped;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SendToMailChimp(string exportDefinitionName)
        {
            var authClient = await _repository.GetAuthorizedVantagePointClientAsync();
            string requestUri = $"exportutility/exportdefinitions/{exportDefinitionName}";

            var mailchimpmembersTyped = await _repository.GetAsync<List<Models.MailChimpMemberModel>>(authClient, requestUri);
            mailchimpmembersTyped.RemoveAll(x => string.IsNullOrEmpty(x.EMailAddress));


            var mcRepository = new Repositories.MailChimpRepository();
            var message = await mcRepository.AddSubscribers("Insight Test", mailchimpmembersTyped);

            return View("Success", message);
        }

    }
}