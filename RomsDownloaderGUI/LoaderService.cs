using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace RomsDownloaderGUI
{
    public class LoaderService
    {
        /*public static async Task<Stream> DownloadFile(string address, string location)
        {
            WebClient client = new WebClient();
            Uri Uri = new Uri(address);   
            MemoryStream stream = new MemoryStream(await client.DownloadDataTaskAsync(Uri));
            return stream;
        }*/

        /*public static  void ExtractZip(string fileName, string outFolder)
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);
            using (var zip = ZipFile.Read(fileName))
            {
                zip.ExtractAll(outFolder, ExtractExistingFileAction.OverwriteSilently);
            }

        }*/

       

        public event Action<bool> OnComplete;
        public event Action<long, long, int> OnProgress;

        private volatile bool _completed;

        public void DownloadFile(string address, string location)
        {
            WebClient client = new WebClient();
            Uri Uri = new Uri(address);
            _completed = false;

            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            client.DownloadFileAsync(Uri, location);

        }

        public bool DownloadCompleted { get { return _completed; } }

        int LastBytesDown = 0;
        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.BytesReceived - LastBytesDown > 1024 * 4)
            {
                OnProgress?.Invoke(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
                LastBytesDown = (int)e.BytesReceived;
            }
        }


        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            _completed = true;
            OnComplete?.Invoke(!e.Cancelled);
        }
    }

}
