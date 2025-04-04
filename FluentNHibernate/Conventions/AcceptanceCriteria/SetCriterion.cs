// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.SetCriterion
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Utils;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class SetCriterion : IAcceptanceCriterion
  {
    private readonly bool inverse;

    public SetCriterion(bool inverse) => this.inverse = inverse;

    public bool IsSatisfiedBy<T>(Expression<Func<T, object>> expression, T inspector) where T : IInspector
    {
      Member member = expression.ToMember<T, object>();
      bool flag = inspector.IsSet(member);
      return !this.inverse ? flag : !flag;
    }
  }
}
