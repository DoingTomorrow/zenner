// Decompiled with JetBrains decompiler
// Type: WpfKb.Properties.Resources
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace WpfKb.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (WpfKb.Properties.Resources.resourceMan == null)
          WpfKb.Properties.Resources.resourceMan = new ResourceManager("WpfKb.Properties.Resources", typeof (WpfKb.Properties.Resources).Assembly);
        return WpfKb.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => WpfKb.Properties.Resources.resourceCulture;
      set => WpfKb.Properties.Resources.resourceCulture = value;
    }
  }
}
