using Essentials.Utils.Extensions;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Essentials.Serialization;

/// <summary>
/// Ключ сериалайзера
/// </summary>
internal readonly record struct SerializerKey
{
    /// <summary>
    /// Ключ
    /// </summary>
    public string? Key { get; }
    
    /// <summary>
    /// Тип сериалайзера
    /// </summary>
    public Type? SerializerType { get; }
    
    private SerializerKey(string? key = null, Type? serializerType = null)
    {
        if (string.IsNullOrWhiteSpace(key) && serializerType is null)
        {
            throw new InvalidOperationException(
                "Ошибка создания ключа сериалайзера. " +
                $"Строковый ключ: '{key}'. " +
                $"Тип сериалайзера: '{serializerType?.FullName}'");
        }
        
        Key = key?.FullTrim().ToLowerInvariant();
        SerializerType = serializerType;
    }
    
    /// <summary>
    /// Создает ключ сериалайзера из строкового ключа
    /// </summary>
    /// <param name="key">Строковый ключ</param>
    /// <returns>Ключ сериалайзера</returns>
    public static SerializerKey New(string key) => new(key);
    
    /// <summary>
    /// Создает ключ сериалайзера по типу
    /// </summary>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Ключ сериалайзера</returns>
    public static SerializerKey New<TSerializer>() => new(serializerType: typeof(TSerializer));

    /// <summary>
    /// Создает ключ сериалайзера из комбинации строкового ключа и типа
    /// </summary>
    /// <param name="key">Строковый ключ</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Ключ сериалайзера</returns>
    public static SerializerKey New<TSerializer>(string key) where TSerializer : IEssentialsSerializer =>
        new(key, typeof(TSerializer));
}