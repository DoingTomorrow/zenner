// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.ExtendedQuery
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class ExtendedQuery
  {
    private QueryPropertyAliasesList aliasesList;
    private Dictionary<string, object> parametersDictionary = new Dictionary<string, object>();
    private Dictionary<string, IEnumerable> parametersListDictionary = new Dictionary<string, IEnumerable>();
    private IQuery query;

    public ExtendedQuery(IQuery query) => this.query = query;

    public IQuery Query
    {
      get => this.query;
      set
      {
        this.query = value;
        foreach (string key in this.parametersDictionary.Keys)
          this.query.SetParameter(key, this.parametersDictionary[key]);
        foreach (string key in this.parametersListDictionary.Keys)
          this.query.SetParameterList(key, this.parametersListDictionary[key]);
      }
    }

    public ExtendedQuery SetParameter(string parameterName, object value)
    {
      this.parametersDictionary.Add(parameterName, value);
      this.query.SetParameter(parameterName, value);
      return this;
    }

    public ExtendedQuery SetParameterList(string name, IEnumerable values)
    {
      this.parametersListDictionary.Add(name, values);
      this.query.SetParameterList(name, values);
      return this;
    }

    public IEnumerable<string> GetNamedParameters()
    {
      return (IEnumerable<string>) this.parametersDictionary.Keys;
    }

    public IEnumerable<string> GetNamedParameterLists()
    {
      return (IEnumerable<string>) this.parametersListDictionary.Keys;
    }

    public object GetParameterValue(string name)
    {
      return this.parametersDictionary.ContainsKey(name) ? this.parametersDictionary[name] : (object) null;
    }

    public IEnumerable GetParameterListValue(string name)
    {
      return this.parametersListDictionary.ContainsKey(name) ? this.parametersListDictionary[name] : (IEnumerable) null;
    }
  }
}
