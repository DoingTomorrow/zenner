// Decompiled with JetBrains decompiler
// Type: Fluent.GalleryGroupIcon
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class GalleryGroupIcon : DependencyObject
  {
    public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof (GroupName), typeof (string), typeof (GalleryGroupIcon), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (GalleryGroupIcon), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public string GroupName
    {
      get => (string) this.GetValue(GalleryGroupIcon.GroupNameProperty);
      set => this.SetValue(GalleryGroupIcon.GroupNameProperty, (object) value);
    }

    public ImageSource Icon
    {
      get => (ImageSource) this.GetValue(GalleryGroupIcon.IconProperty);
      set => this.SetValue(GalleryGroupIcon.IconProperty, (object) value);
    }
  }
}
