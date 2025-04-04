// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Configuration.Utils
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Configuration
{
  public static class Utils
  {
    public static void SetTextBlockColor(this IEnumerable<TextBlock> source)
    {
      foreach (TextBlock textBlock in source)
      {
        textBlock.Background = (Brush) Brushes.LightGreen;
        if (textBlock.Parent is Border)
          ((Border) textBlock.Parent).Background = (Brush) Brushes.LightGreen;
        textBlock.Focus();
      }
    }

    public static TResult IfNotNull<T, TResult>(
      this T @this,
      Func<T, TResult> getter,
      TResult defaultValue = null)
      where T : class
    {
      return (object) @this != null ? getter(@this) : defaultValue;
    }

    public static void IfNotNullAction<T>(this T @this, Action<T> action) where T : class
    {
      if ((object) @this == null)
        return;
      action(@this);
    }

    public static IEnumerable<TextBlock> ResetColor(this IEnumerable<TextBlock> source)
    {
      foreach (TextBlock textBlock in source)
      {
        if (textBlock.Background != null && !textBlock.Background.Equals((object) DynamicGridControl.SELECTED_PARAMETER_BACKGROUND_COLOR))
        {
          textBlock.Background = (Brush) DynamicGridControl.GetBackgroundColor(textBlock.Tag as Config);
          if (textBlock.Parent is Border)
            ((Border) textBlock.Parent).Background = textBlock.Background;
        }
      }
      return source;
    }
  }
}
