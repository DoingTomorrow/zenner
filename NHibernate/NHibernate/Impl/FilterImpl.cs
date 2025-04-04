// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.FilterImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public class FilterImpl : IFilter
  {
    public static readonly string MARKER = "$FILTER_PLACEHOLDER$";
    [NonSerialized]
    private FilterDefinition definition;
    private readonly IDictionary<string, object> parameters = (IDictionary<string, object>) new Dictionary<string, object>();

    public void AfterDeserialize(FilterDefinition factoryDefinition)
    {
      this.definition = factoryDefinition;
    }

    public FilterImpl(FilterDefinition configuration) => this.definition = configuration;

    public FilterDefinition FilterDefinition => this.definition;

    public string Name => this.definition.FilterName;

    public IDictionary<string, object> Parameters => this.parameters;

    public IFilter SetParameter(string name, object value)
    {
      IType parameterType = this.definition.GetParameterType(name);
      if (parameterType == null)
        throw new ArgumentException(name, "Undefined filter parameter [" + name + "]");
      if (value != null && !parameterType.ReturnedClass.IsAssignableFrom(value.GetType()))
        throw new ArgumentException(name, "Incorrect type for parameter [" + name + "]");
      this.parameters[name] = value;
      return (IFilter) this;
    }

    public IFilter SetParameterList(string name, ICollection values)
    {
      if (values == null)
        throw new ArgumentException("Collection must be not null!", nameof (values));
      IType parameterType = this.definition.GetParameterType(name);
      if (parameterType == null)
        throw new HibernateException("Undefined filter parameter [" + name + "]");
      if (values.Count > 0)
      {
        IEnumerator enumerator = values.GetEnumerator();
        enumerator.MoveNext();
        if (!parameterType.ReturnedClass.IsAssignableFrom(enumerator.Current.GetType()))
          throw new HibernateException("Incorrect type for parameter [" + name + "]");
      }
      this.parameters[name] = (object) values;
      return (IFilter) this;
    }

    public IFilter SetParameterList(string name, object[] values)
    {
      return this.SetParameterList(name, (ICollection) new ArrayList((ICollection) values));
    }

    public object GetParameter(string name)
    {
      object parameter;
      this.parameters.TryGetValue(name, out parameter);
      return parameter;
    }

    public void Validate()
    {
      foreach (string parameterName in (IEnumerable<string>) this.definition.ParameterNames)
      {
        if (!this.parameters.ContainsKey(parameterName))
          throw new HibernateException("Filter [" + this.Name + "] parameter [" + parameterName + "] value not set");
      }
    }
  }
}
