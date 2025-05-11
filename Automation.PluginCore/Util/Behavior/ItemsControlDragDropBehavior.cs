using Automation.PluginCore.Base;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Automation.PluginCore.Interface;

namespace Automation.PluginCore.Util.Behavior
{
    public class ItemsControlDragDropBehavior : Behavior<ItemsControl>
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register(nameof(DropCommand), typeof(ICommand), typeof(ItemsControlDragDropBehavior));

        public ICommand DropCommand
        {
            get => (ICommand)GetValue(DropCommandProperty);
            set => SetValue(DropCommandProperty, value);
        }

        private Point _dragStartPoint;
        //private object _draggedItem;

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.Drop += OnDrop;
            AssociatedObject.AllowDrop = true;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.Drop -= OnDrop;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var diff = _dragStartPoint - e.GetPosition(null);
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var item = GetItemContainerAtMouse(e.GetPosition(AssociatedObject));
                if (item?.DataContext != null && item?.DataContext is INode node)
                {
                    if (node is IViewModel) return;
                    DragDrop.DoDragDrop(item, new DataObject("Node", node), DragDropEffects.Move);
                }
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (DropCommand == null) return;

            var targetItem = GetItemContainerAtMouse(e.GetPosition(AssociatedObject))?.DataContext;

            if (e.Data.GetData("Node") is NodeBase node && DropCommand.CanExecute(null))
            {
                DropCommand.Execute(new DropData
                {
                    Source = node,
                    Target = targetItem as INode
                });
            }
        }

        private FrameworkElement GetItemContainerAtMouse(Point position)
        {
            var hit = VisualTreeHelper.HitTest(AssociatedObject, position);
            DependencyObject obj = hit?.VisualHit;
            while (obj != null && !(obj is ListBoxItem || obj is TreeViewItem || obj is ContentPresenter))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            return obj as FrameworkElement;
        }
    }
}
