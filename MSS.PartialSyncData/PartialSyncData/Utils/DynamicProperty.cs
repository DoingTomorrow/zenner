// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Utils.DynamicProperty
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using System;

#nullable disable
namespace MSS.PartialSyncData.Utils
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
