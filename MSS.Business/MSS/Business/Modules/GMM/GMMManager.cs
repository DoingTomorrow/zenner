// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.GMMManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Orders;
using MSS.Interfaces;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class GMMManager
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Rules> _rulesRepository;

    public GMMManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._orderRepository = this._repositoryFactory.GetRepository<Order>();
      this._rulesRepository = this._repositoryFactory.GetRepository<Rules>();
    }

    public List<long> GetFilterListForOrder(Guid selectedOrderId)
    {
      List<long> filter = (List<long>) null;
      Order byId = this._orderRepository.GetById((object) selectedOrderId);
      if (byId != null)
      {
        MSS.Core.Model.DataFilters.Filter orderFilter = byId.Filter;
        IList<Rules> rulesList = this._rulesRepository.SearchFor((Expression<Func<Rules, bool>>) (r => r.Filter == orderFilter));
        if (rulesList != null && rulesList.Count > 0)
        {
          filter = new List<long>();
          TypeHelperExtensionMethods.ForEach<Rules>((IEnumerable<Rules>) rulesList, (Action<Rules>) (r => filter.Add(Convert.ToInt64(r.ValueId))));
        }
      }
      return filter;
    }
  }
}
