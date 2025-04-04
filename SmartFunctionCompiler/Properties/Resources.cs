// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.Properties.Resources
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace SmartFunctionCompiler.Properties
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
        if (SmartFunctionCompiler.Properties.Resources.resourceMan == null)
          SmartFunctionCompiler.Properties.Resources.resourceMan = new ResourceManager("SmartFunctionCompiler.Properties.Resources", typeof (SmartFunctionCompiler.Properties.Resources).Assembly);
        return SmartFunctionCompiler.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => SmartFunctionCompiler.Properties.Resources.resourceCulture;
      set => SmartFunctionCompiler.Properties.Resources.resourceCulture = value;
    }
  }
}
