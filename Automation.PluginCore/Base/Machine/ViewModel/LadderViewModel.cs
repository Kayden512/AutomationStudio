using Automation.PluginCore.Base.Machine.Resource;
using Automation.PluginCore.Base.Machine.View;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
namespace Automation.PluginCore.Base.Machine.ViewModel
{
    public class LadderViewModel : DocumentBase
    {
        const int MaxRows = 8;
        const int MaxColumns = 8;

        int _selectedX;
        int _selectedY;

        IMachine Machine => this.Model as IMachine; 

        public override Type ViewType => typeof(LadderView);

        public ICommand CmdTest => new RelayCommand(OnTest);
        public ICommand CmdMove => new RelayCommand<object>(OnMove);
        public ICommand CmdAppend => new RelayCommand<object>(OnAppend);

        public int SelectedX
        {
            get => _selectedX;
            set
            {
                SetProperty(ref _selectedX, value);
                INode node = GetNode(SelectedY, SelectedX);
                if (node != null)
                    this.SelectedNode = node;
            }
        }
        public int SelectedY
        {
            get => _selectedY;
            set
            {
                SetProperty(ref _selectedY, value);
                INode node = GetNode(SelectedY, SelectedX);
                if (node != null)
                    this.SelectedNode = node;
            }
        }
        public void SelectCell(Point clickPoint)
        {
            SelectedX = (int)(clickPoint.X / 60);
            SelectedY = (int)(clickPoint.Y / 40);
        }
        public ILadder GetNode(int row, int column)
        {
            INode node = Machine.Logic.ToList().Find(x => (x as ILadder).X == column && (x as ILadder).Y == row);
            if (node != null)
                return node as ILadder;

            return null;
        }
        public async void OnTest()
        {
            
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
                    if (SelectedX < MaxColumns - 1)
                        SelectedX++;
                    break;
                case "up":
                    if (SelectedY > 0)
                        SelectedY--;
                    break;
                case "down":
                    if (SelectedY < MaxRows - 1)
                        SelectedY++;
                    break;
            }
        }
        public void OnAppend(object param)
        {
            if (Machine.IsRunning) return;
            ILadder node = GetNode(SelectedY, SelectedX);
            if (node != null && param.ToString() != "down")
            {
                node.RemoveFromParent();
            }
            switch (param.ToString())
            {
                case "empty":
                    //Machine.Logic.Add(new LadderNode() { X = SelectedX, Y = SelectedY, Type = LadderType.None });
                    break;
                case "contact_a":
                    Machine.Logic.Add(new Contact() { X = SelectedX, Y = SelectedY, Type = ContactType.A });
                    break;
                case "contact_b":
                    Machine.Logic.Add(new Contact() { X = SelectedX, Y = SelectedY, Type = ContactType.B });
                    break;
                case "line":
                    Machine.Logic.Add(new HorizontalLine() { X = SelectedX, Y = SelectedY });
                    break;
                case "down":
                    if(node == null)
                    {
                        node = new EmptyLadder() { X = SelectedX, Y = SelectedY };
                        Machine.Logic.Add(node);
                    }
                    node.VerticalLine = !node.VerticalLine;
                    break;
                case "coil":
                    Machine.Logic.Add(new Coil() { X = SelectedX, Y = SelectedY });
                    break;
                case "function":
                    Machine.Logic.Add(new Function() { X = SelectedX, Y = SelectedY });
                    break;
            }
            node = GetNode(SelectedY, SelectedX);
            if (node != null)
                SelectedNode = node;
        }

    }
}
