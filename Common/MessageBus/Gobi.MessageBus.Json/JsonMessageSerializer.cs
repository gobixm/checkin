using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Gobi.MessageBus.Services.Serializers;

namespace Gobi.MessageBus.Json;

public sealed class JsonMessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    private readonly Assembly[] _assemblies;
    private readonly ConcurrentDictionary<string, Type> _cachedTypes = new();

    public JsonMessageSerializer(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public byte[] Serialize<T>(Stream stream, T body) where T : class
    {
        JsonSerializer.Serialize(stream, body, JsonSerializerOptions);
        return GetDiscriminator<T>();
    }

    public T? Deserialize<T>(Stream stream, byte[] discriminator) where T : class
    {
        var fullName = Encoding.UTF8.GetString(discriminator);
        var type = FindType<T>(fullName);

        return JsonSerializer.Deserialize(stream, type) as T;
    }

    public T? Deserialize<T>(ReadOnlySpan<byte> data, ReadOnlySpan<byte> discriminator) where T : class
    {
        var fullName = Encoding.UTF8.GetString(discriminator);
        var type = FindType<T>(fullName);

        return JsonSerializer.Deserialize(data, type) as T;
    }

    private Type FindType<T>(string fullName) where T : class
    {
        if (!_cachedTypes.TryGetValue(fullName, out var type))
        {
            type = FindType(fullName, _assemblies);
            _cachedTypes[fullName] = type;
        }

        if (type is null)
            throw new ArgumentException($"Unknown message type: {fullName}");
        return type;
    }

    private static Type FindType(string fullName, IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var type = assembly.GetType(fullName);
            if (type is not null)
                return type;
        }

        throw new ArgumentException($"Unknown message type: {fullName}");
    }

    private static byte[] GetDiscriminator<T>()
    {
        var fullName = typeof(T).FullName;
        if (fullName is null)
            throw new InvalidEnumArgumentException("Type has no full name");
        return Encoding.UTF8.GetBytes(fullName);
    }
}