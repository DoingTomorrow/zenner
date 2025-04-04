// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.ProxyConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class ProxyConfiguration : IProxyConfiguration
  {
    private readonly FluentSessionFactoryConfiguration fc;

    public ProxyConfiguration(FluentSessionFactoryConfiguration parent) => this.fc = parent;

    public IProxyConfiguration DisableValidation()
    {
      this.fc.Configuration.SetProperty("use_proxy_validator", "false");
      return (IProxyConfiguration) this;
    }

    public IFluentSessionFactoryConfiguration Through<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory
    {
      this.fc.Configuration.SetProperty("proxyfactory.factory_class", typeof (TProxyFactoryFactory).AssemblyQualifiedName);
      return (IFluentSessionFactoryConfiguration) this.fc;
    }
  }
}
