// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.SyncBatchParameters
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using Microsoft.Synchronization;
using System.Runtime.Serialization;

#nullable disable
namespace MSS.Interfaces
{
  [DataContract]
  public class SyncBatchParameters
  {
    [DataMember]
    public SyncKnowledge DestinationKnowledge;
    [DataMember]
    public uint BatchSize;
  }
}
