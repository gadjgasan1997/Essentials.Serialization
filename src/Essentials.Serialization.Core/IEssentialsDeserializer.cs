namespace Essentials.Serialization;

/// <summary>
/// Десериалайзер
/// </summary>
public interface IEssentialsDeserializer
{
    /// <summary>
    /// Десериализует спан байтов в объект
    /// </summary>
    /// <param name="type">Тип данных, в который необходимо десериализовать</param>
    /// <param name="data">Данные</param>
    /// <returns>Объект</returns>
    object? Deserialize(Type type, ReadOnlySpan<byte> data);
    
    /// <summary>
    /// Десериализует спан байтов в объект
    /// </summary>
    /// <param name="data">Данные</param>
    /// <typeparam name="T">Тип данных, в который необходимо десериализовать</typeparam>
    /// <returns>Объект</returns>
    T? Deserialize<T>(ReadOnlySpan<byte> data);
    
    /// <summary>
    /// Десериализует байты в объект
    /// </summary>
    /// <param name="type">Тип данных, в который необходимо десериализовать</param>
    /// <param name="data">Данные</param>
    /// <returns>Объект</returns>
    object? Deserialize(Type type, ReadOnlyMemory<byte> data);
    
    /// <summary>
    /// Десериализует байты в объект
    /// </summary>
    /// <param name="data">Данные</param>
    /// <typeparam name="T">Тип данных, в который необходимо десериализовать</typeparam>
    /// <returns>Объект</returns>
    T? Deserialize<T>(ReadOnlyMemory<byte> data);
}