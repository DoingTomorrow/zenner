// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSync.PartialSyncWCFClient
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Business.Modules.WCFRelated;
using MSS.PartialSync.Interfaces;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.PartialSync
{
  public class PartialSyncWCFClient : IPartialSyncWCFClient
  {
    public ServiceClient Client { get; set; }

    public PartialSyncWCFClient(ServiceClient client) => this.Client = client;

    public IEnumerable<T> DeserializeContext<T>(byte[] serializedContext)
    {
      if (serializedContext == null)
        throw new Exception("Serialized Context null .... ");
      using (MemoryStream source = new MemoryStream(serializedContext))
        return Serializer.Deserialize<IEnumerable<T>>((Stream) source);
    }

    public byte[] GetData<T>(Expression predicate = null) where T : class
    {
      return this.Client.GetData(typeof (T).FullName, (object) ExpressionTreeSerializer.CreateSerializer().Serialize(predicate));
    }

    public IPartialSyncWCFClient LockData<T>(object items, Guid idUser) where T : class
    {
      using (MemoryStream destination = new MemoryStream())
      {
        Serializer.Serialize<object>((Stream) destination, items);
        this.Client.UpdateEntities(typeof (T).FullName, (object) Convert.FromBase64String(Convert.ToBase64String(destination.ToArray())), idUser);
      }
      return (IPartialSyncWCFClient) this;
    }

    public void SendSerializedContext(string modelType, byte[] items)
    {
      this.Client.ReceiveData(modelType, (object) items);
    }
  }
}
