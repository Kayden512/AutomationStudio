using Automation.PluginCore.Base;
using Automation.PluginCore.Util;
using AutomationStudio.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutomationStudio.ViewModel
{
    /// <summary>
    ///Node 구성요소 에러 리스트 시각화
    /// </summary>
    public class ErrorListViewModel : PanelBase
    {
        public override Type ViewType => typeof(ErrorListView);
        public override string Name => "Error List";

        public ObservableCollection<ErrorItem> Errors { get; } = new ObservableCollection<ErrorItem>();

        private ErrorItem _selectedError;
        public ErrorItem SelectedError
        {
            get => _selectedError;
            set => SetProperty(ref _selectedError, value);
        }
        public ICommand OpenErrorCommand => new RelayCommand<ErrorItem>(OnOpenError);
        private void OnOpenError(ErrorItem item)
        {
            // 예: 해당 파일의 해당 줄로 이동
            //MessageBox.Show($"Opening: {item.File} ({item.Line}, {item.Column})");
        }

        public ErrorListViewModel()
        {
            // 테스트용 데이터
            Errors.Add(new ErrorItem { Severity = ErrorSeverity.Error, Code = "CS1001", Message = "Identifier expected", File = "MainWindow.xaml.cs", Line = 23, Column = 10 });
            Errors.Add(new ErrorItem { Severity = ErrorSeverity.Warning, Code = "CS0168", Message = "Unused variable", File = "App.xaml.cs", Line = 11, Column = 5 });
        }
    }
    public enum ErrorSeverity
    {
        Error,
        Warning,
        Info
    }
    public class ErrorItem
    {
        public ErrorSeverity Severity { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string File { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
