// Decompiled with JetBrains decompiler
// Type: Fluent.QuickAccessItemsProvider
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  internal static class QuickAccessItemsProvider
  {
    public static bool IsSupported(UIElement element)
    {
      return element is IQuickAccessItemProvider accessItemProvider && accessItemProvider.CanAddToQuickAccessToolBar;
    }

    [SuppressMessage("Microsoft.Performance", "CA1800")]
    public static FrameworkElement GetQuickAccessItem(UIElement element)
    {
      FrameworkElement frameworkElement = (FrameworkElement) null;
      if (element is IQuickAccessItemProvider accessItemProvider && accessItemProvider.CanAddToQuickAccessToolBar)
        frameworkElement = ((IQuickAccessItemProvider) element).CreateQuickAccessItem();
      return frameworkElement != null ? frameworkElement : throw new ArgumentException("The contol " + element.GetType().Name + " is not able to provide a quick access toolbar item");
    }

    public static FrameworkElement FindSupportedControl(Visual visual, Point point)
    {
      HitTestResult hitTestResult = VisualTreeHelper.HitTest(visual, point);
      if (hitTestResult == null)
        return (FrameworkElement) null;
      FrameworkElement parent1;
      FrameworkElement parent2;
      for (FrameworkElement supportedControl = hitTestResult.VisualHit as FrameworkElement; supportedControl != null; supportedControl = parent1 ?? parent2)
      {
        if (QuickAccessItemsProvider.IsSupported((UIElement) supportedControl))
          return supportedControl;
        parent1 = VisualTreeHelper.GetParent((DependencyObject) supportedControl) as FrameworkElement;
        parent2 = LogicalTreeHelper.GetParent((DependencyObject) supportedControl) as FrameworkElement;
      }
      return (FrameworkElement) null;
    }
  }
}
