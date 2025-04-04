// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.ConfigurationHelper
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  public static class ConfigurationHelper
  {
    public static Configuration AddMappingsFromAssembly(
      this Configuration configuration,
      Assembly assembly)
    {
      PersistenceModel persistenceModel = new PersistenceModel();
      persistenceModel.AddMappingsFromAssembly(assembly);
      persistenceModel.Configure(configuration);
      return configuration;
    }

    public static Configuration AddAutoMappings(
      this Configuration configuration,
      AutoPersistenceModel model)
    {
      model.Configure(configuration);
      return configuration;
    }
  }
}
