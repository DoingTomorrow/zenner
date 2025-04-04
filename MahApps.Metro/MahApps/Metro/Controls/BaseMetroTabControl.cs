// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.BaseMetroTabControl
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  public abstract class BaseMetroTabControl : TabControl
  {
    public static readonly DependencyProperty TabStripMarginProperty = DependencyProperty.Register(nameof (TabStripMargin), typeof (Thickness), typeof (BaseMetroTabControl), new PropertyMetadata((object) new Thickness(0.0)));
    public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register(nameof (CloseTabCommand), typeof (ICommand), typeof (BaseMetroTabControl), new PropertyMetadata((PropertyChangedCallback) null));
    private static readonly DependencyProperty InternalCloseTabCommandProperty = DependencyProperty.Register(nameof (InternalCloseTabCommand), typeof (ICommand), typeof (BaseMetroTabControl), new PropertyMetadata((PropertyChangedCallback) null));

    public BaseMetroTabControl()
    {
      this.InternalCloseTabCommand = (ICommand) new BaseMetroTabControl.DefaultCloseTabCommand(this);
    }

    public Thickness TabStripMargin
    {
      get => (Thickness) this.GetValue(BaseMetroTabControl.TabStripMarginProperty);
      set => this.SetValue(BaseMetroTabControl.TabStripMarginProperty, (object) value);
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is TabItem;

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new MetroTabItem();
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      if (element != item)
        element.SetCurrentValue(FrameworkElement.DataContextProperty, item);
      base.PrepareContainerForItemOverride(element, item);
    }

    public ICommand CloseTabCommand
    {
      get => (ICommand) this.GetValue(BaseMetroTabControl.CloseTabCommandProperty);
      set => this.SetValue(BaseMetroTabControl.CloseTabCommandProperty, (object) value);
    }

    internal ICommand InternalCloseTabCommand
    {
      get => (ICommand) this.GetValue(BaseMetroTabControl.InternalCloseTabCommandProperty);
      set => this.SetValue(BaseMetroTabControl.InternalCloseTabCommandProperty, (object) value);
    }

    public event BaseMetroTabControl.TabItemClosingEventHandler TabItemClosingEvent;

    internal bool RaiseTabItemClosingEvent(MetroTabItem closingItem)
    {
      if (this.TabItemClosingEvent != null)
      {
        foreach (BaseMetroTabControl.TabItemClosingEventHandler invocation in this.TabItemClosingEvent.GetInvocationList())
        {
          BaseMetroTabControl.TabItemClosingEventArgs e = new BaseMetroTabControl.TabItemClosingEventArgs(closingItem);
          invocation((object) this, e);
          if (e.Cancel)
            return true;
        }
      }
      return false;
    }

    public delegate void TabItemClosingEventHandler(
      object sender,
      BaseMetroTabControl.TabItemClosingEventArgs e);

    public class TabItemClosingEventArgs : CancelEventArgs
    {
      internal TabItemClosingEventArgs(MetroTabItem item) => this.ClosingTabItem = item;

      public MetroTabItem ClosingTabItem { get; private set; }
    }

    internal class DefaultCloseTabCommand : ICommand
    {
      private readonly BaseMetroTabControl owner;

      internal DefaultCloseTabCommand(BaseMetroTabControl Owner) => this.owner = Owner;

      public bool CanExecute(object parameter) => true;

      public event EventHandler CanExecuteChanged;

      public void Execute(object parameter)
      {
        if (parameter == null)
          return;
        Tuple<object, MetroTabItem> tuple = (Tuple<object, MetroTabItem>) parameter;
        if (this.owner.CloseTabCommand != null)
        {
          this.owner.CloseTabCommand.Execute((object) null);
        }
        else
        {
          if (tuple.Item2 == null)
            return;
          MetroTabItem tabItem = tuple.Item2;
          if (this.owner.RaiseTabItemClosingEvent(tabItem))
            return;
          if (this.owner.ItemsSource == null)
          {
            this.owner.Items.Remove((object) tabItem);
          }
          else
          {
            if (!(this.owner.ItemsSource is IList itemsSource))
              return;
            using (IEnumerator<object> enumerator = this.owner.ItemsSource.Cast<object>().Where<object>((Func<object, bool>) (item => tabItem == item || tabItem.DataContext == item)).GetEnumerator())
            {
              if (!enumerator.MoveNext())
                return;
              object current = enumerator.Current;
              itemsSource.Remove(current);
            }
          }
        }
      }
    }
  }
}
