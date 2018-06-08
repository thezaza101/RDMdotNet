using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMdotNet.Models
{
    public class ChangeSet : IDBElement
    {
        [Key]
        public string ID {get;set;}
        public string ReleaseID {get;set;}
        public bool Active {get;set;}
        public StatusCode ChangeSetStatus {get;set;}
        public string Name {get; set;}
        public Dictionary<string, object> Properties = new Dictionary<string, object>();        
    }
}