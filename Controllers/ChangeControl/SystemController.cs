using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDMdotNet.Models;
using Newtonsoft.Json;

namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, IO.All<RDSystem>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, IO.Single<RDSystem>(id));
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]dynamic value)
        {
            string postedData = value.ToString();
            RDSystem sysData = JsonConvert.DeserializeObject<RDSystem>(postedData);
            sysData.ID = Guid.NewGuid().ToString();
            IO.Add(sysData);
            IO.SaveChanges();
            return StatusCode(201, sysData);            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]dynamic value)
        {
            RDSystem curData = IO.Single<RDSystem>(id);

            string postedData = value.ToString();
            RDSystem newData = JsonConvert.DeserializeObject<RDSystem>(postedData);
            newData.ID = curData.ID;

            IO.Remove(curData);
            IO.Add(newData);
            IO.SaveChanges();           
            
            return StatusCode(202);            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            IO.Remove(new RDSystem(){ID = id});
            return StatusCode(200);
            
        }
    }
}
