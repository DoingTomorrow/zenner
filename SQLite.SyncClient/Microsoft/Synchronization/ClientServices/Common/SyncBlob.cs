// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.Common.SyncBlob
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.Common
{
  [DataContract(Namespace = "Microsoft.Synchronization.ClientServices.Common")]
  internal sealed class SyncBlob
  {
    [DataMember]
    internal byte[] ClientKnowledge { get; set; }

    [DataMember]
    internal string ClientScopeName { get; set; }

    [DataMember]
    internal bool IsLastBatch { get; set; }

    [DataMember]
    internal Guid? BatchCode { get; set; }

    [DataMember]
    internal Guid? NextBatch { get; set; }

    internal byte[] Serialize()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new DataContractJsonSerializer(typeof (SyncBlob)).WriteObject((Stream) memoryStream, (object) this);
        return memoryStream.ToArray();
      }
    }

    internal static SyncBlob DeSerialize(byte[] syncBlob)
    {
      SyncBlob syncBlob1;
      using (MemoryStream memoryStream = new MemoryStream(syncBlob))
      {
        memoryStream.Position = 0L;
        StreamReader streamReader = new StreamReader((Stream) memoryStream);
        memoryStream.Position = 0L;
        syncBlob1 = new DataContractJsonSerializer(typeof (SyncBlob)).ReadObject((Stream) memoryStream) as SyncBlob;
      }
      return syncBlob1;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("ClientScopeName : " + this.ClientScopeName);
      stringBuilder.AppendLine("IsLastBatch : " + this.IsLastBatch.ToString());
      stringBuilder.AppendLine("BatchCode : " + (object) this.BatchCode);
      stringBuilder.AppendLine("NextBatch : " + (object) this.NextBatch);
      return stringBuilder.ToString();
    }
  }
}
