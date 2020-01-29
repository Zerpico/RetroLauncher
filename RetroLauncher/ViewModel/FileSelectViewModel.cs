using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RetroLauncher.ViewModel.Base;

namespace RetroLauncher.ViewModel
{
    public class FileSelectViewModel : ViewModelBase
    {
        string DirectoryPath;
        public FileSelectViewModel(string path)
        {
            DirectoryPath = path;

            RomFiles = new List<string>();

            var r = new System.Text.RegularExpressions.Regex(@"(\([\w\s]*hack[\w\s]*\))",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

            foreach(var file in System.IO.Directory.GetFiles(path).Where(g => !r.IsMatch(g)).ToArray())
                RomFiles.Add(System.IO.Path.GetFileName(file));
        }

        public List<string> RomFiles { get;set; }

        public string SelectRom { get;set; }



    }

}