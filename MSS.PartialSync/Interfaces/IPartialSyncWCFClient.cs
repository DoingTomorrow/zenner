// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Interfaces.IPartialSyncWCFClient
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.Interfaces
{
  public interface IPartialSyncWCFClient
  {
    byte[] GetData<T>(Expression predicate = null) where T : class;

    IEnumerable<T> DeserializeContext<T>(byte[] serializedContext);

    IPartialSyncWCFClient LockData<T>(object items, Guid idUser) where T : class;

    void SendSerializedContext(string modelType, byte[] items);
  }
}
