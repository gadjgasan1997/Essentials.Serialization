using System.Text.Json;
using Essentials.Serialization.Deserializers.Abstractions;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Deserializers;

/// <summary>
/// Десериалайзер, использующий System.Text.Json
/// </summary>
public class NativeJsonDeserializer : BaseEssentialsDeserializer
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
    
    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize(Type, ReadOnlySpan{byte})" />
    public override object? Deserialize(Type type, ReadOnlySpan<byte> data) =>
        JsonSerializer.Deserialize(data, type, DeserializeOptions);
}