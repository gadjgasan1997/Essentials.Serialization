using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace Essentials.Serialization.Deserializers;

/// <summary>
/// Десериалайзер Json, использующий Newtonsoft.Json
/// </summary>
public class NewtonsoftJsonDeserializer : IEssentialsDeserializer
{
    /// <summary>
    /// Опции десериализации
    /// </summary>
    protected JsonSerializerSettings DeserializeOptions { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="deserializeOptions">Опции десериализации</param>
    public NewtonsoftJsonDeserializer(JsonSerializerSettings? deserializeOptions = null)
    {
        DeserializeOptions = deserializeOptions ?? new JsonSerializerSettings();
    }
    
    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlySpan{byte})" />
    public T? Deserialize<T>(ReadOnlySpan<byte> data)
    {
        if (data.IsEmpty)
            throw new InvalidOperationException("Для десериализации передан пустой массив байтов"); 
        
        using var stream = new MemoryStream(data.ToArray());
        using var streamReader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(streamReader);
        
        var serializer = JsonSerializer.CreateDefault(DeserializeOptions);
        return serializer.Deserialize<T>(jsonReader);
    }

    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlyMemory{byte})" />
    public T? Deserialize<T>(ReadOnlyMemory<byte> data) => Deserialize<T>(data.Span);
}