using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;

namespace RomsDownloader.BaseParser
{
    /// <summary>
    /// Парсер списка игр с сайта
    /// </summary>
    public class BaseParse : IParser<IGame[]>
    {
        public IGame[] Parse(IHtmlDocument document, string BaseUrl)
        {
            //возвращаяемый результат
            var result = new List<IGame>();

            //парсим название платформы
            var platname = ParsePlatformName(document.QuerySelectorAll("td").Where(tdh => tdh.ClassName == "hd14").First());

            //отбираем элементы только с таблицей игр
            var items = document.QuerySelectorAll("td").Where(item => item.ClassName == null &&
                item.ParentElement.LocalName == "tr" &&
                item.Children.Where(dd => dd.ClassName == "gdb_table").Count() > 0);

            //парсим всё
            foreach (var item in items)
            {
                try
                {
                    //отбираем только столбцы с инфой о игре
                    var tableValues = item.Children.Where(gg => gg.ClassName == "gdb_table").ToArray();

                    //парсим осн. инфу о игре
                    var newGame = ParseItem(tableValues[0].QuerySelectorAll("tr"));
                    //добавляем инфу о ссылках
                    ParseItemUrl(tableValues[1].QuerySelectorAll("tr"), ref newGame);
                    //добавляем название платформы
                    if (newGame != null) { newGame.Platform = platname; result.Add(newGame); }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return result.ToArray();
        }

        private string ParsePlatformName(IElement element)
        {
            return element.Children.Last().TextContent;
        }

        //парсит ссылки на игру
        private void ParseItemUrl(IHtmlCollection<IElement> items, ref IGame game)
        {
            bool faildeParse = false;
            //парсим столбцы     
            foreach (var item in items)
            {
                if (faildeParse) game = null;
                if (item.ChildElementCount < 2) //дальше идёт кнопка добавить информацию о игре
                    return;
                //аннотация к игре
                game.Annotation = item.Children[1].TextContent;
                //две ссылки на картинку и ссылку на страницу игры
                var parseA = item.Children[0].QuerySelector("a");
                if (parseA != null)
                {
                    game.Url = parseA.Attributes["href"].Value;
                    game.ImgUrl = item.Children[0].QuerySelector("img").Attributes["src"].Value;
                }
                else faildeParse = true;

            }
            if (faildeParse) game = null;
        }

        //парсит поля игры
        private IGame ParseItem(IHtmlCollection<IElement> items)
        {
            //парсим столбцы           
            var game = new Game();
            foreach (var item in items)
            {
                //парсим названия
                for (int i = 0; i < item.Children.Count(); i = i + 2)
                {
                    //все названия
                    if (item.Children[i].TextContent.Contains("названи"))
                    {
                        if (string.IsNullOrEmpty(game.Name)) game.Name = item.Children[i + 1].TextContent;
                        else if (string.IsNullOrEmpty(game.SecondName)) game.SecondName = item.Children[i + 1].TextContent;
                        else game.OtherName = item.Children[i + 1].TextContent;
                    }
                    else if (item.Children[i].TextContent.ToLower().Contains("год"))
                    {
                        int parseInt;
                        game.Year = int.TryParse(item.Children[i + 1].TextContent, out parseInt) ? parseInt : new Nullable<int>();
                    }
                    else if (item.Children[i].TextContent.ToLower().Contains("разработчик"))
                        game.Developer = item.Children[i + 1].TextContent.Trim();
                    else if (item.Children[i].TextContent.ToLower().Contains("жанр"))
                        game.Genre = item.Children[i + 1].TextContent.Trim();
                    else if (item.Children[i].TextContent.ToLower().Contains("игроки"))
                    {
                        int parseInt;
                        game.Players = int.TryParse(item.Children[i + 1].TextContent, out parseInt) ? parseInt : new Nullable<int>();
                    }

                }
            }
            return game;
        }

    }
}
