// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComponentAsIdLikeComponentAttributesMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ComponentAsIdLikeComponentAttributesMapper : 
    IComponentAttributesMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper
  {
    private readonly IComponentAsIdMapper _realMapper;

    public ComponentAsIdLikeComponentAttributesMapper(IComponentAsIdMapper realMapper)
    {
      this._realMapper = realMapper != null ? realMapper : throw new ArgumentNullException(nameof (realMapper));
    }

    public void Access(Accessor accessor) => this._realMapper.Access(accessor);

    public void Access(Type accessorType) => this._realMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
    }

    public void Parent(MemberInfo parent)
    {
    }

    public void Parent(MemberInfo parent, Action<IComponentParentMapper> parentMapping)
    {
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

    public void Class(Type componentType) => this._realMapper.Class(componentType);
  }
}
