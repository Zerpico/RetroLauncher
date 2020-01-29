

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using RetroLauncher.Model;

namespace RetroLauncher.Service
{
    public class FileDownloader : INotifyPropertyChanged
    {
        /// <summary>
        /// Выбранная игра для скачивания
        /// </summary>
        /// <value></value>
        public Game CurrentDownloadGame { get; set; }

        public delegate void CompleteProgressAgrs ();
        public event  CompleteProgressAgrs DownloadComplete;

        //сам клиент для скачивания
        public WebClient webClient = new WebClient();

        //таймер для измерения скорости скачивания (возможно не нужен вовсе)
        public Stopwatch stopWatch = new Stopwatch();

        internal string DownloadGame(Game gameToDownload)
        {
            //ProgressChanged?.Invoke(5, 0, string.Empty); //имитация бурной деятельности
            progress.Report((5, string.Empty));
            string pathDown = string.Empty;


            this.CurrentDownloadGame = gameToDownload;
            if (webClient.IsBusy)
                throw new Exception("The client is busy");

            //начала таймера
            var startDownloading = DateTime.UtcNow;

            //т.к. у нас всё через жопу то сначала получим все настройки прокси для клиента

            //сначала определем тип прокси из настроек
            TypeProxy typeProxy = Service.Storage.Source.GetValue<TypeProxy>("ProxyType");

            if (typeProxy != TypeProxy.Default) //если выбрали http или еще какую дрянь, пока только http поддержка
            {
                //определим настройки для прокси
                WebProxy proxy = new WebProxy();

                string proxyHost = Service.Storage.Source.GetValue("ProxyHost").ToString();
                string proxyPort = Service.Storage.Source.GetValue("ProxyPort").ToString();
                proxy.Address = new Uri($"http://{proxyHost}:{proxyPort}");
                proxy.BypassProxyOnLocal = false;
                proxy.UseDefaultCredentials = false;

                webClient.Proxy = proxy;
            }
            else //иначе системные выбираем
            {
                webClient.Credentials = CredentialCache.DefaultCredentials;
            }

            //следует определить в какую папку заливать файл
            var filePath = SelectFolder();
            if (string.IsNullOrEmpty(filePath))
            {
                DownloadingError();
                return null;
            }

            //начинаем отсчёт, для вычисления скорости и всё такое. хотя хз зачем нам скорость загрузки 300 кб
            stopWatch.Start();

            //прогресс загрузки
            webClient.DownloadProgressChanged += (o, args) =>
            {
                //ProgressChanged?.Invoke(args.ProgressPercentage, args.BytesReceived, PrettyBytes(args.BytesReceived));
                progress.Report((args.ProgressPercentage, PrettyBytes(args.BytesReceived)));
            };

            webClient.DownloadFileCompleted += (o, args) =>
            {
                progress.Report((0, ""));
                DownloadComplete();
            };

            pathDown = filePath + Path.GetExtension(CurrentDownloadGame.RomUrl);
            //запуск асинхроного скачивания
            webClient.DownloadFileAsync(new Uri(CurrentDownloadGame.RomUrl), pathDown);

            return pathDown;

        }


