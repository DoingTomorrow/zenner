// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.MapKeyCustomizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class MapKeyCustomizer : IMapKeyMapper, IColumnsMapper
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly PropertyPath propertyPath;

    public MapKeyCustomizer(PropertyPath propertyPath, ICustomizersHolder customizersHolder)
    {
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Column(columnMapper)));
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Columns(columnMapper)));
    }

    public void Column(string name)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Column(name)));
    }

    public void Type(IType persistentType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Type(persistentType)));
    }

    public void Type<TPersistentType>()
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Type<TPersistentType>()));
    }

    public void Type(System.Type persistentType)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Type(persistentType)));
    }

    public void Length(int length)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Length(length)));
    }

    public void Formula(string formula)
    {
      this.customizersHolder.AddCustomizer(this.propertyPath, (Action<IMapKeyMapper>) (x => x.Formula(formula)));
    }
  }
}
