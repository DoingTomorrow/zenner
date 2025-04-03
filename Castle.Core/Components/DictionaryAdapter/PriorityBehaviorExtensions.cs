// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.PriorityBehaviorExtensions
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  internal static class PriorityBehaviorExtensions
  {
    public static IEnumerable<T> Prioritize<T>(
      this IEnumerable<T> first,
      params IEnumerable<T>[] remaining)
      where T : IDictionaryBehavior
    {
      IEnumerable<T>[] array = ((IEnumerable<IEnumerable<T>>) new IEnumerable<T>[1]
      {
        first
      }).Union<IEnumerable<T>>((IEnumerable<IEnumerable<T>>) remaining).Where<IEnumerable<T>>((Func<IEnumerable<T>, bool>) (behaviors => behaviors != null && behaviors.Any<T>())).ToArray<IEnumerable<T>>();
      switch (array.Length)
      {
        case 0:
          return Enumerable.Empty<T>();
        case 1:
          return (IEnumerable<T>) array[0].OrderBy<T, int>((Func<T, int>) (behavior => behavior.ExecutionOrder));
        default:
          return ((IEnumerable<IEnumerable<T>>) array).SelectMany<IEnumerable<T>, PriorityBehaviorExtensions.PriorityBehavior<T>>((Func<IEnumerable<T>, int, IEnumerable<PriorityBehaviorExtensions.PriorityBehavior<T>>>) ((bs, priority) => bs.Select<T, PriorityBehaviorExtensions.PriorityBehavior<T>>((Func<T, PriorityBehaviorExtensions.PriorityBehavior<T>>) (b => new PriorityBehaviorExtensions.PriorityBehavior<T>(b, priority))))).OrderBy<PriorityBehaviorExtensions.PriorityBehavior<T>, PriorityBehaviorExtensions.PriorityBehavior<T>>((Func<PriorityBehaviorExtensions.PriorityBehavior<T>, PriorityBehaviorExtensions.PriorityBehavior<T>>) (priority => priority)).Select<PriorityBehaviorExtensions.PriorityBehavior<T>, T>((Func<PriorityBehaviorExtensions.PriorityBehavior<T>, T>) (priority => priority.Behavior));
      }
    }

    private class PriorityBehavior<T> : IComparable<PriorityBehaviorExtensions.PriorityBehavior<T>> where T : IDictionaryBehavior
    {
      public PriorityBehavior(T behavior, int priority)
      {
        this.Behavior = behavior;
        this.Priority = priority;
      }

      public T Behavior { get; private set; }

      public int Priority { get; private set; }

      public int CompareTo(
        PriorityBehaviorExtensions.PriorityBehavior<T> other)
      {
        int num = this.Behavior.ExecutionOrder - other.Behavior.ExecutionOrder;
        return num != 0 ? num : this.Priority - other.Priority;
      }
    }
  }
}
