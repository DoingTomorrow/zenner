// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Utils.Signature
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS.PartialSyncData.Utils
{
  internal class Signature : IEquatable<Signature>
  {
    public DynamicProperty[] properties;
    public int hashCode;

    public Signature(IEnumerable<DynamicProperty> properties)
    {
      this.properties = properties.ToArray<DynamicProperty>();
      this.hashCode = 0;
      foreach (DynamicProperty property in properties)
        this.hashCode ^= property.Name.GetHashCode() ^ property.Type.GetHashCode();
    }

    public override int GetHashCode() => this.hashCode;

    public override bool Equals(object obj) => obj is Signature && this.Equals((Signature) obj);

    public bool Equals(Signature other)
    {
      if (this.properties.Length != other.properties.Length)
        return false;
      for (int index = 0; index < this.properties.Length; ++index)
      {
        if (this.properties[index].Name != other.properties[index].Name || this.properties[index].Type != other.properties[index].Type)
          return false;
      }
      return true;
    }
  }
}
