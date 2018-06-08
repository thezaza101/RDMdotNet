using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RDMdotNet
{
    internal static class IO
    {
        private static StreamReader sr;
        private static StreamWriter sw;
        private static Dictionary<string, List<object>> inMemoryDB;
        private static HashSet<Type> changedObjects;
        const string dbPath = "database\\";

        static IO()
        {
            inMemoryDB = new Dictionary<string, List<object>>();
            changedObjects = new HashSet<Type>();
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
                AddItemToSaveList(fileName);
            }
        }

        internal static void Remove<T>(T o)
        {
            string fileName = o.GetType().ToString()+".json";
            List<T> existingElements = ReadObjects<T>();
            if (existingElements.FindIndex(e => e.ToDynamic().ID.Equals(o.ToDynamic().ID))>=0)
            {
                existingElements.RemoveAll(e => e.ToDynamic().ID.Equals(o.ToDynamic().ID));
                AddItemToSaveList(fileName);
            }
            else
            {
                throw new Exception("Key \"" + o.ToDynamic().ID + "\" does not exist");                
            }
        }

        /// <summary>
        /// Commit the changes made to the database
        /// </summary>
        public static void SaveChanges()
        {
            foreach (Type t in changedObjects)
            {
                SaveChanges(t);
            }
        }

        /// <summary>
        /// Commit the changes made to the database for the given type
        /// </summary>
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
                using (sw = new StreamWriter(filePath,false,Encoding.UTF8))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(inMemoryDB[fileName]));
                }
            }
        }
        
        /// <summary>
        /// Returns an object of the supplied type given the object contains a key
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam> 
        /// <returns>Object of the supplied type and key</returns>
        public static T Single<T>(string Id)
        {
            return ReadObjects<T>().Find(e => e.ToDynamic().ID.Equals(Id));
        }
        

        /// <summary>
        /// Returns all the objects stored of the supplied type
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam> 
        /// <returns>List of the data for the given type</returns>
        public static List<T> All<T>()
        {
            return ReadObjects<T>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IReadOnlyCollection<T> InnerList<T>()
        {
            return ReadObjects<T>().AsReadOnly();
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

        public static bool IsTypeSaveable<T>()
        {
            bool output = false;
            foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
            {
                if(Attribute.IsDefined(info,typeof(System.ComponentModel.DataAnnotations.KeyAttribute)))
                {
                    output = true;
                    break;
                }
            }
            return output;
        }
        private static void AddItemToSaveList(string fileName)
        {
            changedObjects.Add(Type.GetType(fileName.Substring(0,fileName.IndexOf(".json"))));
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