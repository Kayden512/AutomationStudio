using Automation.PluginCore.Base.Machine.View;
using System;
using System.Windows;
namespace Automation.PluginCore.Base.Machine.ViewModel
{
    public class LadderViewModel : DocumentBase
    {
        int _selectedX;
        int _selectedY;


        public override Type ViewType => typeof(LadderView);

        public int SelectedX
        {
            get => _selectedX;
            set => SetProperty(ref _selectedX, value);
        }
        public int SelectedY
        {
            get => _selectedY;
            set => SetProperty(ref _selectedY, value);
        }

        public void SelectCell(Point clickPoint)
        {
            SelectedX = (int)(clickPoint.X / 60);
            SelectedY = (int)(clickPoint.Y / 40);
        }

    }
}
