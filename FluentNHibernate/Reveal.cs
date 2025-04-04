// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Reveal
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate
{
  public static class Reveal
  {
    [Obsolete("Use Reveal.Member")]
    public static Expression<Func<TEntity, object>> Property<TEntity>(string propertyName)
    {
      return Reveal.Member<TEntity>(propertyName);
    }

    [Obsolete("Use Reveal.Member")]
    public static Expression<Func<TEntity, TReturn>> Property<TEntity, TReturn>(string propertyName)
    {
      return Reveal.Member<TEntity, TReturn>(propertyName);
    }

    public static Expression<Func<TEntity, object>> Member<TEntity>(string name)
    {
      return Reveal.CreateExpression<TEntity, object>(name);
    }

    public static Expression<Func<TEntity, TReturn>> Member<TEntity, TReturn>(string name)
    {
      return Reveal.CreateExpression<TEntity, TReturn>(name);
    }

    private static Expression<Func<TEntity, TReturn>> CreateExpression<TEntity, TReturn>(
      string propertyName)
    {
      Type type = typeof (TEntity);
      FluentNHibernate.Member member = type.GetInstanceMembers().FirstOrDefault<FluentNHibernate.Member>((Func<FluentNHibernate.Member, bool>) (x => x.Name == propertyName));
      ParameterExpression parameterExpression = !(member == (FluentNHibernate.Member) null) ? Expression.Parameter(member.DeclaringType, "x") : throw new UnknownPropertyException(type, propertyName);
      Expression expression = (Expression) Expression.PropertyOrField((Expression) parameterExpression, propertyName);
      if (member.PropertyType.IsValueType)
        expression = (Expression) Expression.Convert(expression, typeof (object));
      return (Expression<Func<TEntity, TReturn>>) Expression.Lambda(typeof (Func<TEntity, TReturn>), expression, parameterExpression);
    }
  }
}
