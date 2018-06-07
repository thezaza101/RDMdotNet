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
    public class ReleaseController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, IO.All<Release>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, IO.Single<Release>(id));            
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]dynamic value)
        {
            string postedData = value.ToString();
            Release sysData = JsonConvert.DeserializeObject<Release>(postedData);
            sysData.ID = Guid.NewGuid().ToString();
            IO.Add(sysData);
            IO.SaveChanges();
            return StatusCode(201, sysData);    
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]string value)
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
        public IActionResult Delete(int id)
        {
            return StatusCode(201);
            
        }
    }
}
