using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RocketEngine.DataStore
{
    public static class DataStoreService //two methods: save json and load data
    {
        public static bool SaveToJson<T>(string path, T data) where T: DataPreSet
        {
            try 
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true
                };
                string json = JsonSerializer.Serialize(data, options); //Serialize: class -> json file: makes the json format with all its components
                File.WriteAllText(path, json); // c# method: saves the json file
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T? LoadFromJson<T>(string path) where T: DataPreSet
        {
            if (!File.Exists(path)) return null;
            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions //makes sure that file saves its new components
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
                T data = JsonSerializer.Deserialize<T>(json, options); //from class json that desierializes by taking the json string
                                                                                         //and building a SaveAllDataObject from it
                return data;
            }
            catch 
            {
                throw new ArgumentException("Couldn't load from JSON file, file path exists though");
            }
        }
        /*
        private const string SaveFilePath = "SaveData.json";

        public static bool SaveData<T>(string property, object? newValue)
        {
           try
            {

                PropertyInfo? foundProperty = typeof(T).GetType().GetProperty(property); //searches in the class T for the property with the given string

                try
                {
                    foundProperty?.SetValue(typeof(T), 5);
                }
                catch (Exception ex)
                {
                    throw new Exception("the type value you entered is not of the same type as the property look at the save data preset to find the right one");
                }
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true
                };
                string json = JsonSerializer.Serialize(typeof(T), options); //Serialize: class -> json file: makes the json format with all its components
                File.WriteAllText(SaveFilePath, json); // c# method: saves the json file
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DataPreSet LoadData<T>() where T : new()
        {
            string json;

            if (!File.Exists(SaveFilePath))
            {
                Console.WriteLine("Error: SaveData, file not found");
                try
                {
                    var saveOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        IncludeFields = true
                    };

                    T defaultData = new T(); //problem: Instance and GameDataPreSet???

                    json = JsonSerializer.Serialize(defaultData, saveOptions); //Serialize: class -> json file: makes the json format with all its components
                    File.WriteAllText(SaveFilePath, json); // c# method: saves the json file
                }
                catch
                {
                    throw new ArgumentException("Couldn't create json file, try again later");
                }
                Console.WriteLine("default data assigned");
                return null;
            }
                
            json = File.ReadAllText(SaveFilePath);
            var options = new JsonSerializerOptions //makes sure that file saves its new components
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };
            DataPreSet save = JsonSerializer.Deserialize<DataPreSet>(json, options); //from class json that desierializes by taking the json string
                                                                              //and building a SaveAllDataObject from it
            return save;
           
        }*/
    }
    /*
     
            if (!File.Exists(path)) return null;
            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions //makes sure that file saves its new components
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
                DataPreSet data = JsonSerializer.Deserialize<DataPreSet>(json, options); //from class json that desierializes by taking the json string
                                                                                         //and building a SaveAllDataObject from it
                return data;
            }
            catch 
            {
                throw new ArgumentException("Couldn't load from JSON file, file path exists though");
            }
        }
    */
}
