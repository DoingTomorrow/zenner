// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.AnyExpectation`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class AnyExpectation<TInspector> : IExpectation where TInspector : IInspector
  {
    private readonly IList<IAcceptanceCriteria<TInspector>> subCriteria;

    public AnyExpectation(
      IEnumerable<IAcceptanceCriteria<TInspector>> subCriteria)
    {
      this.subCriteria = (IList<IAcceptanceCriteria<TInspector>>) new List<IAcceptanceCriteria<TInspector>>(subCriteria);
    }

    public bool Matches(IInspector inspector)
    {
      return this.subCriteria.Any<IAcceptanceCriteria<TInspector>>((Func<IAcceptanceCriteria<TInspector>, bool>) (e => e.Matches(inspector)));
    }

    bool IExpectation.Matches(IInspector inspector)
    {
      return inspector is TInspector inspector1 && this.Matches((IInspector) inspector1);
    }
  }
}
