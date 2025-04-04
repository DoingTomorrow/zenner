// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.ExpressionTreeSerializer
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using ExpressionSerialization;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Structures;
using MSS.PartialSync.PartialSyncProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace MSS.PartialSync
{
  public static class ExpressionTreeSerializer
  {
    public static ExpressionSerializer CreateSerializer()
    {
      TypeResolver resolver = new TypeResolver((IEnumerable<Assembly>) new Assembly[5]
      {
        typeof (OrderUser).Assembly,
        typeof (ExpressionType).Assembly,
        typeof (IQueryable).Assembly,
        typeof (MSS.Business.Utils.AppContext).Assembly,
        typeof (PartialOrderSyncProvider<Order>).Assembly
      }, (IEnumerable<Type>) new Type[7]
      {
        typeof (OrderUser),
        typeof (User),
        typeof (Order),
        typeof (XElement),
        typeof (MSS.Business.Utils.AppContext),
        typeof (PartialOrderSyncProvider<Order>),
        typeof (Guid)
      });
      CustomExpressionXmlConverter expressionXmlConverter1 = (CustomExpressionXmlConverter) new QueryExpressionXmlConverter(resolver: resolver);
      CustomExpressionXmlConverter expressionXmlConverter2 = (CustomExpressionXmlConverter) new KnownTypeExpressionXmlConverter(resolver);
      return new ExpressionSerializer(resolver, (IEnumerable<CustomExpressionXmlConverter>) new CustomExpressionXmlConverter[2]
      {
        expressionXmlConverter1,
        expressionXmlConverter2
      });
    }

    public static Structure InitStructure(this Structure item)
    {
      item.Nodes = new List<StructureNode>();
      item.Locations = new List<Location>();
      item.Meters = new List<Meter>();
      item.Minomats = new List<Minomat>();
      item.Tenants = new List<Tenant>();
      return item;
    }
  }
}
