// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.MahAppsException
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace MahApps.Metro
{
  [Serializable]
  public class MahAppsException : Exception
  {
    public MahAppsException()
    {
    }

    public MahAppsException(string message)
      : base(message)
    {
    }

    public MahAppsException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected MahAppsException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
