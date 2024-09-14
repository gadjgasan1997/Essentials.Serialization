namespace Essentials.Serialization.Serializers.Abstractions;

/// <summary>
/// Базовый класс для сериалайзеров
/// </summary>
public abstract class BaseEssentialsSerializer : IEssentialsSerializer
{
    /// <inheritdoc cref="IEssentialsSerializer.Serialize(Type, object)" />
    public abstract byte[] Serialize(Type type, object input);

    /// <inheritdoc cref="IEssentialsSerializer.Serialize{T}(T)" />
    public byte[] Serialize<T>(T input) => Serialize(typeof(T), input!);
}