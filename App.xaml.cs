using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using InstrumentMonitor.ViewModel;

namespace InstrumentMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainView = new MainWindow();
            MainWindowViewModel vmWind = new MainWindowViewModel();
            mainView.DataContext = vmWind;
            mainView.Show();

        }
    }
}
