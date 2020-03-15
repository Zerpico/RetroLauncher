using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace RetroLauncher.ServiceTools.Download
{
    public class WebDownloadClient : INotifyPropertyChanged
    {
        #region Fields and Properties

        /// <summary>
        /// Имя файла
        /// </summary>
        /// <value></value>
        public string FileName { get; set; }

        public string ShortFileName { get => Url.ToString().Substring(Url.ToString().LastIndexOf("/") + 1); }

        /// <summary>
        /// URL файла для скачивания
        /// </summary>
        /// <value></value>
        public Uri Url { get; private set; }

        // File type (extension)
        public string FileType
        {
            get
            {
                return Url.ToString().Substring(Url.ToString().LastIndexOf('.') + 1).ToUpper();
            }
        }

        // Username and password for accessing the HTTP server
        public NetworkCredential ServerLogin = null;

        /// <summary>
        /// HTTP proxy
        /// </summary>
        public WebProxy Proxy = null;

        /// <summary>
        /// Поток процесса скачивания
        /// </summary>
        public Thread DownloadThread = null;

        /// <summary>
        /// Временный путь для скачивания
        /// </summary>
        /// <value></value>
        public string TempDownloadPath { get; set; }

        /// <summary>
        /// Путь скачивания файла
        /// </summary>
        /// <value></value>
        public string DownloadPath
        {
            get
            {
                return this.TempDownloadPath.Remove(this.TempDownloadPath.Length - 4);
            }
        }

        /// <summary>
        /// Локальная папка, в которой находится файл
        /// </summary>
        /// <value></value>
        public string DownloadFolder
        {
            get
            {
                return this.TempDownloadPath.Remove(TempDownloadPath.LastIndexOf("\\") + 1);
            }
        }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        /// <value></value>
        public long FileSize { get; set; }

        /// <summary>
        /// Размер файла в читаемом виде
        /// </summary>
        /// <value></value>
        public string FileSizeString
        {
            get
            {
                return DownloadManager.FormatSizeString(FileSize);
            }
        }

        /// <summary>
        /// Размер скачаного файла, который записан в локальный файл
        /// </summary>
        /// <value></value>
        public long DownloadedSize { get; set; }
        public string DownloadedSizeString
        {
            get
            {
                return DownloadManager.FormatSizeString(DownloadedSize + CachedSize);
            }
        }

        /// <summary>
        /// Процент скачаного файла
        /// </summary>
        /// <value></value>
        public float Percent
        {
            get
            {
                return ((float)(DownloadedSize + CachedSize) / (float)FileSize) * 100F;
            }
        }

        public string PercentString
        {
            get
            {
                if (Percent < 0 || float.IsNaN(Percent))
                    return "0.0%";
                else if (Percent > 100)
                    return "100.0%";
                else
                    return String.Format(numberFormat, "{0:0.0}%", Percent);
            }
        }

        /// <summary>
        /// Прогресс скачивания от 0 до 100, для индикаторва (прогресс бара)
        /// </summary>
        /// <value></value>
        public float Progress
        {
            get
            {
                return Percent;
            }
        }

        /// <summary>
        /// Скорость скачивания
        /// </summary>
        public int downloadSpeed;
        public string DownloadSpeed
        {
            get
            {
                if (this.Status == DownloadStatus.Downloading && !this.HasError)
                {
                    return DownloadManager.FormatSpeedString(downloadSpeed);
                }
                return String.Empty;
            }
        }

        // Used for updating download speed on the DataGrid
        private int speedUpdateCount;
/*
        // Average download speed
        public string AverageDownloadSpeed
        {
            get
            {
                return DownloadManager.FormatSpeedString((int)Math.Floor((double)(DownloadedSize + CachedSize) / TotalElapsedTime.TotalSeconds));
            }
        }

        // Список значений скорости загрузки за последние 10 секунд
        private List<int> downloadRates = new List<int>();
*/
        // Average download speed in the last 10 seconds, used for calculating the time left to complete the download
        private int recentAverageRate;

        // Time left to complete the download
        public string TimeLeft
        {
            get
            {
                if (recentAverageRate > 0 && this.Status == DownloadStatus.Downloading && !this.HasError)
                {
                    double secondsLeft = (FileSize - DownloadedSize + CachedSize) / recentAverageRate;

                    TimeSpan span = TimeSpan.FromSeconds(secondsLeft);

                    return DownloadManager.FormatTimeSpanString(span);
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Статус скачивания
        /// </summary>
        private DownloadStatus status;
        public DownloadStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (status != DownloadStatus.Deleting)
                    RaiseStatusChanged();
            }
        }

        // Status text in the DataGrid
        public string StatusText;
        public string StatusString
        {
            get
            {
                if (this.HasError)
                    return StatusText;
                else
                    return Status.ToString();
            }
            set
            {
                StatusText = value;
                RaiseStatusChanged();
            }
        }


        public TimeSpan ElapsedTime = new TimeSpan();


        private DateTime lastStartTime;

/*        // Total elapsed time (includes the time period when the download was paused)
        public TimeSpan TotalElapsedTime
        {
            get
            {
                if (this.Status != DownloadStatus.Downloading)
                {
                    return ElapsedTime;
                }
                else
                {
                    return ElapsedTime.Add(DateTime.UtcNow - lastStartTime);
                }
            }
        }

        public string TotalElapsedTimeString
        {
            get
            {
                return DownloadManager.FormatTimeSpanString(TotalElapsedTime);
            }
        }

 */       // Time and size of downloaded data in the last calculaction of download speed
        private DateTime lastNotificationTime;
        private long lastNotificationDownloadedSize;

        // Last update time of the DataGrid item
        public DateTime LastUpdateTime { get; set; }

        // Date and time when the download was added to the list
        public DateTime AddedOn { get; set; }
        public string AddedOnString
        {
            get
            {
                string format = "dd.MM.yyyy. HH:mm:ss";
                return AddedOn.ToString(format);
            }
        }

        // Date and time when the download was completed
        public DateTime CompletedOn { get; set; }
        public string CompletedOnString
        {
            get
            {
                if (CompletedOn != DateTime.MinValue)
                {
                    string format = "dd.MM.yyyy. HH:mm:ss";
                    return CompletedOn.ToString(format);
                }
                else
                    return String.Empty;
            }
        }
        public int Id { get;set; }

        // Server supports the Range header (resuming the download)
        public bool SupportsRange { get; set; }

        // There was an error during download
        public bool HasError { get; set; }

        public string ErrorMessage { get;set; }

     
        // Temporary file was created
        public bool TempFileCreated { get; set; }

        // Download is selected in the DataGrid
//        public bool IsSelected { get; set; }

        // Download is part of a batch
        public bool IsBatch { get; set; }

        // Batch URL was checked
        public bool BatchUrlChecked { get; set; }

        // Speed limit was changed
//        public bool SpeedLimitChanged { get; set; }

        // Download buffer count per notification (DownloadProgressChanged event)
        public int BufferCountPerNotification { get; set; }

        // Buffer size
        public int BufferSize { get; set; }

        // Size of downloaded data in the cache memory
        public int CachedSize { get; set; }

        // Maxiumum cache size
        public int MaxCacheSize { get; set; }

        // Number format with a dot as the decimal separator
        private NumberFormatInfo numberFormat = NumberFormatInfo.InvariantInfo;

        // Used for blocking other processes when a file is being created or written to
        private static object fileLocker = new object();

        #endregion

        #region Constructor and Events

        public WebDownloadClient(int id, string url)
        {
            this.Id = id;
            this.BufferSize = 1024; // 1KB
            this.MaxCacheSize = 1024 * 1024; // макс. буффер 1MB
            this.BufferCountPerNotification = 64;

            this.Url = new Uri(url, UriKind.Absolute);

            this.SupportsRange = false;
            this.HasError = false;
            this.TempFileCreated = false;
          //  this.IsSelected = false;
            this.IsBatch = false;
            this.BatchUrlChecked = false;
          //  this.SpeedLimitChanged = false;
            this.speedUpdateCount = 0;
            this.recentAverageRate = 0;
            this.StatusText = String.Empty;

            this.Status = DownloadStatus.Initialized;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler StatusChanged;

        public event EventHandler DownloadProgressChanged;

        public event EventHandler DownloadCompleted;

        #endregion

        #region Event Handlers


        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        protected virtual void RaiseStatusChanged()
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }


        protected virtual void RaiseDownloadProgressChanged()
        {
            DownloadProgressChanged?.Invoke(this, EventArgs.Empty);
        }


        protected virtual void RaiseDownloadCompleted()
        {
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        // DownloadProgressChanged event handler
        public void DownloadProgressChangedHandler(object sender, EventArgs e)
        {
            // Обновляет интерфейс каждую секунду
            if (DateTime.UtcNow > this.LastUpdateTime.AddSeconds(1))
            {
                CalculateDownloadSpeed();
                //CalculateAverageRate();
                UpdateDownloadDisplay();
                this.LastUpdateTime = DateTime.UtcNow;
            }
        }

        // DownloadCompleted event handler
        public void DownloadCompletedAfterAction()//(object sender, EventArgs e)
        {
            if (!this.HasError)
            {
                // If the file already exists, delete it
                if (File.Exists(this.DownloadPath))
                {
                    File.Delete(this.DownloadPath);
                }

                // Convert the temporary (.tmp) file to the actual (requested) file
                if (File.Exists(this.TempDownloadPath))
                {
                    File.Move(this.TempDownloadPath, this.DownloadPath);
                }

                this.Status = DownloadStatus.Completed;
                UpdateDownloadDisplay();
            }
            else
            {
                this.Status = DownloadStatus.Error;
                UpdateDownloadDisplay();
            }
        }

        #endregion

        #region Methods

        // Check URL to get file size, set login and/or proxy server information, check if the server supports the Range header
        public void CheckUrl()
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(this.Url);
                webRequest.Method = "HEAD";
                webRequest.Timeout = 5000;

                if (this.ServerLogin != null)
                {
                    webRequest.PreAuthenticate = true;
                    webRequest.Credentials = this.ServerLogin;
                }
                else
                {
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                }

                //сначала определем тип прокси из настроек
                TypeProxy typeProxy = Storage.Source.GetValue<TypeProxy>("ProxyType");

                if (typeProxy != TypeProxy.Default)
                {
                    this.Proxy = new WebProxy();
                    string proxyHost = Storage.Source.GetValue("ProxyHost").ToString();
                    string proxyPort = Storage.Source.GetValue("ProxyPort").ToString();
                    this.Proxy.Address = new Uri("http://" + proxyHost + ":" + proxyPort);
                    this.Proxy.Credentials = new NetworkCredential();
                }


/*                if (Settings.Default.ManualProxyConfig && Settings.Default.HttpProxy != String.Empty)
                {
                    this.Proxy = new WebProxy();
                    this.Proxy.Address = new Uri("http://" + Settings.Default.HttpProxy + ":" + Settings.Default.ProxyPort);
                    this.Proxy.BypassProxyOnLocal = false;
                    if (Settings.Default.ProxyUsername != String.Empty && Settings.Default.ProxyPassword != String.Empty)
                    {
                        this.Proxy.Credentials = new NetworkCredential(Settings.Default.ProxyUsername, Settings.Default.ProxyPassword);
                    }
                }
*/
                if (this.Proxy != null)
                {
                    webRequest.Proxy = this.Proxy;
                }
                else
                {
                    webRequest.Proxy = WebRequest.DefaultWebProxy;
                }

                using (WebResponse response = webRequest.GetResponse())
                {
                    foreach (var header in response.Headers.AllKeys)
                    {
                        if (header.Equals("Accept-Ranges", StringComparison.OrdinalIgnoreCase))
                        {
                            this.SupportsRange = true;
                        }
                    }

                    this.FileSize = response.ContentLength;

                    if (this.FileSize <= 0)
                    {
                        ErrorMessage = "The requested file does not exist!";
                        RaisePropertyChanged("ErrorMessage");
                        this.HasError = true;
                    }
                }
            }
            catch (Exception)
            {
                ErrorMessage = "There was an error while getting the file information. Please make sure the URL is accessible.";
                RaisePropertyChanged("ErrorMessage");
                this.HasError = true;
            }
        }

        // Batch download URL check
        private void CheckBatchUrl()
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(this.Url);
            webRequest.Method = "HEAD";

            if (this.ServerLogin != null)
            {
                webRequest.PreAuthenticate = true;
                webRequest.Credentials = this.ServerLogin;
            }
            else
            {
                webRequest.Credentials = CredentialCache.DefaultCredentials;
            }

             //сначала определем тип прокси из настроек
            TypeProxy typeProxy = Storage.Source.GetValue<TypeProxy>("ProxyType");

            if (typeProxy != TypeProxy.Default)
            {
                this.Proxy = new WebProxy();
                string proxyHost = Storage.Source.GetValue("ProxyHost").ToString();
                string proxyPort = Storage.Source.GetValue("ProxyPort").ToString();
                this.Proxy.Address = new Uri("http://" + proxyHost + ":" + proxyPort);
                this.Proxy.Credentials = new NetworkCredential();
            }


/*          if (Settings.Default.ManualProxyConfig && Settings.Default.HttpProxy != String.Empty)
            {
                this.Proxy = new WebProxy();
                this.Proxy.Address = new Uri("http://" + Settings.Default.HttpProxy + ":" + Settings.Default.ProxyPort);
                this.Proxy.BypassProxyOnLocal = false;
                if (Settings.Default.ProxyUsername != String.Empty && Settings.Default.ProxyPassword != String.Empty)
                {
                    this.Proxy.Credentials = new NetworkCredential(Settings.Default.ProxyUsername, Settings.Default.ProxyPassword);
                }
            }
*/
            if (this.Proxy != null)
            {
                webRequest.Proxy = this.Proxy;
            }
            else
            {
                webRequest.Proxy = WebRequest.DefaultWebProxy;
            }

            using (WebResponse response = webRequest.GetResponse())
            {
                foreach (var header in response.Headers.AllKeys)
                {
                    if (header.Equals("Accept-Ranges", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SupportsRange = true;
                    }
                }

                this.FileSize = response.ContentLength;

                if (this.FileSize <= 0)
                {
                    this.StatusString = "Error: The requested file does not exist";
                    this.FileSize = 0;
                    this.HasError = true;
                }

                RaisePropertyChanged("FileSizeString");
            }
        }

        // Create temporary file
        void CreateTempFile()
        {
            // Lock this block of code so other threads and processes don't interfere with file creation
            lock (fileLocker)
            {
                using (FileStream fileStream = File.Create(this.TempDownloadPath))
                {
                    long createdSize = 0;
                    byte[] buffer = new byte[4096];
                    while (createdSize < this.FileSize)
                    {
                        int bufferSize = (this.FileSize - createdSize) < 4096
                            ? (int)(this.FileSize - createdSize) : 4096;
                        fileStream.Write(buffer, 0, bufferSize);
                        createdSize += bufferSize;
                    }
                }
            }
        }

        // Write data from the cache to the temporary file
        void WriteCacheToFile(MemoryStream downloadCache, int cachedSize)
        {
            // Block other threads and processes from using the file
            lock (fileLocker)
            {
                using (FileStream fileStream = new FileStream(TempDownloadPath, FileMode.Open))
                {
                    byte[] cacheContent = new byte[cachedSize];
                    downloadCache.Seek(0, SeekOrigin.Begin);
                    downloadCache.Read(cacheContent, 0, cachedSize);
                    fileStream.Seek(DownloadedSize, SeekOrigin.Begin);
                    fileStream.Write(cacheContent, 0, cachedSize);
                }
            }
        }

        // Calculate download speed
        private void CalculateDownloadSpeed()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan interval = now - lastNotificationTime;
            double timeDiff = interval.TotalSeconds;
            double sizeDiff = (double)(DownloadedSize + CachedSize - lastNotificationDownloadedSize);

            downloadSpeed = (int)Math.Floor(sizeDiff / timeDiff);

      //      downloadRates.Add(downloadSpeed);

            lastNotificationDownloadedSize = DownloadedSize + CachedSize;
            lastNotificationTime = now;
        }

        // Calculate average download speed in the last 10 seconds
    /*    private void CalculateAverageRate()
        {
            if (downloadRates.Count > 0)
            {
                if (downloadRates.Count > 10)
                    downloadRates.RemoveAt(0);

                int rateSum = 0;
                recentAverageRate = 0;
                foreach (int rate in downloadRates)
                {
                    rateSum += rate;
                }

                recentAverageRate = rateSum / downloadRates.Count;
            }
        }
*/
        // Update download display (on downloadsGrid and propertiesGrid controls)
        private void UpdateDownloadDisplay()
        {
            RaisePropertyChanged("DownloadedSizeString");
            RaisePropertyChanged("PercentString");
            RaisePropertyChanged("Progress");

            // New download speed update every 4 seconds
            TimeSpan startInterval = DateTime.UtcNow - lastStartTime;
            if (speedUpdateCount == 0 || startInterval.TotalSeconds < 4 || this.HasError //|| this.Status == DownloadStatus.Paused
                || this.Status == DownloadStatus.Queued || this.Status == DownloadStatus.Completed)
            {
                RaisePropertyChanged("DownloadSpeed");
            }
            speedUpdateCount++;
            if (speedUpdateCount == 4)
                speedUpdateCount = 0;

            RaisePropertyChanged("TimeLeft");
            RaisePropertyChanged("StatusString");
            RaisePropertyChanged("CompletedOnString");
        }

        // Reset download properties to default values
        private void ResetProperties()
        {
            HasError = false;
            TempFileCreated = false;
            DownloadedSize = 0;
            CachedSize = 0;
            speedUpdateCount = 0;
            recentAverageRate = 0;
         //   downloadRates.Clear();
            ElapsedTime = new TimeSpan();
            CompletedOn = DateTime.MinValue;
        }

        // Start or continue download
        public void Start()
        {
            if (this.Status == DownloadStatus.Initialized //|| this.Status == DownloadStatus.Paused
                || this.Status == DownloadStatus.Queued || this.HasError)
            {
                if (!this.SupportsRange && this.DownloadedSize > 0)
                {
                    this.StatusString = "Error: Server does not support resume";
                    this.HasError = true;
                    this.RaiseDownloadCompleted();
                    return;
                }

                this.HasError = false;
                this.Status = DownloadStatus.Waiting;
                RaisePropertyChanged("StatusString");

                if (DownloadManager.Instance.ActiveDownloads > DownloadManager.Instance.MaxDownloads)
                {
                    this.Status = DownloadStatus.Queued;
                    RaisePropertyChanged("StatusString");
                    return;
                }

                // Start the download thread
                DownloadThread = new Thread(new ThreadStart(DownloadFile));
                DownloadThread.IsBackground = true;
                DownloadThread.Start();
            }
        }
/*
        // Pause download
        public void Pause()
        {
            if (this.Status == DownloadStatus.Waiting || this.Status == DownloadStatus.Downloading)
            {
                this.Status = DownloadStatus.Pausing;
            }
            if (this.Status == DownloadStatus.Queued)
            {
                this.Status = DownloadStatus.Paused;
                RaisePropertyChanged("StatusString");
            }
        }
*/
        // Restart download
        public void Restart()
        {
            if (this.HasError || this.Status == DownloadStatus.Completed)
            {
                if (File.Exists(this.TempDownloadPath))
                {
                    File.Delete(this.TempDownloadPath);
                }
                if (File.Exists(this.DownloadPath))
                {
                    File.Delete(this.DownloadPath);
                }

                ResetProperties();
                this.Status = DownloadStatus.Waiting;
                UpdateDownloadDisplay();

                if (DownloadManager.Instance.ActiveDownloads > DownloadManager.Instance.MaxDownloads)
                {
                    this.Status = DownloadStatus.Queued;
                    RaisePropertyChanged("StatusString");
                    return;
                }

                DownloadThread = new Thread(new ThreadStart(DownloadFile));
                DownloadThread.IsBackground = true;
                DownloadThread.Start();
            }
        }

        // Download file bytes from the HTTP response stream
        private void DownloadFile()
        {
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            Stream responseStream = null;
            ThrottledStream throttledStream = null;
            MemoryStream downloadCache = null;
            speedUpdateCount = 0;
            recentAverageRate = 0;
          /*  if (downloadRates.Count > 0)
                downloadRates.Clear();*/

            try
            {
                if (this.IsBatch && !this.BatchUrlChecked)
                {
                    CheckBatchUrl();
                    if (this.HasError)
                    {
                        this.RaiseDownloadCompleted();
                        return;
                    }
                    this.BatchUrlChecked = true;
                }

                if (!TempFileCreated)
                {
                    // Reserve local disk space for the file
                    CreateTempFile();
                    this.TempFileCreated = true;
                }

                this.lastStartTime = DateTime.UtcNow;

                if (this.Status == DownloadStatus.Waiting)
                    this.Status = DownloadStatus.Downloading;

                // Create request to the server to download the file
                webRequest = (HttpWebRequest)WebRequest.Create(this.Url);
                webRequest.Method = "GET";

                if (this.ServerLogin != null)
                {
                    webRequest.PreAuthenticate = true;
                    webRequest.Credentials = this.ServerLogin;
                }
                else
                {
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                }

                if (this.Proxy != null)
                {
                    webRequest.Proxy = this.Proxy;
                }
                else
                {
                    webRequest.Proxy = WebRequest.DefaultWebProxy;
                }

                // Set download starting point
                webRequest.AddRange(DownloadedSize);

                webRequest.Timeout = 5000;
                // Get response from the server and the response stream
                webResponse = (HttpWebResponse)webRequest.GetResponse();

                responseStream = webResponse.GetResponseStream();

                // Set a 5 second timeout, in case of internet connection break
                //responseStream.ReadTimeout = 5000;

                // Set speed limit
               /* long maxBytesPerSecond = 0;
                if (Settings.Default.EnableSpeedLimit)
                {
                    maxBytesPerSecond = (long)((Settings.Default.SpeedLimit * 1024) / DownloadManager.Instance.ActiveDownloads);
                }
                else
                {*/
                   long maxBytesPerSecond = ThrottledStream.Infinite;
               // }
                throttledStream = new ThrottledStream(responseStream, maxBytesPerSecond);

                // Create memory cache with the specified size
                downloadCache = new MemoryStream(this.MaxCacheSize);

                // Create 1KB buffer
                byte[] downloadBuffer = new byte[this.BufferSize];

                int bytesSize = 0;
                CachedSize = 0;
                int receivedBufferCount = 0;

                // Download file bytes until the download is paused or completed
                while (true)
                {
                    /*if (SpeedLimitChanged)
                    {
                        if (Settings.Default.EnableSpeedLimit)
                        {
                            maxBytesPerSecond = (long)((Settings.Default.SpeedLimit * 1024) / DownloadManager.Instance.ActiveDownloads);
                        }
                        else
                        {
                            maxBytesPerSecond = ThrottledStream.Infinite;
                        }
                        throttledStream.MaximumBytesPerSecond = maxBytesPerSecond;
                        SpeedLimitChanged = false;
                    }*/

                    // Read data from the response stream and write it to the buffer
                    bytesSize = throttledStream.Read(downloadBuffer, 0, downloadBuffer.Length);

                    // If the cache is full or the download is paused or completed, write data from the cache to the temporary file
                    if (this.Status != DownloadStatus.Downloading || bytesSize == 0 || this.MaxCacheSize < CachedSize + bytesSize)
                    {
                        // Write data from the cache to the temporary file
                        WriteCacheToFile(downloadCache, CachedSize);

                        this.DownloadedSize += CachedSize;

                        // Reset the cache
                        downloadCache.Seek(0, SeekOrigin.Begin);
                        CachedSize = 0;

                        // Stop downloading the file if the download is paused or completed
                        if (this.Status != DownloadStatus.Downloading || bytesSize == 0)
                        {
                            break;
                        }
                    }

                    // Write data from the buffer to the cache
                    downloadCache.Write(downloadBuffer, 0, bytesSize);
                    CachedSize += bytesSize;

                    receivedBufferCount++;
                    if (receivedBufferCount == this.BufferCountPerNotification)
                    {
                        this.RaiseDownloadProgressChanged();
                        receivedBufferCount = 0;
                    }
                }

                // Update elapsed time when the download is paused or completed
                ElapsedTime = ElapsedTime.Add(DateTime.UtcNow - lastStartTime);

                // Change status
                if (this.Status != DownloadStatus.Deleting)
                {
                    /*if (this.Status == DownloadStatus.Pausing)
                    {
                        this.Status = DownloadStatus.Paused;
                        UpdateDownloadDisplay();
                    }
                    else */if (this.Status == DownloadStatus.Queued)
                    {
                        UpdateDownloadDisplay();
                    }
                    else
                    {
                        this.CompletedOn = DateTime.UtcNow;
                        DownloadCompletedAfterAction();
                        this.RaiseDownloadCompleted();
                        //DownloadCompletedHandler(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                // Show error in the status
                this.StatusString = "Error: " + ex.Message;
                this.HasError = true;
                this.RaiseDownloadCompleted();
            }
            finally
            {
                // Close the response stream and cache, stop the thread
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (throttledStream != null)
                {
                    throttledStream.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
                if (downloadCache != null)
                {
                    downloadCache.Close();
                }
                /*if (DownloadThread != null)
                {
                    DownloadThread.Abort();
                }*/
            }
        }

        #endregion
    }
}