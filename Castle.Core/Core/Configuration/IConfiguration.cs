// Decompiled with JetBrains decompiler
// Type: Castle.Core.Configuration.IConfiguration
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core.Configuration
{
  public interface IConfiguration
  {
    string Name { get; }

    string Value { get; }

    ConfigurationCollection Children { get; }

    ConfigurationAttributeCollection Attributes { get; }

    object GetValue(Type type, object defaultValue);
  }
}
