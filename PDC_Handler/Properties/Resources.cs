// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Properties.Resources
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace PDC_Handler.Properties
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
        if (PDC_Handler.Properties.Resources.resourceMan == null)
          PDC_Handler.Properties.Resources.resourceMan = new ResourceManager("PDC_Handler.Properties.Resources", typeof (PDC_Handler.Properties.Resources).Assembly);
        return PDC_Handler.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => PDC_Handler.Properties.Resources.resourceCulture;
      set => PDC_Handler.Properties.Resources.resourceCulture = value;
    }
  }
}
