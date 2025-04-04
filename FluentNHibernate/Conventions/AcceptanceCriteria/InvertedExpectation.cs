// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.InvertedExpectation
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class InvertedExpectation : IExpectation
  {
    private readonly IExpectation expectation;

    public InvertedExpectation(IExpectation expectation) => this.expectation = expectation;

    public bool Matches(IInspector inspector) => !this.expectation.Matches(inspector);
  }
}
