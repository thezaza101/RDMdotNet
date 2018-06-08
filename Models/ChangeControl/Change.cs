using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMdotNet.Models
{
    public class Change : IDBElement
    {
        [Key]
        public string ID {get;set;}
        public string ChangeSetID {get; set;}
        public bool Active {get;set;}
        public Dictionary<string, object> Properties = new Dictionary<string, object>(); 
    }
}