using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDMdotNet.Models
{
    public class Archive
    {
        [Key]
        public string ID {get;set;}
        public string Type {get;set;}
        public object Data {get;set;}

        public Archive()
        {

        }
        public Archive(DBElement dataToArchive)
        {
            this.ID = Guid.NewGuid().ToString();
            this.Type = dataToArchive.GetType().ToString();
            this.Data = dataToArchive;
        }
    }
}