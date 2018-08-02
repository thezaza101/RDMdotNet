using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RDMdotNet.Models
{
    public class Element
    {
        [Key]
        public string ID {get;set;}
        public Dictionary<string, string> Values = new Dictionary<string, string>(); 
    }
}