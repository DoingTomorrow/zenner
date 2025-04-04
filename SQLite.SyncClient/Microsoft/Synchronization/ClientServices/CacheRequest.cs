// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.CacheRequest
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  internal class CacheRequest
  {
    public Guid RequestId;
    public ICollection<IOfflineEntity> Changes;
    public CacheRequestType RequestType;
    public SerializationFormat Format;
    public byte[] KnowledgeBlob;
    public bool IsLastBatch;
  }
}
