using System.Text;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization.Extensions;

/// <summary>
/// Методы расширения для <see cref="IEssentialsDeserializer" />
/// </summary>
public static class EssentialsDeserializerExtensions
{
    /// <summary>
    /// Десериализует строку в объект
    /// </summary>
    /// <param name="deserializer">Десериалайзер</param>
    /// <param name="data">Данные</param>
    /// <param name="encoding">Кодировка</param>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <returns>Объект</returns>
    public static T? Deserialize<T>(
        this IEssentialsDeserializer deserializer,
        string data,
        Encoding encoding)
    {
        var bytes = encoding.GetBytes(data);
        return deserializer.Deserialize<T>(bytes.AsSpan());
    }

    /// <summary>
    /// Десериализует строку с кодировкой Utf8 в объект
    /// </summary>
    /// <param name="deserializer">Десериалайзер</param>
    /// <param name="data">Данные</param>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <returns>Объект</returns>
    public static T? DeserializeUtf8<T>(this IEssentialsDeserializer deserializer, string data) =>
        deserializer.Deserialize<T>(data, Encoding.UTF8);
}