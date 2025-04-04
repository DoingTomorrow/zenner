// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.PreApplicationStartCode
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.ComponentModel;
using System.Web.UI;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.WebPages
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
      WebPageHttpHandler.RegisterExtension("cshtml");
      WebPageHttpHandler.RegisterExtension("vbhtml");
      PageParser.EnableLongStringsAsResources = false;
      DynamicModuleUtility.RegisterModule(typeof (WebPageHttpModule));
      ScopeStorage.CurrentProvider = (IScopeStorageProvider) new AspNetRequestScopeStorageProvider();
    }
  }
}
