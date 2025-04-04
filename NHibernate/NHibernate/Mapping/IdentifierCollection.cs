// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.IdentifierCollection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public abstract class IdentifierCollection(PersistentClass owner) : Collection(owner)
  {
    public const string DefaultIdentifierColumnName = "id";
    private IKeyValue identifier;

    public IKeyValue Identifier
    {
      get => this.identifier;
      set => this.identifier = value;
    }

    public override bool IsIdentified => true;

    public override void CreatePrimaryKey()
    {
      if (this.IsOneToMany)
        return;
      PrimaryKey primaryKey = new PrimaryKey();
      primaryKey.AddColumns((IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.Identifier.ColumnIterator));
      this.CollectionTable.PrimaryKey = primaryKey;
    }

    public override void Validate(IMapping mapping)
    {
      base.Validate(mapping);
      if (!this.Identifier.IsValid(mapping))
        throw new MappingException(string.Format("collection id mapping has wrong number of columns: {0} type: {1}", (object) this.Role, (object) this.Identifier.Type.Name));
    }
  }
}
