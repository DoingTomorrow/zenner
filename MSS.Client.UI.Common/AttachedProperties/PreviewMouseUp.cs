// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.PreviewMouseUp
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class PreviewMouseUp
  {
    public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (PreviewMouseUp), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(PreviewMouseUp.CommandChanged)));
    public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof (object), typeof (PreviewMouseUp), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public static void SetCommand(DependencyObject target, ICommand value)
    {
      target.SetValue(PreviewMouseUp.CommandProperty, (object) value);
    }

    public static void SetCommandParameter(DependencyObject target, object value)
    {
      target.SetValue(PreviewMouseUp.CommandParameterProperty, value);
    }

    public static object GetCommandParameter(DependencyObject target)
    {
      return target.GetValue(PreviewMouseUp.CommandParameterProperty);
    }

    private static void CommandChanged(
      DependencyObject target,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(target is Control control))
        return;
      if (e.NewValue != null && e.OldValue == null)
        control.PreviewMouseUp += new MouseButtonEventHandler(PreviewMouseUp.OnPreviewMouseUp);
      else if (e.NewValue == null && e.OldValue != null)
        control.PreviewMouseUp -= new MouseButtonEventHandler(PreviewMouseUp.OnPreviewMouseUp);
    }

    private static void OnPreviewMouseUp(object sender, RoutedEventArgs e)
    {
      Control control = sender as Control;
      ((ICommand) control.GetValue(PreviewMouseUp.CommandProperty)).Execute(control.GetValue(PreviewMouseUp.CommandParameterProperty));
    }
  }
}
