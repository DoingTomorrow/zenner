// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.NamedSQLQueryBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class NamedSQLQueryBinder(Mappings mappings) : Binder(mappings)
  {
    public void AddSqlQuery(HbmSqlQuery querySchema)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (param0 =>
      {
        string name = querySchema.name;
        string text = querySchema.GetText();
        bool cacheable = querySchema.cacheable;
        string cacheregion = querySchema.cacheregion;
        int timeout = string.IsNullOrEmpty(querySchema.timeout) ? RowSelection.NoValue : int.Parse(querySchema.timeout);
        int fetchSize = querySchema.fetchsizeSpecified ? querySchema.fetchsize : -1;
        bool readOnly = querySchema.readonlySpecified && querySchema.@readonly;
        string comment = (string) null;
        bool callable = querySchema.callable;
        string resultsetref = querySchema.resultsetref;
        FlushMode flushMode = FlushModeConverter.GetFlushMode(querySchema);
        CacheMode? cacheMode = querySchema.cachemodeSpecified ? querySchema.cachemode.ToCacheMode() : new CacheMode?();
        IDictionary<string, string> parameterTypes = (IDictionary<string, string>) new LinkedHashMap<string, string>();
        IList<string> synchronizedTables = NamedSQLQueryBinder.GetSynchronizedTables(querySchema);
        NamedSQLQueryDefinition query;
        if (string.IsNullOrEmpty(resultsetref))
        {
          ResultSetMappingDefinition mappingDefinition = new ResultSetMappingBinder(this.Mappings).Create(querySchema);
          query = new NamedSQLQueryDefinition(text, mappingDefinition.GetQueryReturns(), synchronizedTables, cacheable, cacheregion, timeout, fetchSize, flushMode, cacheMode, readOnly, comment, parameterTypes, callable);
        }
        else
          query = new NamedSQLQueryDefinition(text, resultsetref, synchronizedTables, cacheable, cacheregion, timeout, fetchSize, flushMode, cacheMode, readOnly, comment, parameterTypes, callable);
        Binder.log.DebugFormat("Named SQL query: {0} -> {1}", (object) name, (object) query.QueryString);
        this.mappings.AddSQLQuery(name, query);
      }));
    }

    private static IList<string> GetSynchronizedTables(HbmSqlQuery querySchema)
    {
      IList<string> synchronizedTables = (IList<string>) new List<string>();
      foreach (object obj in querySchema.Items ?? new object[0])
      {
        if (obj is HbmSynchronize hbmSynchronize)
          synchronizedTables.Add(hbmSynchronize.table);
      }
      return synchronizedTables;
    }
  }
}
