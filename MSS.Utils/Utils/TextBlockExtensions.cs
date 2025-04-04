// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.TextBlockExtensions
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MSS.Utils.Utils
{
  public static class TextBlockExtensions
  {
    public static void SetTextBlockColor(this IEnumerable<TextBlock> source)
    {
      foreach (TextBlock textBlock in source)
        textBlock.Background = (Brush) Brushes.LightGreen;
    }

    public static IEnumerable<TextBlock> ResetColor(this IEnumerable<TextBlock> source)
    {
      foreach (TextBlock textBlock in source)
        textBlock.Background = (Brush) null;
      return source;
    }
  }
}
