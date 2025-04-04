// Decompiled with JetBrains decompiler
// Type: SQLitePCL.Resources
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace SQLitePCL
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
        if (object.ReferenceEquals((object) SQLitePCL.Resources.resourceMan, (object) null))
          SQLitePCL.Resources.resourceMan = new ResourceManager("SQLitePCL.Resources", typeof (SQLitePCL.Resources).GetTypeInfo().Assembly);
        return SQLitePCL.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => SQLitePCL.Resources.resourceCulture;
      set => SQLitePCL.Resources.resourceCulture = value;
    }

    internal static string Platform_AssemblyNotFound
    {
      get
      {
        return SQLitePCL.Resources.ResourceManager.GetString(nameof (Platform_AssemblyNotFound), SQLitePCL.Resources.resourceCulture);
      }
    }
  }
}
