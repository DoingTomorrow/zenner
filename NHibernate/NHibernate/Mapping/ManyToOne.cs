// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ManyToOne
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class ManyToOne(Table table) : ToOne(table)
  {
    private bool isIgnoreNotFound;
    private IType type;

    public override void CreateForeignKey()
    {
      if (this.ReferencedPropertyName != null || this.HasFormula || this.IsIgnoreNotFound)
        return;
      this.CreateForeignKeyOfEntity(((EntityType) this.Type).GetAssociatedEntityName());
    }

    public bool IsIgnoreNotFound
    {
      get => this.isIgnoreNotFound;
      set => this.isIgnoreNotFound = value;
    }

    public override IType Type
    {
      get
      {
        if (this.type == null)
          this.type = (IType) TypeFactory.ManyToOne(this.ReferencedEntityName, this.ReferencedPropertyName, this.IsLazy, this.UnwrapProxy, this.Embedded, this.IsIgnoreNotFound);
        return this.type;
      }
    }

    public void CreatePropertyRefConstraints(
      IDictionary<string, PersistentClass> persistentClasses)
    {
      if (string.IsNullOrEmpty(this.referencedPropertyName))
        return;
      if (string.IsNullOrEmpty(this.ReferencedEntityName))
        throw new MappingException(string.Format("ReferencedEntityName not specified for property '{0}' on entity {1}", (object) this.ReferencedPropertyName, (object) this));
      PersistentClass persistentClass;
      persistentClasses.TryGetValue(this.ReferencedEntityName, out persistentClass);
      Property property = persistentClass != null ? persistentClass.GetReferencedProperty(this.ReferencedPropertyName) : throw new MappingException(string.Format("Could not find referenced entity '{0}' on {1}", (object) this.ReferencedEntityName, (object) this));
      if (property == null)
        throw new MappingException("Could not find property " + this.ReferencedPropertyName + " on " + this.ReferencedEntityName);
      if (this.HasFormula || "none".Equals(this.ForeignKeyName, StringComparison.InvariantCultureIgnoreCase))
        return;
      IEnumerable<Column> referencedColumns = (IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) property.ColumnIterator);
      IEnumerator<Column> enumerator1 = this.ConstraintColumns.GetEnumerator();
      IEnumerator<Column> enumerator2 = referencedColumns.GetEnumerator();
      while (enumerator1.MoveNext() && enumerator2.MoveNext())
        enumerator1.Current.Length = enumerator2.Current.Length;
      this.Table.CreateForeignKey(this.ForeignKeyName, this.ConstraintColumns, ((EntityType) this.Type).GetAssociatedEntityName(), referencedColumns).CascadeDeleteEnabled = this.IsCascadeDeleteEnabled;
    }
  }
}
