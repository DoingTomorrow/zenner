// Decompiled with JetBrains decompiler
// Type: Castle.Core.Configuration.ConfigurationCollection
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Castle.Core.Configuration
{
  [Serializable]
  public class ConfigurationCollection : List<IConfiguration>
  {
    public ConfigurationCollection()
    {
    }

    public ConfigurationCollection(IEnumerable<IConfiguration> value)
      : base(value)
    {
    }

    public IConfiguration this[string name]
    {
      get
      {
        foreach (IConfiguration configuration in (List<IConfiguration>) this)
        {
          if (name.Equals(configuration.Name))
            return configuration;
        }
        return (IConfiguration) null;
      }
    }
  }
}
