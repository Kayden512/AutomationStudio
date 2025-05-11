using System.Windows;
using AutomationStudio.View;
using AutomationStudio.ViewModel;

namespace AutomationStudio
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainViewModel mainVM = new MainViewModel();
            MainView mainView = new MainView();
            mainVM.DockingManager = mainView.DockManager;
            mainView.DataContext = mainVM;
            mainView.Show();
            mainVM.LoadLayout();
        }
    }
}
