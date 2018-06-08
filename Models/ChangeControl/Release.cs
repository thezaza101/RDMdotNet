using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMdotNet.Models
{
    public class Release : IDBElement
    {
        [Key]
        public string ID {get;set;}
        public string SystemID {get; set;}
        public bool Active {get;set;}
        public string Name {get; set;}

        public DateTime DeploymentDate {get;set;}

        public Dictionary<string, object> Properties = new Dictionary<string, object>();
    }
}