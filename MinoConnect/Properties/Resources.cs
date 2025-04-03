// Decompiled with JetBrains decompiler
// Type: MinoConnect.Properties.Resources
// Assembly: MinoConnect, Version=1.5.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E4D0ECC-943B-4E96-B8E2-CE02CEE9906B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinoConnect.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace MinoConnect.Properties
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
        if (MinoConnect.Properties.Resources.resourceMan == null)
          MinoConnect.Properties.Resources.resourceMan = new ResourceManager("MinoConnect.Properties.Resources", typeof (MinoConnect.Properties.Resources).Assembly);
        return MinoConnect.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => MinoConnect.Properties.Resources.resourceCulture;
      set => MinoConnect.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap refresh
    {
      get
      {
        return (Bitmap) MinoConnect.Properties.Resources.ResourceManager.GetObject(nameof (refresh), MinoConnect.Properties.Resources.resourceCulture);
      }
    }
  }
}
