using System.Windows.Controls;

namespace PEditor
{
    internal class MyTreeViewItem<T> : TreeViewItem
        where T : class
    {
        public T MyItem;

        public MyTreeViewItem(T item)
        {
            MyItem = item;
        }
    }
}