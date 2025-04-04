// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ClasslikeMapBase`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public abstract class ClasslikeMapBase<T>
  {
    private readonly MappingProviderStore providers;

    protected ClasslikeMapBase(MappingProviderStore providers) => this.providers = providers;

    internal virtual void OnMemberMapped(Member member)
    {
    }

    public PropertyPart Map(Expression<Func<T, object>> memberExpression)
    {
      return this.Map(memberExpression, (string) null);
    }

    public PropertyPart Map(Expression<Func<T, object>> memberExpression, string columnName)
    {
      return this.Map(memberExpression.ToMember<T, object>(), columnName);
    }

    private PropertyPart Map(Member member, string columnName)
    {
      this.OnMemberMapped(member);
      PropertyPart propertyPart = new PropertyPart(member, typeof (T));
      if (!string.IsNullOrEmpty(columnName))
        propertyPart.Column(columnName);
      this.providers.Properties.Add((IPropertyMappingProvider) propertyPart);
      return propertyPart;
    }

    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> memberExpression)
    {
      return this.References<TOther>(memberExpression, (string) null);
    }

    public ManyToOnePart<TOther> References<TOther>(
      Expression<Func<T, TOther>> memberExpression,
      string columnName)
    {
      return this.References<TOther>(memberExpression.ToMember<T, TOther>(), columnName);
    }

    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, object>> memberExpression)
    {
      return this.References<TOther>(memberExpression, (string) null);
    }

    public ManyToOnePart<TOther> References<TOther>(
      Expression<Func<T, object>> memberExpression,
      string columnName)
    {
      return this.References<TOther>(memberExpression.ToMember<T, object>(), columnName);
    }

    private ManyToOnePart<TOther> References<TOther>(Member member, string columnName)
    {
      this.OnMemberMapped(member);
      ManyToOnePart<TOther> manyToOnePart = new ManyToOnePart<TOther>(this.EntityType, member);
      if (columnName != null)
        manyToOnePart.Column(columnName);
      this.providers.References.Add((IManyToOneMappingProvider) manyToOnePart);
      return manyToOnePart;
    }

    public AnyPart<TOther> ReferencesAny<TOther>(Expression<Func<T, TOther>> memberExpression)
    {
      return this.ReferencesAny<TOther>(memberExpression.ToMember<T, TOther>());
    }

    private AnyPart<TOther> ReferencesAny<TOther>(Member member)
    {
      this.OnMemberMapped(member);
      AnyPart<TOther> anyPart = new AnyPart<TOther>(typeof (T), member);
      this.providers.Anys.Add((IAnyMappingProvider) anyPart);
      return anyPart;
    }

    public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, object>> memberExpression)
    {
      return this.HasOne<TOther>(memberExpression.ToMember<T, object>());
    }

    public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, TOther>> memberExpression)
    {
      return this.HasOne<TOther>(memberExpression.ToMember<T, TOther>());
    }

    private OneToOnePart<TOther> HasOne<TOther>(Member member)
    {
      this.OnMemberMapped(member);
      OneToOnePart<TOther> oneToOnePart = new OneToOnePart<TOther>(this.EntityType, member);
      this.providers.OneToOnes.Add((IOneToOneMappingProvider) oneToOnePart);
      return oneToOnePart;
    }

    public DynamicComponentPart<IDictionary> DynamicComponent(
      Expression<Func<T, IDictionary>> memberExpression,
      Action<DynamicComponentPart<IDictionary>> dynamicComponentAction)
    {
      return this.DynamicComponent(memberExpression.ToMember<T, IDictionary>(), dynamicComponentAction);
    }

    private DynamicComponentPart<IDictionary> DynamicComponent(
      Member member,
      Action<DynamicComponentPart<IDictionary>> dynamicComponentAction)
    {
      this.OnMemberMapped(member);
      DynamicComponentPart<IDictionary> dynamicComponentPart = new DynamicComponentPart<IDictionary>(typeof (T), member);
      dynamicComponentAction(dynamicComponentPart);
      this.providers.Components.Add((IComponentMappingProvider) dynamicComponentPart);
      return dynamicComponentPart;
    }

    public virtual ReferenceComponentPart<TComponent> Component<TComponent>(
      Expression<Func<T, TComponent>> member)
    {
      ReferenceComponentPart<TComponent> referenceComponentPart = new ReferenceComponentPart<TComponent>(member.ToMember<T, TComponent>(), typeof (T));
      this.providers.Components.Add((IComponentMappingProvider) referenceComponentPart);
      return referenceComponentPart;
    }

    public ComponentPart<TComponent> Component<TComponent>(
      Expression<Func<T, TComponent>> expression,
      Action<ComponentPart<TComponent>> action)
    {
      return this.Component<TComponent>(expression.ToMember<T, TComponent>(), action);
    }

    public ComponentPart<TComponent> Component<TComponent>(
      Expression<Func<T, object>> expression,
      Action<ComponentPart<TComponent>> action)
    {
      return this.Component<TComponent>(expression.ToMember<T, object>(), action);
    }

    private ComponentPart<TComponent> Component<TComponent>(
      Member member,
      Action<ComponentPart<TComponent>> action)
    {
      this.OnMemberMapped(member);
      ComponentPart<TComponent> componentPart = new ComponentPart<TComponent>(typeof (T), member);
      action(componentPart);
      this.providers.Components.Add((IComponentMappingProvider) componentPart);
      return componentPart;
    }

    public void Component(IComponentMappingProvider componentProvider)
    {
      this.providers.Components.Add(componentProvider);
    }

    private OneToManyPart<TChild> MapHasMany<TChild, TReturn>(
      Expression<Func<T, TReturn>> expression)
    {
      return this.HasMany<TChild>(expression.ToMember<T, TReturn>());
    }

    private OneToManyPart<TChild> HasMany<TChild>(Member member)
    {
      this.OnMemberMapped(member);
      OneToManyPart<TChild> oneToManyPart = new OneToManyPart<TChild>(this.EntityType, member);
      this.providers.Collections.Add((ICollectionMappingProvider) oneToManyPart);
      return oneToManyPart;
    }

    public OneToManyPart<TChild> HasMany<TChild>(
      Expression<Func<T, IEnumerable<TChild>>> memberExpression)
    {
      return this.MapHasMany<TChild, IEnumerable<TChild>>(memberExpression);
    }

    public OneToManyPart<TChild> HasMany<TKey, TChild>(
      Expression<Func<T, IDictionary<TKey, TChild>>> memberExpression)
    {
      return this.MapHasMany<TChild, IDictionary<TKey, TChild>>(memberExpression);
    }

    public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, object>> memberExpression)
    {
      return this.MapHasMany<TChild, object>(memberExpression);
    }

    private ManyToManyPart<TChild> MapHasManyToMany<TChild, TReturn>(
      Expression<Func<T, TReturn>> expression)
    {
      return this.HasManyToMany<TChild>(expression.ToMember<T, TReturn>());
    }

    private ManyToManyPart<TChild> HasManyToMany<TChild>(Member member)
    {
      this.OnMemberMapped(member);
      ManyToManyPart<TChild> many = new ManyToManyPart<TChild>(this.EntityType, member);
      this.providers.Collections.Add((ICollectionMappingProvider) many);
      return many;
    }

    public ManyToManyPart<TChild> HasManyToMany<TChild>(
      Expression<Func<T, IEnumerable<TChild>>> memberExpression)
    {
      return this.MapHasManyToMany<TChild, IEnumerable<TChild>>(memberExpression);
    }

    public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, object>> memberExpression)
    {
      return this.MapHasManyToMany<TChild, object>(memberExpression);
    }

    public StoredProcedurePart SqlInsert(string innerText)
    {
      return this.StoredProcedure("sql-insert", innerText);
    }

    public StoredProcedurePart SqlUpdate(string innerText)
    {
      return this.StoredProcedure("sql-update", innerText);
    }

    public StoredProcedurePart SqlDelete(string innerText)
    {
      return this.StoredProcedure("sql-delete", innerText);
    }

    public StoredProcedurePart SqlDeleteAll(string innerText)
    {
      return this.StoredProcedure("sql-delete-all", innerText);
    }

    protected StoredProcedurePart StoredProcedure(string element, string innerText)
    {
      StoredProcedurePart storedProcedurePart = new StoredProcedurePart(element, innerText);
      this.providers.StoredProcedures.Add((IStoredProcedureMappingProvider) storedProcedurePart);
      return storedProcedurePart;
    }

    internal IEnumerable<IPropertyMappingProvider> Properties
    {
      get => (IEnumerable<IPropertyMappingProvider>) this.providers.Properties;
    }

    internal IEnumerable<IComponentMappingProvider> Components
    {
      get => (IEnumerable<IComponentMappingProvider>) this.providers.Components;
    }

    internal Type EntityType => typeof (T);
  }
}
