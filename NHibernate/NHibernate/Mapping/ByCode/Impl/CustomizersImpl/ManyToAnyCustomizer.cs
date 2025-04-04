// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ManyToAnyCustomizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ManyToAnyCustomizer : IManyToAnyMapper
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly PropertyPath propertyPath;

    public ManyToAnyCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsManyToAnyRelation(propertyPath.LocalMember);
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void MetaType(IType metaType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.MetaType(metaType)));
    }

    public void MetaType<TMetaType>()
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.MetaType<TMetaType>()));
    }

    public void MetaType(System.Type metaType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.MetaType(metaType)));
    }

    public void IdType(IType idType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.IdType(idType)));
    }

    public void IdType<TIdType>()
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.IdType<TIdType>()));
    }

    public void IdType(System.Type idType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.IdType(idType)));
    }

    public void Columns(
      Action<IColumnMapper> idColumnMapping,
      Action<IColumnMapper> classColumnMapping)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.Columns(idColumnMapping, classColumnMapping)));
    }

    public void MetaValue(object value, System.Type entityType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToAnyMapper>) (x => x.MetaValue(value, entityType)));
    }
  }
}
