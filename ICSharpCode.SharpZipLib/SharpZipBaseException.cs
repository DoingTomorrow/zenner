﻿// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.SharpZipBaseException
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace ICSharpCode.SharpZipLib
{
  [Serializable]
  public class SharpZipBaseException : ApplicationException
  {
    protected SharpZipBaseException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public SharpZipBaseException()
    {
    }

    public SharpZipBaseException(string message)
      : base(message)
    {
    }

    public SharpZipBaseException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
