// Decompiled with JetBrains decompiler
// Type: ZENNER.Properties.Resources
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ZENNER.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (ZENNER.Properties.Resources.resourceMan == null)
          ZENNER.Properties.Resources.resourceMan = new ResourceManager("ZENNER.Properties.Resources", typeof (ZENNER.Properties.Resources).Assembly);
        return ZENNER.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ZENNER.Properties.Resources.resourceCulture;
      set => ZENNER.Properties.Resources.resourceCulture = value;
    }
  }
}
