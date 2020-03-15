using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.ServiceTools.Download
{
    public class DownloadManager
    {
        // Для образца синглтона
        private static DownloadManager instance = new DownloadManager();

        public static DownloadManager Instance
        {
            get
            {
                return instance;
            }
        }

        private static NumberFormatInfo numberFormat = NumberFormatInfo.InvariantInfo;

        // Collection which contains all download clients, bound to the DataGrid control
        public ObservableCollection<WebDownloadClient> DownloadsList = new ObservableCollection<WebDownloadClient>();

        public async void ClearDownload(int id)
        {
            await Task.Delay(2000);
            DownloadsList.Remove(DownloadsList.Where(d => d.Id == id).FirstOrDefault());
        }

        #region Properties

        public int MaxDownloads {get {return _maxDownloads;}}

        public const int _maxDownloads = 5;

        /// <summary>
        /// Количество активных загрузок
        /// </summary>
        /// <value></value>
        public int ActiveDownloads
        {
            get
            {
                int active = 0;
                foreach (WebDownloadClient d in DownloadsList)
                {
                    if (!d.HasError)
                        if (d.Status == DownloadStatus.Waiting || d.Status == DownloadStatus.Downloading)
                            active++;
                }
                return active;
            }
        }

        /// <summary>
        /// Количество завершенных загрузок
        /// </summary>
        /// <value></value>
        public int CompletedDownloads
        {
            get
            {
                int completed = 0;
                foreach (WebDownloadClient d in DownloadsList)
                {
                    if (d.Status == DownloadStatus.Completed)
                        completed++;
                }
                return completed;
            }
        }

        /// <summary>
        /// Общее количество загрузок в списке
        /// </summary>
        /// <value></value>
        public int TotalDownloads
        {
            get
            {
                return DownloadsList.Count;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Форматирование размера файла в строку
        /// </summary>
        /// <param name="byteSize"></param>
        /// <returns></returns>
        public static string FormatSizeString(long byteSize)
        {
            double kiloByteSize = (double)byteSize / 1024D;
            double megaByteSize = kiloByteSize / 1024D;
            double gigaByteSize = megaByteSize / 1024D;

            if (byteSize < 1024)
                return String.Format(numberFormat, "{0} B", byteSize);
            else if (byteSize < 1048576)
                return String.Format(numberFormat, "{0:0.00} kB", kiloByteSize);
            else if (byteSize < 1073741824)
                return String.Format(numberFormat, "{0:0.00} MB", megaByteSize);
            else
                return String.Format(numberFormat, "{0:0.00} GB", gigaByteSize);
        }

        /// <summary>
        /// Форматирование скорости загрузки
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static string FormatSpeedString(int speed)
        {
            float kbSpeed = (float)speed / 1024F;
            float mbSpeed = kbSpeed / 1024F;

            if (speed <= 0)
                return String.Empty;
            else if (speed < 1024)
                return speed.ToString() + " B/s";
            else if (speed < 1048576)
                return kbSpeed.ToString("#.00", numberFormat) + " kB/s";
            else
                return mbSpeed.ToString("#.00", numberFormat) + " MB/s";
        }

        /// <summary>
        /// Форматирование времени в читаемую строку
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static string FormatTimeSpanString(TimeSpan span)
        {
            string hours = ((int)span.TotalHours).ToString();
            string minutes = span.Minutes.ToString();
            string seconds = span.Seconds.ToString();
            if ((int)span.TotalHours < 10)
                hours = "0" + hours;
            if (span.Minutes < 10)
                minutes = "0" + minutes;
            if (span.Seconds < 10)
                seconds = "0" + seconds;

            return String.Format("{0}:{1}:{2}", hours, minutes, seconds);
        }

        #endregion
    }
}
