using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomsDownloaderGUI
{
    public interface IGame
    {
        int Id { get; set; }
        string Platform { get; set; }
        string Name { get; set; }
        string SecondName { get; set; }
        string OtherName { get; set; }
        int? Year { get; set; }
        string Developer { get; set; }
        int? Players { get; set; }
        string Genre { get; set; }
        string Annotation { get; set; }
        string Url { get; set; }
        string ImgUrl { get; set; }
        bool Downloaded { get; set; }
    }

}
