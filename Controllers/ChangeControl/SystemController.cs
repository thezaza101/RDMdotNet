using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDMdotNet.Models;
using Newtonsoft.Json;
using LStoreJSON;

namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        JSONStore js = new JSONStore();
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, js.All<RDSystem>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, js.Single<RDSystem>(id));
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]dynamic value)
        {
            string postedData = value.ToString();
            RDSystem sysData = JsonConvert.DeserializeObject<RDSystem>(postedData);
            sysData.ID = Guid.NewGuid().ToString();
            js.Add(sysData);
            js.SaveChanges();
            return StatusCode(201, sysData);            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]dynamic value)
        {
            RDSystem curData = js.Single<RDSystem>(id);

            string postedData = value.ToString();
            RDSystem newData = JsonConvert.DeserializeObject<RDSystem>(postedData);
            newData.ID = curData.ID;

            js.Add(new Archive(curData, Reason.Update));            
            js.Remove(curData);
            if (newData.Active)
            {
                js.Add(newData);
            }
            js.SaveChanges();           
            
            return StatusCode(202);            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return StatusCode(405);            
        }
    }
}
