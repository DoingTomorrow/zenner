// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.SessionFactoryConfigurationBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.ConfigurationSchema;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg
{
  public class SessionFactoryConfigurationBase : ISessionFactoryConfiguration
  {
    private string name = string.Empty;
    private readonly IDictionary<string, string> properties = (IDictionary<string, string>) new Dictionary<string, string>();
    private readonly IList<MappingConfiguration> mappings = (IList<MappingConfiguration>) new List<MappingConfiguration>();
    private readonly IList<ClassCacheConfiguration> classesCache = (IList<ClassCacheConfiguration>) new List<ClassCacheConfiguration>();
    private readonly IList<CollectionCacheConfiguration> collectionsCache = (IList<CollectionCacheConfiguration>) new List<CollectionCacheConfiguration>();
    private readonly IList<EventConfiguration> events = (IList<EventConfiguration>) new List<EventConfiguration>();
    private readonly IList<ListenerConfiguration> listeners = (IList<ListenerConfiguration>) new List<ListenerConfiguration>();

    public string Name
    {
      get => this.name;
      protected set => this.name = value;
    }

    public IDictionary<string, string> Properties => this.properties;

    public IList<MappingConfiguration> Mappings => this.mappings;

    public IList<ClassCacheConfiguration> ClassesCache => this.classesCache;

    public IList<CollectionCacheConfiguration> CollectionsCache => this.collectionsCache;

    public IList<EventConfiguration> Events => this.events;

    public IList<ListenerConfiguration> Listeners => this.listeners;
  }
}
