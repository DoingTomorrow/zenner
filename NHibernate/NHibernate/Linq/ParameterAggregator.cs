// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ParameterAggregator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Param;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq
{
  public class ParameterAggregator
  {
    private readonly List<NamedParameter> _parameters = new List<NamedParameter>();

    public NamedParameter AddParameter(object value, IType type)
    {
      NamedParameter namedParameter = new NamedParameter("p" + (object) (this._parameters.Count + 1), value, type);
      this._parameters.Add(namedParameter);
      return namedParameter;
    }

    public NamedParameter[] GetParameters() => this._parameters.ToArray();
  }
}
