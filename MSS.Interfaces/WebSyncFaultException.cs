// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.WebSyncFaultException
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace MSS.Interfaces
{
  [DataContract]
  public class WebSyncFaultException
  {
    public string message;
    public Exception innerException;

    public WebSyncFaultException(string message, Exception innerException)
    {
      this.message = message;
      this.innerException = innerException;
    }

    [DataMember]
    public string Message
    {
      get => this.message;
      set => this.message = value;
    }

    [DataMember]
    public Exception InnerException
    {
      get => this.innerException;
      set => this.innerException = value;
    }
  }
}
