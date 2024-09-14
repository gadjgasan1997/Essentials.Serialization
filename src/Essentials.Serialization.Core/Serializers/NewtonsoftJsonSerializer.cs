using System.Text;
using Newtonsoft.Json;
using Essentials.Utils.Extensions;
using Essentials.Serialization.Serializers.Abstractions;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Serializers;

/// <summary>
/// Сериалайзер Json, использующий Newtonsoft.Json
/// </summary>
public class NewtonsoftJsonSerializer : BaseEssentialsSerializer
{
    /// <summary>
    /// Опции сериализации
    /// </summary>
    protected JsonSerializerSettings SerializeOptions { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="serializeOptions">Опции сериализации</param>
    public NewtonsoftJsonSerializer(JsonSerializerSettings? serializeOptions = null)
    {
        SerializeOptions = serializeOptions ?? new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    /// <inheritdoc cref="IEssentialsSerializer.Serialize(Type, object)" />
    public override byte[] Serialize(Type type, object input)
    {
        input.CheckNotNull("Не передан объект для сериализации");

        var data = JsonConvert.SerializeObject(input, type, SerializeOptions);
        data.CheckNotNullOrEmpty("Строка после сериализации пуста");
        
        return Encoding.UTF8.GetBytes(data);
    }
}