// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.HandleConflicts.ConflictDetails
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.Data;

#nullable disable
namespace MSS.Business.Modules.Synchronization.HandleConflicts
{
  public class ConflictDetails
  {
    public DbSyncConflict ConflictInfo { get; set; }

    public bool DestinationWins { get; set; }
  }
}
