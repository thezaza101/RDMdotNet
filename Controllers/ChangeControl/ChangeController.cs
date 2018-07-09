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
    public class ChangeController : Controller
    {
        JSONStore js = new JSONStore(Environment.GetEnvironmentVariable("LStoreData"));
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, js.All<Change>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, js.Single<Change>(id));   
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]List<Change> value)
        {
            List<Change> changes = value;
            changes.ForEach(c => c.ID = Guid.NewGuid().ToString());
            changes.ForEach(c => c.ObjectReference = js.Single<Element>(c.ElementID));
            foreach (Change c in changes)
            {
                js.Add(c);
            }
            js.SaveChanges();
            return StatusCode(201);            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]dynamic value)
        {
            return StatusCode(201);
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return StatusCode(405);       
        }
    }
}
