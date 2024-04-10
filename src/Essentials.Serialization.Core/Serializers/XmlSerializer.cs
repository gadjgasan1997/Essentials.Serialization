using System.Text;
using System.Xml;
using Essentials.Utils.Extensions;
using Essentials.Utils.IO.Writers;

// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Serializers;

/// <summary>
/// Сериалайзер Xml
/// </summary>
public class XmlSerializer : IEssentialsSerializer
{
    /// <summary>
    /// Опции серилизации
    /// </summary>
    protected XmlWriterSettings SerializeOptions { get; }
    
    /// <summary>
    /// Кодировка
    /// </summary>
    protected Encoding Encoding { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="serializeOptions">Опции серилизации</param>
    /// <param name="encoding">Кодировка</param>
    public XmlSerializer(
        XmlWriterSettings? serializeOptions = null,
        Encoding? encoding = null)
    {
        Encoding = encoding ?? Encoding.Default;
        
        SerializeOptions = serializeOptions ?? new XmlWriterSettings
        {
            Encoding = Encoding
        };
    }
    
    /// <inheritdoc cref="IEssentialsSerializer.Serialize{T}" />
    public byte[] Serialize<T>(T input)
    {
        using var stringWriter = new StringWriterWithEncoding(Encoding);
        using var writer = XmlWriter.Create(stringWriter, SerializeOptions);
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

        xmlSerializer.Serialize(writer, input);
        var resultString = stringWriter.ToString().CheckNotNullOrEmpty("Строка пуста после сериализации в Xml");

        return Encoding.GetBytes(resultString);
    }
}