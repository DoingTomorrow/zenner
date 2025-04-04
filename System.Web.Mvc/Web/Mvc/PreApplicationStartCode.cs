// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.PreApplicationStartCode
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.Mvc
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
      System.Web.WebPages.Razor.PreApplicationStartCode.Start();
      System.Web.WebPages.PreApplicationStartCode.Start();
      ViewContext.GlobalScopeThunk = (Func<IDictionary<object, object>>) (() => ScopeStorage.CurrentScope);
    }
  }
}
