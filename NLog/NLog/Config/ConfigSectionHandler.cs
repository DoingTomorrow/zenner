// Decompiled with JetBrains decompiler
// Type: NLog.Config.ConfigSectionHandler
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal.Fakeables;
using System;
using System.Configuration;
using System.Xml;

#nullable disable
namespace NLog.Config
{
  public sealed class ConfigSectionHandler : IConfigurationSectionHandler
  {
    private object Create(XmlNode section, IAppDomain appDomain)
    {
      try
      {
        string configurationFile = appDomain.ConfigurationFile;
        return (object) new XmlLoggingConfiguration((XmlElement) section, configurationFile);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "ConfigSectionHandler error.");
        throw;
      }
    }

    object IConfigurationSectionHandler.Create(
      object parent,
      object configContext,
      XmlNode section)
    {
      return this.Create(section, LogFactory.CurrentAppDomain);
    }
  }
}
