// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ManyToManyCustomizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ManyToManyCustomizer : IManyToManyMapper, IColumnsMapper
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly PropertyPath propertyPath;

    public ManyToManyCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsManyToManyRelation(propertyPath.LocalMember);
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.Column(columnMapper)));
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.Columns(columnMapper)));
    }

    public void Column(string name)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.Column(name)));
    }

    public void Class(Type entityType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.Class(entityType)));
    }

    public void EntityName(string entityName)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.EntityName(entityName)));
    }

    public void NotFound(NotFoundMode mode)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.NotFound(mode)));
    }

    public void Formula(string formula)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.Formula(formula)));
    }

    public void Lazy(LazyRelation lazyRelation)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.Lazy(lazyRelation)));
    }

    public void ForeignKey(string foreignKeyName)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IManyToManyMapper>) (x => x.ForeignKey(foreignKeyName)));
    }
  }
}
