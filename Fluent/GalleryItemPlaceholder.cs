// Decompiled with JetBrains decompiler
// Type: Fluent.GalleryItemPlaceholder
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;

#nullable disable
namespace Fluent
{
  internal class GalleryItemPlaceholder : UIElement
  {
    private UIElement target;

    public UIElement Target => this.target;

    public Size ArrangedSize { get; private set; }

    public GalleryItemPlaceholder(UIElement target) => this.target = target;

    protected override Size MeasureCore(Size availableSize)
    {
      this.target.Measure(availableSize);
      return this.target.DesiredSize;
    }

    protected override void ArrangeCore(System.Windows.Rect finalRect)
    {
      base.ArrangeCore(finalRect);
      this.ArrangedSize = finalRect.Size;
    }
  }
}
