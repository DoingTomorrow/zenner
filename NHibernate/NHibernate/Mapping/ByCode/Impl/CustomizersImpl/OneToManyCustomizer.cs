// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.OneToManyCustomizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class OneToManyCustomizer : IOneToManyMapper
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly PropertyPath propertyPath;

    public OneToManyCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsOneToManyRelation(propertyPath.LocalMember);
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void Class(Type entityType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IOneToManyMapper>) (x => x.Class(entityType)));
    }

    public void EntityName(string entityName)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IOneToManyMapper>) (x => x.EntityName(entityName)));
    }

    public void NotFound(NotFoundMode mode)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IOneToManyMapper>) (x => x.NotFound(mode)));
    }
  }
}
