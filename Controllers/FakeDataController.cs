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
            /*RDSystem sys = new RDSystem(){
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
            
            return StatusCode(201, x);*/

            Change c = new Change(){
                ID=Guid.NewGuid().ToString(),
                ChangeSetID = "92aec884-cb79-471d-9450-34f668e84226",
                TableID="edu",
                ElementID="http://dxa.gov.au/definition/edu/edu316",
                Active = true,
                Action=ChangeAction.UpdateElement,
                ElementName="definition",
                NewValue="Identifies whether or not the student/applicant identifies as being of Aboriginal and/or Torres Strait Islander descent"
            };
            Change c1 = new Change(){
                ID=Guid.NewGuid().ToString(),
                ChangeSetID = "19f421bd-5cc2-45cc-9c33-99c958ef8a20",
                TableID="edu",
                ElementID="http://dxa.gov.au/definition/edu/edu333",
                Active = true,
                Action=ChangeAction.UpdateElement,
                ElementName="guidance",
                NewValue="AOU"
            };
            Change c4 = new Change(){
                ID=Guid.NewGuid().ToString(),
                ChangeSetID = "b2933c47-28f4-4bb1-b522-26077a276553",
                TableID="edu",
                ElementID="http://dxa.gov.au/definition/edu/edu514",
                Active = true,
                Action=ChangeAction.RemoveElement
            };
            Element n = new Element(){ID = Guid.NewGuid().ToString()};
            n.Values.Add("name", "TestNewElement");
            n.Values.Add("domain", "Education");
            n.Values.Add("status", "Standard");
            n.Values.Add("definition", "A new element for testing");
            n.Values.Add("guidance", "Field Name: FTE-NEW-TEST");
            n.Values.Add("identifier", "http://dxa.gov.au/definition/edu/edu999");
            n.Values.Add("usage", "[  \"See source for more information\"]");
            n.Values.Add("datatype", "[]]");
            n.Values.Add("values", "[]");
            n.Values.Add("sourceURL", "Who knows");


            Change c5 = new Change(){
                ID=Guid.NewGuid().ToString(),
                ChangeSetID = "92aec884-cb79-471d-9450-34f668e84226",
                TableID="edu",
                ElementID="http://dxa.gov.au/definition/edu/edu999",
                Active = true,
                Action=ChangeAction.AddElement,
                NewElementPayload = n
            };

            js.Add(c);
            js.Add(c1);
            js.Add(c4);
            js.Add(c5);
            js.SaveChanges();



            return StatusCode(201);

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
