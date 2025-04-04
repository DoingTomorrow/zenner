// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Repository.OrdersRepository
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using MSS.Core.Model.Orders;
using NHibernate;

#nullable disable
namespace MSS.PartialSyncData.Repository
{
  public class OrdersRepository(ISession session) : MSS.PartialSyncData.Repository.Repository<Order>(session)
  {
  }
}
