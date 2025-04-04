// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.RootClass
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
  public class RootClass : PersistentClass, ITableOwner
  {
    public const string DefaultIdentifierColumnName = "id";
    public const string DefaultDiscriminatorColumnName = "class";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (RootClass));
    private Property identifierProperty;
    private IKeyValue identifier;
    private Property version;
    private bool polymorphic;
    private string cacheConcurrencyStrategy;
    private string cacheRegionName;
    private bool lazyPropertiesCacheable = true;
    private IValue discriminator;
    private bool mutable;
    private bool embeddedIdentifier;
    private bool explicitPolymorphism;
    private Type entityPersisterClass;
    private bool forceDiscriminator;
    private string where;
    private Table table;
    private bool discriminatorInsertable = true;
    private int nextSubclassId;

    public override int SubclassId => 0;

    public override bool IsInherited => false;

    public override IEnumerable<Property> PropertyClosureIterator => this.PropertyIterator;

    public override IEnumerable<Table> TableClosureIterator
    {
      get => (IEnumerable<Table>) new SingletonEnumerable<Table>(this.Table);
    }

    public override IEnumerable<IKeyValue> KeyClosureIterator
    {
      get => (IEnumerable<IKeyValue>) new SingletonEnumerable<IKeyValue>(this.Key);
    }

    public override bool IsVersioned => this.version != null;

    public override Type EntityPersisterClass
    {
      get => this.entityPersisterClass;
      set => this.entityPersisterClass = value;
    }

    public override Table RootTable => this.Table;

    public override bool HasEmbeddedIdentifier
    {
      get => this.embeddedIdentifier;
      set => this.embeddedIdentifier = value;
    }

    public string CacheRegionName
    {
      get => this.cacheRegionName ?? this.EntityName;
      set => this.cacheRegionName = value;
    }

    public override bool IsJoinedSubclass => false;

    public virtual ISet<Table> IdentityTables
    {
      get
      {
        ISet<Table> identityTables = (ISet<Table>) new HashedSet<Table>();
        foreach (PersistentClass persistentClass in this.SubclassClosureIterator)
        {
          if (!persistentClass.IsAbstract.GetValueOrDefault())
            identityTables.Add(persistentClass.IdentityTable);
        }
        return identityTables;
      }
    }

    internal override int NextSubclassId() => ++this.nextSubclassId;

    public override Table Table => this.table;

    Table ITableOwner.Table
    {
      set => this.table = value;
    }

    public override Property IdentifierProperty
    {
      get => this.identifierProperty;
      set
      {
        this.identifierProperty = value;
        this.identifierProperty.PersistentClass = (PersistentClass) this;
      }
    }

    public override IKeyValue Identifier
    {
      get => this.identifier;
      set => this.identifier = value;
    }

    public override bool HasIdentifierProperty => this.identifierProperty != null;

    public override IValue Discriminator
    {
      get => this.discriminator;
      set => this.discriminator = value;
    }

    public override bool IsPolymorphic
    {
      get => this.polymorphic;
      set => this.polymorphic = value;
    }

    public override RootClass RootClazz => this;

    public override void AddSubclass(Subclass subclass)
    {
      base.AddSubclass(subclass);
      this.polymorphic = true;
    }

    public override bool IsExplicitPolymorphism
    {
      get => this.explicitPolymorphism;
      set => this.explicitPolymorphism = value;
    }

    public override Property Version
    {
      get => this.version;
      set => this.version = value;
    }

    public override bool IsMutable
    {
      get => this.mutable;
      set => this.mutable = value;
    }

    public override PersistentClass Superclass
    {
      get => (PersistentClass) null;
      set => throw new InvalidOperationException("Can not set the Superclass on a RootClass.");
    }

    public override IKeyValue Key
    {
      get => this.Identifier;
      set => throw new InvalidOperationException();
    }

    public override bool IsDiscriminatorInsertable
    {
      get => this.discriminatorInsertable;
      set => this.discriminatorInsertable = value;
    }

    public override bool IsForceDiscriminator
    {
      get => this.forceDiscriminator;
      set => this.forceDiscriminator = value;
    }

    public override string Where
    {
      get => this.where;
      set => this.where = value;
    }

    public override void Validate(IMapping mapping)
    {
      base.Validate(mapping);
      if (!this.Identifier.IsValid(mapping))
        throw new MappingException(string.Format("identifier mapping has wrong number of columns: {0} type: {1}", (object) this.EntityName, (object) this.Identifier.Type.Name));
      this.CheckCompositeIdentifier();
    }

    private void CheckCompositeIdentifier()
    {
      if (!(this.Identifier is Component identifier) || identifier.IsDynamic)
        return;
      Type componentClass = identifier.ComponentClass;
      if (componentClass != null && !ReflectHelper.OverridesEquals(componentClass))
        RootClass.log.Warn((object) ("composite-id class does not override Equals(): " + identifier.ComponentClass.FullName));
      if (ReflectHelper.OverridesGetHashCode(componentClass))
        return;
      RootClass.log.Warn((object) ("composite-id class does not override GetHashCode(): " + identifier.ComponentClass.FullName));
    }

    public override string CacheConcurrencyStrategy
    {
      get => this.cacheConcurrencyStrategy;
      set => this.cacheConcurrencyStrategy = value;
    }

    public override bool IsLazyPropertiesCacheable => this.lazyPropertiesCacheable;

    public void SetLazyPropertiesCacheable(bool isLazyPropertiesCacheable)
    {
      this.lazyPropertiesCacheable = isLazyPropertiesCacheable;
    }

    public override object Accept(IPersistentClassVisitor mv) => mv.Accept((PersistentClass) this);
  }
}
