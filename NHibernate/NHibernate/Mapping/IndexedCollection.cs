// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.IndexedCollection
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
  public abstract class IndexedCollection(PersistentClass owner) : Collection(owner)
  {
    public const string DefaultIndexColumnName = "idx";
    private SimpleValue index;
    private string indexNodeName;

    public SimpleValue Index
    {
      get => this.index;
      set => this.index = value;
    }

    public override bool IsIndexed => true;

    public virtual bool IsList => false;

    public string IndexNodeName
    {
      get => this.indexNodeName;
      set => this.indexNodeName = value;
    }

    public override void CreatePrimaryKey()
    {
      if (this.IsOneToMany)
        return;
      PrimaryKey primaryKey = new PrimaryKey();
      primaryKey.AddColumns((IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.Key.ColumnIterator));
      bool flag = false;
      foreach (ISelectable selectable in this.Index.ColumnIterator)
      {
        if (selectable.IsFormula)
          flag = true;
      }
      if (flag)
        primaryKey.AddColumns((IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.Element.ColumnIterator));
      else
        primaryKey.AddColumns((IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.Index.ColumnIterator));
      this.CollectionTable.PrimaryKey = primaryKey;
    }

    public override void Validate(IMapping mapping)
    {
      base.Validate(mapping);
      if (!this.Index.IsValid(mapping))
        throw new MappingException(string.Format("collection index mapping has wrong number of columns: {0} type: {1}", (object) this.Role, (object) this.Index.Type.Name));
      if (this.indexNodeName != null && !this.indexNodeName.StartsWith("@"))
        throw new MappingException("index node must be an attribute: " + this.indexNodeName);
    }
  }
}
