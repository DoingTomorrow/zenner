// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.TreeViewItemExtensions
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Converters
{
  public static class TreeViewItemExtensions
  {
    public static int GetDepth(this TreeViewItem item)
    {
      TreeViewItem parent;
      return (parent = TreeViewItemExtensions.GetParent(item)) != null ? parent.GetDepth() + 1 : 0;
    }

    private static TreeViewItem GetParent(TreeViewItem item)
    {
      DependencyObject parent = item != null ? VisualTreeHelper.GetParent((DependencyObject) item) : (DependencyObject) null;
      while (true)
      {
        switch (parent)
        {
          case null:
          case TreeViewItem _:
          case TreeView _:
            goto label_3;
          default:
            parent = VisualTreeHelper.GetParent(parent);
            continue;
        }
      }
label_3:
      return parent as TreeViewItem;
    }
  }
}
