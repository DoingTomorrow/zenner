// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ParamBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ParamBuilder
  {
    private readonly IDictionary<string, string> parameters;

    public ParamBuilder(IDictionary<string, string> parameters) => this.parameters = parameters;

    public ParamBuilder AddParam(string name, string value)
    {
      this.parameters.Add(name, value);
      return this;
    }
  }
}
