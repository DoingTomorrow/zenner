// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ISessionFactoryConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.ConfigurationSchema;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg
{
  public interface ISessionFactoryConfiguration
  {
    string Name { get; }

    IDictionary<string, string> Properties { get; }

    IList<MappingConfiguration> Mappings { get; }

    IList<ClassCacheConfiguration> ClassesCache { get; }

    IList<CollectionCacheConfiguration> CollectionsCache { get; }

    IList<EventConfiguration> Events { get; }

    IList<ListenerConfiguration> Listeners { get; }
  }
}
