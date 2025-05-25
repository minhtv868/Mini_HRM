using Newtonsoft.Json;

namespace IC.WebJob.Helpers.Extensions
{
    public static class SessionExtensions
    {
        public static T GetComplexData<T>(this ISession session, string key)
        {
            if (string.IsNullOrEmpty(key)) return default;

            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            if (!string.IsNullOrEmpty(key))  session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
