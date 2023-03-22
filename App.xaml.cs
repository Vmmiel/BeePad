using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace BeePad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.Properties["AppName"] = Assembly.GetEntryAssembly().GetName().Name;

            if (e.Args != null && e.Args.Count() > 0)
                this.Properties["FilePath"] = e.Args[0];

            base.OnStartup(e);
        }
    }
}
