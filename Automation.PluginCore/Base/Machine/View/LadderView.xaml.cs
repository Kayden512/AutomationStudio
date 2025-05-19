using Automation.PluginCore.Base.Machine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Automation.PluginCore.Base.Machine.View
{
    /// <summary>
    /// LadderView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LadderView : UserControl
    {
        public LadderView()
        {
            InitializeComponent();
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            var point = e.GetPosition(canvas);

            if (DataContext is LadderViewModel vm)
            {
                vm.SelectCell(point);
            }
        }
    }
}
