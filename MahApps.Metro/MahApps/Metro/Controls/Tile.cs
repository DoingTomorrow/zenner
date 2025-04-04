// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Tile
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class Tile : Button
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (Tile), new PropertyMetadata((object) null));
    public static readonly DependencyProperty CountProperty = DependencyProperty.Register(nameof (Count), typeof (string), typeof (Tile), new PropertyMetadata((object) null));
    public static readonly DependencyProperty KeepDraggingProperty = DependencyProperty.Register(nameof (KeepDragging), typeof (bool), typeof (Tile), new PropertyMetadata((object) true));
    public static readonly DependencyProperty TiltFactorProperty = DependencyProperty.Register(nameof (TiltFactor), typeof (int), typeof (Tile), new PropertyMetadata((object) 5));
    public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(nameof (TitleFontSize), typeof (int), typeof (Tile), new PropertyMetadata((object) 16));
    public static readonly DependencyProperty CountFontSizeProperty = DependencyProperty.Register(nameof (CountFontSize), typeof (int), typeof (Tile), new PropertyMetadata((object) 28));

    static Tile()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Tile), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Tile)));
    }

    public string Title
    {
      get => (string) this.GetValue(Tile.TitleProperty);
      set => this.SetValue(Tile.TitleProperty, (object) value);
    }

    public string Count
    {
      get => (string) this.GetValue(Tile.CountProperty);
      set => this.SetValue(Tile.CountProperty, (object) value);
    }

    public bool KeepDragging
    {
      get => (bool) this.GetValue(Tile.KeepDraggingProperty);
      set => this.SetValue(Tile.KeepDraggingProperty, (object) value);
    }

    public int TiltFactor
    {
      get => (int) this.GetValue(Tile.TiltFactorProperty);
      set => this.SetValue(Tile.TiltFactorProperty, (object) value);
    }

    public int TitleFontSize
    {
      get => (int) this.GetValue(Tile.TitleFontSizeProperty);
      set => this.SetValue(Tile.TitleFontSizeProperty, (object) value);
    }

    public int CountFontSize
    {
      get => (int) this.GetValue(Tile.CountFontSizeProperty);
      set => this.SetValue(Tile.CountFontSizeProperty, (object) value);
    }
  }
}
