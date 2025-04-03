// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.InvalidMixinConfigurationException
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy
{
  [Serializable]
  public class InvalidMixinConfigurationException : Exception
  {
    public InvalidMixinConfigurationException(string message)
      : base(message)
    {
    }

    public InvalidMixinConfigurationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected InvalidMixinConfigurationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
