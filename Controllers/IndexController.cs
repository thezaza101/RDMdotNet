using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDMdotNet.Models;
using Newtonsoft.Json;
using LStoreJSON;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class IndexController : Controller
    {
        JSONStore js = new JSONStore(Environment.GetEnvironmentVariable("LStoreData"));
        // GET api/values
        [HttpGet]
        public IActionResult Get(bool list)
        {
            /*HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("https://service-catalogue-repository.herokuapp.com/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            string data = _client.GetAsync("index").Result.Content.ReadAsStringAsync().Result;
            RootObject d = JsonConvert.DeserializeObject<RootObject>(data);
            js.Add(d);
            js.SaveChanges();
            return StatusCode(200,d);*/
            return StatusCode(200,js.All<RootObject>().First());            
        }

        private class Content
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        private class RootObject
        {
            public List<Content> content { get; set; }
        }
    }
}
