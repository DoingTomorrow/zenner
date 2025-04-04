// Decompiled with JetBrains decompiler
// Type: StartupLib.Properties.Resources
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace StartupLib.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  public class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
      get
      {
        if (StartupLib.Properties.Resources.resourceMan == null)
          StartupLib.Properties.Resources.resourceMan = new ResourceManager("StartupLib.Properties.Resources", typeof (StartupLib.Properties.Resources).Assembly);
        return StartupLib.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => StartupLib.Properties.Resources.resourceCulture;
      set => StartupLib.Properties.Resources.resourceCulture = value;
    }

    public static Bitmap HeaderImage
    {
      get
      {
        return (Bitmap) StartupLib.Properties.Resources.ResourceManager.GetObject(nameof (HeaderImage), StartupLib.Properties.Resources.resourceCulture);
      }
    }
  }
}
