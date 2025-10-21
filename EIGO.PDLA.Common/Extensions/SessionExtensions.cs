using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
// TAKEN FROM https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/blob/master/5-WebApp-AuthZ/5-2-Group  and adapted
namespace EIGO.PDLA.Common.Extensions
{
    public static class SessionExtensions
    {
        public static void SetAsByteArray(this ISession session, string key, object toSerialize)
        {
            byte[] ba = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toSerialize));
            session.Set(key, ba);
        }

        public static T GetAsByteArray<T>(this ISession session, string key)
        {
            var objectBytes = session.Get(key) ?? Array.Empty<byte>();

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(objectBytes)) ?? (T)(new object());

        }
    }
}
