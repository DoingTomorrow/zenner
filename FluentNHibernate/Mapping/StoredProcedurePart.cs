// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.StoredProcedurePart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class StoredProcedurePart : IStoredProcedureMappingProvider
  {
    private readonly StoredProcedureMapping mapping = new StoredProcedureMapping();

    public StoredProcedurePart(string element, string innerText)
    {
      this.mapping.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.SPType), 0, element);
      this.mapping.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.Query), 0, innerText);
      this.mapping.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.Check), 0, "rowcount");
    }

    public CheckTypeExpression<StoredProcedurePart> Check
    {
      get
      {
        return new CheckTypeExpression<StoredProcedurePart>(this, (Action<string>) (value => this.mapping.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.Check), 2, value)));
      }
    }

    StoredProcedureMapping IStoredProcedureMappingProvider.GetStoredProcedureMapping()
    {
      return this.mapping;
    }
  }
}
