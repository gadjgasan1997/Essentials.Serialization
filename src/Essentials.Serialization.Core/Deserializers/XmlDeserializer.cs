using System.Xml;
using Essentials.Utils.Extensions;
using Essentials.Serialization.Deserializers.Abstractions;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Deserializers;

/// <summary>
/// Десериалайзер Xml
/// </summary>
public class XmlDeserializer : BaseEssentialsDeserializer
{
    /// <summary>
    /// Опции десериализации
    /// </summary>
    protected XmlReaderSettings DeserializeOptions { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="deserializeOptions">Опции десериализации</param>
    public XmlDeserializer(XmlReaderSettings? deserializeOptions = null)
    {
        DeserializeOptions = deserializeOptions ?? new XmlReaderSettings();
    }

    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize(Type, ReadOnlySpan{byte})" />
    public override object? Deserialize(Type type, ReadOnlySpan<byte> data)
    {
        using var stream = new MemoryStream(data.ToArray());
        using var reader = XmlReader.Create(stream, DeserializeOptions);
        
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
        var result = xmlSerializer.Deserialize(reader);
        result.CheckNotNull("Объект null после десериализации из Xml");

        return result;
    }
}