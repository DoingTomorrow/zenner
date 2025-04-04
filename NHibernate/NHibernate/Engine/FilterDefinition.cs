// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.FilterDefinition
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class FilterDefinition
  {
    private readonly string filterName;
    private readonly string defaultFilterCondition;
    private readonly IDictionary<string, IType> parameterTypes = (IDictionary<string, IType>) new Dictionary<string, IType>();
    private readonly bool useInManyToOne;

    public FilterDefinition(
      string name,
      string defaultCondition,
      IDictionary<string, IType> parameterTypes,
      bool useManyToOne)
    {
      this.filterName = name;
      this.defaultFilterCondition = defaultCondition;
      this.parameterTypes = parameterTypes;
      this.useInManyToOne = useManyToOne;
    }

    public bool UseInManyToOne => this.useInManyToOne;

    public string FilterName => this.filterName;

    public ICollection<string> ParameterNames => this.parameterTypes.Keys;

    public IType GetParameterType(string parameterName)
    {
      IType parameterType;
      this.parameterTypes.TryGetValue(parameterName, out parameterType);
      return parameterType;
    }

    public string DefaultFilterCondition => this.defaultFilterCondition;

    public IDictionary<string, IType> ParameterTypes => this.parameterTypes;
  }
}
