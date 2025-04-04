// Decompiled with JetBrains decompiler
// Type: Fluent.BackstageTabControl
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  public class BackstageTabControl : Selector
  {
    private static readonly DependencyPropertyKey SelectedContentPropertyKey = DependencyProperty.RegisterReadOnly(nameof (SelectedContent), typeof (object), typeof (BackstageTabControl), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedContentProperty = BackstageTabControl.SelectedContentPropertyKey.DependencyProperty;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object SelectedContent
    {
      get => this.GetValue(BackstageTabControl.SelectedContentProperty);
      internal set => this.SetValue(BackstageTabControl.SelectedContentPropertyKey, value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static BackstageTabControl()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BackstageTabControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BackstageTabControl)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (BackstageTabControl), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(BackstageTabControl.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (BackstageTabControl));
      return basevalue;
    }

    public BackstageTabControl()
    {
      ContextMenu contextMenu = new ContextMenu();
      contextMenu.Width = 0.0;
      contextMenu.Height = 0.0;
      contextMenu.HasDropShadow = false;
      this.ContextMenu = (System.Windows.Controls.ContextMenu) contextMenu;
      this.ContextMenu.Opened += (RoutedEventHandler) delegate
      {
        this.ContextMenu.IsOpen = false;
      };
    }

    protected override void OnInitialized(EventArgs e)
    {
      base.OnInitialized(e);
      this.ItemContainerGenerator.StatusChanged += new EventHandler(this.OnGeneratorStatusChanged);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new BackstageTabItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is BackstageTabItem || item is Button;
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (e.Action != NotifyCollectionChangedAction.Remove || this.SelectedIndex != -1)
        return;
      int startIndex = e.OldStartingIndex + 1;
      if (startIndex > this.Items.Count)
        startIndex = 0;
      BackstageTabItem nextTabItem = this.FindNextTabItem(startIndex, -1);
      if (nextTabItem == null)
        return;
      nextTabItem.IsSelected = true;
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      base.OnSelectionChanged(e);
      if (e.AddedItems.Count > 0)
        this.UpdateSelectedContent();
      e.Handled = true;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      e.Handled = true;
    }

    private BackstageTabItem GetSelectedTabItem()
    {
      object selectedItem = this.SelectedItem;
      if (selectedItem == null)
        return (BackstageTabItem) null;
      if (!(selectedItem is BackstageTabItem selectedTabItem))
      {
        selectedTabItem = this.FindNextTabItem(this.SelectedIndex, 1);
        this.SelectedItem = (object) selectedTabItem;
      }
      return selectedTabItem;
    }

    private BackstageTabItem FindNextTabItem(int startIndex, int direction)
    {
      if (direction != 0)
      {
        int index1 = startIndex;
        for (int index2 = 0; index2 < this.Items.Count; ++index2)
        {
          index1 += direction;
          if (index1 >= this.Items.Count)
            index1 = 0;
          else if (index1 < 0)
            index1 = this.Items.Count - 1;
          if (this.ItemContainerGenerator.ContainerFromIndex(index1) is BackstageTabItem nextTabItem && nextTabItem.IsEnabled && nextTabItem.Visibility == Visibility.Visible)
            return nextTabItem;
        }
      }
      return (BackstageTabItem) null;
    }

    private void UpdateSelectedContent()
    {
      if (this.SelectedIndex < 0)
      {
        this.SelectedContent = (object) null;
      }
      else
      {
        BackstageTabItem selectedTabItem = this.GetSelectedTabItem();
        if (selectedTabItem == null)
          return;
        this.SelectedContent = selectedTabItem.Content;
        this.UpdateLayout();
      }
    }

    private void OnGeneratorStatusChanged(object sender, EventArgs e)
    {
      if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        return;
      if (this.HasItems && this.SelectedIndex == -1)
        this.SelectedIndex = 0;
      this.UpdateSelectedContent();
    }
  }
}
