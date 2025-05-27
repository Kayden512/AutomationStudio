using Automation.PluginCore.Base.Device.PLC.Resource;
using Automation.PluginCore.Base.Device.PLC.View;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Automation.PluginCore.Base.Device.PLC.ViewModel
{
    public class IOViewModel : DocumentBase
    {
        public override Type ViewType => typeof(IOView);
        public int PageSize => 32;

        #region Input
        private int _currentInputPage;
        public int CurrentInputPage
        {
            get => _currentInputPage;
            set
            {
                SetProperty(ref _currentInputPage, value);
                UpdateInputPagedBits();
                NotifyPropertyChanged(nameof(TotalInputPages));
                NotifyPropertyChanged(nameof(InputPageDisplay));
            }
        }

        public int TotalInputPages => (int)Math.Ceiling((double)AllInputBits.Count / PageSize);
        public string InputPageDisplay => $"{CurrentInputPage + 1} / {TotalInputPages}";

        public NodeCollection AllInputBits { get; } = new NodeCollection();

        public NodeCollection PagedInputBits { get; } = new NodeCollection();

        public void RebuildAllInputBits()
        {
            AllInputBits.Clear();
            foreach (var memory in (Model as VirtualDevice).Input)
            {
                foreach (var bit in memory.Items)
                    AllInputBits.Add(bit);
            }
            CurrentInputPage = 0;
            UpdateInputPagedBits();
            NotifyPropertyChanged(nameof(TotalInputPages));
            NotifyPropertyChanged(nameof(InputPageDisplay));
        }

        public void NextInputPage()
        {
            int maxPages = (int)Math.Ceiling((double)AllInputBits.Count / PageSize);
            if (CurrentInputPage < maxPages - 1)
            {
                CurrentInputPage++;
            }
        }

        public void PreviousInputPage()
        {
            if (CurrentInputPage > 0)
            {
                CurrentInputPage--;
            }
        }

        private void UpdateInputPagedBits()
        {
            PagedInputBits.Clear();
            var pageBits = AllInputBits.Skip(CurrentInputPage * PageSize).Take(PageSize);
            foreach (var bit in pageBits)
                PagedInputBits.Add(bit);
        }


        private int _currentOutputPage;
        public int CurrentOutputPage
        {
            get => _currentOutputPage;
            set
            {
                SetProperty(ref _currentOutputPage, value);
                UpdateOutputPagedBits();
                NotifyPropertyChanged(nameof(TotalOutputPages));
                NotifyPropertyChanged(nameof(OutputPageDisplay));
            }
        }
#endregion

        #region Output
        public int TotalOutputPages => (int)Math.Ceiling((double)AllOutputBits.Count / PageSize);
        public string OutputPageDisplay => $"{CurrentOutputPage + 1} / {TotalOutputPages}";

        public NodeCollection AllOutputBits { get; } = new NodeCollection();

        public NodeCollection PagedOutputBits { get; } = new NodeCollection();

        public void RebuildAllOutputBits()
        {
            AllOutputBits.Clear();
            foreach (var memory in (Model as VirtualDevice).Output)
            {
                foreach (var bit in memory.Items)
                    AllOutputBits.Add(bit);
            }
            CurrentOutputPage = 0;
            UpdateOutputPagedBits();
            NotifyPropertyChanged(nameof(TotalOutputPages));
            NotifyPropertyChanged(nameof(OutputPageDisplay));
        }

        public void NextOutputPage()
        {
            int maxPages = (int)Math.Ceiling((double)AllOutputBits.Count / PageSize);
            if (CurrentOutputPage < maxPages - 1)
            {
                CurrentOutputPage++;
            }
        }

        public void PreviousOutputPage()
        {
            if (CurrentOutputPage > 0)
            {
                CurrentOutputPage--;
            }
        }

        private void UpdateOutputPagedBits()
        {
            PagedOutputBits.Clear();
            var pageBits = AllOutputBits.Skip(CurrentOutputPage * PageSize).Take(PageSize);
            foreach (var bit in pageBits)
                PagedOutputBits.Add(bit);
        }

#endregion

        public ICommand CmdToggle => new RelayCommand<object>(OnToggle);
        public ICommand CmdAppend => new RelayCommand<object>(OnAppend);
        public ICommand CmdRemove => new RelayCommand<object>(OnRemove);
        public ICommand CmdInputPrevPage => new RelayCommand(PreviousInputPage);
        public ICommand CmdInputNextPage => new RelayCommand(NextInputPage);
        public ICommand CmdOutputPrevPage => new RelayCommand(PreviousOutputPage);
        public ICommand CmdOutputNextPage => new RelayCommand(NextOutputPage);

        public void OnToggle(object param)
        {
            if (param == null) return;
            if (param is IValueHolder node)
                node.Value = !(bool)node.Value;
        }

        public override void OnSelect(object param)
        {
            if (param == null) return;
            if (param is INode node)
                this.SelectedNode = node;
        }

        public void OnAppend(object param)
        {
            Memory newMemory = new Memory();
            newMemory.Name = param.ToString();

            Extension.Register(newMemory);
            for (int i = 0; i<newMemory.MemotyLength; i++)
            {
                Bit newBit = new Bit();
                if(param.ToString() == "Input")
                    newBit.AccessMode = AccessMode.Read;
                else
                    newBit.AccessMode = AccessMode.Write;
                Extension.Register(newBit);
                newMemory.AddChild(newBit);
            }

            if (param.ToString() == "Input")
            {
                (Model as VirtualDevice).Input.Add(newMemory);
                RebuildAllInputBits();
            }
            else
            {
                (Model as VirtualDevice).Output.Add(newMemory);
                RebuildAllOutputBits();
            }

        }
        public void OnRemove(object param)
        {

        }
    }
}
