namespace Essentials.Serialization.Exceptions;

/// <summary>
/// Исключение о не найденном сериалайзере
/// </summary>
public class SerializerNotFoundException : KeyNotFoundException
{
    internal SerializerNotFoundException(SerializerKey serializerKey)
        : base($"Не найден сериалайзер по ключу '{serializerKey}'")
    { }
}