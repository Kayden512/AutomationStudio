using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Automation.PluginCore.Util
{
    public static class FocusExtension
    {
        public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached(
            "IsFocused",
            typeof(bool),
            typeof(FocusExtension),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsFocusedChanged));

        public static bool GetIsFocused(DependencyObject obj)
            => (bool)obj.GetValue(IsFocusedProperty);

        public static void SetIsFocused(DependencyObject obj, bool value)
            => obj.SetValue(IsFocusedProperty, value);

        private static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && (bool)e.NewValue)
            {
                element.Focus();
                Keyboard.Focus(element); // 키보드 포커스까지 주기
            }
        }
    }
}
