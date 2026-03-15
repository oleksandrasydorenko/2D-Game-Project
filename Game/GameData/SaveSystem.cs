using RocketEngine;
using RocketEngine.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Data
{
    public static class SaveSystem
    {
        private const string SAVE_FILE_PATH = "SaveData.json";
        public static GameDataPreSet LoadData() 
        {
            
            GameDataPreSet? data = DataStoreService.LoadFromJson<GameDataPreSet>(SAVE_FILE_PATH);

            if (data == null) //if no data was found, we set the data to the default data
            {
                data = new GameDataPreSet();
                bool scuccses = DataStoreService.SaveToJson(SAVE_FILE_PATH, data);
                Console.WriteLine($"loaded default data: {scuccses}");
            }
            return data;
        }

        public static bool SaveData(string propertyName, object? newValue) //searches 
        {
            GameDataPreSet currentData = LoadData(); //two uses: one to load current data and one to sve the new Data

            try
            {
                PropertyInfo? foundProperty = currentData.GetType().GetProperty(propertyName); //seaches in GameDataPreSet for the property

                if (foundProperty == null) return false; //if property not found in GameDataPreSet

                foundProperty?.SetValue(currentData, newValue);
            }
            catch
            {
                throw new Exception("the type value you entered is not of the same type as the property look at the save data preset to find the right one");
            }

            DataStoreService.SaveToJson(SAVE_FILE_PATH, currentData);

            return true;
        }
    }
}