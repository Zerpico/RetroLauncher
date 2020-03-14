using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Client.Interfaces
{
    public interface IDialogAware
    {
        bool CanCloseDialog();
        void OnDialogClosed();
        void OnDialogOpened(IDialogParameters parameters);
        string Title { get; set; }
        event Action<IDialogResult> RequestClose;
    }
}
