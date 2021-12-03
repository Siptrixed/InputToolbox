using InputToolbox.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputToolbox
{
    public class RemoteViewModel : ObservableObject, IDisposable
    {
        public string IP { get; private set; }
        private HTTPRemote Rem;
        public RemoteViewModel()
        {
            Rem = new HTTPRemote();
        }
        public void Dispose()
        {
            Rem.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
