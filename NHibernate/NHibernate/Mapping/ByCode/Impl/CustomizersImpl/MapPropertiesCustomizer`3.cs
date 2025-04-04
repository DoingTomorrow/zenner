// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.MapPropertiesCustomizer`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class MapPropertiesCustomizer<TEntity, TKey, TElement> : 
    CollectionPropertiesCustomizer<TEntity, TElement>,
    IMapPropertiesMapper<TEntity, TKey, TElement>,
    ICollectionPropertiesMapper<TEntity, TElement>,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    ICollectionSqlsMapper
    where TEntity : class
  {
    public MapPropertiesCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, propertyPath, customizersHolder)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsMap(propertyPath.LocalMember);
    }
  }
}
