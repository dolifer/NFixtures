#if NETCOREAPP
using System.Text.Json;
#elif NETSTANDARD
using Newtonsoft.Json;
#endif

namespace NFixtures.xUnit.Infrastructure
{
    /// <summary>
    /// TestCase data serializer.
    /// </summary>
    public static class ValueSerializer
    {
        /// <summary>
        /// Serializes a given <paramref name="data"/> to a string.
        /// </summary>
        /// <param name="data">Value to serialize.</param>
        /// <returns>A JSON string.</returns>
        public static string ToJson(object data)
        {
#if NETCOREAPP
           return JsonSerializer.Serialize(data);
#elif NETSTANDARD
            return JsonConvert.SerializeObject(data);
#endif
        }

        /// <summary>
        /// Reads a given <paramref name="json"/> string into <typeparamref name="T"/>.
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <typeparam name="T">Type of the object to read.</typeparam>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public static T FromJson<T>(string json)
        {
#if NETCOREAPP
            return JsonSerializer.Deserialize<T>(json);
#elif NETSTANDARD
            return JsonConvert.DeserializeObject<T>(json);
#endif
        }
    }
}
