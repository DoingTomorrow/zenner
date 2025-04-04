// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Glow
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class Glow : Control
  {
    public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof (GlowBrush), typeof (Brush), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Transparent));
    public static readonly DependencyProperty NonActiveGlowBrushProperty = DependencyProperty.Register(nameof (NonActiveGlowBrush), typeof (Brush), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Transparent));
    public static readonly DependencyProperty IsGlowProperty = DependencyProperty.Register(nameof (IsGlow), typeof (bool), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) Orientation.Vertical));
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), typeof (GlowDirection), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) GlowDirection.Top));

    static Glow()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Glow), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Glow)));
    }

    public Brush GlowBrush
    {
      get => (Brush) this.GetValue(Glow.GlowBrushProperty);
      set => this.SetValue(Glow.GlowBrushProperty, (object) value);
    }

    public Brush NonActiveGlowBrush
    {
      get => (Brush) this.GetValue(Glow.NonActiveGlowBrushProperty);
      set => this.SetValue(Glow.NonActiveGlowBrushProperty, (object) value);
    }

    public bool IsGlow
    {
      get => (bool) this.GetValue(Glow.IsGlowProperty);
      set => this.SetValue(Glow.IsGlowProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(Glow.OrientationProperty);
      set => this.SetValue(Glow.OrientationProperty, (object) value);
    }

    public GlowDirection Direction
    {
      get => (GlowDirection) this.GetValue(Glow.DirectionProperty);
      set => this.SetValue(Glow.DirectionProperty, (object) value);
    }
  }
}
