using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Automation.PluginCore.Base;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using AvalonDock.Layout.Serialization;
using AvalonDock;
using Automation.PluginCore;
using System.Linq;
using Automation.PluginCore.Util.Extension;
using AutomationStudio.ViewModel.Panel;

namespace AutomationStudio.ViewModel
{
    public class MainViewModel : PanelBase, IMain
    {
        IViewModel _activeContent;
        INode _clipboard;

        public DockingManager DockingManager { get; set; }
        public ObservableCollection<IViewModel> Documents { get; protected set; } = new ObservableCollection<IViewModel>();
        public ObservableCollection<IViewModel> Panels { get; protected set; } = new ObservableCollection<IViewModel>();

        public DeviceManagerViewModel deviceManager { get; set; }
        public WorkTableViewModel workTable { get; set; }
        public ActionViewModel action { get; set; }
        public ScheduleViewModel schedule { get; set; }
        public DashboardEditorViewModel CustomScreenEditor { get; set; }
        public PropertyEditorViewModel propertyEditor { get; set; }
        public LogViewModel log { get; set; }
        public ErrorListViewModel errorList { get; set; }

        public IViewModel ActiveContent
        {
            get => _activeContent;
            set
            {
                if(_activeContent != null)
                    _activeContent.IsFocused = false;
                SetProperty(ref _activeContent, value);
                if (_activeContent != null)
                    _activeContent.IsFocused = true;
            }
        }
        public INode Clipboard
        {
            get => _clipboard;
            set => SetProperty(ref _clipboard, value);
        }

        #region Command
        public ICommand CmdEnvironment => new RelayCommand(OnEnvironment);
        public ICommand CmdHelp => new RelayCommand(OnHelp);
        public ICommand CmdCopy => new RelayCommand(OnHelp);
        public ICommand CmdPaste => new RelayCommand(OnHelp);
        #endregion
        public void OnEnvironment()
        {
            Automation.PluginCore.Environment.Instance.CurrentNodes = Extension._nodes;
            this.propertyEditor.SelectedObject = Automation.PluginCore.Environment.Instance;
        }

        public override void OnSave()
        {
            if (DockingManager == null) return;
            var serializer = new XmlLayoutSerializer(DockingManager);
            serializer.Serialize("Layout.xml");
        }

        public void LoadLayout()
        {
            if (DockingManager == null || !File.Exists("Layout.xml")) return;

            var serializer = new XmlLayoutSerializer(DockingManager);
            serializer.Deserialize("Layout.xml");
        }
        public void OnHelp()
        {
            this.Clipboard = propertyEditor.SelectedObject as INode;
        }

        public MainViewModel()
        {
            Extension.Main = this;
            PluginManager.LoadPlugins("Plugin");
            
            LoadPanel();
            
            LoadData();

            Register();
            Activate();
        }
        void LoadPanel()
        {
            CustomScreenEditor = new DashboardEditorViewModel();
            deviceManager = new DeviceManagerViewModel();
            workTable = new WorkTableViewModel();
            action = new ActionViewModel();
            schedule = new ScheduleViewModel();
            propertyEditor = new PropertyEditorViewModel();
            log = new LogViewModel();
            errorList = new ErrorListViewModel();
            Panels = new ObservableCollection<IViewModel>() { deviceManager, workTable, schedule, propertyEditor, action , CustomScreenEditor, log, errorList };
            foreach (INode node in Panels)
            {
                node.PropertyChanged += viewModelPropertyChanged;
                try
                {
                    if (node is IViewModel viewModel)
                        MapViewType(viewModel);
                }
                catch(Exception ex)
                {
                }
            }
        }
        void LoadData()
        {
            this.deviceManager.LoadData();
            this.deviceManager.Menu = new ObservableCollection<Type>(PluginManager.LoadType(typeof(IDevice)));
        }
        private void viewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (sender is IPanel panel && panel.SelectedNode != null)
            if (sender is IViewModel panel && panel.SelectedNode != null)
            {
                if (e.PropertyName == nameof(SelectedNode))
                    propertyEditor.SelectedObject = panel.SelectedNode;
                if (sender.Equals(deviceManager))
                {
                    action.Device = panel.SelectedNode as IDevice;
                    if (panel.SelectedNode is IMachine)
                        schedule.Machine = panel.SelectedNode as IMachine;
                    else if(panel.SelectedNode.Parent is IMachine)
                        schedule.Machine = panel.SelectedNode.Parent as IMachine;
                }
            }
            errorList.Errors.Clear();
            foreach (var err in deviceManager.CollectErrors())
            {
                errorList.Errors.Add((ErrorItem)err);
            }
        }
        #region View

        public void OpenDocument(IDocument document)
        {
            MapViewType(document);
            if(!this.Documents.Contains(document))
            {
                this.Documents.Add(document);
                document.PropertyChanged += viewModelPropertyChanged;
            }
            this.ActiveContent = document;
        }
        public void CloseDocument(IDocument document)
        {
            if (this.Documents.Contains(document))
            {
                document.PropertyChanged -= viewModelPropertyChanged;
                this.Documents.Remove(document);
            }
        }

        public void AppendLog(ErrorSeverity level, string messege)
        {
            this.log.Items.Add(new LogMessage(level, messege));
        }
        
        public override INode FindNode(string path)
        {
            if (path == null) return null;
            var segments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments[0] == "Device")
            {
                return deviceManager.FindNode(path);
            }
            else if(segments[0] == "Custom")
            {
                return CustomScreenEditor.FindNode(path);
            }
            return null;
        }

        /// <summary>
        /// ViewModel에 대한 View를 등록
        /// </summary>
        /// <param name="viewModel"></param>
        static void MapViewType(IViewModel viewModel)
        {
            if (viewModel == null) return;
            var template = new DataTemplate
            {
                DataType = viewModel.GetType(),
                VisualTree = new FrameworkElementFactory(viewModel.ViewType)
            };
            var key = new DataTemplateKey(viewModel.GetType());
            if (Application.Current.Resources.Contains(key)) return;
            Application.Current.Resources.Add(key, template);
        }
        #endregion
    }
}
