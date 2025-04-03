// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceMessageException
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace HandlerLib
{
  internal class DeviceMessageException : Exception
  {
    public DeviceMessageException()
    {
    }

    public DeviceMessageException(string message)
      : base(message)
    {
    }

    public DeviceMessageException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DeviceMessageException(SerializationInfo info, StreamingContext context)
    {
    }
  }
}
