using System.Text.Json;
using Essentials.Utils.Extensions;
using Essentials.Serialization.Serializers.Abstractions;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Serializers;

/// <summary>
/// Сериалайзер Json, использующий System.Text.Json
/// </summary>
public class NativeJsonSerializer : BaseEssentialsSerializer
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

    /// <inheritdoc cref="IEssentialsSerializer.Serialize(Type, object)" />
    public override byte[] Serialize(Type type, object input)
    {
        input.CheckNotNull("Не передан объект для сериализации");
        
        return JsonSerializer.SerializeToUtf8Bytes(input, type, SerializeOptions);
    }
}