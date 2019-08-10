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
        int gameId;
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        public string Name { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }        
        //public string Platform{ get; set; }
        public string Genre { get; set; }
        public int? Year { get; set; }
        public string Developer { get; set; }

        string annotation;
        public string Annotation
        {
            get { return annotation; }
            set { if (value.Length > 1000) annotation = value.Substring(0, 1000); else annotation = value; }
        }

        public Platform Platform { get; set; }

        private List<GameLink> gameLinks { get; set; }
        public List<GameLink> GameLinks
        {
            get { return gameLinks; }
            set
            {
                gameLinks = value;
             //   LoadLinks(value); // игнорируем возвращаемое значение
             //   OnPropertyChanged(); // загрузка произойдёт в фоне}
            }
        }

        private string imgUrl;
        public string ImgUrl
        {
            get
            {
               
                if (GameLinks != null && string.IsNullOrEmpty(imgUrl))
                {
                    TaskAwaiter<string> awaiter = Services.RepositoryImage.GET(GameLinks.Where(lnk => lnk.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url).GetAwaiter();
                    awaiter.OnCompleted(() =>
                    {
                        imgUrl = awaiter.GetResult();
                        OnPropertyChanged("ImgUrl");
                    });
                }
                return imgUrl;
            }
        }

        private void UpdateLink()
        {
            OnPropertyChanged("ImgUrl");
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
