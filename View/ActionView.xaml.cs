using Automation.PluginCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutomationStudio.View
{
    /// <summary>
    /// ActionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ActionView : UserControl
    {
        public ActionView()
        {
            InitializeComponent();
        }
        private void TreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TreeView treeView = sender as TreeView;
                if (treeView == null) return;

                TreeViewItem item = GetTreeViewItemAtMouse(treeView, e.GetPosition(treeView));
                if (item?.DataContext is NodeBase node)
                {
                    DragDrop.DoDragDrop(item, new DataObject("Node", node), DragDropEffects.Move);
                }
            }
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("Node") is NodeBase draggedNode)
            {
                TreeView tree = sender as TreeView;
                Point dropPosition = e.GetPosition(tree);
                TreeViewItem dropTargetItem = GetTreeViewItemAtMouse(tree, dropPosition);

                if (dropTargetItem == null)//Devic Group에 직접 Drop
                {
                    (this.DataContext as NodeBase).AddChild(draggedNode);
                }
                else if (dropTargetItem?.DataContext is NodeBase targetNode && targetNode != draggedNode && !IsDescendantOf(draggedNode, targetNode))
                {
                    targetNode.AddChild(draggedNode);
                }
            }
        }

        private TreeViewItem GetTreeViewItemAtMouse(ItemsControl container, Point point)
        {
            HitTestResult result = VisualTreeHelper.HitTest(container, point);
            DependencyObject obj = result?.VisualHit;

            while (obj != null && !(obj is TreeViewItem))
                obj = VisualTreeHelper.GetParent(obj);

            return obj as TreeViewItem;
        }
        private bool IsDescendantOf(NodeBase node, NodeBase potentialParent)
        {
            NodeBase parent = potentialParent;
            while (parent != null)
            {
                if (parent == node)
                    return true;
                parent = parent.Parent as NodeBase;
            }
            return false;
        }
    }
}

