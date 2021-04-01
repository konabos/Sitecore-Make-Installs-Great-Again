namespace MIGA.Tool.Windows
{
  using System.Windows.Controls;
  using System.Windows.Media;

  public static class TreeViewItemExtensions
  {
    #region Public methods

    public static int GetDepth(this TreeViewItem item)
    {
      TreeViewItem parent;
      while ((parent = GetParent(item)) != null)
      {
        return GetDepth(parent) + 1;
      }

      return 0;
    }

    #endregion

    #region Private methods

    private static TreeViewItem GetParent(TreeViewItem item)
    {
      var parent = VisualTreeHelper.GetParent(item);
      while (!(parent is TreeViewItem || parent is System.Windows.Controls.TreeView))
      {
        parent = VisualTreeHelper.GetParent(parent);
      }

      return parent as TreeViewItem;
    }

    #endregion
  }
}