using System.Windows;
using PuppyMapper.WPFApp.ViewModels;
using PuppyMapper.WPFApp.Views;

namespace PuppyMapper.WPFApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel(),
        };
        MainWindow.Show();
    }
}