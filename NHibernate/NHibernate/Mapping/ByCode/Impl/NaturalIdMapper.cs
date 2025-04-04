// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.NaturalIdMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class NaturalIdMapper : 
    AbstractBasePropertyContainerMapper,
    INaturalIdMapper,
    INaturalIdAttributesMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly HbmClass classMapping;
    private readonly HbmNaturalId naturalIdmapping;

    public NaturalIdMapper(Type rootClass, HbmClass classMapping, HbmMapping mapDoc)
      : base(rootClass, mapDoc)
    {
      this.classMapping = classMapping != null ? classMapping : throw new ArgumentNullException(nameof (classMapping));
      this.naturalIdmapping = new HbmNaturalId();
    }

    protected override void AddProperty(object property)
    {
      if (property == null)
        throw new ArgumentNullException(nameof (property));
      if (this.classMapping.naturalid == null)
        this.classMapping.naturalid = this.naturalIdmapping;
      object[] second = new object[1]{ property };
      this.naturalIdmapping.Items = this.naturalIdmapping.Items == null ? second : ((IEnumerable<object>) this.naturalIdmapping.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }

    public void Mutable(bool isMutable) => this.naturalIdmapping.mutable = isMutable;
  }
}
