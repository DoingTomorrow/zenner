// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.AssociationKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  internal sealed class AssociationKey
  {
    private readonly EntityKey ownerKey;
    private readonly string propertyName;
    private readonly int hashCode;

    public AssociationKey(EntityKey ownerKey, string propertyName)
    {
      this.ownerKey = ownerKey;
      this.propertyName = propertyName;
      this.hashCode = ownerKey.GetHashCode() ^ propertyName.GetHashCode() ^ ownerKey.EntityName.GetHashCode();
    }

    public override bool Equals(object that)
    {
      if (this == that)
        return true;
      return that is AssociationKey associationKey && associationKey.propertyName.Equals(this.propertyName) && associationKey.ownerKey.Equals((object) this.ownerKey) && associationKey.ownerKey.EntityName.Equals(this.ownerKey.EntityName);
    }

    public override int GetHashCode() => this.hashCode;
  }
}
