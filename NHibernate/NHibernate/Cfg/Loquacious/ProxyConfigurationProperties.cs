// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.ProxyConfigurationProperties
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class ProxyConfigurationProperties : IProxyConfigurationProperties
  {
    private readonly Configuration configuration;

    public ProxyConfigurationProperties(Configuration configuration)
    {
      this.configuration = configuration;
    }

    public bool Validation
    {
      set
      {
        this.configuration.SetProperty("use_proxy_validator", value.ToString().ToLowerInvariant());
      }
    }

    public void ProxyFactoryFactory<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory
    {
      this.configuration.SetProperty("proxyfactory.factory_class", typeof (TProxyFactoryFactory).AssemblyQualifiedName);
    }
  }
}
