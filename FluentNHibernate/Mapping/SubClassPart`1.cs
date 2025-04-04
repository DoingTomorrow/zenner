// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.SubClassPart`1
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
  [Obsolete("REMOVE ME")]
  public class SubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclassMappingProvider
  {
    private readonly DiscriminatorPart parent;
    private readonly object discriminatorValue;
    private readonly MappingProviderStore providers;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly List<SubclassMapping> subclassMappings = new List<SubclassMapping>();
    private bool nextBool = true;

    public SubClassPart(DiscriminatorPart parent, object discriminatorValue)
      : this(parent, discriminatorValue, new MappingProviderStore())
    {
    }

    protected SubClassPart(
      DiscriminatorPart parent,
      object discriminatorValue,
      MappingProviderStore providers)
      : base(providers)
    {
      this.parent = parent;
      this.discriminatorValue = discriminatorValue;
      this.providers = providers;
    }

    SubclassMapping ISubclassMappingProvider.GetSubclassMapping()
    {
      SubclassMapping subclassMapping = new SubclassMapping(SubclassType.Subclass, this.attributes.Clone());
      if (this.discriminatorValue != null)
        subclassMapping.Set<object>((Expression<Func<SubclassMapping, object>>) (x => x.DiscriminatorValue), 0, this.discriminatorValue);
      subclassMapping.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Type), 0, typeof (TSubclass));
      subclassMapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Name), 0, typeof (TSubclass).AssemblyQualifiedName);
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        subclassMapping.AddProperty(property.GetPropertyMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        subclassMapping.AddComponent(component.GetComponentMapping());
      foreach (IOneToOneMappingProvider oneToOne in (IEnumerable<IOneToOneMappingProvider>) this.providers.OneToOnes)
        subclassMapping.AddOneToOne(oneToOne.GetOneToOneMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        subclassMapping.AddCollection(collection.GetCollectionMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        subclassMapping.AddReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        subclassMapping.AddAny(any.GetAnyMapping());
      this.subclassMappings.Each<SubclassMapping>(new Action<SubclassMapping>(((ClassMappingBase) subclassMapping).AddSubclass));
      return subclassMapping;
    }

    public DiscriminatorPart SubClass<TChild>(object value, Action<SubClassPart<TChild>> action)
    {
      SubClassPart<TChild> subClassPart = new SubClassPart<TChild>(this.parent, value);
      action(subClassPart);
      this.subclassMappings.Add(((ISubclassMappingProvider) subClassPart).GetSubclassMapping());
      return this.parent;
    }

    public DiscriminatorPart SubClass<TChild>(Action<SubClassPart<TChild>> action)
    {
      return this.SubClass<TChild>((object) null, action);
    }

    public SubClassPart<TSubclass> LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public SubClassPart<TSubclass> Proxy(Type type)
    {
      this.attributes.Set(nameof (Proxy), 2, (object) type.AssemblyQualifiedName);
      return this;
    }

    public SubClassPart<TSubclass> Proxy<T>() => this.Proxy(typeof (T));

    public SubClassPart<TSubclass> DynamicUpdate()
    {
      this.attributes.Set(nameof (DynamicUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public SubClassPart<TSubclass> DynamicInsert()
    {
      this.attributes.Set(nameof (DynamicInsert), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public SubClassPart<TSubclass> SelectBeforeUpdate()
    {
      this.attributes.Set(nameof (SelectBeforeUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public SubClassPart<TSubclass> Abstract()
    {
      this.attributes.Set(nameof (Abstract), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public void EntityName(string entityName)
    {
      this.attributes.Set(nameof (EntityName), 2, (object) entityName);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public SubClassPart<TSubclass> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }
  }
}
