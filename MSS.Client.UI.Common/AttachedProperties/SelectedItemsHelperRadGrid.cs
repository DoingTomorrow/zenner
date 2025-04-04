// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.SelectedItemsHelperRadGrid
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using NHibernate.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class SelectedItemsHelperRadGrid
  {
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("MySelectedItems", typeof (IList), typeof (SelectedItemsHelperRadGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(SelectedItemsHelperRadGrid.OnSelectedItemsChanged))
    {
      BindsTwoWayByDefault = true,
      DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
    });

    public static IList GetMySelectedItems(DependencyObject d)
    {
      return (IList) d.GetValue(SelectedItemsHelperRadGrid.SelectedItemsProperty);
    }

    public static void SetMySelectedItems(DependencyObject d, IList value)
    {
      d.SetValue(SelectedItemsHelperRadGrid.SelectedItemsProperty, (object) value);
    }

    private static void OnSelectedItemsChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      RadGridView senderGrid = sender as RadGridView;
      if (senderGrid == null)
        return;
      senderGrid.Loaded += (RoutedEventHandler) delegate
      {
        SelectedItemsHelperRadGrid.SetSelectedItems(senderGrid);
      };
      senderGrid.SelectionChanged += (EventHandler<SelectionChangeEventArgs>) delegate
      {
        SelectedItemsHelperRadGrid.ReSetSelectedItems(senderGrid);
      };
    }

    private static void SetSelectedItems(RadGridView grid)
    {
      foreach (object obj in Enumerable.Cast<object>(SelectedItemsHelperRadGrid.GetMySelectedItems((DependencyObject) grid)).ToList<object>())
      {
        if (!grid.SelectedItems.Contains(obj))
          grid.SelectedItems.Add(obj);
      }
    }

    private static void ReSetSelectedItems(RadGridView grid)
    {
      IList selectedItems = SelectedItemsHelperRadGrid.GetMySelectedItems((DependencyObject) grid);
      selectedItems.Clear();
      grid.SelectedItems.ForEach<object>((Action<object>) (x => selectedItems.Add(x)));
    }
  }
}
