// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.PasswordBoxHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class PasswordBoxHelper
  {
    public static readonly DependencyProperty CapsLockIconProperty = DependencyProperty.RegisterAttached("CapsLockIcon", typeof (object), typeof (PasswordBoxHelper), new PropertyMetadata((object) "!", new PropertyChangedCallback(PasswordBoxHelper.ShowCapslockWarningChanged)));
    public static readonly DependencyProperty CapsLockWarningToolTipProperty = DependencyProperty.RegisterAttached("CapsLockWarningToolTip", typeof (object), typeof (PasswordBoxHelper), new PropertyMetadata((object) "Caps lock is on"));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    public static object GetCapsLockIcon(PasswordBox element)
    {
      return element.GetValue(PasswordBoxHelper.CapsLockIconProperty);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    public static object GetCapsLockWarningToolTip(PasswordBox element)
    {
      return element.GetValue(PasswordBoxHelper.CapsLockWarningToolTipProperty);
    }

    public static void SetCapsLockIcon(PasswordBox element, object value)
    {
      element.SetValue(PasswordBoxHelper.CapsLockIconProperty, value);
    }

    public static void SetCapsLockWarningToolTip(PasswordBox element, object value)
    {
      element.SetValue(PasswordBoxHelper.CapsLockWarningToolTipProperty, value);
    }

    private static void RefreshCapslockStatus(object sender, RoutedEventArgs e)
    {
      FrameworkElement capsLockIndicator = PasswordBoxHelper.FindCapsLockIndicator((Control) sender);
      if (capsLockIndicator == null)
        return;
      capsLockIndicator.Visibility = Keyboard.IsKeyToggled(Key.Capital) ? Visibility.Visible : Visibility.Collapsed;
    }

    private static FrameworkElement FindCapsLockIndicator(Control pb)
    {
      return pb.Template.FindName("PART_CapsLockIndicator", (FrameworkElement) pb) as FrameworkElement;
    }

    private static void HandlePasswordBoxLostFocus(object sender, RoutedEventArgs e)
    {
      FrameworkElement capsLockIndicator = PasswordBoxHelper.FindCapsLockIndicator((Control) sender);
      if (capsLockIndicator == null)
        return;
      capsLockIndicator.Visibility = Visibility.Collapsed;
    }

    private static void ShowCapslockWarningChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PasswordBox passwordBox = (PasswordBox) d;
      passwordBox.KeyDown -= new KeyEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
      passwordBox.GotFocus -= new RoutedEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
      passwordBox.LostFocus -= new RoutedEventHandler(PasswordBoxHelper.HandlePasswordBoxLostFocus);
      if (e.NewValue == null)
        return;
      passwordBox.KeyDown += new KeyEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
      passwordBox.GotFocus += new RoutedEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
      passwordBox.LostFocus += new RoutedEventHandler(PasswordBoxHelper.HandlePasswordBoxLostFocus);
    }
  }
}
