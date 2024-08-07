namespace NotesApi.Models;

public class Page<T> where T : class
{
    /// <summary>
    /// Общее кол-во элементов в базе
    /// </summary>
    public int ObjectsCount { get; set; }

    /// <summary>
    /// Количество страниц
    /// </summary>
    public int PagesCount
    {
        get => (CountOnPage == 0 ? (ObjectsCount == 0 ? 0 : 1) :
            (ObjectsCount / CountOnPage) == 0 ? 1 : (ObjectsCount / CountOnPage));
    }

    /// <summary>
    /// Сколько записей запрошено в запросе
    /// </summary>
    public int CountOnPage { get; set; }
    
    /// <summary>
    /// Теущая выбранная страница
    /// </summary>
    public long CurrentPage { get; set; }

    public ICollection<T> Objects { get; set; }
    
    public Page(RequestFilter requestFilter)
    {
        CurrentPage = requestFilter.Page;
        CountOnPage = requestFilter.CountOnPage.GetValueOrDefault();
    }
}