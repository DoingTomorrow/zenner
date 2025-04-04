// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Planner
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Ninject.Planning
{
  public class Planner : NinjectComponent, IPlanner, INinjectComponent, IDisposable
  {
    private readonly ReaderWriterLock plannerLock = new ReaderWriterLock();
    private readonly Dictionary<Type, IPlan> plans = new Dictionary<Type, IPlan>();

    public Planner(IEnumerable<IPlanningStrategy> strategies)
    {
      Ensure.ArgumentNotNull((object) strategies, nameof (strategies));
      this.Strategies = (IList<IPlanningStrategy>) strategies.ToList<IPlanningStrategy>();
    }

    public IList<IPlanningStrategy> Strategies { get; private set; }

    public IPlan GetPlan(Type type)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      this.plannerLock.AcquireReaderLock(-1);
      try
      {
        IPlan plan;
        return this.plans.TryGetValue(type, out plan) ? plan : this.CreateNewPlan(type);
      }
      finally
      {
        this.plannerLock.ReleaseReaderLock();
      }
    }

    protected virtual IPlan CreateEmptyPlan(Type type)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      return (IPlan) new Plan(type);
    }

    private IPlan CreateNewPlan(Type type)
    {
      LockCookie writerLock = this.plannerLock.UpgradeToWriterLock(-1);
      try
      {
        IPlan plan;
        if (this.plans.TryGetValue(type, out plan))
          return plan;
        plan = this.CreateEmptyPlan(type);
        this.plans.Add(type, plan);
        this.Strategies.Map<IPlanningStrategy>((Action<IPlanningStrategy>) (s => s.Execute(plan)));
        return plan;
      }
      finally
      {
        this.plannerLock.DowngradeFromWriterLock(ref writerLock);
      }
    }
  }
}
