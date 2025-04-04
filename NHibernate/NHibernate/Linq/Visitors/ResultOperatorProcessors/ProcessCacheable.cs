// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessCacheable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Param;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessCacheable : IResultOperatorProcessor<CacheableResultOperator>
  {
    public void Process(
      CacheableResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      NamedParameter parameterName;
      switch (resultOperator.ParseInfo.ParsedExpression.Method.Name)
      {
        case "Cacheable":
          tree.AddAdditionalCriteria((Action<IQuery, IDictionary<string, Tuple<object, IType>>>) ((q, p) => q.SetCacheable(true)));
          break;
        case "CacheMode":
          queryModelVisitor.VisitorParameters.ConstantToParameterMap.TryGetValue(resultOperator.Data, out parameterName);
          if (parameterName != null)
          {
            tree.AddAdditionalCriteria((Action<IQuery, IDictionary<string, Tuple<object, IType>>>) ((q, p) => q.SetCacheMode((CacheMode) p[parameterName.Name].First)));
            break;
          }
          tree.AddAdditionalCriteria((Action<IQuery, IDictionary<string, Tuple<object, IType>>>) ((q, p) => q.SetCacheMode((CacheMode) resultOperator.Data.Value)));
          break;
        case "CacheRegion":
          queryModelVisitor.VisitorParameters.ConstantToParameterMap.TryGetValue(resultOperator.Data, out parameterName);
          if (parameterName != null)
          {
            tree.AddAdditionalCriteria((Action<IQuery, IDictionary<string, Tuple<object, IType>>>) ((q, p) => q.SetCacheRegion((string) p[parameterName.Name].First)));
            break;
          }
          tree.AddAdditionalCriteria((Action<IQuery, IDictionary<string, Tuple<object, IType>>>) ((q, p) => q.SetCacheRegion((string) resultOperator.Data.Value)));
          break;
      }
    }
  }
}
