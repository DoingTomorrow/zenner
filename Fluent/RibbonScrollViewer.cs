// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonScrollViewer
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class RibbonScrollViewer : ScrollViewer
  {
    protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
    {
      return this.VisualChildrenCount > 0 ? VisualTreeHelper.HitTest(this.GetVisualChild(0), hitTestParameters.HitPoint) : base.HitTestCore(hitTestParameters);
    }
  }
}
