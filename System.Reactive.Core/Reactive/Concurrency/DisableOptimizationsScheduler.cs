// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DisableOptimizationsScheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal class DisableOptimizationsScheduler : SchedulerWrapper
  {
    private readonly Type[] _optimizationInterfaces;

    public DisableOptimizationsScheduler(IScheduler scheduler)
      : base(scheduler)
    {
      this._optimizationInterfaces = Scheduler.OPTIMIZATIONS;
    }

    public DisableOptimizationsScheduler(IScheduler scheduler, Type[] optimizationInterfaces)
      : base(scheduler)
    {
      this._optimizationInterfaces = optimizationInterfaces;
    }

    public DisableOptimizationsScheduler(
      IScheduler scheduler,
      Type[] optimizationInterfaces,
      ConditionalWeakTable<IScheduler, IScheduler> cache)
      : base(scheduler, cache)
    {
      this._optimizationInterfaces = optimizationInterfaces;
    }

    protected override SchedulerWrapper Clone(
      IScheduler scheduler,
      ConditionalWeakTable<IScheduler, IScheduler> cache)
    {
      return (SchedulerWrapper) new DisableOptimizationsScheduler(scheduler, this._optimizationInterfaces, cache);
    }

    protected override bool TryGetService(
      IServiceProvider provider,
      Type serviceType,
      out object service)
    {
      service = (object) null;
      return ((IEnumerable<Type>) this._optimizationInterfaces).Contains<Type>(serviceType);
    }
  }
}
