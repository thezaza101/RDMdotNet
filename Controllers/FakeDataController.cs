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
            RDSystem sys = new RDSystem(){
                ID = Guid.NewGuid().ToString(), 
                Name = "DXA"+IO.InnerList<RDSystem>().Count().ToString(), 
                Active = true
            }; 
            sys.Properties.Add("Bool", true);
            sys.Properties.Add("Int", (int)-24);
            sys.Properties.Add("UInt", (uint)12);
            sys.Properties.Add("Float", 43.27F);
            sys.Properties.Add("Char", 'x');
            sys.Properties.Add("Decimal", 53005.25M);

            Release r = new Release(){
                ID = Guid.NewGuid().ToString(), 
                SystemID = sys.ID, 
                Active = true, 
                Name = "DXA_"+IO.InnerList<Release>().Count().ToString(), 
                DeploymentDate = new DateTime(2018,7,1)
            };

            ChangeSet cs = new ChangeSet(){
                ID = Guid.NewGuid().ToString(), 
                ReleaseID = r.ID, 
                Active = true, 
                Name = "DXA_Test_ChangeSet_"+IO.InnerList<ChangeSet>().Count().ToString()
            };


            IO.Add(sys);
            IO.Add(r);
            IO.Add(cs);
            IO.SaveChanges();

            var x = new {
                System = IO.All<RDSystem>(),
                Release = IO.All<Release>(),
                ChangeSet = IO.All<ChangeSet>()
            };

            var xx = IO.IsTypeSaveable<RDSystem>();
            var xx1 = IO.IsTypeSaveable<ChangeSet>();
            


            return StatusCode(201, x);
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
