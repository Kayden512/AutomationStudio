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

namespace AutomationStudio.ViewModel
{
    public class MainViewModel : PanelBase
    {
        IViewModel _activeContent;
        INode _clipboard;

        public DockingManager DockingManager { get; set; }
        public ObservableCollection<IViewModel> Documents { get; protected set; } = new ObservableCollection<IViewModel>();
        public ObservableCollection<IViewModel> Panels { get; protected set; } = new ObservableCollection<IViewModel>();

        public DeviceGroupViewModel deviceGroup;
        public ActionViewModel action;
        public ScheduleViewModel schedule;
        public CustomScreenEditorViewModel CustomScreenEditor;
        public PropertyEditorViewModel propertyEditor;
        public LogViewModel log;
        public ErrorListViewModel errorList;

        public IViewModel ActiveContent
        {
            get => _activeContent;
            set => SetProperty(ref _activeContent, value);
        }
        public INode Clipboard
        {
            get => _clipboard;
            set => SetProperty(ref _clipboard, value);
        }

        #region Command
        public ICommand CmdHelp => new RelayCommand(OnHelp);
        public ICommand CmdCopy => new RelayCommand(OnHelp);
        public ICommand CmdPaste => new RelayCommand(OnHelp);
        #endregion

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
            serializer.LayoutSerializationCallback += (s, e) =>
            {
            };
            serializer.Deserialize("Layout.xml");
        }
        public void OnHelp()
        {
            this.Clipboard = propertyEditor.SelectedObject;
        }

        public MainViewModel()
        {
            PluginManager.LoadPlugins("Plugin");
            LoadPanel();
            LoadData();
        }
        void LoadPanel()
        {
            CustomScreenEditor = new CustomScreenEditorViewModel();
            deviceGroup = new DeviceGroupViewModel();
            action = new ActionViewModel();
            schedule = new ScheduleViewModel();
            propertyEditor = new PropertyEditorViewModel();
            log = new LogViewModel();
            errorList = new ErrorListViewModel();
            Panels = new ObservableCollection<IViewModel>() {  deviceGroup, schedule, propertyEditor, action , CustomScreenEditor, log, errorList };
            foreach (INode node in Panels)
            {
                node.PropertyChanged += viewModelPropertyChanged;
                try
                {
                    if (node is IViewModel viewModel)
                    {
                        MapViewType(viewModel);
                    }
                }
                catch(Exception ex)
                {
                }
            }
        }
        void LoadData()
        {
            this.deviceGroup.LoadData();
            this.deviceGroup.Menu = new ObservableCollection<Type>(PluginManager.LoadType(typeof(IDevice)));
        }
        private void viewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IPanel panel)
            {
                if (e.PropertyName == nameof(SelectedNode))
                    propertyEditor.SelectedObject = panel.SelectedNode;
                if (sender == deviceGroup)
                {
                    action.Device = panel.SelectedNode as IDevice;
                    if(panel.SelectedNode is IMachine)
                        schedule.Machine = panel.SelectedNode as IMachine;
                }
            }
            errorList.Errors.Clear();
            foreach (var err in deviceGroup.CollectErrors())
            {
                errorList.Errors.Add((ErrorItem)err);
            }
        }
        #region View

        void OpenDocument(IDocument document)
        {
            MapViewType(document);
            if(!this.Documents.Contains(document))
            {
                this.Documents.Add(document);
            }
            this.ActiveContent = document;
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
