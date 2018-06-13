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
    public class FakeDataController : Controller
    {
        JSONStore js = new JSONStore();
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            RDSystem sys = new RDSystem(){
                ID = Guid.NewGuid().ToString(), 
                Name = "DXA"+js.InnerList<RDSystem>().Count().ToString(), 
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
                Name = "DXA_"+js.InnerList<Release>().Count().ToString(), 
                DeploymentDate = new DateTime(2018,7,1)
            };

            ChangeSet cs = new ChangeSet(){
                ID = Guid.NewGuid().ToString(), 
                ReleaseID = r.ID, 
                Active = true, 
                Name = "DXA_Test_ChangeSet_"+js.InnerList<ChangeSet>().Count().ToString()
            };


            js.Add(sys);
            js.Add(r);
            js.Add(cs);
            js.SaveChanges();

            var x = new {
                System = js.All<RDSystem>(),
                Release = js.All<Release>(),
                ChangeSet = js.All<ChangeSet>()
            };

            var xx = JSONStore.IsTypeSaveable<RDSystem>();
            var xx1 = JSONStore.IsTypeSaveable<ChangeSet>();
            


            return StatusCode(201, x);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            List<string> files = new List<string>();

            files.Add("edu.json");
            files.Add("ce.json");
            files.Add("fi.json");
            files.Add("fs.json");
            files.Add("ss.json");
            files.Add("trc.json");

            foreach (string x in files)
            {
                SaveTableToFile(x);                
            }
            

            return StatusCode(201, js.Single<RDSystem>(id));
        }

        private void SaveTableToFile (string filename)
        {
            Table tbl = new Table();
            string jsonData;
            using (StreamReader sr = new StreamReader("wwwroot\\"+filename))
            {
                jsonData = sr.ReadToEnd();
            }
            Newtonsoft.Json.Linq.JObject JsonObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jsonData);
            var proep = JsonObj.Properties();
            foreach (Newtonsoft.Json.Linq.JProperty p in proep)
            {
                if (!(p.Name.Equals("acronym") | p.Name.Equals("content")))
                {
                    tbl.TableProperties.Add(p.Name, p.Value.ToString());
                } else if (p.Name.Equals("acronym"))
                {
                    tbl.ID = p.Value.ToString();
                } else if (p.Name.Equals("content"))
                {
                    Element e;
                    Newtonsoft.Json.Linq.JArray v = (Newtonsoft.Json.Linq.JArray)p.Value;
                    foreach(var attributex in v)
                    {
                        e = new Element();
                        foreach (Newtonsoft.Json.Linq.JProperty t in attributex)
                        {
                            if(t.Name.Equals("identifier"))
                            {
                                e.ID = t.Value.ToString();
                            }
                            e.Values.Add(t.Name,t.Value.ToString());
                        }
                        tbl.TableElements.Add(e.ID,e);
                    }
                }
            }
            js.Add(tbl);
            js.SaveChanges();
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
            js.Remove(new RDSystem(){ID = id});
            return StatusCode(201);
        }
    }
}
