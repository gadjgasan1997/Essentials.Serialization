using System.Collections.Concurrent;
using Essentials.Utils.Extensions;
using Essentials.Serialization.Exceptions;
using Essentials.Serialization.Deserializers;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.Serialization;

/// <summary>
/// Фабрика для получения десериалайзера
/// </summary>
public static class EssentialsDeserializersFactory
{
    private static uint _defaultDeserializersAdded;
    
    /// <summary>
    /// Список десериалайзеров
    /// </summary>
    private static readonly ConcurrentDictionary<DeserializerKey, IEssentialsDeserializer> _deserializers = new();

    /// <summary>
    /// Регистрирует десериалайзеры по-умолчанию
    /// </summary>
    public static void RegisterDefaultDeserializers()
    {
        if (Interlocked.Exchange(ref _defaultDeserializersAdded, 1) == 1)
            return;

        AddByType(() => new NewtonsoftJsonDeserializer());
        AddByType(() => new NativeJsonDeserializer());
        AddByType(() => new XmlDeserializer());
    }

    #region TryGet
    
    /// <summary>
    /// Возвращает десериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>Десериалайзер</returns>
    public static IEssentialsDeserializer TryGet(string key)
    {
        key.Check();
        
        var deserializerKey = DeserializerKey.New(key);
        
        return _deserializers.TryGetValue(deserializerKey, out var deserializer)
            ? deserializer
            : throw new DeserializerNotFoundException(deserializerKey);
    }

    /// <summary>
    /// Возвращает десериалайзер по типу
    /// </summary>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Десериалайзер</returns>
    public static TDeserializer TryGet<TDeserializer>()
        where TDeserializer : IEssentialsDeserializer
    {
        var deserializerKey = DeserializerKey.New<TDeserializer>();
        return TryGet<TDeserializer>(deserializerKey);
    }
    
    /// <summary>
    /// Возвращает десериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Десериалайзер</returns>
    public static TDeserializer TryGet<TDeserializer>(string key)
        where TDeserializer : IEssentialsDeserializer
    {
        key.Check();
        
        var deserializerKey = DeserializerKey.New<TDeserializer>(key);
        return TryGet<TDeserializer>(deserializerKey);
    }
    
    /// <summary>
    /// Возвращает десериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Десериалайзер</returns>
    private static TDeserializer TryGet<TDeserializer>(DeserializerKey key)
        where TDeserializer : IEssentialsDeserializer
    {
        return _deserializers.TryGetValue(key, out var deserializer)
            ? (TDeserializer) deserializer
            : throw new DeserializerNotFoundException(key);
    }    

    #endregion

    #region Add

    /// <summary>
    /// Добавляет десериалайзер по типу
    /// </summary>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    public static void AddByType<TDeserializer>(Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer => GetOrAddByType(func);

    /// <summary>
    /// Добавляет десериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    public static void AddByKey(string key, Func<IEssentialsDeserializer> func) => GetOrAddByKey(key, func);

    /// <summary>
    /// Добавляет десериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    public static void AddByTypeAndKey<TDeserializer>(string key, Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer => GetOrAddByTypeAndKey(key, func);

    #endregion
    
    #region AddOrUpdate

    /// <summary>
    /// Добавляет или изменяет десериалайзер по типу
    /// </summary>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    public static void AddOrUpdateByType<TDeserializer>(Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer
    {
        var deserializerKey = DeserializerKey.New<TDeserializer>();
        var deserializer = func();
        
        _deserializers.AddOrUpdate(deserializerKey, _ => deserializer, (_, _) => deserializer);
    }

    /// <summary>
    /// Добавляет или изменяет десериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    public static void AddOrUpdateByKey(string key, Func<IEssentialsDeserializer> func)
    {
        var deserializerKey = DeserializerKey.New(key);
        var deserializer = func();
        
        _deserializers.AddOrUpdate(deserializerKey, _ => deserializer, (_, _) => deserializer);
    }

    /// <summary>
    /// Добавляет или изменяет десериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    public static void AddOrUpdateByTypeAndKey<TDeserializer>(string key, Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer
    {
        var deserializerKey = DeserializerKey.New<TDeserializer>(key);
        var deserializer = func();
        
        _deserializers.AddOrUpdate(deserializerKey, _ => deserializer, (_, _) => deserializer);
    }
    
    #endregion
    
    #region GetOrAdd
    
    /// <summary>
    /// Возвращает или добавляет десериалайзер по типу
    /// </summary>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Десериалайзер</returns>
    public static TDeserializer GetOrAddByType<TDeserializer>(Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer
    {
        var deserializerKey = DeserializerKey.New<TDeserializer>();
        return GetOrAdd(deserializerKey, func);
    }
    
    /// <summary>
    /// Возвращает или добавляет десериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <returns>Десериалайзер</returns>
    public static IEssentialsDeserializer GetOrAddByKey(string key, Func<IEssentialsDeserializer> func)
    {
        var deserializerKey = DeserializerKey.New(key);
        return GetOrAdd(deserializerKey, func);
    }
    
    /// <summary>
    /// Возвращает или добавляет десериалайзер по комбинации ключа и типа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Десериалайзер</returns>
    public static TDeserializer GetOrAddByTypeAndKey<TDeserializer>(string key, Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer
    {
        var deserializerKey = DeserializerKey.New<TDeserializer>(key);
        return GetOrAdd(deserializerKey, func);
    }
    
    /// <summary>
    /// Возвращает или добавляет десериалайзер по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="func">Делегат для создания десериалайзера</param>
    /// <typeparam name="TDeserializer">Тип десериалайзера</typeparam>
    /// <returns>Десериалайзер</returns>
    private static TDeserializer GetOrAdd<TDeserializer>(DeserializerKey key, Func<TDeserializer> func)
        where TDeserializer : IEssentialsDeserializer
    {
        if (_deserializers.TryGetValue(key, out var existsDeserializer))
            return (TDeserializer) existsDeserializer;

        var deserializer = func();
        return (TDeserializer) _deserializers.AddOrUpdate(key, _ => deserializer, (_, _) => deserializer);
    }
    
    #endregion

    private static void Check(this string key) =>
        key.CheckNotNullOrEmpty($"Передан пустой ключ десериалайзера: '{key}'");
}