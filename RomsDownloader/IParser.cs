using AngleSharp.Html.Dom;

namespace RomsDownloader
{
    /// <summary>
    /// Реализация парсера
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document, string BaseUrl);
    }

}
