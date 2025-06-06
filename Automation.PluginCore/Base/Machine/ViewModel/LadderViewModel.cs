﻿using Automation.PluginCore.Base.Machine.Resource;
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
        bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        IMachine Machine => this.Model as IMachine; 

        CancellationTokenSource _cts;

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
        public async void OnTest()
        {
            if (!IsRunning)
            {
                IsRunning = true;

                List<ILadder> toRemove = new List<ILadder>();

                foreach (ILadder node in this.Items)
                {
                    if (node is EmptyLadder && node.VerticalLine == false)
                        toRemove.Add(node);
                }
                // 이후에 제거
                foreach (ILadder node in toRemove)
                {
                    node.RemoveFromParent();
                }

                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                await Task.Run(async () =>
                {
                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            Compute();
                            await Task.Delay(100, token);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        //무시
                    }
                }, token);
            }
            else
            {
                _cts.Cancel();
                IsRunning = false;
            }
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
            if (IsRunning) return;
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
        
        public ILadder GetNode(int row, int column)
        {
            INode node = Machine.Logic.ToList().Find(x => (x as ILadder).X == column && (x as ILadder).Y == row);
            if(node != null) 
                return node as ILadder;

            return null;
        }

        public void ComputeRow(int row, int startColumn, bool flow)
        {
            if (row < 0 || row >= MaxRows) return;
            bool currentFlow = flow;
            for (int column = startColumn; column < MaxColumns; column++)
            {
                var node = GetNode(row, column);
                if (node == null)
                {
                    currentFlow = false;
                    continue;
                }
                if (node is EmptyLadder && node.VerticalLine == false) return;

                if (startColumn != 0 && column == startColumn)
                {
                    currentFlow = node.Flow || flow;
                }
                else if(column != 0)
                {
                    var backNode = GetNode(row, column - 1);
                    if (backNode == null)
                    {
                        var upperNode = GetNode(row - 1, column);
                        if (upperNode != null && upperNode.VerticalLine == true)
                        {
                            currentFlow = upperNode.Flow;
                        }
                    }
                }
                node.Flow = currentFlow;
                if (node.VerticalLine && column == startColumn && column != 0)
                {
                    var upperNode = GetNode(row - 1, column);
                    if (upperNode != null && upperNode.VerticalLine == true)
                    {
                        ComputeRow(row - 1, column, currentFlow);
                    }
                }
                if(node is Contact contact)
                {
                    if (contact.Type == ContactType.A)//A
                    {
                        currentFlow = currentFlow && node.Value;  // Normally Open
                    }
                    else if (contact.Type == ContactType.B)//B
                    {
                        currentFlow = currentFlow && !node.Value; // Normally Closed
                    }
                }
                var upperNextNode = GetNode(row - 1, column + 1);
                if(upperNextNode != null && upperNextNode.VerticalLine == true)
                {
                    ComputeRow(row -1, column + 1, currentFlow);
                }
            }
        }
        public void Compute()
        {
            foreach(ILadder node in Machine.Logic)
            {
                node.Flow = false;
            }
            for (int row = 0; row < MaxRows; row++)
            {
                ComputeRow(row, 0, true);
            }
            foreach (ILadder node in Machine.Logic)
            {
                if(node is Coil || node is Function)
                    node.Value = node.Flow;
            }
        }
    }
}
