// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.OneToOne
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
  public class OneToOne : ToOne
  {
    private bool constrained;
    private ForeignKeyDirection foreignKeyType;
    private IKeyValue identifier;
    private string propertyName;
    private string entityName;

    public OneToOne(Table table, PersistentClass owner)
      : base(table)
    {
      this.identifier = owner.Key;
      this.entityName = owner.EntityName;
    }

    public override void CreateForeignKey()
    {
      if (!this.constrained || this.referencedPropertyName != null)
        return;
      this.CreateForeignKeyOfEntity(((EntityType) this.Type).GetAssociatedEntityName());
    }

    public override IEnumerable<Column> ConstraintColumns
    {
      get
      {
        return (IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.identifier.ColumnIterator);
      }
    }

    public bool IsConstrained
    {
      get => this.constrained;
      set => this.constrained = value;
    }

    public ForeignKeyDirection ForeignKeyType
    {
      get => this.foreignKeyType;
      set => this.foreignKeyType = value;
    }

    public IKeyValue Identifier
    {
      get => this.identifier;
      set => this.identifier = value;
    }

    public override bool IsNullable => !this.constrained;

    public string EntityName
    {
      get => this.entityName;
      set => this.entityName = StringHelper.InternedIfPossible(value);
    }

    public string PropertyName
    {
      get => this.propertyName;
      set => this.propertyName = StringHelper.InternedIfPossible(value);
    }

    public override IType Type
    {
      get
      {
        return this.ColumnSpan > 0 ? (IType) new SpecialOneToOneType(this.ReferencedEntityName, this.foreignKeyType, this.referencedPropertyName, this.IsLazy, this.UnwrapProxy, this.entityName, this.propertyName) : (IType) TypeFactory.OneToOne(this.ReferencedEntityName, this.foreignKeyType, this.referencedPropertyName, this.IsLazy, this.UnwrapProxy, this.Embedded, this.entityName, this.propertyName);
      }
    }
  }
}
