// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.JoinKeyCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class JoinKeyCustomizer<TEntity> : IKeyMapper<TEntity>, IColumnsMapper where TEntity : class
  {
    public JoinKeyCustomizer(ICustomizersHolder customizersHolder)
    {
      this.CustomizersHolder = customizersHolder;
    }

    public ICustomizersHolder CustomizersHolder { get; private set; }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.Column(columnMapper)))));
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.Columns(columnMapper)))));
    }

    public void Column(string columnName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.Column(columnName)))));
    }

    public void OnDelete(OnDeleteAction deleteAction)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.OnDelete(deleteAction)))));
    }

    public void PropertyRef<TProperty>(
      Expression<Func<TEntity, TProperty>> propertyGetter)
    {
      MemberInfo member = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(propertyGetter);
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.PropertyRef(member)))));
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.Update(consideredInUpdateQuery)))));
    }

    public void ForeignKey(string foreignKeyName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.ForeignKey(foreignKeyName)))));
    }

    public void NotNullable(bool notnull)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.NotNullable(notnull)))));
    }

    public void Unique(bool unique)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Key((Action<IKeyMapper>) (x => x.Unique(unique)))));
    }
  }
}
