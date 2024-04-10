namespace Essentials.Serialization;

/// <summary>
/// Сериалайзер
/// </summary>
public interface IEssentialsSerializer
{
    /// <summary>
    /// Сериализует объект
    /// </summary>
    /// <param name="input">Объект</param>
    /// <typeparam name="T">Тип данных, в который необходимо сериализовать</typeparam>
    /// <returns>Массив байтов</returns>
    byte[] Serialize<T>(T input);
}