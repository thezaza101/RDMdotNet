using System.ComponentModel.DataAnnotations;

namespace RDMdotNet.Models
{
    public interface IDBElement
    {
        [Key]
        string ID {get;set;}
        
        
    } 
}