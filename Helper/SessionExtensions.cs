using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace NextUses.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            if (value == null) return;
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            if (string.IsNullOrEmpty(value))
            {
                return default; // default(T) => for ref types = null
            }

            return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
