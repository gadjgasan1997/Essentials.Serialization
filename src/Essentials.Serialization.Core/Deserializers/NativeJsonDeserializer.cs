using System.Text.Json;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Deserializers;

/// <summary>
/// Десериалайзер, использующий System.Text.Json
/// </summary>
public class NativeJsonDeserializer : IEssentialsDeserializer
{
    /// <summary>
    /// Опции десериализации
    /// </summary>
    protected JsonSerializerOptions DeserializeOptions { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="deserializeOptions">Опции десериализации</param>
    public NativeJsonDeserializer(JsonSerializerOptions? deserializeOptions = null)
    {
        DeserializeOptions = deserializeOptions ?? new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    
    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlySpan{byte})" />
    public T? Deserialize<T>(ReadOnlySpan<byte> data) => JsonSerializer.Deserialize<T>(data, DeserializeOptions);

    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlyMemory{byte})" />
    public T? Deserialize<T>(ReadOnlyMemory<byte> data) => Deserialize<T>(data.Span);
}