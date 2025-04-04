// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComponentNestedElementMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ComponentNestedElementMapper : 
    IComponentElementMapper,
    IComponentAttributesMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper
  {
    private readonly IAccessorPropertyMapper _accessorPropertyMapper;
    private readonly HbmNestedCompositeElement _component;
    private readonly Type _componentType;
    private readonly HbmMapping _mapDoc;
    private IComponentParentMapper _parentMapper;

    public ComponentNestedElementMapper(
      Type componentType,
      HbmMapping mapDoc,
      HbmNestedCompositeElement component,
      MemberInfo declaringComponentMember)
    {
      this._componentType = componentType;
      this._mapDoc = mapDoc;
      this._component = component;
      this._accessorPropertyMapper = (IAccessorPropertyMapper) new AccessorPropertyMapper(declaringComponentMember.DeclaringType, declaringComponentMember.Name, (Action<string>) (x => component.access = x));
    }

    public void Parent(MemberInfo parent)
    {
      this.Parent(parent, (Action<IComponentParentMapper>) (x => { }));
    }

    public void Parent(MemberInfo parent, Action<IComponentParentMapper> parentMapping)
    {
      IComponentParentMapper componentParentMapper = parent != null ? this.GetParentMapper(parent) : throw new ArgumentNullException(nameof (parent));
      parentMapping(componentParentMapper);
    }

    public void Update(bool consideredInUpdateQuery)
    {
    }

    public void Insert(bool consideredInInsertQuery)
    {
    }

    public void Lazy(bool isLazy)
    {
    }

    public void Unique(bool unique)
    {
    }

    public void Class(Type componentConcreteType)
    {
      this._component.@class = componentConcreteType.GetShortClassName(this._mapDoc);
    }

    public void Property(MemberInfo property, Action<IPropertyMapper> mapping)
    {
      HbmProperty hbmProperty = new HbmProperty()
      {
        name = property.Name
      };
      mapping((IPropertyMapper) new PropertyMapper(property, hbmProperty));
      this.AddProperty((object) hbmProperty);
    }

    public void Component(MemberInfo property, Action<IComponentElementMapper> mapping)
    {
      Type propertyOrFieldType = property.GetPropertyOrFieldType();
      HbmNestedCompositeElement compositeElement = new HbmNestedCompositeElement()
      {
        name = property.Name,
        @class = propertyOrFieldType.GetShortClassName(this._mapDoc)
      };
      mapping((IComponentElementMapper) new ComponentNestedElementMapper(propertyOrFieldType, this._mapDoc, compositeElement, property));
      this.AddProperty((object) compositeElement);
    }

    public void ManyToOne(MemberInfo property, Action<IManyToOneMapper> mapping)
    {
      HbmManyToOne hbmManyToOne = new HbmManyToOne()
      {
        name = property.Name
      };
      mapping((IManyToOneMapper) new ManyToOneMapper(property, hbmManyToOne, this._mapDoc));
      this.AddProperty((object) hbmManyToOne);
    }

    public void Access(Accessor accessor) => this._accessorPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this._accessorPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
    }

    protected void AddProperty(object property)
    {
      object[] second = property != null ? new object[1]
      {
        property
      } : throw new ArgumentNullException(nameof (property));
      this._component.Items = this._component.Items == null ? second : ((IEnumerable<object>) this._component.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }

    private IComponentParentMapper GetParentMapper(MemberInfo parent)
    {
      if (this._parentMapper != null)
        return this._parentMapper;
      this._component.parent = new HbmParent();
      return this._parentMapper = (IComponentParentMapper) new ComponentParentMapper(this._component.parent, parent);
    }
  }
}
