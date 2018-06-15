using System;
using System.Collections.Generic;

namespace RDMdotNet.Models
{
    public class ChangeSet : ChangeControlElement
    {
        public string ReleaseID {get;set;}
        public bool Active {get;set;}
        public StatusCode ChangeSetStatus {get;set;}
        public string Name {get; set;}
        public List<Change> Changes = new List<Change>();

        

        public string PublishChanges(LStoreJSON.JSONStore js = null)
        {
            string output = "";
            LStoreJSON.JSONStore store = (js == null) ? new LStoreJSON.JSONStore() : js;
            foreach (Change c in Changes)
            {
                if (c.Active)
                {
                    switch (c.Action)
                    {
                        case ChangeAction.AddElement:
                            store.Add(c.ObjectReference);
                        break;

                        case ChangeAction.UpdateElement:
                            store.Remove(c.ObjectReference);
                            ((Element)c.ObjectReference).Values[c.ElementName] = c.NewValue;
                            store.Add(c.ObjectReference);
                        break;

                        case ChangeAction.RemoveElement:
                            store.Remove(c.ObjectReference);
                        break;

                        default: break;
                    }
                }
            }
            this.ChangeSetStatus = StatusCode.Deployed;
            store.Add(new Archive(this));
            store.Remove(this);
            store.SaveChanges();            
            return output;
        }   
    }
}