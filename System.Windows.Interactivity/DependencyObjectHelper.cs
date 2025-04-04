// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.DependencyObjectHelper
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace System.Windows.Interactivity
{
  public static class DependencyObjectHelper
  {
    public static IEnumerable<DependencyObject> GetSelfAndAncestors(
      this DependencyObject dependencyObject)
    {
      for (; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
        yield return dependencyObject;
    }
  }
}
