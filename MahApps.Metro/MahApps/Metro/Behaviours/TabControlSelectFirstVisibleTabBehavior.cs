// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.TabControlSelectFirstVisibleTabBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class TabControlSelectFirstVisibleTabBehavior : Behavior<TabControl>
  {
    protected override void OnAttached()
    {
      this.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
    {
      List<TabItem> list = this.AssociatedObject.Items.Cast<TabItem>().ToList<TabItem>();
      if (this.AssociatedObject.SelectedItem is TabItem selectedItem && selectedItem.Visibility == Visibility.Visible)
        return;
      TabItem tabItem = list.FirstOrDefault<TabItem>((Func<TabItem, bool>) (t => t.Visibility == Visibility.Visible));
      if (tabItem != null)
        this.AssociatedObject.SelectedIndex = list.IndexOf(tabItem);
      else
        this.AssociatedObject.SelectedItem = (object) null;
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.SelectionChanged -= new SelectionChangedEventHandler(this.OnSelectionChanged);
    }
  }
}
