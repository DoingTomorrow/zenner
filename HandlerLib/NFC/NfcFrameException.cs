// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NfcFrameException
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib.NFC
{
  public class NfcFrameException : Exception
  {
    public NfcFrameException()
    {
    }

    public NfcFrameException(string message)
      : base(message)
    {
    }

    public NfcFrameException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
