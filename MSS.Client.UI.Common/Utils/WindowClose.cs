// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.Utils.WindowClose
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS.Client.UI.Common.Utils
{
  public static class WindowClose
  {
    public static DependencyProperty CloseCommandProperty = DependencyProperty.RegisterAttached("CloseCommand", typeof (ICommand), typeof (WindowClose), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowClose.CloseCommandPropertyChangedCallback)));
    public static DependencyProperty CloseFailCommandProperty = DependencyProperty.RegisterAttached("CloseFailCommand", typeof (ICommand), typeof (WindowClose), new PropertyMetadata((PropertyChangedCallback) null));

    public static ICommand GetCloseCommand(DependencyObject dependencyObject)
    {
      return (ICommand) dependencyObject.GetValue(WindowClose.CloseCommandProperty);
    }

    public static void SetCloseCommand(DependencyObject dependencyObject, ICommand value)
    {
      dependencyObject.SetValue(WindowClose.CloseCommandProperty, (object) value);
    }

    public static ICommand GetCloseFailCommand(DependencyObject dependencyObject)
    {
      return (ICommand) dependencyObject.GetValue(WindowClose.CloseFailCommandProperty);
    }

    public static void SetCloseFailCommand(DependencyObject dependencyObject, ICommand value)
    {
      dependencyObject.SetValue(WindowClose.CloseFailCommandProperty, (object) value);
    }

    private static void CloseCommandPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is Window window))
        return;
      ICommand newValue = e.NewValue as ICommand;
      if (e.OldValue is ICommand)
      {
        window.Closing -= new CancelEventHandler(WindowClose.OnWindowClosing);
        window.Closed -= new EventHandler(WindowClose.OnWindowClosed);
      }
      if (newValue != null)
      {
        window.Closing += new CancelEventHandler(WindowClose.OnWindowClosing);
        window.Closed += new EventHandler(WindowClose.OnWindowClosed);
      }
    }

    private static void OnWindowClosed(object sender, EventArgs e)
    {
      DependencyObject dependencyObject = (DependencyObject) (sender as Window);
      if (dependencyObject == null)
        return;
      WindowClose.GetCloseCommand(dependencyObject)?.Execute((object) null);
    }

    private static void OnWindowClosing(object sender, CancelEventArgs e)
    {
      DependencyObject dependencyObject = (DependencyObject) (sender as Window);
      if (dependencyObject == null)
        return;
      bool flag = false;
      ICommand closeCommand = WindowClose.GetCloseCommand(dependencyObject);
      ICommand closeFailCommand = WindowClose.GetCloseFailCommand(dependencyObject);
      if (closeCommand != null)
        flag = !closeCommand.CanExecute((object) null);
      if (flag && closeFailCommand != null)
        closeFailCommand.Execute((object) null);
      if (closeCommand != null)
        flag = !closeCommand.CanExecute((object) null);
      e.Cancel = flag;
    }
  }
}
