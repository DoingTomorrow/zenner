// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.OrdersManagement.OrderResult
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Modules.OrdersManagement
{
  public class OrderResult
  {
    public List<Order> Orders { get; set; }

    public List<Location> Locations { get; set; }

    public List<Tenant> Tenants { get; set; }

    public List<Meter> Meters { get; set; }

    public List<MSS.Core.Model.Structures.StructureNodeLinks> StructureNodeLinks { get; set; }

    public List<StructureNode> StructureNodes { get; set; }
  }
}
