using System.Xml;
using Essentials.Utils.Extensions;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Deserializers;

/// <summary>
/// Десериалайзер Xml
/// </summary>
public class XmlDeserializer : IEssentialsDeserializer
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
    
    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlySpan{byte})" />
    public T? Deserialize<T>(ReadOnlySpan<byte> data)
    {
        using var stream = new MemoryStream(data.ToArray());
        using var reader = XmlReader.Create(stream, DeserializeOptions);
        
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        var result = xmlSerializer.Deserialize(reader).CheckNotNull("Объект null после десериализации из Xml");

        return (T?) result;
    }

    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlyMemory{byte})" />
    public T? Deserialize<T>(ReadOnlyMemory<byte> data) => Deserialize<T>(data.Span);
}