// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SR
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Data.SQLite
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal sealed class SR
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal SR()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) SR.resourceMan, (object) null))
          SR.resourceMan = new ResourceManager("System.Data.SQLite.SR", typeof (SR).Assembly);
        return SR.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => SR.resourceCulture;
      set => SR.resourceCulture = value;
    }

    internal static string DataTypes
    {
      get => SR.ResourceManager.GetString(nameof (DataTypes), SR.resourceCulture);
    }

    internal static string Keywords
    {
      get => SR.ResourceManager.GetString(nameof (Keywords), SR.resourceCulture);
    }

    internal static string MetaDataCollections
    {
      get => SR.ResourceManager.GetString(nameof (MetaDataCollections), SR.resourceCulture);
    }
  }
}
