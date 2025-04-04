// Decompiled with JetBrains decompiler
// Type: Fluent.BackstageAdorner
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  internal class BackstageAdorner : Adorner
  {
    private readonly UIElement backstage;
    private readonly double topOffset;
    private readonly VisualCollection visualChildren;

    public BackstageAdorner(FrameworkElement adornedElement, UIElement backstage, double topOffset)
      : base((UIElement) adornedElement)
    {
      this.backstage = backstage;
      this.topOffset = topOffset;
      this.visualChildren = new VisualCollection((Visual) this);
      this.visualChildren.Add((Visual) backstage);
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.Unloaded += new RoutedEventHandler(this.OnUnloaded);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      CompositionTarget.Rendering += new EventHandler(this.CompositionTargetRendering);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      CompositionTarget.Rendering -= new EventHandler(this.CompositionTargetRendering);
    }

    private void CompositionTargetRendering(object sender, EventArgs e)
    {
      if (!(this.RenderSize != this.AdornedElement.RenderSize))
        return;
      this.InvalidateMeasure();
    }

    public void Clear() => this.visualChildren.Clear();

    protected override Size ArrangeOverride(Size finalSize)
    {
      this.backstage.Arrange(new System.Windows.Rect(0.0, this.topOffset, finalSize.Width, Math.Max(0.0, finalSize.Height - this.topOffset)));
      return finalSize;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      this.backstage.Measure(new Size(this.AdornedElement.RenderSize.Width, Math.Max(0.0, this.AdornedElement.RenderSize.Height - this.topOffset)));
      return this.AdornedElement.RenderSize;
    }

    protected override int VisualChildrenCount => this.visualChildren.Count;

    protected override Visual GetVisualChild(int index) => this.visualChildren[index];
  }
}
