// Decompiled with JetBrains decompiler
// Type: CommonWPF.Properties.Resources
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace CommonWPF.Properties
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
        if (CommonWPF.Properties.Resources.resourceMan == null)
          CommonWPF.Properties.Resources.resourceMan = new ResourceManager("CommonWPF.Properties.Resources", typeof (CommonWPF.Properties.Resources).Assembly);
        return CommonWPF.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => CommonWPF.Properties.Resources.resourceCulture;
      set => CommonWPF.Properties.Resources.resourceCulture = value;
    }

    public static Bitmap HeaderImage
    {
      get
      {
        return (Bitmap) CommonWPF.Properties.Resources.ResourceManager.GetObject(nameof (HeaderImage), CommonWPF.Properties.Resources.resourceCulture);
      }
    }

    public static Icon ZR
    {
      get => (Icon) CommonWPF.Properties.Resources.ResourceManager.GetObject(nameof (ZR), CommonWPF.Properties.Resources.resourceCulture);
    }
  }
}
