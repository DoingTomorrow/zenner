// Decompiled with JetBrains decompiler
// Type: MSS.Data.Utils.DynamicProperty
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using System;

#nullable disable
namespace MSS.Data.Utils
{
  public class DynamicProperty
  {
    private string name;
    private Type type;

    public DynamicProperty(string name, Type type)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      this.name = name;
      this.type = type;
    }

    public string Name => this.name;

    public Type Type => this.type;
  }
}
