// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComponentMapper
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
  public class ComponentMapper : 
    AbstractPropertyContainerMapper,
    IComponentMapper,
    IComponentAttributesMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IPropertyContainerMapper,
    ICollectionPropertiesContainerMapper,
    IPlainPropertyContainerMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly IAccessorPropertyMapper _accessorPropertyMapper;
    private readonly HbmComponent _component;
    private ComponentParentMapper _parentMapper;

    public ComponentMapper(
      HbmComponent component,
      Type componentType,
      MemberInfo declaringTypeMember,
      HbmMapping mapDoc)
      : this(component, componentType, (IAccessorPropertyMapper) new AccessorPropertyMapper(declaringTypeMember.DeclaringType, declaringTypeMember.Name, (Action<string>) (x => component.access = x)), mapDoc)
    {
    }

    public ComponentMapper(
      HbmComponent component,
      Type componentType,
      IAccessorPropertyMapper accessorMapper,
      HbmMapping mapDoc)
      : base(componentType, mapDoc)
    {
      this._component = component;
      this._component.@class = componentType.GetShortClassName(mapDoc);
      this._accessorPropertyMapper = accessorMapper;
    }

    protected override void AddProperty(object property)
    {
      object[] second = property != null ? new object[1]
      {
        property
      } : throw new ArgumentNullException(nameof (property));
      this._component.Items = this._component.Items == null ? second : ((IEnumerable<object>) this._component.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
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
      this._component.update = consideredInUpdateQuery;
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this._component.insert = consideredInInsertQuery;
    }

    public void Lazy(bool isLazy) => this._component.lazy = isLazy;

    public void Unique(bool unique) => this._component.unique = unique;

    public void Class(Type componentType)
    {
      this._component.@class = componentType.GetShortClassName(this.mapDoc);
    }

    public void Access(Accessor accessor) => this._accessorPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this._accessorPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this._component.optimisticlock = takeInConsiderationForOptimisticLock;
    }

    private IComponentParentMapper GetParentMapper(MemberInfo parent)
    {
      if (this._parentMapper != null)
        return (IComponentParentMapper) this._parentMapper;
      this._component.parent = new HbmParent();
      return (IComponentParentMapper) (this._parentMapper = new ComponentParentMapper(this._component.parent, parent));
    }
  }
}
