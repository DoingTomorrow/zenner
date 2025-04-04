// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Actions.CloseTabItemAction
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

#nullable disable
namespace MahApps.Metro.Actions
{
  public class CloseTabItemAction : TriggerAction<DependencyObject>
  {
    public static readonly DependencyProperty TabControlProperty = DependencyProperty.Register(nameof (TabControl), typeof (TabControl), typeof (CloseTabItemAction), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TabItemProperty = DependencyProperty.Register(nameof (TabItem), typeof (TabItem), typeof (CloseTabItemAction), new PropertyMetadata((object) null));

    public TabControl TabControl
    {
      get => (TabControl) this.GetValue(CloseTabItemAction.TabControlProperty);
      set => this.SetValue(CloseTabItemAction.TabControlProperty, (object) value);
    }

    public TabItem TabItem
    {
      get => (TabItem) this.GetValue(CloseTabItemAction.TabItemProperty);
      set => this.SetValue(CloseTabItemAction.TabItemProperty, (object) value);
    }

    protected override void Invoke(object parameter)
    {
      this.TabControl.Items.Remove((object) this.TabItem);
    }
  }
}
