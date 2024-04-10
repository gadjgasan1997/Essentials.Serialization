using System.Collections.Concurrent;
using Essentials.Utils.Extensions;
using Essentials.Serialization.Exceptions;
using Essentials.Serialization.Serializers;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization;

/// <summary>
/// Фабрика для получения сериалайзера
/// </summary>
public static class EssentialsSerializersFactory
{
    private static uint _defaultSerializersAdded;
    
    /// <summary>
    /// Список сериалайзеров
    /// </summary>
    private static readonly ConcurrentDictionary<SerializerKey, IEssentialsSerializer> _serializers = new();

    /// <summary>
    /// Регистрирует сериалайзеры по-умолчанию
    /// </summary>
    public static void RegisterDefaultSerializers()
    {
        if (Interlocked.Exchange(ref _defaultSerializersAdded, 1) == 1)
            return;

        AddByType(() => new NewtonsoftJsonSerializer());
        AddByType(() => new NativeJsonSerializer());
        AddByType(() => new XmlSerializer());
    }

    #region TryGet
    
    /// <summary>
    /// Возвращает сериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>Сериалайзер</returns>
    public static IEssentialsSerializer TryGet(string key)
    {
        key.Check();
        
        var serializerKey = SerializerKey.New(key);
        
        return _serializers.TryGetValue(serializerKey, out var serializer)
            ? serializer
            : throw new SerializerNotFoundException(serializerKey);
    }

    /// <summary>
    /// Возвращает сериалайзер по типу
    /// </summary>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Сериалайзер</returns>
    public static TSerializer TryGet<TSerializer>()
        where TSerializer : IEssentialsSerializer
    {
        var serializerKey = SerializerKey.New<TSerializer>();
        return TryGet<TSerializer>(serializerKey);
    }
    
    /// <summary>
    /// Возвращает сериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Сериалайзер</returns>
    public static TSerializer TryGet<TSerializer>(string key)
        where TSerializer : IEssentialsSerializer
    {
        key.Check();
        
        var serializerKey = SerializerKey.New<TSerializer>(key);
        return TryGet<TSerializer>(serializerKey);
    }
    
    /// <summary>
    /// Возвращает сериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Сериалайзер</returns>
    private static TSerializer TryGet<TSerializer>(SerializerKey key)
        where TSerializer : IEssentialsSerializer
    {
        return _serializers.TryGetValue(key, out var serializer)
            ? (TSerializer) serializer
            : throw new SerializerNotFoundException(key);
    }

    #endregion

    #region Add

    /// <summary>
    /// Добавляет сериалайзер по типу
    /// </summary>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    public static void AddByType<TSerializer>(Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer => GetOrAddByType(func);

    /// <summary>
    /// Добавляет сериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    public static void AddByKey(string key, Func<IEssentialsSerializer> func) => GetOrAddByKey(key, func);

    /// <summary>
    /// Добавляет сериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    public static void AddByTypeAndKey<TSerializer>(string key, Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer => GetOrAddByTypeAndKey(key, func);
    
    #endregion
    
    #region AddOrUpdate

    /// <summary>
    /// Добавляет или изменяет сериалайзер по типу
    /// </summary>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    public static void AddOrUpdateByType<TSerializer>(Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer
    {
        var serializerKey = SerializerKey.New<TSerializer>();
        var serializer = func();
        
        _serializers.AddOrUpdate(serializerKey, _ => serializer, (_, _) => serializer);
    }

    /// <summary>
    /// Добавляет или изменяет сериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    public static void AddOrUpdateByKey(string key, Func<IEssentialsSerializer> func)
    {
        var serializerKey = SerializerKey.New(key);
        var serializer = func();
        
        _serializers.AddOrUpdate(serializerKey, _ => serializer, (_, _) => serializer);
    }

    /// <summary>
    /// Добавляет или изменяет сериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    public static void AddOrUpdateByTypeAndKey<TSerializer>(string key, Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer
    {
        var serializerKey = SerializerKey.New<TSerializer>(key);
        var serializer = func();
        
        _serializers.AddOrUpdate(serializerKey, _ => serializer, (_, _) => serializer);
    }
    
    #endregion

    #region GetOrAdd
    
    /// <summary>
    /// Возвращает или добавляет сериалайзер по типу
    /// </summary>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Сериалайзер</returns>
    public static TSerializer GetOrAddByType<TSerializer>(Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer
    {
        var serializerKey = SerializerKey.New<TSerializer>();
        return GetOrAdd(serializerKey, func);
    }
    
    /// <summary>
    /// Возвращает или добавляет сериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <returns>Сериалайзер</returns>
    public static IEssentialsSerializer GetOrAddByKey(string key, Func<IEssentialsSerializer> func)
    {
        var serializerKey = SerializerKey.New(key);
        return GetOrAdd(serializerKey, func);
    }
    
    /// <summary>
    /// Возвращает или добавляет сериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Сериалайзер</returns>
    public static TSerializer GetOrAddByTypeAndKey<TSerializer>(string key, Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer
    {
        var serializerKey = SerializerKey.New<TSerializer>(key);
        return GetOrAdd(serializerKey, func);
    }
    
    /// <summary>
    /// Возвращает или добавляет сериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания сериалайзера</param>
    /// <typeparam name="TSerializer">Тип сериалайзера</typeparam>
    /// <returns>Сериалайзер</returns>
    private static TSerializer GetOrAdd<TSerializer>(SerializerKey key, Func<TSerializer> func)
        where TSerializer : IEssentialsSerializer
    {
        if (_serializers.TryGetValue(key, out var existsSerializer))
            return (TSerializer) existsSerializer;

        var serializer = func();
        return (TSerializer) _serializers.AddOrUpdate(key, _ => serializer, (_, _) => serializer);
    }

    #endregion

    private static void Check(this string key) =>
        key.CheckNotNullOrEmpty($"Передан пустой ключ сериалайзера: '{key}'");
}