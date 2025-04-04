// Decompiled with JetBrains decompiler
// Type: Fluent.StatusBarMenuItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;

#nullable disable
namespace Fluent
{
  public class StatusBarMenuItem : MenuItem
  {
    public static readonly DependencyProperty StatusBarItemProperty = DependencyProperty.Register(nameof (StatusBarItem), typeof (StatusBarItem), typeof (StatusBarMenuItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public StatusBarItem StatusBarItem
    {
      get => (StatusBarItem) this.GetValue(StatusBarMenuItem.StatusBarItemProperty);
      set => this.SetValue(StatusBarMenuItem.StatusBarItemProperty, (object) value);
    }

    static StatusBarMenuItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (StatusBarMenuItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (StatusBarMenuItem)));
    }

    public StatusBarMenuItem(StatusBarItem item) => this.StatusBarItem = item;
  }
}
