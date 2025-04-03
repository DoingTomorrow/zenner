// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.SynchronizationResults
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization;

#nullable disable
namespace MSS.Business.Modules.Synchronization
{
  public class SynchronizationResults
  {
    public SyncOperationStatistics Stats { get; set; }

    public string Message { get; set; }

    public SynchronizationResults(string message, SyncOperationStatistics stats)
    {
      this.Message = message;
      this.Stats = stats;
    }
  }
}
