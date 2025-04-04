// Decompiled with JetBrains decompiler
// Type: MBusLib.Exceptions.NackException
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using System.Collections.Generic;

#nullable disable
namespace MBusLib.Exceptions
{
  [Serializable]
  public class NackException : Exception
  {
    public string Command { get; private set; }

    public Nack Nack { get; private set; }

    public byte[] Buffer { get; private set; }

    public NackException(string command, Nack nack, byte[] buffer)
      : base(string.Format("{0} [NACK] {1}, {2}", (object) command, (object) nack, (object) Util.ByteArrayToHexString((IEnumerable<byte>) buffer)))
    {
      this.Command = command;
      this.Buffer = buffer;
      this.Nack = nack;
    }
  }
}
