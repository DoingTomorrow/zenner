// Decompiled with JetBrains decompiler
// Type: Castle.Core.Configuration.AbstractConfiguration
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Threading;

#nullable disable
namespace Castle.Core.Configuration
{
  [Serializable]
  public abstract class AbstractConfiguration : IConfiguration
  {
    private readonly ConfigurationAttributeCollection attributes = new ConfigurationAttributeCollection();
    private readonly ConfigurationCollection children = new ConfigurationCollection();
    private string internalName;
    private string internalValue;

    public string Name
    {
      get => this.internalName;
      protected set => this.internalName = value;
    }

    public string Value
    {
      get => this.internalValue;
      protected set => this.internalValue = value;
    }

    public virtual ConfigurationCollection Children => this.children;

    public virtual ConfigurationAttributeCollection Attributes => this.attributes;

    public virtual object GetValue(Type type, object defaultValue)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      try
      {
        return Convert.ChangeType((object) this.Value, type, (IFormatProvider) Thread.CurrentThread.CurrentCulture);
      }
      catch (InvalidCastException ex)
      {
        return defaultValue;
      }
    }
  }
}
