using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputToolbox
{
    internal class ClickerViewModel : ObservableObject, IDisposable
    {
        public ClickerViewModel()
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
