using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMdotNet.Models
{
    public class Change : ChangeControlElement
    {
        public string ChangeSetID {get; set;}
        public string TableID {get;set;}
        public string ElementID {get; set;}
        public bool Active {get;set;}
        public ChangeAction Action {get;set;}
        public string ElementName {get;set;}
        public string NewValue {get;set;}
        [Newtonsoft.Json.JsonIgnore]    
        public object ObjectReference {get;set;}   
    }
}