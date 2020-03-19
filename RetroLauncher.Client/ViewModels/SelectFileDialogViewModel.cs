using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RetroLauncher.Client.ViewModels
{
    public class SelectFileDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private FileInfo[] _files;
        public FileInfo[] Files
        {
            get 
            { 
                return isHackShowChecked ? _files : ShowHackedList(); 
            }
        }

        private bool isHackShowChecked;
        public bool IsHackShowChecked
        {
            get { return isHackShowChecked; }
            set { SetProperty(ref isHackShowChecked, value); RaisePropertyChanged(nameof(Files)); }
        }

        private FileInfo[] ShowHackedList()
        {
            if (_files == null) return null;
            
            var r = new System.Text.RegularExpressions.Regex(@"(\([\w\s]*hack[\w\s]*\))",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

            return _files.Where(g => !r.IsMatch(g.Name)).ToArray();
        }

        private FileInfo _selectedFile;
        public FileInfo SelectedFile
        {
            get { return _selectedFile; }
            set 
            { 
                SetProperty(ref _selectedFile, value);
                if (_selectedFile == null) return;
                CloseDialog(_selectedFile.FullName);
            }
        }
        

        private string _title = "Выберите файл для запуска";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public event Action<IDialogResult> RequestClose;

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.OK;

            RaiseRequestClose(new DialogResult(result, new DialogParameters($"file={parameter}")));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            var path = parameters.GetValue<string>("path");
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            _files = dir.GetFiles();
            RaisePropertyChanged(nameof(Files));
        }
    }
   
}
