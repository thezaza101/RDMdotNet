using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RDMdotNet.Models;
using LStoreJSON;
using System.IO;

namespace RDMdotNet.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        class node
        {
            public string name {get;set;}
            public int size {get;set;}
            public List<node> children {get;set;}
        }
        JSONStore js = new JSONStore(Environment.GetEnvironmentVariable("LStoreData"));
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            int systemsCount = js.All<RDSystem>().Count();
            int releaseCount = js.All<Release>().Count();
            int changeSetCount = js.All<ChangeSet>().Count();
            List<Table> tableRef = js.All<Table>();
            int tableCount = tableRef.Count();
            List<string> tableList = new List<string>();
            tableRef.ForEach(e => tableList.Add(e.ID));

            node root = new node(){name="RDM", size=0,children=new List<node>()};
            node sysnode;
            node relnode;
            node csnode;

            foreach (RDSystem s in js.All<RDSystem>())
            {
                sysnode = new node(){name=s.Name, size=150,children=new List<node>()};
                foreach (Release r in js.All<Release>().Where(r => r.SystemID.Equals(s.ID)))
                {
                    relnode = new node(){name=r.Name, size=100,children=new List<node>()};
                    foreach (ChangeSet c in js.All<ChangeSet>().Where(cs => cs.ReleaseID.Equals(r.ID)))
                    {
                        csnode = new node(){name=c.Name, size = c.Changes.Count};
                        relnode.children.Add(csnode);
                    }
                    relnode.size = relnode.children.Count;
                    sysnode.children.Add(relnode);
                }
                sysnode.size = sysnode.children.Count;
                root.children.Add(sysnode);
            }

           

            Dictionary<string, Dictionary<string,string>> returnobj = new Dictionary<string, Dictionary<string,string>>();

            var statusObject = new {
                SystemCount = systemsCount,
                ReleaseCount = releaseCount,
                ChangeSetCount = changeSetCount,
                TableCount = tableCount,
                status = root,
                TableList = tableList
            };

            return StatusCode(201, statusObject);
        }
    }
}
