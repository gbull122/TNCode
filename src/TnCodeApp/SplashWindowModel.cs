using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace TnCode.TnCodeApp
{
    public class SplashWindowModel:BindableBase
    {
        private string status;
        private bool isEnabled = true;

        public string Title { get; } = $"{Application.ResourceAssembly.GetName().Name} {Application.ResourceAssembly.GetName().Version}";


        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

    }
}
