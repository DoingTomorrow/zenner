// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.Virtualization.ServerOperationProcessor`1
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Common.Library.NHibernate.Data.Extensions;
using MSS.DTO.Users;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.Utils.Virtualization
{
  public static class ServerOperationProcessor<T> where T : class
  {
    public static IEnumerable<T> ProcessQueryByCommand(
      IRepository<T> repository,
      int startIndex,
      int pageSize,
      VirtualQueryableCollectionView<UserDTO> collection,
      Expression<Func<T, bool>> condition,
      out int totalCount)
    {
      IEnumerable<OrderClauseInfo> orderClauses = ServerOperationProcessor<T>.ProcessOrderConditions((IList<ISortDescriptor>) collection.SortDescriptors);
      string expressionAsString = collection.FilterDescriptors.CreateLambdaExpressionAsString();
      return (IEnumerable<T>) repository.SearchFor_ByPage(condition, orderClauses, expressionAsString, startIndex, pageSize, out totalCount);
    }

    public static IEnumerable<OrderClauseInfo> ProcessOrderConditions(
      IList<ISortDescriptor> sortDescriptors)
    {
      return (IEnumerable<OrderClauseInfo>) sortDescriptors.Select<ISortDescriptor, OrderClauseInfo>((Func<ISortDescriptor, OrderClauseInfo>) (descriptor =>
      {
        OrderClauseInfo orderClauseInfo = new OrderClauseInfo();
        ref OrderClauseInfo local = ref orderClauseInfo;
        string str;
        switch (descriptor)
        {
          case SortDescriptor _:
            str = ((SortDescriptor) descriptor).Member;
            break;
          case ColumnSortDescriptor columnSortDescriptor2:
            str = columnSortDescriptor2.Column.UniqueName;
            break;
          default:
            str = (string) null;
            break;
        }
        local.PropertyName = str;
        orderClauseInfo.Direction = descriptor.SortDirection == ListSortDirection.Ascending ? OrderDirection.Asc : OrderDirection.Desc;
        return orderClauseInfo;
      })).ToList<OrderClauseInfo>();
    }
  }
}
