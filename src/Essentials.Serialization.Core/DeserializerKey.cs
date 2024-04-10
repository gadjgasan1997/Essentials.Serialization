// ReSharper disable UnusedAutoPropertyAccessor.Global

using Essentials.Utils.Extensions;

namespace Essentials.Serialization;

/// <summary>
/// Ключ десериалайзера
/// </summary>
internal readonly record struct DeserializerKey
{
    /// <summary>
    /// Ключ
    /// </summary>
    public string? Key { get; }
    
    /// <summary>
    /// Тип десериалайзера
    /// </summary>
    public Type? DeserializerType { get; }
    
    private DeserializerKey(string? key = null, Type? deserializerType = null)
    {
        if (string.IsNullOrWhiteSpace(key) && deserializerType is null)
        {
            throw new InvalidOperationException(
                "Ошибка создания ключа десериалайзера. " +
                $"Строковый ключ: '{key}'. " +
                $"Тип десериалайзера: '{deserializerType?.FullName}'");
        }
        
        Key = key?.FullTrim().ToLowerInvariant();
        DeserializerType = deserializerType;
    }
    
    /// <summary>
    /// Создает ключ десериалайзера из строкового ключа
    /// </summary>
    /// <param name="key">Строковый ключ</param>
    /// <returns>Ключ десериалайзера</returns>
    public static DeserializerKey New(string key) => new(key);
    
    /// <summary>
    /// Создает ключ десериалайзера по типу
    /// </summary>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Ключ десериалайзера</returns>
    public static DeserializerKey New<TDeserializer>() => new(deserializerType: typeof(TDeserializer));

    /// <summary>
    /// Создает ключ десериалайзера из комбинации строкового ключа и типа десериалайзера
    /// </summary>
    /// <param name="key">Строковый ключ</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Ключ десериалайзера</returns>
    public static DeserializerKey New<TDeserializer>(string key) where TDeserializer : IEssentialsDeserializer =>
        new(key, typeof(TDeserializer));
}