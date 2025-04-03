// Decompiled with JetBrains decompiler
// Type: AutoMapper.DelegateFactoryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class DelegateFactoryOverride : DelegateFactory
  {
    public override LateBoundFieldSet CreateSet(FieldInfo field)
    {
      ParameterExpression parameterExpression;
      return ((Expression<LateBoundFieldSet>) ((target, value) => Expression.Assign((Expression) Expression.Field((Expression) Expression.Convert((Expression) parameterExpression, field.DeclaringType), field), (Expression) Expression.Convert(value, field.FieldType)))).Compile();
    }

    public override LateBoundPropertySet CreateSet(PropertyInfo property)
    {
      ParameterExpression parameterExpression;
      return ((Expression<LateBoundPropertySet>) ((target, value) => Expression.Assign((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, property.DeclaringType), property), (Expression) Expression.Convert(value, property.PropertyType)))).Compile();
    }
  }
}
