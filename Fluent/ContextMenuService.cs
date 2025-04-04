// Decompiled with JetBrains decompiler
// Type: Fluent.ContextMenuService
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Windows;

#nullable disable
namespace Fluent
{
  public static class ContextMenuService
  {
    public static void Attach(Type type)
    {
      System.Windows.Controls.ContextMenuService.ShowOnDisabledProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
      FrameworkElement.ContextMenuProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(ContextMenuService.OnContextMenuChanged), new CoerceValueCallback(ContextMenuService.CoerceContextMenu)));
    }

    private static void OnContextMenuChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(FrameworkElement.ContextMenuProperty);
    }

    private static object CoerceContextMenu(DependencyObject d, object basevalue)
    {
      IQuickAccessItemProvider accessItemProvider = d as IQuickAccessItemProvider;
      return basevalue == null && (accessItemProvider == null || accessItemProvider.CanAddToQuickAccessToolBar) ? (object) Ribbon.RibbonContextMenu : basevalue;
    }

    public static void Coerce(DependencyObject o)
    {
      o.CoerceValue(FrameworkElement.ContextMenuProperty);
    }
  }
}
