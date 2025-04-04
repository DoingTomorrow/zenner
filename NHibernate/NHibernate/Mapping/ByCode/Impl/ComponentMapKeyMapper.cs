// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComponentMapKeyMapper
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
  public class ComponentMapKeyMapper : IComponentMapKeyMapper
  {
    private readonly HbmCompositeMapKey component;
    private readonly HbmMapping mapDoc;

    public ComponentMapKeyMapper(
      Type componentType,
      HbmCompositeMapKey component,
      HbmMapping mapDoc)
    {
      this.component = component;
      this.mapDoc = mapDoc;
      component.@class = componentType.GetShortClassName(mapDoc);
    }

    public HbmCompositeMapKey CompositeMapKeyMapping => this.component;

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
      this.component.Items = this.component.Items == null ? second : ((IEnumerable<object>) this.component.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }
  }
}
