// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.EqualCriterion
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class EqualCriterion : IAcceptanceCriterion
  {
    private readonly bool inverse;
    private readonly object value;

    public EqualCriterion(bool inverse, object value)
    {
      this.inverse = inverse;
      this.value = value;
    }

    public bool IsSatisfiedBy<T>(Expression<Func<T, object>> propertyExpression, T inspector) where T : IInspector
    {
      bool flag = propertyExpression.Compile()(inspector).Equals(this.value);
      return !this.inverse ? flag : !flag;
    }
  }
}
