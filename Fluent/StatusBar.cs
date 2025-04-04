// Decompiled with JetBrains decompiler
// Type: Fluent.StatusBar
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Fluent
{
  public class StatusBar : System.Windows.Controls.Primitives.StatusBar
  {
    private ContextMenu contextMenu = new ContextMenu();
    private Window ownerWindow;
    public static readonly DependencyProperty IsWindowMaximizedProperty = DependencyProperty.Register(nameof (IsWindowMaximized), typeof (bool), typeof (StatusBar), (PropertyMetadata) new UIPropertyMetadata((object) false));

    public bool IsWindowMaximized
    {
      get => (bool) this.GetValue(StatusBar.IsWindowMaximizedProperty);
      set => this.SetValue(StatusBar.IsWindowMaximizedProperty, (object) value);
    }

    static StatusBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (StatusBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (StatusBar)));
    }

    public StatusBar()
    {
      this.RecreateMenu();
      this.ContextMenu = (System.Windows.Controls.ContextMenu) this.contextMenu;
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.Unloaded += new RoutedEventHandler(this.OnUnloaded);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      if (this.ownerWindow == null)
        return;
      this.ownerWindow.StateChanged -= new EventHandler(this.OnWindowStateChanged);
      this.ownerWindow = (Window) null;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (this.ownerWindow == null)
        this.ownerWindow = Window.GetWindow((DependencyObject) this);
      if (this.ownerWindow == null)
        return;
      this.ownerWindow.StateChanged += new EventHandler(this.OnWindowStateChanged);
      if (this.ownerWindow.ResizeMode == ResizeMode.CanResizeWithGrip && this.ownerWindow.WindowState == WindowState.Maximized)
        this.IsWindowMaximized = true;
      else
        this.IsWindowMaximized = false;
    }

    private void OnWindowStateChanged(object sender, EventArgs e)
    {
      if (this.ownerWindow.ResizeMode == ResizeMode.CanResizeWithGrip && this.ownerWindow.WindowState == WindowState.Maximized)
        this.IsWindowMaximized = true;
      else
        this.IsWindowMaximized = false;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new StatusBarItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is StatusBarItem || item is Separator;
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (e.NewItems[index] is StatusBarItem newItem)
            {
              newItem.Checked += new RoutedEventHandler(this.OnItemChecked);
              newItem.Unchecked += new RoutedEventHandler(this.OnItemUnchecked);
              this.contextMenu.Items.Insert(e.NewStartingIndex + index + 1, (object) new StatusBarMenuItem(newItem));
            }
            else
              this.contextMenu.Items.Insert(e.NewStartingIndex + index + 1, (object) new Separator());
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          for (int index = 0; index < e.OldItems.Count; ++index)
          {
            if (this.contextMenu.Items[e.OldStartingIndex + 1] is StatusBarMenuItem statusBarMenuItem)
            {
              statusBarMenuItem.StatusBarItem.Checked += new RoutedEventHandler(this.OnItemChecked);
              statusBarMenuItem.StatusBarItem.Unchecked += new RoutedEventHandler(this.OnItemUnchecked);
            }
            this.contextMenu.Items.RemoveAt(e.OldStartingIndex + 1);
          }
          break;
        case NotifyCollectionChangedAction.Replace:
          for (int index = 0; index < e.OldItems.Count; ++index)
          {
            if (this.contextMenu.Items[e.OldStartingIndex + 1] is StatusBarMenuItem statusBarMenuItem)
            {
              statusBarMenuItem.StatusBarItem.Checked += new RoutedEventHandler(this.OnItemChecked);
              statusBarMenuItem.StatusBarItem.Unchecked += new RoutedEventHandler(this.OnItemUnchecked);
            }
            this.contextMenu.Items.RemoveAt(e.OldStartingIndex + 1);
          }
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (e.NewItems[index] is StatusBarItem newItem)
            {
              newItem.Checked += new RoutedEventHandler(this.OnItemChecked);
              newItem.Unchecked += new RoutedEventHandler(this.OnItemUnchecked);
              this.contextMenu.Items.Insert(e.NewStartingIndex + index + 1, (object) new StatusBarMenuItem(newItem));
            }
            else
              this.contextMenu.Items.Insert(e.NewStartingIndex + index + 1, (object) new Separator());
          }
          break;
        case NotifyCollectionChangedAction.Move:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            object insertItem = this.contextMenu.Items[e.OldStartingIndex + 1];
            this.contextMenu.Items.RemoveAt(e.OldStartingIndex + 1);
            this.contextMenu.Items.Insert(e.NewStartingIndex + index + 1, insertItem);
          }
          break;
        case NotifyCollectionChangedAction.Reset:
          this.RecreateMenu();
          break;
      }
    }

    private void OnItemUnchecked(object sender, RoutedEventArgs e)
    {
      this.UpdateSeparartorsVisibility();
    }

    private void OnItemChecked(object sender, RoutedEventArgs e)
    {
      this.UpdateSeparartorsVisibility();
    }

    private void RecreateMenu()
    {
      this.contextMenu.Items.Clear();
      this.contextMenu.Items.Add((object) new GroupSeparatorMenuItem());
      RibbonControl.Bind((object) Ribbon.Localization, this.contextMenu.Items[0] as FrameworkElement, "CustomizeStatusBar", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] is StatusBarItem statusBarItem)
        {
          statusBarItem.Checked += new RoutedEventHandler(this.OnItemChecked);
          statusBarItem.Unchecked += new RoutedEventHandler(this.OnItemUnchecked);
          this.contextMenu.Items.Add((object) new StatusBarMenuItem(statusBarItem));
        }
        else
          this.contextMenu.Items.Add((object) new Separator());
      }
      this.UpdateSeparartorsVisibility();
    }

    private void UpdateSeparartorsVisibility()
    {
      bool flag1 = false;
      bool flag2 = true;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] is Separator separator)
        {
          if (flag1 || flag2)
            separator.Visibility = Visibility.Collapsed;
          else
            separator.Visibility = Visibility.Visible;
          flag1 = true;
          flag2 = false;
        }
        else if (this.Items[index] is StatusBarItem && (this.Items[index] as StatusBarItem).Visibility == Visibility.Visible)
        {
          flag1 = false;
          flag2 = false;
        }
      }
    }
  }
}
