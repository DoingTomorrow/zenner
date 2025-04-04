// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.HistoricalSchedulerBase
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Concurrency
{
  public abstract class HistoricalSchedulerBase : VirtualTimeSchedulerBase<DateTimeOffset, TimeSpan>
  {
    protected HistoricalSchedulerBase()
      : base(DateTimeOffset.MinValue, (IComparer<DateTimeOffset>) System.Collections.Generic.Comparer<DateTimeOffset>.Default)
    {
    }

    protected HistoricalSchedulerBase(DateTimeOffset initialClock)
      : base(initialClock, (IComparer<DateTimeOffset>) System.Collections.Generic.Comparer<DateTimeOffset>.Default)
    {
    }

    protected HistoricalSchedulerBase(
      DateTimeOffset initialClock,
      IComparer<DateTimeOffset> comparer)
      : base(initialClock, comparer)
    {
    }

    protected override DateTimeOffset Add(DateTimeOffset absolute, TimeSpan relative)
    {
      return absolute.Add(relative);
    }

    protected override DateTimeOffset ToDateTimeOffset(DateTimeOffset absolute) => absolute;

    protected override TimeSpan ToRelative(TimeSpan timeSpan) => timeSpan;
  }
}