        internal void DownloadFile(string url, string pathToLoad)
        {
            //ProgressChanged?.Invoke(5, 0, string.Empty); //имитация бурной деятельности
            progress.Report((5, string.Empty));

            this.CurrentDownloadGame = null;
            if (webClient.IsBusy)
                throw new Exception("The client is busy");

            //начала таймера
            var startDownloading = DateTime.UtcNow;

            //т.к. у нас всё через жопу то сначала получим все настройки прокси для клиента

            //сначала определем тип прокси из настроек
            TypeProxy typeProxy = Service.Storage.Source.GetValue<TypeProxy>("ProxyType");

            if (typeProxy != TypeProxy.Default) //если выбрали http или еще какую дрянь, пока только http поддержка
            {
                //определим настройки для прокси
                WebProxy proxy = new WebProxy();

                string proxyHost = Service.Storage.Source.GetValue("ProxyHost").ToString();
                string proxyPort = Service.Storage.Source.GetValue("ProxyPort").ToString();
                proxy.Address = new Uri($"http://{proxyHost}:{proxyPort}");
                proxy.BypassProxyOnLocal = false;
                proxy.UseDefaultCredentials = false;

                webClient.Proxy = proxy;
            }
            else //иначе системные выбираем
            {
                webClient.Credentials = CredentialCache.DefaultCredentials;
            }


            //начинаем отсчёт, для вычисления скорости и всё такое. хотя хз зачем нам скорость загрузки 300 кб
            stopWatch.Start();

            //прогресс загрузки
            webClient.DownloadProgressChanged += (o, args) =>
            {
                //ProgressChanged?.Invoke(args.ProgressPercentage, args.BytesReceived, PrettyBytes(args.BytesReceived));
                progress.Report((args.ProgressPercentage, PrettyBytes(args.BytesReceived)));
            };

            webClient.DownloadFileCompleted += (o, args) =>
            {
                if (args.Error != null)
                {
                    System.IO.File.WriteAllText("log.txt",args.Error.ToString()+Environment.NewLine);
                }
                progress.Report((0, ""));
            };




            //запуск асинхроного скачивания
            webClient.DownloadFileAsync(new Uri(url), pathToLoad);

        }

        public void CancelDownloading()
        {
            webClient.CancelAsync();
            webClient.Dispose();
            progress.Report((0, string.Empty));
        }

        /// <summary>
        /// Читаемый вид кол-ва скачаных байт
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string PrettyBytes(double bytes)
        {
            if (bytes < 1024)
                return bytes + "Байт";
            if (bytes < Math.Pow(1024, 2))
                return (bytes / 1024).ToString("F" + 2) + "Кб";
            if (bytes < Math.Pow(1024, 3))
                return (bytes / Math.Pow(1024, 2)).ToString("F" + 2) + "Мб";
            if (bytes < Math.Pow(1024, 4))
                return (bytes / Math.Pow(1024, 5)).ToString("F" + 2) + "Гб";
            return bytes + "Байт";
        }

        private string DownloadingSpeed(long received, TimeSpan time)
        {
            return ((double)received / 1024 / 1024 / time.TotalSeconds).ToString("F" + 2) + " Мб/сек";
        }

        private string DownloadingTime(long received, long total, TimeSpan time)
        {
            var receivedD = (double)received;
            var totalD = (double)total;
            return ((totalD / (receivedD / time.TotalSeconds)) - time.TotalSeconds).ToString("F" + 1) + "сек";
        }


        private void DownloadingError() => this.ErrorMessage = "Downloading Error";

        /// <summary>
        /// Определить директорию для скачивания и создать её если не существует
        /// </summary>
        /// <returns></returns>
        private string SelectFolder()
        {
            //создаем директории для скачивания
            if (!Directory.Exists(Path.Combine(Storage.Source.PathGames, CurrentDownloadGame.Platform.Alias)))
                Directory.CreateDirectory(Path.Combine(Storage.Source.PathGames, CurrentDownloadGame.Platform.Alias));

            if (!Directory.Exists(Path.Combine(Storage.Source.PathGames, CurrentDownloadGame.Platform.Alias, CurrentDownloadGame.GameId + "-" + CurrentDownloadGame.Name)))
                Directory.CreateDirectory(Path.Combine(Storage.Source.PathGames, CurrentDownloadGame.Platform.Alias, CurrentDownloadGame.GameId + "-" + CurrentDownloadGame.Name));

            //возвращаем путь куда сохранять
            return Path.Combine(Storage.Source.PathGames, CurrentDownloadGame.Platform.Alias, CurrentDownloadGame.GameId +
                                "-" + CurrentDownloadGame.Name, CurrentDownloadGame.Name);
        }


        private string errorMessage;

        private IProgress<(int progress, string bytes)> progress;
        public FileDownloader(IProgress<(int progress, string bytes)> progress)
        {
            this.progress = progress;
        }

        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                if (value == this.errorMessage) return;
                this.errorMessage = value;
                OnPropertyChanged();
            }
        }

        #region PropertyChanged
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}