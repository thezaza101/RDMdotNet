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
using System.ComponentModel.DataAnnotations;


namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        JSONStore js = new JSONStore(Environment.GetEnvironmentVariable("LStoreData"));
        // GET api/values
        [HttpGet]
        public IActionResult Get(bool list)
        {
            /*List<Service> lstService = new List<Service>();
            string[] serviceIds = new string[]{
                "5b3eb47b7d0e99000457ffa6",
                "5b3eb4917d0e99000457ffa8",
                "5b3eb4a47d0e99000457ffaa",
                "5b3eb4857d0e99000457ffa7",
                "5b3eb49c7d0e99000457ffa9",
                "5b3eb4707d0e99000457ffa5"};
                HttpClient _client = new HttpClient();
                _client.BaseAddress = new Uri("https://service-catalogue-repository.herokuapp.com/service/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                Service ser ;
                foreach (string s in serviceIds)
                {
                    string data = _client.GetAsync(s).Result.Content.ReadAsStringAsync().Result;
                    ser = JsonConvert.DeserializeObject<Service>(data);
                    ser.id = s;
                    lstService.Add(ser);
                }
                foreach (Service s in lstService)
                {
                    js.Add(s);
                }
                js.SaveChanges(); */
            

            return StatusCode(200, js.All<Service>());
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Service v = js.Single<Service>(id);

            var vx = new {
                name = v.name,
                description = v.description,
                pages = v.pages
            };
            return StatusCode(200, vx);
        }

        private class Service
        {
            [Key]
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public List<string> pages { get; set; }
        }
    }
}
