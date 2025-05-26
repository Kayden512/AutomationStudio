using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace Automation.PluginCore.Util.Behavior
{
    public class ItemsControlClickBehavior : Behavior<ItemsControl>
    {
        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand), typeof(ItemsControlClickBehavior));

        public static readonly DependencyProperty ItemDoubleClickCommandProperty =
            DependencyProperty.Register(nameof(ItemDoubleClickCommand), typeof(ICommand), typeof(ItemsControlClickBehavior));

        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        public ICommand ItemDoubleClickCommand
        {
            get => (ICommand)GetValue(ItemDoubleClickCommandProperty);
            set => SetValue(ItemDoubleClickCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            base.OnDetaching();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var itemContainer = GetItemContainer(e.OriginalSource as DependencyObject);
            if (itemContainer != null)
            {
                var data = itemContainer.DataContext;

                if (e.ClickCount == 2)
                {
                    if (ItemDoubleClickCommand?.CanExecute(data) == true)
                        ItemDoubleClickCommand.Execute(data);
                }
            }
        }
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var itemContainer = GetItemContainer(e.OriginalSource as DependencyObject);
            if (itemContainer != null)
            {
                var data = itemContainer.DataContext;

                if (ItemClickCommand?.CanExecute(data) == true)
                    ItemClickCommand.Execute(data);
            }
        }

        private FrameworkElement GetItemContainer(DependencyObject source)
        {
            DependencyObject current = source;
            while (current != null)
            {
                if (current is TreeViewItem || current is ListBoxItem || current is ListViewItem || current is DataGridRow)
                    return current as FrameworkElement;

                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
    //public class ItemsControlClickBehavior : Behavior<ItemsControl>
    //{
    //    public static readonly DependencyProperty ItemClickCommandProperty =
    //        DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand), typeof(ItemsControlClickBehavior));

    //    public ICommand ItemClickCommand
    //    {
    //        get => (ICommand)GetValue(ItemClickCommandProperty);
    //        set => SetValue(ItemClickCommandProperty, value);
    //    }

    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();
    //        //AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
    //        AssociatedObject.MouseLeftButtonUp += OnPreviewMouseLeftButtonDown;
    //    }

    //    protected override void OnDetaching()
    //    {
    //        //AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
    //        AssociatedObject.MouseLeftButtonUp -= OnPreviewMouseLeftButtonDown;
    //        base.OnDetaching();
    //    }

    //    private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    //    {
    //        var itemContainer = GetItemContainer(e.OriginalSource as DependencyObject);
    //        if (itemContainer != null)
    //        {
    //            var data = itemContainer.DataContext;
    //            if (ItemClickCommand?.CanExecute(data) == true)
    //                ItemClickCommand.Execute(data);
    //        }
    //    }

    //    private FrameworkElement GetItemContainer(DependencyObject source)
    //    {
    //        DependencyObject current = source;
    //        while (current != null)
    //        {
    //            if (current is TreeViewItem || current is ListBoxItem || current is ListViewItem || current is DataGridRow)
    //                return current as FrameworkElement;

    //            current = VisualTreeHelper.GetParent(current);
    //        }
    //        return null;
    //    }
    //}
}
