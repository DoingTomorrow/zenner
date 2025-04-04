// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.OneToMany
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class OneToMany : IValue
  {
    private string referencedEntityName;
    private readonly Table referencingTable;
    private PersistentClass associatedClass;
    private bool ignoreNotFound;
    private bool embedded;

    public OneToMany(PersistentClass owner)
    {
      this.referencingTable = owner == null ? (Table) null : owner.Table;
    }

    private EntityType EntityType
    {
      get
      {
        return TypeFactory.ManyToOne(this.ReferencedEntityName, (string) null, false, false, this.IsEmbedded, this.IsIgnoreNotFound);
      }
    }

    public bool IsIgnoreNotFound
    {
      get => this.ignoreNotFound;
      set => this.ignoreNotFound = value;
    }

    public IEnumerable<ISelectable> ColumnIterator => this.associatedClass.Key.ColumnIterator;

    public int ColumnSpan => this.associatedClass.Key.ColumnSpan;

    public string ReferencedEntityName
    {
      get => this.referencedEntityName;
      set => this.referencedEntityName = value == null ? (string) null : string.Intern(value);
    }

    public Table ReferencingTable => this.referencingTable;

    public IType Type => (IType) this.EntityType;

    public PersistentClass AssociatedClass
    {
      get => this.associatedClass;
      set => this.associatedClass = value;
    }

    public Formula Formula => (Formula) null;

    public Table Table => this.referencingTable;

    public bool IsNullable => false;

    public bool IsSimpleValue => false;

    public bool HasFormula => false;

    public bool IsUnique => false;

    public bool IsValid(IMapping mapping)
    {
      if (this.referencedEntityName == null)
        throw new MappingException("one to many association must specify the referenced entity");
      return true;
    }

    public FetchMode FetchMode => FetchMode.Join;

    public bool IsAlternateUniqueKey => false;

    public void SetTypeUsingReflection(string className, string propertyName, string accesorName)
    {
    }

    public object Accept(IValueVisitor visitor) => visitor.Accept((IValue) this);

    public void CreateForeignKey()
    {
    }

    public bool[] ColumnInsertability => throw new InvalidOperationException();

    public bool[] ColumnUpdateability => throw new InvalidOperationException();

    public bool IsEmbedded
    {
      get => this.embedded;
      set => this.embedded = value;
    }
  }
}
