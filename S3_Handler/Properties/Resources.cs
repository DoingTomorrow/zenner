// Decompiled with JetBrains decompiler
// Type: S3_Handler.Properties.Resources
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace S3_Handler.Properties
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
        if (S3_Handler.Properties.Resources.resourceMan == null)
          S3_Handler.Properties.Resources.resourceMan = new ResourceManager("S3_Handler.Properties.Resources", typeof (S3_Handler.Properties.Resources).Assembly);
        return S3_Handler.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => S3_Handler.Properties.Resources.resourceCulture;
      set => S3_Handler.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap UndoIcon
    {
      get
      {
        return (Bitmap) S3_Handler.Properties.Resources.ResourceManager.GetObject(nameof (UndoIcon), S3_Handler.Properties.Resources.resourceCulture);
      }
    }
  }
}
