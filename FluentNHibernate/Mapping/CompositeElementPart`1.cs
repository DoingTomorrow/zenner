// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.CompositeElementPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class CompositeElementPart<T> : 
    ICompositeElementMappingProvider,
    INestedCompositeElementMappingProvider
  {
    private readonly Type entity;
    private readonly Member member;
    private readonly List<IPropertyMappingProvider> properties = new List<IPropertyMappingProvider>();
    private readonly List<IManyToOneMappingProvider> references = new List<IManyToOneMappingProvider>();
    private readonly List<INestedCompositeElementMappingProvider> components = new List<INestedCompositeElementMappingProvider>();
    private readonly AttributeStore attributes = new AttributeStore();

    public CompositeElementPart(Type entity) => this.entity = entity;

    public CompositeElementPart(Type entity, Member member)
      : this(entity)
    {
      this.member = member;
    }

    public PropertyPart Map(Expression<Func<T, object>> expression)
    {
      return this.Map(expression, (string) null);
    }

    public PropertyPart Map(Expression<Func<T, object>> expression, string columnName)
    {
      return this.Map(expression.ToMember<T, object>(), columnName);
    }

    protected virtual PropertyPart Map(Member property, string columnName)
    {
      PropertyPart propertyPart = new PropertyPart(property, typeof (T));
      if (!string.IsNullOrEmpty(columnName))
        propertyPart.Column(columnName);
      this.properties.Add((IPropertyMappingProvider) propertyPart);
      return propertyPart;
    }

    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression)
    {
      return this.References<TOther>(expression, (string) null);
    }

    public ManyToOnePart<TOther> References<TOther>(
      Expression<Func<T, TOther>> expression,
      string columnName)
    {
      return this.References<TOther>(expression.ToMember<T, TOther>(), columnName);
    }

    protected virtual ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
    {
      ManyToOnePart<TOther> manyToOnePart = new ManyToOnePart<TOther>(typeof (T), property);
      if (columnName != null)
        manyToOnePart.Column(columnName);
      this.references.Add((IManyToOneMappingProvider) manyToOnePart);
      return manyToOnePart;
    }

    public void ParentReference(Expression<Func<T, object>> expression)
    {
      ParentMapping parentMapping = new ParentMapping()
      {
        ContainingEntityType = this.entity
      };
      parentMapping.Set<string>((Expression<Func<ParentMapping, string>>) (x => x.Name), 0, expression.ToMember<T, object>().Name);
      this.attributes.Set("Parent", 0, (object) parentMapping);
    }

    public void Component<TChild>(
      Expression<Func<T, TChild>> property,
      Action<CompositeElementPart<TChild>> nestedCompositeElementAction)
    {
      CompositeElementPart<TChild> compositeElementPart = new CompositeElementPart<TChild>(this.entity, property.ToMember<T, TChild>());
      nestedCompositeElementAction(compositeElementPart);
      this.components.Add((INestedCompositeElementMappingProvider) compositeElementPart);
    }

    private void PopulateMapping(CompositeElementMapping mapping)
    {
      mapping.ContainingEntityType = this.entity;
      mapping.Set<TypeReference>((Expression<Func<CompositeElementMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(typeof (T)));
      foreach (IPropertyMappingProvider property in this.properties)
        mapping.AddProperty(property.GetPropertyMapping());
      foreach (IManyToOneMappingProvider reference in this.references)
        mapping.AddReference(reference.GetManyToOneMapping());
      foreach (INestedCompositeElementMappingProvider component in this.components)
        mapping.AddCompositeElement(component.GetCompositeElementMapping());
    }

    CompositeElementMapping ICompositeElementMappingProvider.GetCompositeElementMapping()
    {
      CompositeElementMapping mapping = new CompositeElementMapping(this.attributes.Clone());
      this.PopulateMapping(mapping);
      return mapping;
    }

    NestedCompositeElementMapping INestedCompositeElementMappingProvider.GetCompositeElementMapping()
    {
      NestedCompositeElementMapping mapping = new NestedCompositeElementMapping(this.attributes.Clone());
      mapping.Set<string>((Expression<Func<NestedCompositeElementMapping, string>>) (x => x.Name), 0, this.member.Name);
      this.PopulateMapping((CompositeElementMapping) mapping);
      return mapping;
    }
  }
}
