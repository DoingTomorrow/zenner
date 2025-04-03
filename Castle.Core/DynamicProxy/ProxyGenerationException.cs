// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.ProxyGenerationException
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.DynamicProxy
{
  public class ProxyGenerationException : Exception
  {
    public ProxyGenerationException(string message)
      : base(message)
    {
    }

    public ProxyGenerationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
