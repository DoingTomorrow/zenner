// Decompiled with JetBrains decompiler
// Type: NLog.NLogConfigurationException
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NLog
{
  [Serializable]
  public class NLogConfigurationException : Exception
  {
    public NLogConfigurationException()
    {
    }

    public NLogConfigurationException(string message)
      : base(message)
    {
    }

    [StringFormatMethod("message")]
    public NLogConfigurationException(string message, params object[] messageParameters)
      : base(string.Format(message, messageParameters))
    {
    }

    [StringFormatMethod("message")]
    public NLogConfigurationException(
      Exception innerException,
      string message,
      params object[] messageParameters)
      : base(string.Format(message, messageParameters), innerException)
    {
    }

    public NLogConfigurationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected NLogConfigurationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
