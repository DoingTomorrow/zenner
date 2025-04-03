// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Resourcen
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ZR_ClassLibrary
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resourcen
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resourcen()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Resourcen.resourceMan == null)
          Resourcen.resourceMan = new ResourceManager("ZR_ClassLibrary.Resourcen", typeof (Resourcen).Assembly);
        return Resourcen.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Resourcen.resourceCulture;
      set => Resourcen.resourceCulture = value;
    }

    internal static string confirm_question
    {
      get
      {
        return Resourcen.ResourceManager.GetString(nameof (confirm_question), Resourcen.resourceCulture);
      }
    }

    internal static string delete
    {
      get => Resourcen.ResourceManager.GetString(nameof (delete), Resourcen.resourceCulture);
    }
  }
}
