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
    public class ChangeSetController : Controller
    {
        JSONStore js = new JSONStore();
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, js.All<ChangeSet>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, js.Single<ChangeSet>(id));   
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]dynamic value)
        {
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
            js.Remove(new ChangeSet(){ID = id});
            return StatusCode(201);
            
        }
    }
}
