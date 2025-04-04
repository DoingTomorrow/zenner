// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.PreApplicationStartCode
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.ComponentModel;
using System.Web.Compilation;

#nullable disable
namespace System.Web.WebPages.Razor
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class PreApplicationStartCode
  {
    private static bool _startWasCalled;

    public static void Start()
    {
      if (PreApplicationStartCode._startWasCalled)
        return;
      PreApplicationStartCode._startWasCalled = true;
      BuildProvider.RegisterBuildProvider(".cshtml", typeof (RazorBuildProvider));
      BuildProvider.RegisterBuildProvider(".vbhtml", typeof (RazorBuildProvider));
    }
  }
}
