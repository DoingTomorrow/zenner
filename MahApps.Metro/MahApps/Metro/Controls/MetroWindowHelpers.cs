// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroWindowHelpers
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Microsoft.Windows.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  internal static class MetroWindowHelpers
  {
    public static void SetIsHitTestVisibleInChromeProperty<T>(this MetroWindow window, string name) where T : DependencyObject
    {
      if (window == null)
        return;
      T part = window.GetPart<T>(name);
      if ((object) part == null)
        return;
      part.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, (object) true);
    }

    public static void SetWindowChromeResizeGripDirection(
      this MetroWindow window,
      string name,
      ResizeGripDirection direction)
    {
      if (window == null || !(window.GetPart(name) is IInputElement part))
        return;
      WindowChrome.SetResizeGripDirection(part, direction);
    }

    public static void HandleWindowCommandsForFlyouts(
      this MetroWindow window,
      IEnumerable<Flyout> flyouts,
      Brush resetBrush = null)
    {
      IEnumerable<Flyout> source = flyouts.Where<Flyout>((Func<Flyout, bool>) (x => x.IsOpen));
      if (!source.Any<Flyout>((Func<Flyout, bool>) (x => x.Position != Position.Bottom)))
      {
        if (resetBrush == null)
          window.ResetAllWindowCommandsBrush();
        else
          window.ChangeAllWindowCommandsBrush(resetBrush);
      }
      Flyout flyout1 = source.Where<Flyout>((Func<Flyout, bool>) (x => x.Position == Position.Top)).OrderByDescending<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex)).FirstOrDefault<Flyout>();
      if (flyout1 != null)
      {
        window.UpdateWindowCommandsForFlyout(flyout1);
      }
      else
      {
        Flyout flyout2 = source.Where<Flyout>((Func<Flyout, bool>) (x => x.Position == Position.Left)).OrderByDescending<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex)).FirstOrDefault<Flyout>();
        if (flyout2 != null)
          window.UpdateWindowCommandsForFlyout(flyout2);
        Flyout flyout3 = source.Where<Flyout>((Func<Flyout, bool>) (x => x.Position == Position.Right)).OrderByDescending<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex)).FirstOrDefault<Flyout>();
        if (flyout3 == null)
          return;
        window.UpdateWindowCommandsForFlyout(flyout3);
      }
    }

    public static void ResetAllWindowCommandsBrush(this MetroWindow window)
    {
      window.ChangeAllWindowCommandsBrush((Brush) window.OverrideDefaultWindowCommandsBrush);
    }

    public static void UpdateWindowCommandsForFlyout(this MetroWindow window, Flyout flyout)
    {
      window.ChangeAllWindowCommandsBrush(flyout.Foreground, flyout.Position);
    }

    private static void InvokeActionOnWindowCommands(
      this MetroWindow window,
      Action<Control> action1,
      Action<Control> action2 = null,
      Position position = Position.Top)
    {
      if (window.LeftWindowCommandsPresenter == null || window.RightWindowCommandsPresenter == null || window.WindowButtonCommands == null)
        return;
      if (position == Position.Left || position == Position.Top)
        action1((Control) window.LeftWindowCommands);
      if (position != Position.Right && position != Position.Top)
        return;
      action1((Control) window.RightWindowCommands);
      if (action2 == null)
        action1((Control) window.WindowButtonCommands);
      else
        action2((Control) window.WindowButtonCommands);
    }

    private static void ChangeAllWindowCommandsBrush(
      this MetroWindow window,
      Brush brush,
      Position position = Position.Top)
    {
      if (brush == null)
      {
        window.InvokeActionOnWindowCommands((Action<Control>) (x => x.SetValue(WindowCommands.ThemeProperty, (object) Theme.Light)), (Action<Control>) (x => x.SetValue(WindowButtonCommands.ThemeProperty, (object) Theme.Light)), position);
        window.InvokeActionOnWindowCommands((Action<Control>) (x => x.ClearValue(Control.ForegroundProperty)), position: position);
      }
      else
      {
        Color color = ((SolidColorBrush) brush).Color;
        float num1 = (float) color.R / (float) byte.MaxValue;
        float num2 = (float) color.G / (float) byte.MaxValue;
        float num3 = (float) color.B / (float) byte.MaxValue;
        float num4 = num1;
        float num5 = num1;
        if ((double) num2 > (double) num4)
          num4 = num2;
        if ((double) num3 > (double) num4)
          num4 = num3;
        if ((double) num2 < (double) num5)
          num5 = num2;
        if ((double) num3 < (double) num5)
          num5 = num3;
        if (((double) num4 + (double) num5) / 2.0 > 0.1)
          window.InvokeActionOnWindowCommands((Action<Control>) (x => x.SetValue(WindowCommands.ThemeProperty, (object) Theme.Light)), (Action<Control>) (x => x.SetValue(WindowButtonCommands.ThemeProperty, (object) Theme.Light)), position);
        else
          window.InvokeActionOnWindowCommands((Action<Control>) (x => x.SetValue(WindowCommands.ThemeProperty, (object) Theme.Dark)), (Action<Control>) (x => x.SetValue(WindowButtonCommands.ThemeProperty, (object) Theme.Dark)), position);
        window.InvokeActionOnWindowCommands((Action<Control>) (x => x.SetValue(Control.ForegroundProperty, (object) brush)), position: position);
      }
    }
  }
}
