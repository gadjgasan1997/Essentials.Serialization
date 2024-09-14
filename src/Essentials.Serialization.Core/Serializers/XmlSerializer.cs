using System.Text;
using System.Xml;
using Essentials.Utils.Extensions;
using Essentials.Utils.IO.Writers;
using Essentials.Serialization.Serializers.Abstractions;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Serializers;

/// <summary>
/// Сериалайзер Xml
/// </summary>
public class XmlSerializer : BaseEssentialsSerializer
{
    /// <summary>
    /// Опции сериализации
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
    
    /// <inheritdoc cref="IEssentialsSerializer.Serialize(Type, object)" />
    public override byte[] Serialize(Type type, object input)
    {
        using var stringWriter = new StringWriterWithEncoding(Encoding);
        using var writer = XmlWriter.Create(stringWriter, SerializeOptions);
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);

        xmlSerializer.Serialize(writer, input);
        var resultString = stringWriter.ToString();
        resultString.CheckNotNullOrEmpty("Строка пуста после сериализации в Xml");

        return Encoding.GetBytes(resultString);
    }
}