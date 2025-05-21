using Automation.PluginCore.Base.Machine.Resource;
using Automation.PluginCore.Base.Machine.View;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
namespace Automation.PluginCore.Base.Machine.ViewModel
{
    public class LadderViewModel : DocumentBase
    {
        int _selectedX;
        int _selectedY;


        public override Type ViewType => typeof(LadderView);

        public ICommand CmdMove => new RelayCommand<object>(OnMove);
        public ICommand CmdAppend => new RelayCommand<object>(OnAppend);

        //public ObservableCollection<INode>

        public int SelectedX
        {
            get => _selectedX;
            set
            {
                SetProperty(ref _selectedX, value);
                INode node = this.Items.ToList().Find(x => (x as LadderNode).X == SelectedX && (x as LadderNode).Y == SelectedY);
                if (node != null)
                {
                    this.SelectedNode = node;
                }
            }
        }
        public int SelectedY
        {
            get => _selectedY;
            set
            {
                SetProperty(ref _selectedY, value);
                INode node = this.Items.ToList().Find(x => (x as LadderNode).X == SelectedX && (x as LadderNode).Y == SelectedY);
                if (node != null)
                {
                    this.SelectedNode = node;
                }
            }
        }

        public void SelectCell(Point clickPoint)
        {
            SelectedX = (int)(clickPoint.X / 60);
            SelectedY = (int)(clickPoint.Y / 40);
        }

        public void OnMove(object param)
        {
            switch(param.ToString())
            {
                case "left":
                    if (SelectedX > 0)
                        SelectedX--;
                    break;
                case "right":
                    if (SelectedX < 16)
                        SelectedX++;
                    break;
                case "up":
                    if (SelectedY > 0)
                        SelectedY--;
                    break;
                case "down":
                    if (SelectedY < 16)
                        SelectedY++;
                    break;
            }
        }
        public void OnAppend(object param)
        {
            INode node = this.Items.ToList().Find(x => (x as LadderNode).X == SelectedX && (x as LadderNode).Y == SelectedY);
            if (node != null && param.ToString() != "down")
            {
                node.RemoveFromParent();
            }
            switch (param.ToString())
            {
                case "empty":
                    this.Items.Add(new LadderNode() { X = SelectedX, Y = SelectedY, Type = LadderType.None });
                    break;
                case "contact_a":
                    this.Items.Add(new LadderNode() { X = SelectedX, Y = SelectedY , Type = LadderType.Contact_A});
                    break;
                case "contact_b":
                    this.Items.Add(new LadderNode() { X = SelectedX, Y = SelectedY, Type = LadderType.Contact_B });
                    break;
                case "line":
                    this.Items.Add(new LadderNode() { X = SelectedX, Y = SelectedY, Type = LadderType.HorizontalLine });
                    break;
                case "down":
                    if(node == null)
                    {
                        node = new LadderNode() { X = SelectedX, Y = SelectedY, Type = LadderType.None };
                        this.Items.Add(node);
                    }
                    (node as LadderNode).VerticalLine = !(node as LadderNode).VerticalLine;
                    break;
                case "coil":
                    this.Items.Add(new LadderNode() { X = SelectedX, Y = SelectedY, Type = LadderType.Coil });
                    break;
            }
            node = this.Items.ToList().Find(x => (x as LadderNode).X == SelectedX && (x as LadderNode).Y == SelectedY);
            if (node != null)
                SelectedNode = node;
        }
    }
}
