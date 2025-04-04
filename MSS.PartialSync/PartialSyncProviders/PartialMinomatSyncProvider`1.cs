// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSyncProviders.PartialMinomatSyncProvider`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Core.Model.Orders;
using MSS.Interfaces;
using MSS.PartialSync.Interfaces;
using NHibernate;
using System;

#nullable disable
namespace MSS.PartialSync.PartialSyncProviders
{
  public class PartialMinomatSyncProvider<TEntity> : 
    BasePartialSyncProvider<TEntity>,
    IPartialSyncOperation<IPartialSynchronizableEntity>
    where TEntity : Order
  {
    public void ApplyUpdate(ISession session, IPartialSyncWCFClient wcfClient)
    {
      this.SaveMinomats();
    }

    public int GetPriotity() => throw new NotImplementedException();

    public void UploadLocalModifications(ISession session, IPartialSyncWCFClient wcfClient)
    {
      throw new NotImplementedException();
    }

    private void GetMinomats()
    {
    }

    private void SaveMinomats() => this.GetMinomats();
  }
}
