// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.SynchronizationHelperBase
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization;

#nullable disable
namespace MSS.Business.Modules.Synchronization
{
  public abstract class SynchronizationHelperBase
  {
    public bool IsVersionUpToDateDownload()
    {
      return this.IsVersionUpToDateDownload(SyncScopesEnum.Users) && this.IsVersionUpToDateDownload(SyncScopesEnum.Configuration) && this.IsVersionUpToDateDownload(SyncScopesEnum.Application);
    }

    protected abstract bool IsVersionUpToDateDownload(SyncScopesEnum syncScopesEnum);

    public bool IsVersionUpToDateSend(bool IsPartialSync)
    {
      return IsPartialSync ? this.IsVersionUpToDateSend(SyncScopesEnum.Users) : this.IsVersionUpToDateSend(SyncScopesEnum.Application) && this.IsVersionUpToDateSend(SyncScopesEnum.Users) && this.IsVersionUpToDateSend(SyncScopesEnum.Configuration);
    }

    protected abstract bool IsVersionUpToDateSend(SyncScopesEnum syncScopesEnum);

    public abstract SynchronizationResults SynchronizeScope(
      SyncScopesEnum syncScope,
      SyncDirectionOrder syncDirection);
  }
}
