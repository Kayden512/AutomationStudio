using Automation.PluginCore.Base.Machine.Resource;
using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine
{
    public partial class Machine
    {
        const int MaxRows = 8;
        const int MaxColumns = 8;

        bool _isRunning;
        CancellationTokenSource _ctsLogic;

        [Editor(typeof(LogicButton), typeof(LogicButton))]
        public NodeCollection Logic { get; set; } = new NodeCollection();
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public async void ToggleLadder()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                List<ILadder> toRemove = new List<ILadder>();

                foreach (ILadder node in this.Logic)
                {
                    if (node is EmptyLadder && node.VerticalLine == false)
                        toRemove.Add(node);
                }
                // 이후에 제거
                foreach (ILadder node in toRemove)
                {
                    node.RemoveFromParent();
                }
                _ctsLogic = new CancellationTokenSource();
                CancellationToken token = _ctsLogic.Token;

                await Task.Run(async () =>
                {
                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            Compute();
                            await Task.Delay(50, token);
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
                _ctsLogic.Cancel();
                IsRunning = false;
            }
        }
        public ILadder GetNode(int row, int column)
        {
            INode node = this.Logic.ToList().Find(x => (x as ILadder).X == column && (x as ILadder).Y == row);
            if (node != null)
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
                else if (column != 0)
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
                if (node is Contact contact)
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
                if (upperNextNode != null && upperNextNode.VerticalLine == true)
                {
                    ComputeRow(row - 1, column + 1, currentFlow);
                }
            }
        }
        public void Compute()
        {
            foreach (ILadder node in this.Logic)
            {
                node.Flow = false;
            }
            for (int row = 0; row < MaxRows; row++)
            {
                ComputeRow(row, 0, true);
            }
            foreach (ILadder node in this.Logic)
            {
                if (node is Coil || node is Function)
                    node.Value = node.Flow;
            }
        }
    }
}
