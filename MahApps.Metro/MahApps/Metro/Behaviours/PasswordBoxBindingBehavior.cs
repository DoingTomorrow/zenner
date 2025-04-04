// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.PasswordBoxBindingBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
  {
    private static bool IsUpdating;
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof (string), typeof (PasswordBoxBindingBehavior), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, new PropertyChangedCallback(PasswordBoxBindingBehavior.OnPasswordPropertyChanged)));

    [Category("MahApps.Metro")]
    public static string GetPassword(DependencyObject dpo)
    {
      return (string) dpo.GetValue(PasswordBoxBindingBehavior.PasswordProperty);
    }

    public static void SetPassword(DependencyObject dpo, string value)
    {
      dpo.SetValue(PasswordBoxBindingBehavior.PasswordProperty, (object) value);
    }

    private static void OnPasswordPropertyChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is PasswordBox passwordBox))
        return;
      passwordBox.PasswordChanged -= new RoutedEventHandler(PasswordBoxBindingBehavior.PasswordBox_PasswordChanged);
      if (!PasswordBoxBindingBehavior.IsUpdating)
        passwordBox.Password = (string) e.NewValue;
      passwordBox.PasswordChanged += new RoutedEventHandler(PasswordBoxBindingBehavior.PasswordBox_PasswordChanged);
    }

    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
      PasswordBox dpo = sender as PasswordBox;
      PasswordBoxBindingBehavior.IsUpdating = true;
      PasswordBoxBindingBehavior.SetPassword((DependencyObject) dpo, dpo.Password);
      PasswordBoxBindingBehavior.IsUpdating = false;
    }
  }
}
