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
    public class ReleaseController : Controller
    {
        JSONStore js = new JSONStore();
        // GET api/values
        [HttpGet]
        public IActionResult Get(string tableID = "", bool list = false)
        {
            List<Release> all = js.All<Release>();
            List<Release> filterd;
            List<Release> output;

            if (!string.IsNullOrWhiteSpace(tableID))
            {
                Table t  = js.Single<Table>(tableID);
                RDSystem s = js.Single<RDSystem>(t.SystemID);
                filterd = js.All<Release>().Where(r => r.SystemID == s.ID).ToList();
            }
            else
            {
                filterd = all;
            }

            if(list)
            {
                output = new List<Release>();
                filterd.ForEach(c => output.Add(new Release(){Name = c.Name, Active = c.Active, ID = c.ID, SystemID = c.SystemID}));
            }
            else
            {
                output = filterd;
            }    


            return StatusCode(200, output);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return StatusCode(200, js.Single<Release>(id));            
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]dynamic value)
        {
            string postedData = value.ToString();
            Release sysData = JsonConvert.DeserializeObject<Release>(postedData);
            sysData.ID = Guid.NewGuid().ToString();
            js.Add(sysData);
            js.SaveChanges();
            return StatusCode(201, sysData);    
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]dynamic value)
        {
            Release curData = js.Single<Release>(id);

            string postedData = value.ToString();
            Release newData = JsonConvert.DeserializeObject<Release>(postedData);
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
            js.Remove(new Release(){ID = id});
            return StatusCode(201);
            
        }
    }
}
