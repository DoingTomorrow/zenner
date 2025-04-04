// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ContentControlEx
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class ContentControlEx : ContentControl
  {
    public static readonly DependencyProperty ContentCharacterCasingProperty = DependencyProperty.Register(nameof (ContentCharacterCasing), typeof (CharacterCasing), typeof (ContentControlEx), (PropertyMetadata) new FrameworkPropertyMetadata((object) CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits), (ValidateValueCallback) (value => CharacterCasing.Normal <= (CharacterCasing) value && (CharacterCasing) value <= CharacterCasing.Upper));
    public static readonly DependencyProperty RecognizesAccessKeyProperty = DependencyProperty.Register(nameof (RecognizesAccessKey), typeof (bool), typeof (ContentControlEx), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));

    public CharacterCasing ContentCharacterCasing
    {
      get => (CharacterCasing) this.GetValue(ContentControlEx.ContentCharacterCasingProperty);
      set => this.SetValue(ContentControlEx.ContentCharacterCasingProperty, (object) value);
    }

    public bool RecognizesAccessKey
    {
      get => (bool) this.GetValue(ContentControlEx.RecognizesAccessKeyProperty);
      set => this.SetValue(ContentControlEx.RecognizesAccessKeyProperty, (object) value);
    }

    static ContentControlEx()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ContentControlEx), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ContentControlEx)));
    }
  }
}
