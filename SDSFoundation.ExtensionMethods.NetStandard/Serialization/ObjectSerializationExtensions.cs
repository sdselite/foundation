using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
namespace SDSFoundation.ExtensionMethods.NetStandard.Serialization
{
    public static class ObjectSerializationExtensions
    {

        /// <summary>
        /// Converts an object to a string of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T obj) where T : class
        {
             var jsonString = JsonConvert.SerializeObject(obj);
            return jsonString;
        }


        /// <summary>
        /// Converts an string serialized as T back into the object of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objOfT"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string objOfT) where T : class
        {
            var result = JsonConvert.DeserializeObject<T>(objOfT);
            return result;
        }

    }
}
