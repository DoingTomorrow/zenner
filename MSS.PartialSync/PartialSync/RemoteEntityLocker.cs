// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSync.RemoteEntityLocker
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Interfaces;
using MSS.PartialSync.Interfaces;
using MSS.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS.PartialSync.PartialSync
{
  internal class RemoteEntityLocker
  {
    private readonly IEnumerable<IPartialSynchronizableEntity> _items;
    private static GenericMethodInvoker<IPartialSyncWCFClient> invoker = new GenericMethodInvoker<IPartialSyncWCFClient>("LockData");

    public RemoteEntityLocker(IEnumerable<IPartialSynchronizableEntity> items)
    {
      this._items = items;
    }

    public void ApplyExecutionPlan(IPartialSyncWCFClient wcfClient)
    {
      KeyValuePair<Type, IEnumerable<Guid>> keyValuePair = this._items.IfNotNull<IEnumerable<IPartialSynchronizableEntity>, IEnumerable<IPartialSynchronizableEntity>>((Func<IEnumerable<IPartialSynchronizableEntity>, IEnumerable<IPartialSynchronizableEntity>>) (_ => _)).GroupBy<IPartialSynchronizableEntity, Type>((Func<IPartialSynchronizableEntity, Type>) (g => g.GetType())).ToDictionary<IGrouping<Type, IPartialSynchronizableEntity>, Type, IEnumerable<Guid>>((Func<IGrouping<Type, IPartialSynchronizableEntity>, Type>) (g => g.Key), (Func<IGrouping<Type, IPartialSynchronizableEntity>, IEnumerable<Guid>>) (v => this._items.Select<IPartialSynchronizableEntity, Guid>((Func<IPartialSynchronizableEntity, Guid>) (i => i.Id)))).FirstOrDefault<KeyValuePair<Type, IEnumerable<Guid>>>();
      RemoteEntityLocker.invoker.Invoke(new Type[1]
      {
        keyValuePair.Key
      }, (object) wcfClient, (object) keyValuePair.Value, (object) MSS.Business.Utils.AppContext.Current.LoggedUser.Id);
    }
  }
}
