using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomsDownloaderGUI.UrlParse
{
    /// <summary>
    /// Настройки парсера ссылки на архив с ромами игр
    /// </summary>
    public class UrlSettings : IParserSettings
    {
        public UrlSettings(string gameUrl)
        {
            this.BaseUrl = gameUrl;
        }

        //задаем при инициализации
        public string BaseUrl { get; set; }
        //он просто не может быть null из-за алгоритма
        public string[] Prefix { get; set; } = { "" };
    }

}
