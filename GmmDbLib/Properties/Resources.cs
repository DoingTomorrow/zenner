// Decompiled with JetBrains decompiler
// Type: GmmDbLib.Properties.Resources
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace GmmDbLib.Properties
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
        if (GmmDbLib.Properties.Resources.resourceMan == null)
          GmmDbLib.Properties.Resources.resourceMan = new ResourceManager("GmmDbLib.Properties.Resources", typeof (GmmDbLib.Properties.Resources).Assembly);
        return GmmDbLib.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => GmmDbLib.Properties.Resources.resourceCulture;
      set => GmmDbLib.Properties.Resources.resourceCulture = value;
    }
  }
}
