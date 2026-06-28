using Newtonsoft.Json;
using Tastehub.Utility.Helper;
using System;
using System.Collections;
using System.Web;

namespace Tastehub.Web.Helper
{
    public static class UserAuthenticate
    {
        private const string CookieName = "ES";
        private const string CookieKey = "US";

        private static Hashtable GetDecryptedData()
        {
            try
            {
                var cookie = HttpContext.Current?.Request?.Cookies[CookieName];
                if (cookie == null || cookie[CookieKey] == null) return null;

                var jsonData = SecurityHelper.Decrypt(cookie[CookieKey]);
                return JsonConvert.DeserializeObject<Hashtable>(jsonData);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                var data = GetDecryptedData();
                return data != null;
            }
        }

        public static string UserName => Convert.ToString(GetDecryptedData()?["UserName"]);
        public static string RememberMe => Convert.ToString(GetDecryptedData()?["RememberMe"]) ?? "false";
        public static string LogId => Convert.ToString(GetDecryptedData()?["LogId"]);
        public static string Role => Convert.ToString(GetDecryptedData()?["Role"]);
        public static string UserType => Convert.ToString(GetDecryptedData()?["UserType"]);
        public static long UserTypeId => Convert.ToInt64(GetDecryptedData()?["UserTypeId"] ?? 0);

        public static void AddLoginCookie(string userName, string roleName, string logId, string userType, string rememberMe, long userTypeId)
        {
            Logout(HttpContext.Current);

            var loggedData = new Hashtable
            {
                { "LogId", logId },
                { "UserName", userName },
                { "Role", roleName },
                { "UserType", userType },
                { "RememberMe", rememberMe },
                { "UserTypeId", userTypeId }
            };

            var encryptedCookie = new HttpCookie(CookieName)
            {
                Expires = DateTime.Now.AddHours(24)
            };
            encryptedCookie.Values.Add(CookieKey, SecurityHelper.Encrypt(JsonConvert.SerializeObject(loggedData)));

            HttpContext.Current.Response.Cookies.Add(encryptedCookie);
        }

        public static void Logout(HttpContext context)
        {
            if (context?.Request?.Cookies[CookieName] != null)
            {
                context.Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}
