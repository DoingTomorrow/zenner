// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.FilterDefinitionFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class FilterDefinitionFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (FilterDefinitionFactory));

    public static FilterDefinition CreateFilterDefinition(HbmFilterDef filterDefSchema)
    {
      FilterDefinitionFactory.log.DebugFormat("Parsing filter-def [{0}]", (object) filterDefSchema.name);
      string defaultCondition = filterDefSchema.GetDefaultCondition();
      IDictionary<string, IType> filterParameterTypes = FilterDefinitionFactory.GetFilterParameterTypes(filterDefSchema);
      FilterDefinitionFactory.log.DebugFormat("Parsed filter-def [{0}]", (object) filterDefSchema.name);
      return new FilterDefinition(filterDefSchema.name, defaultCondition, filterParameterTypes, filterDefSchema.usemanytoone);
    }

    private static IDictionary<string, IType> GetFilterParameterTypes(HbmFilterDef filterDefSchema)
    {
      Dictionary<string, IType> filterParameterTypes = new Dictionary<string, IType>();
      foreach (HbmFilterParam listParameter in filterDefSchema.ListParameters())
      {
        FilterDefinitionFactory.log.DebugFormat("Adding filter parameter : {0} -> {1}", (object) listParameter.name, (object) listParameter.type);
        IType type = TypeFactory.HeuristicType(listParameter.type);
        FilterDefinitionFactory.log.DebugFormat("Parameter heuristic type : {0}", (object) type);
        filterParameterTypes.Add(listParameter.name, type);
      }
      return (IDictionary<string, IType>) filterParameterTypes;
    }
  }
}
