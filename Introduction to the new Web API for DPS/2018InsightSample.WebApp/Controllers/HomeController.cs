using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InsightAPISample.WebApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InsightAPISample.WebApp.Models;
using System.Diagnostics;

namespace InsightAPISample.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repositories.IRepository _repository;

        public HomeController(Repositories.IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(Models.ContactUsModel model)
        {
            var authClient = await _repository.GetAuthorizedVantagePointClientAsync();

            //set up the query string to post contacts:
            string requestUri = $"contact";

            //this only works because the model and the contact class in VantagePoint
            //share the same names!
            var result = await _repository.PostAsync(authClient, requestUri, model);
            
            return RedirectToAction("ThankYou", model);
        }

        public IActionResult ThankYou(Models.ContactUsModel model)
        {
            ViewData["FirstName"] = model.FirstName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
