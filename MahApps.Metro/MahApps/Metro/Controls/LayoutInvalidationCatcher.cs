// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.LayoutInvalidationCatcher
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class LayoutInvalidationCatcher : Decorator
  {
    public Planerator PlaParent => this.Parent as Planerator;

    protected override Size MeasureOverride(Size constraint)
    {
      this.PlaParent?.InvalidateMeasure();
      return base.MeasureOverride(constraint);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      this.PlaParent?.InvalidateArrange();
      return base.ArrangeOverride(arrangeSize);
    }
  }
}
