// Decompiled with JetBrains decompiler
// Type: Fluent.GalleryGroupFilter
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;

#nullable disable
namespace Fluent
{
  public class GalleryGroupFilter : DependencyObject
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (GalleryGroupFilter), (PropertyMetadata) new UIPropertyMetadata((object) nameof (GalleryGroupFilter)));
    public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register("ContextualGroups", typeof (string), typeof (GalleryGroupFilter), (PropertyMetadata) new UIPropertyMetadata((object) ""));

    public string Title
    {
      get => (string) this.GetValue(GalleryGroupFilter.TitleProperty);
      set => this.SetValue(GalleryGroupFilter.TitleProperty, (object) value);
    }

    public string Groups
    {
      get => (string) this.GetValue(GalleryGroupFilter.GroupsProperty);
      set => this.SetValue(GalleryGroupFilter.GroupsProperty, (object) value);
    }
  }
}
