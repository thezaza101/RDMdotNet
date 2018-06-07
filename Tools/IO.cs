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
        private static Dictionary<string, List<object>> inMemoryDB;
        const string dbPath = "database\\";

        static IO()
        {
            inMemoryDB = new Dictionary<string, List<object>>();
            if(!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }            
        }
        internal static void Add<T>(T o)
        {
            string fileName = o.GetType().ToString()+".json";
            List<T> existingElements = ReadObjects<T>();
            if (existingElements.FindIndex(e => e.ToDynamic().ID.Equals(o.ToDynamic().ID))>=0)
            {
                throw new Exception("Key \"" + o.ToDynamic().ID + "\" already exists");
            }
            else
            {
                inMemoryDB[fileName].Add(o);
            }
        }

        internal static void Remove<T>(T o)
        {
            string fileName = o.GetType().ToString()+".json";
            List<T> existingElements = ReadObjects<T>();
            if (existingElements.FindIndex(e => e.ToDynamic().ID.Equals(o.ToDynamic().ID))>=0)
            {
                existingElements.RemoveAll(e => e.ToDynamic().ID.Equals(o.ToDynamic().ID));
            }
            else
            {
                throw new Exception("Key \"" + o.ToDynamic().ID + "\" does not exist");                
            }
        }


        public static void SaveChanges()
        {
            foreach (KeyValuePair<string,List<object>> kvp in inMemoryDB)
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
            return ReadObjects<T>().Find(e => e.ToDynamic().ID.Equals(Id));
        }
        

        /// <summary>
        /// Reads the JSON file related to the supplied type and initialises the memory storage for that type
        /// </summary>
        /// <param>Type of the data to be read and initialised</param>
        /// <returns>List of the data for the given type</returns>
        public static List<T> All<T>()
        {
            return ReadObjects<T>();
        }
        
        private static List<T> ReadObjects<T>()
        {            
            string fileName = typeof(T).ToString()+".json";
            string filePath = dbPath + fileName;

            if (inMemoryDB.ContainsKey(fileName))
            {
                return inMemoryDB[fileName].ConvertObjectToGenericType<T>();
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
                List<T> dataList = string.IsNullOrWhiteSpace(data) ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(data);                
                inMemoryDB.Add(fileName, dataList.ConvertGenericTypeToObjectList<T>());
                return dataList;
            }
        }
        private static List<object> ConvertGenericTypeToObjectList<T>(this List<T> inList)
        {
            List<object> outputObject = new List<object>();
            inList.ForEach(a => outputObject.Add((object)a));
            return outputObject;
        }
        private static List<T> ConvertObjectToGenericType<T>(this List<object> inList)
        {
            List<T> outputObject = new List<T>();
            inList.ForEach(a => outputObject.Add((T)a));
            return outputObject;
        }
        public static dynamic ToDynamic<T>(this T input)
        {
            return (dynamic)input;
        }
    }
}