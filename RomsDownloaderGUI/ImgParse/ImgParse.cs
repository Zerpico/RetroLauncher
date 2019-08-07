using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;


namespace RomsDownloaderGUI.ImgParse
{
    /// <summary>
    /// Парсер сайта конкретной игры, возвращает ссылку архива с ромами
    /// ссылка на архив генерируется временная, поэтому приходится перед скачиванием самого архива сначала узнать ссылку
    /// </summary>
    public class ImgParse : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document, string BaseUrl)
        {
            //возвращаяемый результат
            List<string> result = new List<string>(); ;

            //отбираем элементы только с таблицей 
            var parseItems = document.QuerySelectorAll("table").Where(dd => dd.ClassName == "gdb_table");

            foreach (var parseItem in parseItems)
            {
                var urlElement = parseItem.QuerySelectorAll("td").Where(dd => dd.ClassName == "gdb_left_col");

                foreach (var urlItem in urlElement)
                {
                    foreach (var child in urlItem.Children)
                    {
                        if (child.Attributes["src"] != null)
                            result.Add(child.Attributes["src"].Value);
                        if (child.NodeName.ToLower() == "div")
                        {
                            for(int i=0; i< child.Children.Count();i++)
                                if (child.Children[i].Attributes["src"] != null)
                                    result.Add(child.Children[i].Attributes["src"].Value);

                        }

                      /*  if (child is IHtmlImageElement)
                            result.Add((child as IHtmlImageElement).Source);  */

                    }
                    //получаем ссылку на картинку
                    //    if (urlItem.Children.Count())
                    /* var url = urlElement.Children[0].QuerySelector("img");
                     result = (url as IHtmlAnchorElement).Href;*/
                }
            }
            return result.ToArray();
        }
    }

}
