// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.JoinedSubclass
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class JoinedSubclass(PersistentClass superclass) : Subclass(superclass), ITableOwner
  {
    private Table table;
    private IKeyValue key;

    public override Table Table => this.table;

    Table ITableOwner.Table
    {
      set
      {
        this.table = value;
        this.Superclass.AddSubclassTable(this.table);
      }
    }

    public override IKeyValue Key
    {
      get => this.key;
      set => this.key = value;
    }

    public override void Validate(IMapping mapping)
    {
      base.Validate(mapping);
      if (this.Key != null && !this.Key.IsValid(mapping))
        throw new MappingException(string.Format("subclass key has wrong number of columns: {0} type: {1}", (object) this.MappedClass.Name, (object) this.Key.Type.Name));
    }

    public override IEnumerable<Property> ReferenceablePropertyIterator => this.PropertyIterator;
  }
}
