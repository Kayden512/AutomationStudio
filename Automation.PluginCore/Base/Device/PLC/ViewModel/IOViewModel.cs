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

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                UpdatePagedBits();
                NotifyPropertyChanged(nameof(TotalPages));
                NotifyPropertyChanged(nameof(PageDisplay));
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)AllInputBits.Count / PageSize);
        public string PageDisplay => $"{CurrentPage + 1} / {TotalPages}";

        public NodeCollection AllInputBits { get; } = new NodeCollection();

        public NodeCollection PagedInputBits { get; } = new NodeCollection();
        public int PageSize => 32;

        public void RebuildAllBits()
        {
            AllInputBits.Clear();
            foreach (var memory in (Model as VirtualDevice).Input)
            {
                foreach (var bit in memory.Items)
                    AllInputBits.Add(bit);
            }
            CurrentPage = 0;
            UpdatePagedBits();
            NotifyPropertyChanged(nameof(TotalPages));
            NotifyPropertyChanged(nameof(PageDisplay));
        }

        public void NextPage()
        {
            int maxPages = (int)Math.Ceiling((double)AllInputBits.Count / PageSize);
            if (CurrentPage < maxPages - 1)
            {
                CurrentPage++;
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
            }
        }

        private void UpdatePagedBits()
        {
            PagedInputBits.Clear();
            var pageBits = AllInputBits.Skip(CurrentPage * PageSize).Take(PageSize);
            foreach (var bit in pageBits)
                PagedInputBits.Add(bit);
        }

        public ICommand CmdAppend => new RelayCommand<object>(OnAppend);
        public ICommand CmdRemove => new RelayCommand<object>(OnRemove);
        public ICommand CmdInputPrevPage => new RelayCommand(PreviousPage);
        public ICommand CmdInputNextPage => new RelayCommand(NextPage);

        public override void OnSelect(object param)
        {
            if (param == null) return;
            if (param is INode node)
                this.SelectedNode = node;
        }

        public void OnAppend(object param)
        {
            Memory newMemory = new Memory();
            Extension.Register(newMemory);
            for (int i = 0; i<newMemory.MemotyLength; i++)
            {
                Bit newBit = new Bit();
                Extension.Register(newBit);
                newMemory.AddChild(newBit);
            }

            if (param.ToString() == "Input")
                (Model as VirtualDevice).Input.Add(newMemory);
            else
                (Model as VirtualDevice).Output.Add(newMemory);

            RebuildAllBits();
        }
        public void OnRemove(object param)
        {

        }
    }
}
