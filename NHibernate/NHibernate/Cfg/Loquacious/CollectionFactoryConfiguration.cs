// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.CollectionFactoryConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class CollectionFactoryConfiguration : ICollectionFactoryConfiguration
  {
    private readonly FluentSessionFactoryConfiguration fc;

    public CollectionFactoryConfiguration(FluentSessionFactoryConfiguration parent)
    {
      this.fc = parent;
    }

    public IFluentSessionFactoryConfiguration Through<TCollecionsFactory>() where TCollecionsFactory : ICollectionTypeFactory
    {
      this.fc.Configuration.SetProperty("collectiontype.factory_class", typeof (TCollecionsFactory).AssemblyQualifiedName);
      return (IFluentSessionFactoryConfiguration) this.fc;
    }
  }
}
