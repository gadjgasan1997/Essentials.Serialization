using Newtonsoft.Json;

namespace Essentials.Serialization.Helpers;

/// <summary>
/// Хелперы для работы с Json
/// </summary>
public static class JsonHelpers
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    /// <summary>
    /// Серилизует объект в строку
    /// </summary>
    /// <param name="obj">Объект</param>
    /// <returns>Серилизованная строка</returns>
    public static string Serialize(object? obj) => JsonConvert.SerializeObject(obj, _settings);
}