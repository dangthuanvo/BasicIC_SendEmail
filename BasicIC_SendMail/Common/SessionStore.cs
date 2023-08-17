using System.Web;

namespace BasicIC_SendEmail.Common
{
    public class SessionStore
    {
        public static string InitTenantId;
        public static string InitUserId;

        public static string Get(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            {
                return HttpContext.Current.Session[key].ToString();
            }
            return null;
        }
        public static void Set(string key, object value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
        }
    }
}