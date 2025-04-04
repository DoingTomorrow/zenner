// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Orders.ArchiveOrderMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Orders
{
  public class ArchiveOrderMap : ClassMap<ArchiveOrder>
  {
    public ArchiveOrderMap()
    {
      this.Table("t_Order");
      this.Id((Expression<Func<ArchiveOrder, object>>) (m => (object) m.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.Id));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => o.InstallationNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.RootStructureNodeId));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => o.Details)).Length(1000);
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.Exported));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.Status));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => o.DeviceNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => o.TestConfig));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.DueDate));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.IsDeactivated));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.OrderType));
      this.Map((Expression<Func<ArchiveOrder, object>>) (o => (object) o.CloseOrderReason));
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => (object) m.LockedBy)).Nullable();
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => m.StructureBytes)).Nullable().Length(int.MaxValue);
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => (object) m.StructureType)).Nullable();
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => (object) m.CreatedOn));
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => (object) m.LastUpdatedOn));
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => m.OrderRules));
      this.Map((Expression<Func<ArchiveOrder, object>>) (m => m.OrderUsers));
      this.References<ArchiveInformation>((Expression<Func<ArchiveOrder, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_Order_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
