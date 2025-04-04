// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ComponentPartBase`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public abstract class ComponentPartBase<TEntity, TBuilder> : ClasslikeMapBase<TEntity> where TBuilder : ComponentPartBase<TEntity, TBuilder>
  {
    private readonly Member member;
    private readonly MappingProviderStore providers;
    private readonly AccessStrategyBuilder<TBuilder> access;
    private readonly AttributeStore attributes;
    protected bool nextBool = true;

    protected ComponentPartBase(AttributeStore attributes, Member member)
      : this(attributes, member, new MappingProviderStore())
    {
    }

    protected ComponentPartBase(
      AttributeStore attributes,
      Member member,
      MappingProviderStore providers)
      : base(providers)
    {
      this.attributes = attributes;
      this.access = new AccessStrategyBuilder<TBuilder>((TBuilder) this, (Action<string>) (value => attributes.Set(nameof (Access), 2, (object) value)));
      this.member = member;
      this.providers = providers;
      if (!(member != (Member) null))
        return;
      this.SetDefaultAccess();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public AccessStrategyBuilder<TBuilder> Access => this.access;

    public TBuilder ParentReference(Expression<Func<TEntity, object>> expression)
    {
      return this.ParentReference(expression.ToMember<TEntity, object>());
    }

    private TBuilder ParentReference(Member property)
    {
      ParentMapping parentMapping = new ParentMapping()
      {
        ContainingEntityType = typeof (TEntity)
      };
      parentMapping.Set<string>((Expression<Func<ParentMapping, string>>) (x => x.Name), 0, property.Name);
      this.attributes.Set("Parent", 0, (object) parentMapping);
      return (TBuilder) this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TBuilder Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (TBuilder) this;
      }
    }

    public TBuilder ReadOnly()
    {
      this.attributes.Set("Insert", 2, (object) !this.nextBool);
      this.attributes.Set("Update", 2, (object) !this.nextBool);
      this.nextBool = true;
      return (TBuilder) this;
    }

    public TBuilder Insert()
    {
      this.attributes.Set(nameof (Insert), 2, (object) this.nextBool);
      this.nextBool = true;
      return (TBuilder) this;
    }

    public TBuilder Update()
    {
      this.attributes.Set(nameof (Update), 2, (object) this.nextBool);
      this.nextBool = true;
      return (TBuilder) this;
    }

    public TBuilder Unique()
    {
      this.attributes.Set(nameof (Unique), 2, (object) this.nextBool);
      this.nextBool = true;
      return (TBuilder) this;
    }

    public TBuilder OptimisticLock()
    {
      this.attributes.Set(nameof (OptimisticLock), 2, (object) this.nextBool);
      this.nextBool = true;
      return (TBuilder) this;
    }

    protected abstract ComponentMapping CreateComponentMappingRoot(AttributeStore store);

    protected ComponentMapping CreateComponentMapping()
    {
      ComponentMapping componentMappingRoot = this.CreateComponentMappingRoot(this.attributes.Clone());
      if (this.member != (Member) null)
        componentMappingRoot.Set<string>((Expression<Func<ComponentMapping, string>>) (x => x.Name), 0, this.member.Name);
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        componentMappingRoot.AddProperty(property.GetPropertyMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        componentMappingRoot.AddComponent(component.GetComponentMapping());
      foreach (IOneToOneMappingProvider oneToOne in (IEnumerable<IOneToOneMappingProvider>) this.providers.OneToOnes)
        componentMappingRoot.AddOneToOne(oneToOne.GetOneToOneMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        componentMappingRoot.AddCollection(collection.GetCollectionMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        componentMappingRoot.AddReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        componentMappingRoot.AddAny(any.GetAnyMapping());
      return componentMappingRoot;
    }
  }
}
