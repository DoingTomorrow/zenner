// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.NoSetterAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class NoSetterAccessor : IPropertyAccessor
  {
    private readonly IFieldNamingStrategy namingStrategy;

    public NoSetterAccessor(IFieldNamingStrategy namingStrategy)
    {
      this.namingStrategy = namingStrategy;
    }

    public IGetter GetGetter(Type type, string propertyName)
    {
      return (IGetter) (BasicPropertyAccessor.GetGetterOrNull(type, propertyName) ?? throw new PropertyNotFoundException(type, propertyName, "getter"));
    }

    public ISetter GetSetter(Type type, string propertyName)
    {
      string fieldName = this.namingStrategy.GetFieldName(propertyName);
      return (ISetter) new FieldAccessor.FieldSetter(FieldAccessor.GetField(type, fieldName), type, fieldName);
    }

    public bool CanAccessThroughReflectionOptimizer => true;
  }
}
