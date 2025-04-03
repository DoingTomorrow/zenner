// Decompiled with JetBrains decompiler
// Type: Castle.Core.Configuration.ConfigurationAttributeCollection
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.Core.Configuration
{
  [Serializable]
  public class ConfigurationAttributeCollection : NameValueCollection
  {
    public ConfigurationAttributeCollection()
    {
    }

    protected ConfigurationAttributeCollection(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
