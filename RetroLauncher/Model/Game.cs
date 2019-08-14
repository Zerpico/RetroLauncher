using GalaSoft.MvvmLight.Ioc;
using RetroLauncher.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Model
{
    public class Game : INotifyPropertyChanged
    {
        /*public Game()
        {
            GameLinks = new List<GameLink>();
        }
        */
        public int GameId { get; set; }

        public string Name { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }
        //public string Platform{ get; set; }
        public string Genre { get; set; }
        public int? Year { get; set; }
        public string Developer { get; set; }

        private string _annotation;
        public string Annotation
        {
            get { return _annotation; }
            set
            {
                _annotation = value.Length > 1000 ? value.Substring(0, 1000) : value;
            }
        }

        public Platform Platform { get; set; }
        public List<GameLink> GameLinks { get; set; }

        private string imgUrl;
        public string ImgUrl
        {
            get
            {
                if (GameLinks != null && string.IsNullOrEmpty(imgUrl))
                {
                    var fireUrlService = SimpleIoc.Default.GetInstance<IFileUrlService>();
                    var file = GameLinks.Where(lnk => lnk.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url;

                    var awaiter = fireUrlService.GetFileDirectUrl(file).GetAwaiter();
                    awaiter.OnCompleted(() =>
                    {
                        imgUrl = awaiter.GetResult();
                        OnPropertyChanged(nameof(ImgUrl));
                    });
                }
                return imgUrl;
            }
        }

        private void UpdateLink()
        {
            OnPropertyChanged(nameof(ImgUrl));
        }

        /*async Task LoadLinks(List<GameLink> value)
        {
            try
            {
                IsCurrentPageEditable = false;
                // Получение данных
                for(int i=0;i<value.Count;i++)
                {
                    value[i].UrlRemote = await Services.RepositoryImage.GET(value[i].Url);
                }

                OnPropertyChanged("ImgUrl");
            }
            catch (Exception)
            {
                // тут обработка
            }
            finally
            {
                IsCurrentPageEditable = true;
            }
        }
        */
        bool _isCurrentPageEditable = true;
        public bool IsCurrentPageEditable
        {
            get { return _isCurrentPageEditable; }
            private set { _isCurrentPageEditable = value; OnPropertyChanged(); }
        }


        /*  public string ImageUrl
          {
              get {  return Services.RepositoryImage.GET(GameLinks.Where(lnk => lnk.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url).GetAwaiter().on }            
          }

          async Task<string> GetImgUrl()
          {
              return Services.RepositoryImage.GET(GameLinks.Where(lnk => lnk.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url);
          }
          */
        /*
        int progressDownload { get; set; }
        public int ProgressDownload
        {
            get { return progressDownload; }
            set { progressDownload = value; OnPropertyChanged("ProgressDownload"); }
        }

        bool downloaded { get; set; }
        public bool Downloaded
        {
            get { return downloaded; }
            set { downloaded = value; OnPropertyChanged("Downloaded"); }
        }
        */
        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
