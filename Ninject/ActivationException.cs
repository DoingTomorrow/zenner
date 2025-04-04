// Decompiled with JetBrains decompiler
// Type: Ninject.ActivationException
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Ninject
{
  [Serializable]
  public class ActivationException : Exception
  {
    public ActivationException()
    {
    }

    public ActivationException(string message)
      : base(message)
    {
    }

    public ActivationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ActivationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
