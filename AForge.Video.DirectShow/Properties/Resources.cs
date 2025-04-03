// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Properties.Resources
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace AForge.Video.DirectShow.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
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
        if (object.ReferenceEquals((object) AForge.Video.DirectShow.Properties.Resources.resourceMan, (object) null))
          AForge.Video.DirectShow.Properties.Resources.resourceMan = new ResourceManager("AForge.Video.DirectShow.Properties.Resources", typeof (AForge.Video.DirectShow.Properties.Resources).Assembly);
        return AForge.Video.DirectShow.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AForge.Video.DirectShow.Properties.Resources.resourceCulture;
      set => AForge.Video.DirectShow.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap camera
    {
      get
      {
        return (Bitmap) AForge.Video.DirectShow.Properties.Resources.ResourceManager.GetObject(nameof (camera), AForge.Video.DirectShow.Properties.Resources.resourceCulture);
      }
    }
  }
}
