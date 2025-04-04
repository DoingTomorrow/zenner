// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.NamedQueryBinder
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
  public class NamedQueryBinder(Mappings mappings) : Binder(mappings)
  {
    public void AddQuery(HbmQuery querySchema)
    {
      string name = querySchema.name;
      string text = querySchema.GetText();
      Binder.log.DebugFormat("Named query: {0} -> {1}", (object) name, (object) text);
      bool cacheable = querySchema.cacheable;
      string cacheregion = querySchema.cacheregion;
      int timeout = string.IsNullOrEmpty(querySchema.timeout) ? RowSelection.NoValue : int.Parse(querySchema.timeout);
      int fetchSize = querySchema.fetchsizeSpecified ? querySchema.fetchsize : -1;
      bool readOnly = querySchema.readonlySpecified && querySchema.@readonly;
      string comment = querySchema.comment;
      FlushMode flushMode = FlushModeConverter.GetFlushMode(querySchema);
      CacheMode? cacheMode = querySchema.cachemodeSpecified ? querySchema.cachemode.ToCacheMode() : new CacheMode?();
      IDictionary<string, string> parameterTypes = (IDictionary<string, string>) new LinkedHashMap<string, string>();
      NamedQueryDefinition query = new NamedQueryDefinition(text, cacheable, cacheregion, timeout, fetchSize, flushMode, cacheMode, readOnly, comment, parameterTypes);
      this.mappings.AddQuery(name, query);
    }
  }
}
