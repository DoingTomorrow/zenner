// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.WindowCommandsItem
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_ContentPresenter", Type = typeof (UIElement))]
  [TemplatePart(Name = "PART_Separator", Type = typeof (UIElement))]
  public class WindowCommandsItem : ContentControl
  {
    private const string PART_ContentPresenter = "PART_ContentPresenter";
    private const string PART_Separator = "PART_Separator";
    public static readonly DependencyProperty IsSeparatorVisibleProperty = DependencyProperty.Register(nameof (IsSeparatorVisible), typeof (bool), typeof (WindowCommandsItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

    internal PropertyChangeNotifier VisibilityPropertyChangeNotifier { get; set; }

    public bool IsSeparatorVisible
    {
      get => (bool) this.GetValue(WindowCommandsItem.IsSeparatorVisibleProperty);
      set => this.SetValue(WindowCommandsItem.IsSeparatorVisibleProperty, (object) value);
    }
  }
}
