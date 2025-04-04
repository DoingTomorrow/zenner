// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComposedIdMapper
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
  public class ComposedIdMapper : IComposedIdMapper, IMinimalPlainPropertyContainerMapper
  {
    private readonly Type container;
    private readonly HbmCompositeId id;
    private readonly HbmMapping mapDoc;

    public ComposedIdMapper(Type container, HbmCompositeId id, HbmMapping mapDoc)
    {
      this.container = container;
      this.id = id;
      this.mapDoc = mapDoc;
    }

    public HbmCompositeId ComposedId => this.id;

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
