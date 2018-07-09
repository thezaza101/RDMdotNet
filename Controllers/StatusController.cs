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

            var statusObject = new {
                SystemCount = systemsCount,
                ReleaseCount = releaseCount,
                ChangeSetCount = changeSetCount,
                TableCount = tableCount,
                TableList = tableList
            };

            return StatusCode(201, statusObject);
        }
    }
}
