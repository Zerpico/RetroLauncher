using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RomsDownloaderGUI
{
    public class ParserWorker<T> where T : class
    {
        HtmlLoader loader;
        public event Action<object, T> OnNewData;
        public event Action<object> OnComplete;
        public event Action<object> OnError;

        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings)
        {
            Parser = parser;
            ParserSettings = parserSettings;
        }

        /// <summary>
        /// Начинаем парсить
        /// </summary>
        public void Start()
        {
            isActive = true;
            Worker();
        }

        /// <summary>
        /// Остановить парсер
        /// </summary>
        public void Abort()
        {
            isActive = false;
        }

        /// <summary>
        /// Работа самого парсера
        /// </summary>
        private async void Worker()
        {
            //в цикле скачиваем и парсим сайт по каждому префиксу к основному url
            foreach (var prefix in ParserSettings.Prefix)
            {
                // если не вызвали Abort то продолжаем работу
                if (!isActive) { OnComplete?.Invoke(this); return; }
                var source = await loader.GetSource(prefix);
                if (loader.ErrorMessage == null)
                {
                    //начинаем парсить 
                    var domParser = new HtmlParser();
                    if (!isActive) { OnComplete?.Invoke(this); return; }
                    var document = await domParser.ParseDocumentAsync(source);
                    var result = parser.Parse(document, ParserSettings.BaseUrl);
                    //уведомляем о новых данных и возвращаем их
                    OnNewData?.Invoke(this, result);
                    result = null;
                }
                else OnError?.Invoke(loader.ErrorMessage);
                System.Threading.Thread.Sleep(100);  // чтобы сайт не подумал что мы его ддосим
            }
            isActive = false;
            OnComplete?.Invoke(this);
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
        }
        IParser<T> parser;
        public IParser<T> Parser
        {
            get { return parser; }
            set { parser = value; }
        }

        IParserSettings parserSettings;
        public IParserSettings ParserSettings
        {
            get { return parserSettings; }
            set { parserSettings = value; loader = new HtmlLoader(parserSettings); }
        }
    }

}
