using System.Text.Json;

namespace Sample.ElasticLoadTests
{
    public static class ObjectExtensions
    {
        public static string DumpAsJson(this object value)
            => JsonSerializer.Serialize(value, new JsonSerializerOptions() { WriteIndented = true });
    }
}
