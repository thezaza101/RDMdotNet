using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using RDMdotNet.Models;

namespace RDMdotNet
{
    internal static class IO
    {
        private static StreamReader sr;
        private static StreamWriter sw;
        private static Dictionary<string, List<IDBElement>> inMemoryDB;
        const string dbPath = "database\\";

        static IO()
        {
            inMemoryDB = new Dictionary<string, List<IDBElement>>();
            if(!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }            
        }


        internal static void Add(IDBElement o)
        {
            string fileName = o.GetType().ToString()+".json";
            List<IDBElement> existingElements = ReadObjects(o.GetType());
            if (existingElements.FindIndex(e => e.ID.Equals(o.ID))>=0)
            {
                throw new Exception("Key \"" + o.ID + "\" already exists");
            }
            else
            {
                inMemoryDB[fileName].Add(o);
            }
        }

        internal static void Remove(IDBElement o)
        {
            string fileName = o.GetType().ToString()+".json";
            List<IDBElement> existingElements = ReadObjects(o.GetType());
            if (existingElements.FindIndex(e => e.ID.Equals(o.ID))>=0)
            {
                existingElements.RemoveAll(e => e.ID.Equals(o.ID));
            }
            else
            {
                throw new Exception("Key \"" + o.ID + "\" does not exist");                
            }
        }


        public static void SaveChanges()
        {
            foreach (KeyValuePair<string,List<IDBElement>> kvp in inMemoryDB)
            {
                SaveChanges(Type.GetType(kvp.Key.Substring(0,kvp.Key.IndexOf(".json"))));
            }
        }
        public static void SaveChanges(Type t)
        {
            string fileName = t.ToString()+".json";
            string filePath = dbPath + fileName;

            if(!inMemoryDB.ContainsKey(fileName))
            {
                throw new KeyNotFoundException("No changes to key \"" + fileName + "\" found");
            }
            else
            {
                if(!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                using (sw = new StreamWriter(filePath,false,Encoding.UTF8))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(inMemoryDB[fileName]));
                    sw.Close();
                }
            }
        }
        
        public static T Single<T>(string Id)
        {
            return (T)ReadObjects(typeof(T)).Find(e => e.ID.Equals(Id));
        }

        /// <summary>
        /// Reads the JSON file related to the supplied type and initialises the memory storage for that type
        /// </summary>
        /// <param>Type of the data to be read and initialised</param>
        /// <returns>List of the data for the given type</returns>
        public static List<IDBElement> All<T>()
        {
            return ReadObjects(typeof(T));
        }
        
        private static List<IDBElement> ReadObjects(Type t)
        {            
            string fileName = t.ToString()+".json";
            string filePath = dbPath + fileName;

            if (inMemoryDB.ContainsKey(fileName))
            {
                return inMemoryDB[fileName];
            } 
            else 
            {
                string data;
                try
                {                                
                    using(sr = new StreamReader(filePath,Encoding.UTF8))
                    {
                        data = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    data ="";
                }
                List<IDBElement> dataList = string.IsNullOrWhiteSpace(data) ? new List<IDBElement>() : JsonConvert.DeserializeObject<List<IDBElement>>(data);                
                inMemoryDB.Add(fileName, dataList);
                return dataList;
            }
        }
    }
}