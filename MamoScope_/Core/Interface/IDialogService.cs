using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Core.Interface
{
    public interface IDialogService
    {
        void ShowResultDialog(string message, bool IsSuccessed);
        event Action<string, bool> OnShowDialog;
        event Action OnCloseDialog;

        event Action<bool> OnLoadingChanged;
        void ShowLoading();
        void HideLoading();
        void CloseDialog();

    }
}
