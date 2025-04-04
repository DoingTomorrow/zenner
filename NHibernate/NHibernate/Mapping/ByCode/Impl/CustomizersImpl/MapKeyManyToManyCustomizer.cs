// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.MapKeyManyToManyCustomizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class MapKeyManyToManyCustomizer : IMapKeyManyToManyMapper, IColumnsMapper
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly PropertyPath propertyPath;

    public MapKeyManyToManyCustomizer(
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
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyManyToManyMapper>) (x => x.Column(columnMapper)));
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyManyToManyMapper>) (x => x.Columns(columnMapper)));
    }

    public void Column(string name)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyManyToManyMapper>) (x => x.Column(name)));
    }

    public void ForeignKey(string foreignKeyName)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyManyToManyMapper>) (x => x.ForeignKey(foreignKeyName)));
    }

    public void Formula(string formula)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyManyToManyMapper>) (x => x.Formula(formula)));
    }
  }
}
