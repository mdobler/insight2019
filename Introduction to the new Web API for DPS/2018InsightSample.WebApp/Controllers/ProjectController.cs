using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InsightAPISample.WebApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InsightAPISample.WebApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly Repositories.IRepository _repository;

        //initializes class and gets the repository (dependency injection)
        public ProjectController(Repositories.IRepository repository)
        {
            _repository = repository;
        }

        // GET: returns a list of projects - should implement carrousel view for this
        public ActionResult Index()
        {
            return View();
        }

        // GET: Project/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }


        public async Task<ActionResult> TopTen()
        {
            var authClient = await _repository.GetAuthorizedVantagePointClientAsync();

            //set up the query string to return projects:
            string requestUri = $"project?limit=10";
            //only display top level projects
            requestUri += $"&wbstype=wbs1";
            //add a fieldFilter to the query string so the API only returns fields that are needed            
            requestUri += $"&{RESTHelper.GetFieldFilterParamString(new string[] {"WBS1", "Name", "LongName" })}";
            //add a search to it
            List<Helpers.FilterHash> searchItems = new List<FilterHash>() {
                new Helpers.FilterHash() {name="ChargeType", value="R",
                    tablename ="PR", opp="=", searchlevel=1 },   //regular projects only
                new Helpers.FilterHash() {name="ProjectType", value="07",
                    tablename ="PR", opp="=", searchlevel=1 }  //only items with project type 07
            };
            requestUri += RESTHelper.GetSearchFilterParamString(searchItems);

            //call with dynamic type (returns JObject)
            var TopTenList = await _repository.GetAsync(authClient, requestUri);

            //if you build a model that matches the expected result then it can be cast automatically
            var TopTenListTyped = await _repository.GetAsync<List<Models.ProjectViewModel>>(authClient, requestUri);

            return View(TopTenListTyped);
        }


        
    }
}