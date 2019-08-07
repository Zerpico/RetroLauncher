using RomsDownloader.BaseParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomsDownloader
{
    class Program
    {
        static List<IGame> games;

        static void Main(string[] args)
        {
            Start();

            Console.ReadKey();
        }

        public static void Start()
        {
            Worker();
        }


        //Начинаю всё парсить и скачивать
        public static void Worker()
        {
            //await Task.Factory.StartNew(() => { Storage.Load(); System.Threading.Thread.Sleep(100); Executer.OnUIThread(() => { TextLog = "Создан settings.xml"; }); });
            var emupars = new BaseSettings();
           


            var parser = new ParserWorker<IGame[]>(new BaseParse(), emupars);

            parser.OnComplete += Parser_OnComplete;
            parser.OnNewData += Parser_OnNewData;

            Console.WriteLine("\nСкачивание базы");
            parser.Start();
        }

        

        //При добавлении новых данных об игры и в базу
        private static void Parser_OnNewData(object arg1, IGame[] arg2)
        {
            var countGame = arg2.Count();
            Console.WriteLine("\nОбновлено " + countGame + " игр  для  " + arg2[0].Platform);

            games.AddRange(arg2);
           
        }

        //при обновлении всей базы игр
        private static void Parser_OnComplete(object obj)
        {
            Console.WriteLine("\nБаза полностью обновлена!");
            Console.WriteLine("Количество записей: " + games.Count.ToString());
        }


    }
}
