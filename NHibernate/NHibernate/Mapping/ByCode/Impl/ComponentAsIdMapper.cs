// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComponentAsIdMapper
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
  public class ComponentAsIdMapper : 
    IComponentAsIdMapper,
    IComponentAsIdAttributesMapper,
    IAccessorPropertyMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly IAccessorPropertyMapper accessorPropertyMapper;
    private readonly HbmCompositeId id;
    private readonly HbmMapping mapDoc;

    public ComponentAsIdMapper(
      Type componentType,
      MemberInfo declaringTypeMember,
      HbmCompositeId id,
      HbmMapping mapDoc)
    {
      this.id = id;
      this.mapDoc = mapDoc;
      id.@class = componentType.GetShortClassName(mapDoc);
      id.name = declaringTypeMember.Name;
      this.accessorPropertyMapper = (IAccessorPropertyMapper) new AccessorPropertyMapper(declaringTypeMember.DeclaringType, declaringTypeMember.Name, (Action<string>) (x => id.access = x));
    }

    public HbmCompositeId CompositeId => this.id;

    public void Access(Accessor accessor) => this.accessorPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this.accessorPropertyMapper.Access(accessorType);

    public void Class(Type componentType)
    {
      this.id.@class = componentType.GetShortClassName(this.mapDoc);
    }

    public void Property(MemberInfo property, Action<IPropertyMapper> mapping)
    {
      HbmKeyProperty hbmKeyProperty = new HbmKeyProperty()
      {
        name = property.Name
      };
      mapping((IPropertyMapper) new KeyPropertyMapper(property, hbmKeyProperty));
      this.AddProperty((object) hbmKeyProperty);
    }

    public void ManyToOne(MemberInfo property, Action<IManyToOneMapper> mapping)
    {
      HbmKeyManyToOne hbmKeyManyToOne = new HbmKeyManyToOne()
      {
        name = property.Name
      };
      mapping((IManyToOneMapper) new KeyManyToOneMapper(property, hbmKeyManyToOne, this.mapDoc));
      this.AddProperty((object) hbmKeyManyToOne);
    }

    protected void AddProperty(object property)
    {
      object[] second = property != null ? new object[1]
      {
        property
      } : throw new ArgumentNullException(nameof (property));
      this.id.Items = this.id.Items == null ? second : ((IEnumerable<object>) this.id.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }
  }
}
