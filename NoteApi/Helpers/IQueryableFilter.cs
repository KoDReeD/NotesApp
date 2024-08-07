using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Models;

namespace NotesApi;

public static class IQueryableFilter
{
    public static IQueryable<T> Pagination<T>(this IQueryable<T> sourse, RequestFilter filter) where T : class
    {
        if (filter.CountOnPage.HasValue)
        {
            var skip = filter.Page * filter.CountOnPage.GetValueOrDefault();
            var count = filter.CountOnPage.GetValueOrDefault();
            return sourse.Skip(skip).Take(count);
        }
        else
        {
            return sourse;
        }
    }

    public static IQueryable<Note> NotesFilter(this IQueryable<Note> source, RequestFilter filter)
    {
        if (filter.GetSearchString(out var searchTerms))
        {
            foreach (var term in searchTerms)
            {
                source = source.Where(x =>
                    EF.Functions.ILike(x.Title, term) ||
                    EF.Functions.ILike(x.CreatedDate.ToString("o", CultureInfo.InvariantCulture), term) ||
                    x.Tags.Any(tag => EF.Functions.ILike(tag.Tag.Title, term)));
            }
        }

        return OrderFiltration(source, filter);
    }

    /// <summary>
    /// Определяет необходимость и порядок сортировки
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    private static IQueryable<T> OrderFiltration<T>(this IQueryable<T> source, RequestFilter filter) where T : class
    {
        if (!string.IsNullOrEmpty(filter.OrderBy))
            source = filter.OrderByDescending
                ? source.OrderByDescending(filter.OrderBy)
                : source.OrderBy(filter.OrderBy);
        return source;
    }

    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName) where T : class
    {
        var propertyDec = propertyName.Split('.');
        return source.OrderBy(propertyDec.Length > 1
            ? ToLambda<T>(propertyDec[0], propertyDec[1])
            : ToLambda<T>(propertyName));
    }
    
    
    private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        where T : class
    {
        var propertyDec = propertyName.Split('.');
        return source.OrderByDescending(propertyDec.Length > 1
            ? ToLambda<T>(propertyDec[0], propertyDec[1])
            : ToLambda<T>(propertyName));
    }


    private static Expression<Func<T, object>> ToLambda<T>(string propertyName) where T : class
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyFatherName, string propertyName)
        where T : class
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(Expression.Property(parameter, propertyFatherName), propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}