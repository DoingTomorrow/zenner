// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.OneToManyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class OneToManyMapper : IOneToManyMapper
  {
    private readonly Type collectionElementType;
    private readonly HbmMapping mapDoc;
    private readonly HbmOneToMany oneToManyMapping;

    public OneToManyMapper(
      Type collectionElementType,
      HbmOneToMany oneToManyMapping,
      HbmMapping mapDoc)
    {
      if (oneToManyMapping == null)
        throw new ArgumentNullException(nameof (oneToManyMapping));
      this.collectionElementType = collectionElementType;
      if (collectionElementType != null)
        oneToManyMapping.@class = collectionElementType.GetShortClassName(mapDoc);
      this.oneToManyMapping = oneToManyMapping;
      this.mapDoc = mapDoc;
    }

    public void Class(Type entityType)
    {
      this.oneToManyMapping.@class = this.collectionElementType.IsAssignableFrom(entityType) ? entityType.GetShortClassName(this.mapDoc) : throw new ArgumentOutOfRangeException(nameof (entityType), string.Format("The type is incompatible; expected assignable to {0}", (object) this.collectionElementType));
    }

    public void EntityName(string entityName) => this.oneToManyMapping.entityname = entityName;

    public void NotFound(NotFoundMode mode)
    {
      if (mode == null)
        return;
      this.oneToManyMapping.notfound = mode.ToHbm();
    }
  }
}
