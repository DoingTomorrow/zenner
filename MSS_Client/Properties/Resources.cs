// Decompiled with JetBrains decompiler
// Type: MSS_Client.Properties.Resources
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace MSS_Client.Properties
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
        if (MSS_Client.Properties.Resources.resourceMan == null)
          MSS_Client.Properties.Resources.resourceMan = new ResourceManager("MSS_Client.Properties.Resources", typeof (MSS_Client.Properties.Resources).Assembly);
        return MSS_Client.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => MSS_Client.Properties.Resources.resourceCulture;
      set => MSS_Client.Properties.Resources.resourceCulture = value;
    }
  }
}
