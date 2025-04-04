// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSyncProviders.ZippedOrder
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Core.Model.Orders;

#nullable disable
namespace MSS.PartialSync.PartialSyncProviders
{
  public class ZippedOrder
  {
    public Order Order { get; set; }

    public OrderUser OrderUser { get; set; }
  }
}
