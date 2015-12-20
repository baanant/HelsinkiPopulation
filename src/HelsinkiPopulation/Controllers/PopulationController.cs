
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using HelsinkiPopulation.Utilities;
using HelsinkiPopulation.ViewModels;

namespace HelsinkiPopulation.Controllers
{
    /// <summary>
    /// NOTICE! Due to description of the assignment I decided to create additional API methods to handle the dev.helsinki.fi API calls.
    /// In real life application I would have probably handled the api data directly in AngularJS.
    /// </summary>
    [Route("api/population")]
    public class PopulationController : Controller
    {

        /// <summary>
        /// Fetches the population data to be shown as a diagram.
        /// NOTICE! Exception handling is still missing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<JsonResult> ShowPopulation(int? id)
        {
            //TODO! Add here the condition where the id is null. In that case check if there is a value in the cache. 
            if (id != null)
            {
                var data = await GetData((int)id);
                return Json(data);
            }
            return null;
        }


        /// <summary>
        /// Fetches the year data to be shown in the client dropdown menu.
        /// NOTICE! Exception handling is still missing!
        /// </summary>
        /// <returns></returns>
        [HttpGet("years")]
        public async Task<JsonResult> GetAvailableYears()
        {
            List<YearData> yearData = new List<YearData>();
            var years = await GetYears();
            foreach (var year in years)
            {
                yearData.Add(new YearData() {Id = year.Key, Year = year.Value});
            }
            return Json(yearData);
        }

        /// <summary>
        /// Call the dev.helsinki.fi API and parse the relevant population data for the client.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static async Task<Dictionary<string, string>> GetData(int year)
        {
            var apiAddress = Startup.Configuration["ApiAddresses:Population"];
            using (var client = new HttpClient())
            {
                var address = String.Format(apiAddress, year);
                InitializeClient(client,address);

                //HTTP GET
                var response = await client.GetStringAsync("");
                JObject result = JObject.Parse(response);

                //Parse data.
                var resultDictionary = JSonParser.ParsePopulationData(result);
                return resultDictionary;
            }
        }

        /// <summary>
        /// Call the dev.helsinki.fi API and parse the relevant year data for the client.
        /// </summary>
        /// <returns></returns>
        private static async Task<Dictionary<int,int>> GetYears()
        {
  
            var apiAddress = Startup.Configuration["ApiAddresses:Years"];
            using (var client = new HttpClient())
            {
                InitializeClient(client, apiAddress);

                //HTTP GET
                var response = await client.GetStringAsync("");
                JObject result = JObject.Parse(response);

                //Parse data.
                return JSonParser.ParseYearData(result);
            }

        }

     
        /// <summary>
        /// Initializes the HttpClient instance for API calls.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        private static void InitializeClient(HttpClient client, string uri)
        {
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
