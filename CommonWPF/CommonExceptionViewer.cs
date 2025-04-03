// Decompiled with JetBrains decompiler
// Type: CommonWPF.CommonExceptionViewer
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;

#nullable disable
namespace CommonWPF
{
  public static class CommonExceptionViewer
  {
    public static void Show(Exception ex, string headerInfo = null)
    {
      ExceptionViewer.Show(ex, headerInfo);
    }
  }
}
