// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Interfaces.PartialSyncOperationExtensions
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Interfaces;
using NHibernate;
using System;

#nullable disable
namespace MSS.PartialSync.Interfaces
{
  public static class PartialSyncOperationExtensions
  {
    public static void SaveEntity(
      this IPartialSynchronizableEntity entity,
      ISession session,
      Action<IPartialSynchronizableEntity> action)
    {
      session.Save((object) entity);
      action(entity);
    }
  }
}
