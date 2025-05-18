using Automation.PluginCore.Base;
using Automation.PluginCore.Interface;
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

        public ObservableCollection<ErrorItem> Errors { get; set; } = new ObservableCollection<ErrorItem>();

        private ErrorItem _selectedError;
        public ErrorItem SelectedError
        {
            get => _selectedError;
            set => SetProperty(ref _selectedError, value);
        }

        public ErrorListViewModel()
        {
            // 테스트용 데이터
            Errors.Add(new ErrorItem { Severity = ErrorSeverity.Error, Code = "CS1001", Message = "Identifier expected", Node = "Pylon", Path="Device/Robot1/Pylon"});
            Errors.Add(new ErrorItem { Severity = ErrorSeverity.Warning, Code = "CS0168", Message = "Unused variable", Node = "Mitsubishi", Path = "Device/Robot1/Pylon" });
            Errors.Add(new ErrorItem { Severity = ErrorSeverity.Info, Code = "CS0168", Message = "Unused variable", Node = "Pylon", Path = "Device/Robot1/Pylon"     });
        }
    }
}
