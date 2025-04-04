// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Interfaces.IPartialSyncManager
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Interfaces;

#nullable disable
namespace MSS.PartialSync.Interfaces
{
  public interface IPartialSyncManager
  {
    IPartialSyncManager RegisterProviders(
      IPartialSyncOperation<IPartialSynchronizableEntity> provider);

    void PerformPartialSync();

    void RemoveData();
  }
}
