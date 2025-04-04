// Decompiled with JetBrains decompiler
// Type: MBusLib.InvalidFrameException
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;

#nullable disable
namespace MBusLib
{
  public class InvalidFrameException : Exception
  {
    public byte[] Buffer { get; private set; }

    public InvalidFrameException(byte[] buffer) => this.Buffer = buffer;

    public InvalidFrameException(string message, byte[] buffer)
      : base(message)
    {
      this.Buffer = buffer;
    }
  }
}
