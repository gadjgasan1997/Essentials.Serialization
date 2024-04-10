namespace Essentials.Serialization.Exceptions;

/// <summary>
/// Исключение о не найденном десериалайзере
/// </summary>
public class DeserializerNotFoundException : KeyNotFoundException
{
    internal DeserializerNotFoundException(DeserializerKey deserializerKey)
        : base($"Не найден десериалайзер по ключу '{deserializerKey}'")
    { }
}