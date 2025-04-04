// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.PivotItem
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class PivotItem : ContentControl
  {
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (string), typeof (PivotItem), new PropertyMetadata((object) null));

    public string Header
    {
      get => (string) this.GetValue(PivotItem.HeaderProperty);
      set => this.SetValue(PivotItem.HeaderProperty, (object) value);
    }

    static PivotItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PivotItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PivotItem)));
    }

    public PivotItem()
    {
      this.RequestBringIntoView += (RequestBringIntoViewEventHandler) ((s, e) => e.Handled = true);
    }
  }
}
