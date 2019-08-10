using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;


namespace RomsDownloaderGUI.UrlParse
{
    /// <summary>
    /// Парсер сайта конкретной игры, возвращает ссылку архива с ромами
    /// ссылка на архив генерируется временная, поэтому приходится перед скачиванием самого архива сначала узнать ссылку
    /// </summary>
    public class UrlParse : IParser<string>
    {
        public string Parse(IHtmlDocument document, string BaseUrl)
        {
            //возвращаяемый результат
            string result = string.Empty;
            try
            {
                //отбираем элементы только с таблицей 
                var parseItem = document.QuerySelectorAll("table").Where(dd => dd.ClassName == "gdb_table").First();

                var urlElement = parseItem.QuerySelectorAll("td").Where(dd => dd.ClassName == "gdb_right_col").First();

                //получаем ссылку на архив
                var url = urlElement.Children[0].QuerySelector("a");
                result = (url as IHtmlAnchorElement).Href;
            }
            catch (Exception) { }
            return result;
        }
    }

}
