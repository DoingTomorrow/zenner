// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.SelectedItemsRadTreeList
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class SelectedItemsRadTreeList
  {
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("MySelectedItems", typeof (IList), typeof (SelectedItemsRadTreeList), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(SelectedItemsRadTreeList.OnSelectedItemsChanged))
    {
      BindsTwoWayByDefault = true,
      DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
    });

    public static IList GetMySelectedItems(DependencyObject d)
    {
      return (IList) d.GetValue(SelectedItemsRadTreeList.SelectedItemsProperty);
    }

    public static void SetMySelectedItems(DependencyObject d, IList value)
    {
      d.SetValue(SelectedItemsRadTreeList.SelectedItemsProperty, (object) value);
    }

    private static void OnSelectedItemsChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      RadTreeListView listBox = sender as RadTreeListView;
      if (listBox == null)
        return;
      listBox.Loaded += (RoutedEventHandler) delegate
      {
        SelectedItemsRadTreeList.SetSelectedItems(listBox);
      };
      listBox.SelectionChanged += (EventHandler<SelectionChangeEventArgs>) delegate
      {
        SelectedItemsRadTreeList.ReSetSelectedItems(listBox);
      };
    }

    private static void SetSelectedItems(RadTreeListView listBox)
    {
      foreach (object obj in Enumerable.Cast<object>(SelectedItemsRadTreeList.GetMySelectedItems((DependencyObject) listBox)).ToList<object>())
      {
        if (!listBox.SelectedItems.Contains(obj))
          listBox.SelectedItems.Add(obj);
      }
    }

    private static void ReSetSelectedItems(RadTreeListView listBox)
    {
      IList mySelectedItems = SelectedItemsRadTreeList.GetMySelectedItems((DependencyObject) listBox);
      List<object> list = Enumerable.Cast<object>(mySelectedItems).ToList<object>();
      if (mySelectedItems == null || listBox.SelectedItems == null)
        return;
      foreach (object selectedItem in (Collection<object>) listBox.SelectedItems)
        mySelectedItems.Add(selectedItem);
      foreach (object obj in list)
      {
        if (!listBox.SelectedItems.Contains(obj))
          mySelectedItems.Remove(obj);
      }
    }
  }
}
