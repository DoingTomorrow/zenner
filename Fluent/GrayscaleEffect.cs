// Decompiled with JetBrains decompiler
// Type: Fluent.GrayscaleEffect
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

#nullable disable
namespace Fluent
{
  public class GrayscaleEffect : ShaderEffect
  {
    public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof (Input), typeof (GrayscaleEffect), 0);
    public static readonly DependencyProperty FilterColorProperty = DependencyProperty.Register(nameof (FilterColor), typeof (Color), typeof (GrayscaleEffect), (PropertyMetadata) new UIPropertyMetadata((object) Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), ShaderEffect.PixelShaderConstantCallback(0)));

    public GrayscaleEffect()
    {
      PixelShader pixelShader = new PixelShader();
      if (!(bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof (FrameworkElement)).Metadata.DefaultValue)
        pixelShader.UriSource = new Uri("/Fluent;component/Themes/Office2010/Effects/Grayscale.ps", UriKind.Relative);
      this.PixelShader = pixelShader;
      this.UpdateShaderValue(GrayscaleEffect.InputProperty);
      this.UpdateShaderValue(GrayscaleEffect.FilterColorProperty);
    }

    public Brush Input
    {
      get => (Brush) this.GetValue(GrayscaleEffect.InputProperty);
      set => this.SetValue(GrayscaleEffect.InputProperty, (object) value);
    }

    public Color FilterColor
    {
      get => (Color) this.GetValue(GrayscaleEffect.FilterColorProperty);
      set => this.SetValue(GrayscaleEffect.FilterColorProperty, (object) value);
    }
  }
}
