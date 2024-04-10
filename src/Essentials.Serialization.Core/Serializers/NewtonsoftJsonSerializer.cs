using System.Text;
using Newtonsoft.Json;
using Essentials.Utils.Extensions;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Serializers;

/// <summary>
/// Сериалайзер Json, использующий Newtonsoft.Json
/// </summary>
public class NewtonsoftJsonSerializer : IEssentialsSerializer
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

    /// <inheritdoc cref="IEssentialsSerializer.Serialize{T}" />
    public byte[] Serialize<T>(T input)
    {
        input.CheckNotNull("Не передан объект для сериализации");

        var data = JsonConvert.SerializeObject(input, SerializeOptions);
        data.CheckNotNullOrEmpty("Строка после сериализации пуста");
        
        return Encoding.UTF8.GetBytes(data);
    }
}