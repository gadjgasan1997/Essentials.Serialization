namespace Essentials.Serialization;

/// <summary>
/// Десериалайзер
/// </summary>
public interface IEssentialsDeserializer
{
    /// <summary>
    /// Десериализует спан байтов в объект
    /// </summary>
    /// <param name="data">Данные</param>
    /// <typeparam name="T">Тип данных, в который необходимо десериализовать</typeparam>
    /// <returns>Объект</returns>
    public T? Deserialize<T>(ReadOnlySpan<byte> data);

    /// <summary>
    /// Десериализует спан байтов в объект
    /// </summary>
    /// <param name="data">Данные</param>
    /// <typeparam name="T">Тип данных, в который необходимо десериализовать</typeparam>
    /// <returns>Объект</returns>
    public T? Deserialize<T>(ReadOnlyMemory<byte> data);
}