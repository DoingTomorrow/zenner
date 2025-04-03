// Decompiled with JetBrains decompiler
// Type: Common.Library.Core.Business.Database.DefaultConnectionStringProvider
// Assembly: Common.Library.Core.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 848DF87A-D999-47E1-B1BF-1A19BA680E53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.Core.Business.dll

using System;
using System.Configuration;
using System.Linq;

#nullable disable
namespace Common.Library.Core.Business.Database
{
  public class DefaultConnectionStringProvider : IConnectionStringProvider
  {
    private const string DEFAULT_KEY_NAME = "Default";

    public string GetConnectionString()
    {
      if (ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>().SingleOrDefault<ConnectionStringSettings>((Func<ConnectionStringSettings, bool>) (s => s.Name.Equals("Default", StringComparison.InvariantCultureIgnoreCase))) != null)
        return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
      throw new SettingsPropertyNotFoundException("Could not find the connection string setting named \"Default\" in the configuration file.");
    }

    public string GetConnectionString(params object[] parameters) => this.GetConnectionString();
  }
}
