using Newtonsoft.Json;
using System;
using TMS.Utility.Helper;

namespace TMS.Web.Helper
{
    public static class Helper
    {
        public static T DecryptParamData<T>(string data) where T : class
        {

            T classInstance;
            if (string.IsNullOrWhiteSpace(data))
            {
                classInstance = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                string jsonData = SecurityHelper.Decrypt(data);
                classInstance = JsonConvert.DeserializeObject<T>(jsonData);
            }
            return classInstance;
        }

        public static T GetParamData<T>(string data) where T : class
        {

            T classInstance;
            if (string.IsNullOrWhiteSpace(data))
            {
                classInstance = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                classInstance = JsonConvert.DeserializeObject<T>(data);
            }
            return classInstance;
        }


        // Deep clone
        public static T DeepClone<T>(this T data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }
    }
}