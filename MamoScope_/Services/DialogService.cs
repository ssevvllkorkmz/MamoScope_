using MaMoScope_.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using MaMoScope_.Core;
using System.Diagnostics.Tracing;

namespace MaMoScope_.Services
{
    public class DialogService : IDialogService
    {
        public event Action<string, bool> OnShowDialog;
        public event Action<bool> OnLoadingChanged;
        public event Action OnCloseDialog;
        public void ShowResultDialog(string message,bool IsSuccessed)
        {
            OnShowDialog?.Invoke(message, IsSuccessed);
        }

        public void CloseDialog()
        {
            OnCloseDialog?.Invoke();
        }

        public void ShowLoading()
        {
            OnLoadingChanged?.Invoke(true);
        }
        public void HideLoading()
        {
            OnLoadingChanged?.Invoke(false);
        }

    }
}
