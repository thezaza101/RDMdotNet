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
        public IActionResult Get(string tableID = "", bool ignoreChanges = false, bool list = false)
        {
            List<ChangeSet> allchanges;
            List<ChangeSet> filterdList;
            List<ChangeSet> output;
            
            allchanges = js.All<ChangeSet>();

            if (!string.IsNullOrWhiteSpace(tableID))
            {
                if(ignoreChanges)
                {
                    Table t  = js.Single<Table>(tableID);
                    RDSystem s = js.Single<RDSystem>(t.SystemID);
                    List<Release> rel = js.All<Release>().Where(r => r.SystemID == s.ID).ToList();
                    HashSet<string> uniqueReleaseIDs = new HashSet<string>();
                    rel.ForEach(r => uniqueReleaseIDs.Add(r.ID));
                    filterdList = new List<ChangeSet>();
                    foreach (ChangeSet c in allchanges)
                    {
                        if (uniqueReleaseIDs.Contains(c.ReleaseID))
                        {
                            filterdList.Add(c);
                        }
                    }
                } 
                else
                {
                    List<Change> changes = js.All<Change>().Where(c => c.TableID == tableID).ToList();
                    HashSet<string> uniqueCSIds = new HashSet<string>();
                    changes.ForEach(c => uniqueCSIds.Add(c.ChangeSetID));
                    filterdList = new List<ChangeSet>();
                    foreach (ChangeSet c in allchanges)
                    {
                        if (uniqueCSIds.Contains(c.ID))
                        {
                            filterdList.Add(c);
                        }
                    }
                }              
            }
            else
            {
                filterdList = allchanges;
            }

            if(list)
            {
                output = new List<ChangeSet>();
                filterdList.ForEach(c => output.Add(new ChangeSet(){Name = c.Name, Active = c.Active, ID = c.ID, ReleaseID = c.ReleaseID}));
            }
            else
            {
                output = filterdList;
                foreach (ChangeSet cs in output)
                {
                    cs.Changes = js.All<Change>().Where(c => c.ChangeSetID == cs.ID).ToList();
                }
            }            

            return StatusCode(200, output);
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
            string postedData = value.ToString();
            ChangeSet sysData = JsonConvert.DeserializeObject<ChangeSet>(postedData);
            sysData.ID = Guid.NewGuid().ToString();
            js.Add(sysData);
            js.SaveChanges();
            return StatusCode(201, sysData);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]dynamic value)
        {
            ChangeSet curData = js.Single<ChangeSet>(id);
            string postedData = value.ToString();
            ChangeSet newData = JsonConvert.DeserializeObject<ChangeSet>(postedData);
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
            js.Remove(new ChangeSet(){ID = id});
            return StatusCode(201);
            
        }
    }
}
