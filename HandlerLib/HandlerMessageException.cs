// Decompiled with JetBrains decompiler
// Type: HandlerLib.HandlerMessageException
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace HandlerLib
{
  public class HandlerMessageException : Exception
  {
    public HandlerMessageException()
    {
    }

    public HandlerMessageException(string message)
      : base(message)
    {
    }

    public HandlerMessageException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected HandlerMessageException(SerializationInfo info, StreamingContext context)
    {
    }
  }
}
