// Decompiled with JetBrains decompiler
// Type: Castle.Core.Configuration.MutableConfiguration
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core.Configuration
{
  [Serializable]
  public class MutableConfiguration : AbstractConfiguration
  {
    public MutableConfiguration(string name)
      : this(name, (string) null)
    {
    }

    public MutableConfiguration(string name, string value)
    {
      this.Name = name;
      this.Value = value;
    }

    public new string Value
    {
      get => base.Value;
      set => base.Value = value;
    }

    public static MutableConfiguration Create(string name) => new MutableConfiguration(name);

    public MutableConfiguration Attribute(string name, string value)
    {
      this.Attributes[name] = value;
      return this;
    }

    public MutableConfiguration CreateChild(string name)
    {
      MutableConfiguration child = new MutableConfiguration(name);
      this.Children.Add((IConfiguration) child);
      return child;
    }

    public MutableConfiguration CreateChild(string name, string value)
    {
      MutableConfiguration child = new MutableConfiguration(name, value);
      this.Children.Add((IConfiguration) child);
      return child;
    }
  }
}
