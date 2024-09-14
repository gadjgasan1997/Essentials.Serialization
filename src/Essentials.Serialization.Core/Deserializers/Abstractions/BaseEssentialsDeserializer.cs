namespace Essentials.Serialization.Deserializers.Abstractions;

/// <summary>
/// Базовый класс для десериалайзеров
/// </summary>
public abstract class BaseEssentialsDeserializer : IEssentialsDeserializer
{
    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize(Type, ReadOnlySpan{byte})" />
    public abstract object? Deserialize(Type type, ReadOnlySpan<byte> data);

    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlySpan{byte})" />
    public T? Deserialize<T>(ReadOnlySpan<byte> data) => (T?) Deserialize(typeof(T), data);

    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize(Type, ReadOnlyMemory{byte})" />
    public object? Deserialize(Type type, ReadOnlyMemory<byte> data) => Deserialize(type, data.Span);
    
    /// <inheritdoc cref="IEssentialsDeserializer.Deserialize{T}(ReadOnlyMemory{byte})" />
    public T? Deserialize<T>(ReadOnlyMemory<byte> data) => (T?) Deserialize(typeof(T), data);
}