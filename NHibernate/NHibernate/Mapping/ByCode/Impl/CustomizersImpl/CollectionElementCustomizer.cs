// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.CollectionElementCustomizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class CollectionElementCustomizer : IElementMapper, IColumnsMapper
  {
    private readonly PropertyPath propertyPath;

    public CollectionElementCustomizer(
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      this.propertyPath = propertyPath;
      this.CustomizersHolder = customizersHolder;
    }

    public ICustomizersHolder CustomizersHolder { get; private set; }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Column(columnMapper)));
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Columns(columnMapper)));
    }

    public void Column(string name)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Column(name)));
    }

    public void Type(IType persistentType)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Type(persistentType)));
    }

    public void Type<TPersistentType>()
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Type<TPersistentType>()));
    }

    public void Type<TPersistentType>(object parameters)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Type<TPersistentType>(parameters)));
    }

    public void Type(System.Type persistentType, object parameters)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Type(persistentType, parameters)));
    }

    public void Length(int length)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Length(length)));
    }

    public void Precision(short precision)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Precision(precision)));
    }

    public void Scale(short scale)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Scale(scale)));
    }

    public void NotNullable(bool notnull)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.NotNullable(notnull)));
    }

    public void Unique(bool unique)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Unique(unique)));
    }

    public void Formula(string formula)
    {
      this.CustomizersHolder.AddCustomizer(this.propertyPath, (Action<IElementMapper>) (e => e.Formula(formula)));
    }
  }
}
