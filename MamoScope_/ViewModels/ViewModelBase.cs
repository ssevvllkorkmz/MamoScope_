using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.ViewModels
{
    [ObservableObject]
    public partial class ViewModelBase : IDisposable
    {
        public virtual void Dispose() { }
    }
}
