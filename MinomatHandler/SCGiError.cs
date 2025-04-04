// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiError
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;

#nullable disable
namespace MinomatHandler
{
  public sealed class SCGiError : Exception
  {
    public SCGiErrorType Type { get; set; }

    public byte[] Buffer { get; set; }

    public SCGiError(SCGiErrorType type, string message)
      : base(message)
    {
      this.Type = type;
    }

    public SCGiError(SCGiErrorType type, string message, Exception innerException)
      : base(message, innerException)
    {
      this.Type = type;
    }

    public SCGiError(SCGiErrorType type, string message, byte[] buffer)
      : this(type, message)
    {
      this.Buffer = buffer;
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Message))
        return this.Type.ToString();
      return this.InnerException == null ? this.Message : this.Message + " InnerException: " + this.InnerException.Message;
    }
  }
}
