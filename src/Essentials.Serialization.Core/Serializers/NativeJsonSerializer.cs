using System.Text.Json;
using Essentials.Utils.Extensions;

// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Serializers;

/// <summary>
/// Сериалайзер Json, использующий System.Text.Json
/// </summary>
public class NativeJsonSerializer : IEssentialsSerializer
{
    /// <summary>
    /// Опции сериализации
    /// </summary>
    protected JsonSerializerOptions SerializeOptions { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="serializeOptions">Опции сериализации</param>
    public NativeJsonSerializer(JsonSerializerOptions? serializeOptions = null)
    {
        SerializeOptions = serializeOptions ?? new JsonSerializerOptions();
    }

    /// <inheritdoc cref="IEssentialsSerializer.Serialize{T}" />
    public byte[] Serialize<T>(T input)
    {
        input.CheckNotNull("Не передан объект для сериализации");
        
        return JsonSerializer.SerializeToUtf8Bytes(input, SerializeOptions);
    }
}