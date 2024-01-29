using System.IO;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    static class SerializationHelper
    {
        public static void SerializeObject<T>(T obj, string filePath)
        {
            string jsonString = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, jsonString);
        }

        public static T DeserializeObject<T>(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            T obj = JsonConvert.DeserializeObject<T>(jsonString);
            return obj;
        }
    }
}