using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RomsDownloader
{
    public class Game : IGame, INotifyPropertyChanged
    {
        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string platform;
        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string secondName;
        public string SecondName
        {
            get { return secondName; }
            set { secondName = value; }
        }

        string otherName;
        public string OtherName
        {
            get { return otherName; }
            set { otherName = value; }
        }

        int? year;
        public int? Year
        {
            get { return year; }
            set { year = value; }
        }

        string develop;
        public string Developer
        {
            get { return develop; }
            set { develop = value; }
        }

        string genre;
        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        int? players;
        public int? Players
        {
            get { return players; }
            set { players = value; }
        }

        string annotation;
        public string Annotation
        {
            get { return annotation; }
            set { if (value.Length > 1000) annotation = value.Substring(0, 1000); else annotation = value; }
        }

        string imgUrl;
        public string ImgUrl
        {
            get { return imgUrl; }
            set { imgUrl = value; }
        }

        string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }


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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
