// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.UnionSubclass
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class UnionSubclass(PersistentClass superclass) : Subclass(superclass), ITableOwner
  {
    private Table table;

    Table ITableOwner.Table
    {
      set
      {
        this.table = value;
        this.Superclass.AddSubclassTable(this.table);
      }
    }

    public override Table Table => this.table;

    protected internal override IEnumerable<Property> NonDuplicatedPropertyIterator
    {
      get => this.PropertyClosureIterator;
    }

    public override Table IdentityTable => this.Table;
  }
}
