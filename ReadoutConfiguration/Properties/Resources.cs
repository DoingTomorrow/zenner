// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.Properties.Resources
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ReadoutConfiguration.Properties
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
        if (ReadoutConfiguration.Properties.Resources.resourceMan == null)
          ReadoutConfiguration.Properties.Resources.resourceMan = new ResourceManager("ReadoutConfiguration.Properties.Resources", typeof (ReadoutConfiguration.Properties.Resources).Assembly);
        return ReadoutConfiguration.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => ReadoutConfiguration.Properties.Resources.resourceCulture;
      set => ReadoutConfiguration.Properties.Resources.resourceCulture = value;
    }
  }
}
