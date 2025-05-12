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
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace AutomationStudio.ViewModel
{
    public class MainViewModel : PanelBase
    {
        IDocument _activeDocument;

        public DockingManager DockingManager { get; set; }
        public ObservableCollection<IViewModel> Documents { get; protected set; } = new ObservableCollection<IViewModel>();
        public ObservableCollection<IViewModel> Panels { get; protected set; } = new ObservableCollection<IViewModel>();

        public CustomScreenEditorViewModel CustomScreenEditor;
        public DeviceGroupViewModel deviceGroup;
        public ActionViewModel action;
        public PropertyEditorViewModel propertyEditor;
        public LogViewModel log;
        public ErrorListViewModel errorList;

        public IDocument ActiveDocument
        {
            get => _activeDocument;
            set => SetProperty(ref _activeDocument, value);
        }

        #region Command
        #endregion

        public void SaveLayout()
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

        public MainViewModel()
        {
            CmdSave = new RelayCommand(SaveLayout);
            PluginManager.LoadPlugins("Plugin");
            LoadPanel();
            LoadData();
        }
        void LoadPanel()
        {
            CustomScreenEditor = new CustomScreenEditorViewModel();
            deviceGroup = new DeviceGroupViewModel();
            action = new ActionViewModel();
            propertyEditor = new PropertyEditorViewModel();
            log = new LogViewModel();
            errorList = new ErrorListViewModel();
            Panels = new ObservableCollection<IViewModel>() {  deviceGroup, propertyEditor, action , CustomScreenEditor, log, errorList };
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
            if (e.PropertyName == nameof(SelectedNode))
                propertyEditor.SelectedObject = (sender as IPanel).SelectedNode;
        }
        #region View

        void OpenDocument(IDocument document)
        {
            MapViewType(document);
            if(!this.Documents.Contains(document))
            {
                this.Documents.Add(document);
            }
            this.ActiveDocument = document;
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
