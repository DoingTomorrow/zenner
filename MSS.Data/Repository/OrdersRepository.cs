// Decompiled with JetBrains decompiler
// Type: MSS.Data.Repository.OrdersRepository
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using MSS.Core.Model.Orders;
using NHibernate;

#nullable disable
namespace MSS.Data.Repository
{
  public class OrdersRepository(ISession session) : MSS.Data.Repository.Repository<Order>(session)
  {
  }
}
