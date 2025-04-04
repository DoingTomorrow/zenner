// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.Resources.RazorWebResources
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.WebPages.Razor.Resources
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal class RazorWebResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal RazorWebResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) RazorWebResources.resourceMan, (object) null))
          RazorWebResources.resourceMan = new ResourceManager("System.Web.WebPages.Razor.Resources.RazorWebResources", typeof (RazorWebResources).Assembly);
        return RazorWebResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => RazorWebResources.resourceCulture;
      set => RazorWebResources.resourceCulture = value;
    }

    internal static string BuildProvider_No_CodeLanguageService_For_Path
    {
      get
      {
        return RazorWebResources.ResourceManager.GetString(nameof (BuildProvider_No_CodeLanguageService_For_Path), RazorWebResources.resourceCulture);
      }
    }

    internal static string Could_Not_Locate_FactoryType
    {
      get
      {
        return RazorWebResources.ResourceManager.GetString(nameof (Could_Not_Locate_FactoryType), RazorWebResources.resourceCulture);
      }
    }
  }
}
