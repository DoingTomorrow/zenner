// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSectionHandler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.ConfigurationSchema;
using System.Configuration;
using System.Xml;

#nullable disable
namespace NHibernate.Cfg
{
  public class ConfigurationSectionHandler : IConfigurationSectionHandler
  {
    object IConfigurationSectionHandler.Create(
      object parent,
      object configContext,
      XmlNode section)
    {
      return (object) HibernateConfiguration.FromAppConfig(section);
    }
  }
}
