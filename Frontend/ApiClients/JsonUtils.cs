using System.Text.Json;

namespace ApiClients;

internal static class JsonUtils
{
    private static readonly JsonSerializerOptions caseInsensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? DeserializeInsensitive<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, caseInsensitiveOptions);
    }

    public static object? DeserializeInsensitive(string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type, caseInsensitiveOptions);
    }
}
