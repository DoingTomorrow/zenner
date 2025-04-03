// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Exceptions.InvalidMeterException
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ZENNER.CommonLibrary.Exceptions
{
  [Serializable]
  public class InvalidMeterException : Exception
  {
    public Meter Meter { get; private set; }

    public InvalidMeterException(Meter meter, string message)
      : base(message)
    {
      this.Meter = meter;
    }

    public InvalidMeterException(Meter meter, Exception innerException)
      : base(innerException.Message, innerException)
    {
      this.Meter = meter;
    }
  }
}
