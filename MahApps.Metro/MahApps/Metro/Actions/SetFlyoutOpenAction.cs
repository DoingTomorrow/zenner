// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Actions.SetFlyoutOpenAction
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Interactivity;

#nullable disable
namespace MahApps.Metro.Actions
{
  public class SetFlyoutOpenAction : TargetedTriggerAction<FrameworkElement>
  {
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (bool), typeof (SetFlyoutOpenAction), new PropertyMetadata((object) false));

    public bool Value
    {
      get => (bool) this.GetValue(SetFlyoutOpenAction.ValueProperty);
      set => this.SetValue(SetFlyoutOpenAction.ValueProperty, (object) value);
    }

    protected override void Invoke(object parameter)
    {
      ((Flyout) this.TargetObject).IsOpen = this.Value;
    }
  }
}
