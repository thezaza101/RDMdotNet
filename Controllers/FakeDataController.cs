using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDMdotNet.Models;

namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class FakeDataController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            RDSystem sys = new RDSystem(){ID = Guid.NewGuid().ToString(), Name = DateTime.Now.ToString()};
            IO.Add(sys);
            IO.SaveChanges();

            return StatusCode(201, IO.All<RDSystem>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(201, IO.Single<RDSystem>(id));
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {

            return StatusCode(201);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return StatusCode(201);
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            IO.Remove(new RDSystem(){ID = id});
            return StatusCode(201);
            
        }
    }
}
