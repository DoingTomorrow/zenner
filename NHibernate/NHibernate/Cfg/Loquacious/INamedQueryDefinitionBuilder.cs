// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.INamedQueryDefinitionBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  public interface INamedQueryDefinitionBuilder
  {
    bool IsCacheable { get; set; }

    string CacheRegion { get; set; }

    int FetchSize { get; set; }

    int Timeout { get; set; }

    FlushMode FlushMode { get; set; }

    string Query { get; set; }

    bool IsReadOnly { get; set; }

    string Comment { get; set; }

    NHibernate.CacheMode? CacheMode { get; set; }
  }
}
