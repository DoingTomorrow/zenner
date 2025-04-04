// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Subclass
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Subclass : PersistentClass
  {
    private PersistentClass superclass;
    private Type classPersisterClass;
    private readonly int subclassId;

    public Subclass(PersistentClass superclass)
    {
      this.superclass = superclass;
      this.subclassId = superclass.NextSubclassId();
    }

    public override int SubclassId => this.subclassId;

    public override bool IsInherited => true;

    public override IEnumerable<Property> PropertyClosureIterator
    {
      get
      {
        return (IEnumerable<Property>) new JoinedEnumerable<Property>(this.Superclass.PropertyClosureIterator, this.PropertyIterator);
      }
    }

    public override IEnumerable<Table> TableClosureIterator
    {
      get
      {
        return (IEnumerable<Table>) new JoinedEnumerable<Table>(this.Superclass.TableClosureIterator, (IEnumerable<Table>) new SingletonEnumerable<Table>(this.Table));
      }
    }

    public override IEnumerable<IKeyValue> KeyClosureIterator
    {
      get
      {
        return (IEnumerable<IKeyValue>) new JoinedEnumerable<IKeyValue>(this.Superclass.KeyClosureIterator, (IEnumerable<IKeyValue>) new SingletonEnumerable<IKeyValue>(this.Key));
      }
    }

    public override bool IsVersioned => this.Superclass.IsVersioned;

    public override Type EntityPersisterClass
    {
      get
      {
        return this.classPersisterClass == null ? this.Superclass.EntityPersisterClass : this.classPersisterClass;
      }
      set => this.classPersisterClass = value;
    }

    public override Table RootTable => this.Superclass.RootTable;

    public override bool IsJoinedSubclass => this.Table != this.RootTable;

    public override int JoinClosureSpan => this.Superclass.JoinClosureSpan + base.JoinClosureSpan;

    public override int PropertyClosureSpan
    {
      get => this.Superclass.PropertyClosureSpan + base.PropertyClosureSpan;
    }

    public override IEnumerable<Join> JoinClosureIterator
    {
      get
      {
        return (IEnumerable<Join>) new JoinedEnumerable<Join>(this.Superclass.JoinClosureIterator, base.JoinClosureIterator);
      }
    }

    public override ISet<string> SynchronizedTables
    {
      get
      {
        HashedSet<string> synchronizedTables = new HashedSet<string>();
        synchronizedTables.AddAll((ICollection<string>) base.SynchronizedTables);
        synchronizedTables.AddAll((ICollection<string>) this.Superclass.SynchronizedTables);
        return (ISet<string>) synchronizedTables;
      }
    }

    public override IDictionary<string, string> FilterMap => this.Superclass.FilterMap;

    public override IDictionary<EntityMode, string> TuplizerMap
    {
      get
      {
        IDictionary<EntityMode, string> tuplizerMap1 = base.TuplizerMap;
        IDictionary<EntityMode, string> tuplizerMap2 = this.Superclass.TuplizerMap;
        if (tuplizerMap1 == null && tuplizerMap2 == null)
          return (IDictionary<EntityMode, string>) null;
        IDictionary<EntityMode, string> dictionary = (IDictionary<EntityMode, string>) new Dictionary<EntityMode, string>();
        if (tuplizerMap2 != null)
          ArrayHelper.AddAll<EntityMode, string>(dictionary, tuplizerMap2);
        if (tuplizerMap1 != null)
          ArrayHelper.AddAll<EntityMode, string>(dictionary, tuplizerMap1);
        return (IDictionary<EntityMode, string>) new UnmodifiableDictionary<EntityMode, string>(dictionary);
      }
    }

    internal override int NextSubclassId() => this.Superclass.NextSubclassId();

    public override string CacheConcurrencyStrategy
    {
      get => this.Superclass.CacheConcurrencyStrategy;
      set
      {
      }
    }

    public override RootClass RootClazz => this.Superclass.RootClazz;

    public override PersistentClass Superclass
    {
      get => this.superclass;
      set => this.superclass = value;
    }

    public override Property IdentifierProperty
    {
      get => this.Superclass.IdentifierProperty;
      set
      {
      }
    }

    public override IKeyValue Identifier
    {
      get => this.Superclass.Identifier;
      set
      {
      }
    }

    public override bool HasIdentifierProperty => this.Superclass.HasIdentifierProperty;

    public override IValue Discriminator
    {
      get => this.Superclass.Discriminator;
      set
      {
      }
    }

    public override bool IsMutable
    {
      get => this.Superclass.IsMutable;
      set
      {
      }
    }

    public override bool IsPolymorphic
    {
      get => true;
      set
      {
      }
    }

    public override void AddProperty(Property p)
    {
      base.AddProperty(p);
      this.Superclass.AddSubclassProperty(p);
    }

    public override void AddJoin(Join join)
    {
      base.AddJoin(join);
      this.Superclass.AddSubclassJoin(join);
    }

    public override void AddSubclassProperty(Property p)
    {
      base.AddSubclassProperty(p);
      this.Superclass.AddSubclassProperty(p);
    }

    public override void AddSubclassJoin(Join join)
    {
      base.AddSubclassJoin(join);
      this.Superclass.AddSubclassJoin(join);
    }

    public override void AddSubclassTable(Table table)
    {
      base.AddSubclassTable(table);
      this.Superclass.AddSubclassTable(table);
    }

    public override Property Version
    {
      get => this.Superclass.Version;
      set
      {
      }
    }

    public override bool HasEmbeddedIdentifier
    {
      get => this.Superclass.HasEmbeddedIdentifier;
      set
      {
      }
    }

    public override IKeyValue Key
    {
      get => this.Superclass.Identifier;
      set
      {
      }
    }

    public override bool IsExplicitPolymorphism
    {
      get => this.Superclass.IsExplicitPolymorphism;
      set
      {
      }
    }

    public override string Where
    {
      get => this.Superclass.Where;
      set
      {
        throw new InvalidOperationException("The Where string can not be set on the Subclass - use the RootClass instead.");
      }
    }

    public void CreateForeignKey()
    {
      if (!this.IsJoinedSubclass)
        throw new AssertionFailure("Not a joined-subclass");
      this.Key.CreateForeignKeyOfEntity(this.Superclass.EntityName);
    }

    public override bool IsLazyPropertiesCacheable => this.Superclass.IsLazyPropertiesCacheable;

    public override bool IsClassOrSuperclassJoin(Join join)
    {
      return base.IsClassOrSuperclassJoin(join) || this.Superclass.IsClassOrSuperclassJoin(join);
    }

    public override bool IsClassOrSuperclassTable(Table closureTable)
    {
      return base.IsClassOrSuperclassTable(closureTable) || this.Superclass.IsClassOrSuperclassTable(closureTable);
    }

    public override Table Table => this.Superclass.Table;

    public override bool IsForceDiscriminator => this.Superclass.IsForceDiscriminator;

    public override bool IsDiscriminatorInsertable
    {
      get => this.Superclass.IsDiscriminatorInsertable;
      set
      {
        throw new InvalidOperationException("The DiscriminatorInsertable property can not be set on the Subclass - use the Superclass instead.");
      }
    }

    public override object Accept(IPersistentClassVisitor mv) => mv.Accept((PersistentClass) this);

    public override bool HasSubselectLoadableCollections
    {
      get
      {
        return base.HasSubselectLoadableCollections || this.Superclass.HasSubselectLoadableCollections;
      }
      set => base.HasSubselectLoadableCollections = value;
    }

    public override string GetTuplizerImplClassName(EntityMode mode)
    {
      return base.GetTuplizerImplClassName(mode) ?? this.Superclass.GetTuplizerImplClassName(mode);
    }

    public override Component IdentifierMapper => this.superclass.IdentifierMapper;

    public override Versioning.OptimisticLock OptimisticLockMode
    {
      get => this.superclass.OptimisticLockMode;
    }
  }
}
