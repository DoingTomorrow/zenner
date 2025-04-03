// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Properties.Resources
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace CommunicationPort.Properties
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
        if (CommunicationPort.Properties.Resources.resourceMan == null)
          CommunicationPort.Properties.Resources.resourceMan = new ResourceManager("CommunicationPort.Properties.Resources", typeof (CommunicationPort.Properties.Resources).Assembly);
        return CommunicationPort.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => CommunicationPort.Properties.Resources.resourceCulture;
      set => CommunicationPort.Properties.Resources.resourceCulture = value;
    }
  }
}
