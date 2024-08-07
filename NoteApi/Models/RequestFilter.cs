using System;
using System.Collections.Generic;
using System.Linq;

namespace NotesApi.Models;

public class RequestFilter
{
    /// <summary>
    /// Номер запрашиваемой страницы
    /// </summary>
    public int Page { get; set; } = 0;

    /// <summary>
    /// Количество записей на странице
    /// </summary>
    public int? CountOnPage { get; set; }

    /// <summary>
    /// Строка поиска
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// По чему сортировать
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Порядок сортировки
    /// </summary>
    public bool OrderByDescending { get; set; } = false;

    /// <summary>
    /// Проверка задано ли значение для поиска и его форматирование для Like запроса
    /// </summary>
    /// <returns></returns>
    public bool GetSearchString(out List<string> results)
    {
        results = Search?.Trim()
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(term => $"%{term}%")
            .ToList();
        return results is { Count: > 0 };
    }
}