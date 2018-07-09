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
    public class DataController : Controller
    {
        JSONStore js = new JSONStore(Environment.GetEnvironmentVariable("LStoreData"));
        // GET api/values
        [HttpGet]
        public IActionResult Get(bool list)
        {
            List<Table> output;

            if (list)
            {
                output = new List<Table>();
                js.All<Table>().ForEach(t => output.Add(new Table(){ID = t.ID, SystemID = t.SystemID}));
            }
            else
            {
                output = js.All<Table>();
            }

            return StatusCode(200, output);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, js.Single<Table>(id));
        }
        // GET api/values/5
        [HttpGet("{id}/{innerID}")]
        public IActionResult Get(string id, string innerID)
        {
            return StatusCode(200, js.Single<Table>(id).TableElements["http://dxa.gov.au/definition/"+id+"/"+innerID]);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]dynamic value)
        {
            string postedData = value.ToString();
            Table sysData = JsonConvert.DeserializeObject<Table>(postedData);
            sysData.ID = Guid.NewGuid().ToString();
            js.Add(sysData);
            js.SaveChanges();
            return StatusCode(201, sysData);            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]dynamic value)
        {
            Table curData = js.Single<Table>(id);

            string postedData = value.ToString();
            Table newData = JsonConvert.DeserializeObject<Table>(postedData);
            newData.ID = curData.ID;

            js.Remove(curData);
            js.Add(newData);
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
