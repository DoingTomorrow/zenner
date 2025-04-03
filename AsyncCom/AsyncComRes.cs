// Decompiled with JetBrains decompiler
// Type: AsyncCom.AsyncComRes
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace AsyncCom
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class AsyncComRes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal AsyncComRes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (AsyncComRes.resourceMan == null)
          AsyncComRes.resourceMan = new ResourceManager("AsyncCom.AsyncComRes", typeof (AsyncComRes).Assembly);
        return AsyncComRes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AsyncComRes.resourceCulture;
      set => AsyncComRes.resourceCulture = value;
    }

    internal static Bitmap RefreshButton
    {
      get
      {
        return (Bitmap) AsyncComRes.ResourceManager.GetObject(nameof (RefreshButton), AsyncComRes.resourceCulture);
      }
    }
  }
}
