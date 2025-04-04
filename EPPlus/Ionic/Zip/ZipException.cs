// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipException
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

#nullable disable
namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d00006")]
  [Serializable]
  public class ZipException : Exception
  {
    public ZipException()
    {
    }

    public ZipException(string message)
      : base(message)
    {
    }

    public ZipException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ZipException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
