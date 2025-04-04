// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.ClassMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public class ClassMapping : ClassMappingBase
  {
    private readonly AttributeStore attributes;

    public ClassMapping()
      : this(new AttributeStore())
    {
    }

    public ClassMapping(AttributeStore attributes)
      : base(attributes)
    {
      this.attributes = attributes;
    }

    public IIdentityMapping Id => this.attributes.GetOrDefault<IIdentityMapping>(nameof (Id));

    public NaturalIdMapping NaturalId
    {
      get => this.attributes.GetOrDefault<NaturalIdMapping>(nameof (NaturalId));
    }

    public override string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public override Type Type => this.attributes.GetOrDefault<Type>(nameof (Type));

    public CacheMapping Cache => this.attributes.GetOrDefault<CacheMapping>(nameof (Cache));

    public VersionMapping Version => this.attributes.GetOrDefault<VersionMapping>(nameof (Version));

    public DiscriminatorMapping Discriminator
    {
      get => this.attributes.GetOrDefault<DiscriminatorMapping>(nameof (Discriminator));
    }

    public bool IsUnionSubclass => this.attributes.GetOrDefault<bool>(nameof (IsUnionSubclass));

    public TuplizerMapping Tuplizer
    {
      get => this.attributes.GetOrDefault<TuplizerMapping>(nameof (Tuplizer));
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessClass(this);
      if (this.Id != null)
        visitor.Visit(this.Id);
      if (this.NaturalId != null)
        visitor.Visit(this.NaturalId);
      if (this.Discriminator != null)
        visitor.Visit(this.Discriminator);
      if (this.Cache != null)
        visitor.Visit(this.Cache);
      if (this.Version != null)
        visitor.Visit(this.Version);
      if (this.Tuplizer != null)
        visitor.Visit(this.Tuplizer);
      base.AcceptVisitor(visitor);
    }

    public string TableName => this.attributes.GetOrDefault<string>(nameof (TableName));

    public int BatchSize => this.attributes.GetOrDefault<int>(nameof (BatchSize));

    public object DiscriminatorValue
    {
      get => this.attributes.GetOrDefault<object>(nameof (DiscriminatorValue));
    }

    public string Schema => this.attributes.GetOrDefault<string>(nameof (Schema));

    public bool Lazy => this.attributes.GetOrDefault<bool>(nameof (Lazy));

    public bool Mutable => this.attributes.GetOrDefault<bool>(nameof (Mutable));

    public bool DynamicUpdate => this.attributes.GetOrDefault<bool>(nameof (DynamicUpdate));

    public bool DynamicInsert => this.attributes.GetOrDefault<bool>(nameof (DynamicInsert));

    public string OptimisticLock => this.attributes.GetOrDefault<string>(nameof (OptimisticLock));

    public string Polymorphism => this.attributes.GetOrDefault<string>(nameof (Polymorphism));

    public string Persister => this.attributes.GetOrDefault<string>(nameof (Persister));

    public string Where => this.attributes.GetOrDefault<string>(nameof (Where));

    public string Check => this.attributes.GetOrDefault<string>(nameof (Check));

    public string Proxy => this.attributes.GetOrDefault<string>(nameof (Proxy));

    public bool SelectBeforeUpdate
    {
      get => this.attributes.GetOrDefault<bool>(nameof (SelectBeforeUpdate));
    }

    public bool Abstract => this.attributes.GetOrDefault<bool>(nameof (Abstract));

    public string Subselect => this.attributes.GetOrDefault<string>(nameof (Subselect));

    public string SchemaAction => this.attributes.GetOrDefault<string>(nameof (SchemaAction));

    public string EntityName => this.attributes.GetOrDefault<string>(nameof (EntityName));

    public bool Equals(ClassMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Equals((ClassMappingBase) other) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (ClassMapping) && this.Equals((ClassMapping) obj);
    }

    public override int GetHashCode()
    {
      return this.attributes == null ? 0 : this.attributes.GetHashCode();
    }

    public void Set<T>(Expression<Func<ClassMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<ClassMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
