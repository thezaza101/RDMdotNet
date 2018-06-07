using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class ChangeSetController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return StatusCode(201);
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
        public IActionResult Delete(int id)
        {
            return StatusCode(201);
            
        }
    }
}
