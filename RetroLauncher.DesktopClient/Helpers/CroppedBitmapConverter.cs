using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RetroLauncher.DesktopClient.Helpers
{
    public class CroppedBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var split = value.ToString().Split('/').Last();
                var dirs = split.Split('\\');
                var incDir = "";
                for(int i=0;i<dirs.Count()-1;i++)
                {
                    incDir += (i > 0 ? "\\" : "")+dirs[i];
                    if (!System.IO.Directory.Exists(incDir))
                        System.IO.Directory.CreateDirectory(incDir);
                }

                string file = "";
                for (int i = 0; i < dirs.Count(); i++)
                {
                    file += (i > 0 ? "\\":"")+ dirs[i];
                }

                using (var client = new System.Net.Http.HttpClient())
                {
                    var stream = client.GetByteArrayAsync(new Uri(value.ToString())).Result;
                    System.IO.File.WriteAllBytes(file, stream);
                    //  client.DownloadFile(new Uri(value.ToString()), file);
                }


                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(file as string, UriKind.Relative);
                image.EndInit();

                CroppedBitmap chunk = new CroppedBitmap(image, new Int32Rect(image.PixelWidth / 4, image.PixelHeight / 4, image.PixelWidth / 2, image.PixelHeight / 3));

                return chunk;
            }
            catch ( Exception ex)
            { System.IO.File.WriteAllText("logconv.txt", ex.Message); }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
