// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonToolBarControlGroup
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Children")]
  public class RibbonToolBarControlGroup : ItemsControl
  {
    public static readonly DependencyProperty IsFirstInRowProperty = DependencyProperty.Register(nameof (IsFirstInRow), typeof (bool), typeof (RibbonToolBarControlGroup), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsLastInRowProperty = DependencyProperty.Register(nameof (IsLastInRow), typeof (bool), typeof (RibbonToolBarControlGroup), (PropertyMetadata) new UIPropertyMetadata((object) true));

    public bool IsFirstInRow
    {
      get => (bool) this.GetValue(RibbonToolBarControlGroup.IsFirstInRowProperty);
      set => this.SetValue(RibbonToolBarControlGroup.IsFirstInRowProperty, (object) value);
    }

    public bool IsLastInRow
    {
      get => (bool) this.GetValue(RibbonToolBarControlGroup.IsLastInRowProperty);
      set => this.SetValue(RibbonToolBarControlGroup.IsLastInRowProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonToolBarControlGroup()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonToolBarControlGroup), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonToolBarControlGroup)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonToolBarControlGroup), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonToolBarControlGroup.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonToolBarControlGroup));
      return basevalue;
    }
  }
}
