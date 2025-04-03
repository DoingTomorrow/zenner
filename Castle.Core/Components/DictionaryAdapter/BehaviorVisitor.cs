// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.BehaviorVisitor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class BehaviorVisitor
  {
    private List<KeyValuePair<Func<object, bool>, Func<object, bool>>> conditions;

    public BehaviorVisitor()
    {
      this.conditions = new List<KeyValuePair<Func<object, bool>, Func<object, bool>>>();
    }

    public BehaviorVisitor OfType<T>(Func<T, bool> apply)
    {
      return this.AddCondition((Func<object, bool>) (b => b is T), (Func<object, bool>) (b => apply((T) b)));
    }

    public BehaviorVisitor OfType<T>(Action<T> apply)
    {
      return this.AddCondition((Func<object, bool>) (b => b is T), (Func<object, bool>) (b =>
      {
        apply((T) b);
        return true;
      }));
    }

    public BehaviorVisitor Match<T>(Func<T, bool> match, Func<T, bool> apply)
    {
      return this.AddCondition((Func<object, bool>) (b => b is T obj && match(obj)), (Func<object, bool>) (b => apply((T) b)));
    }

    public BehaviorVisitor Match<T>(Func<T, bool> match, Action<T> apply)
    {
      return this.AddCondition((Func<object, bool>) (b => b is T obj && match(obj)), (Func<object, bool>) (b =>
      {
        apply((T) b);
        return true;
      }));
    }

    public BehaviorVisitor Match(Func<object, bool> match, Func<object, bool> apply)
    {
      return this.Match<object>(match, apply);
    }

    public BehaviorVisitor Match(Func<object, bool> match, Action<object> apply)
    {
      return this.Match<object>(match, apply);
    }

    public void Apply(IEnumerable behaviors)
    {
      foreach (object behavior in behaviors)
      {
        foreach (KeyValuePair<Func<object, bool>, Func<object, bool>> condition in this.conditions)
        {
          if (condition.Key(behavior))
          {
            int num = condition.Value(behavior) ? 1 : 0;
          }
        }
      }
    }

    private BehaviorVisitor AddCondition(Func<object, bool> predicate, Func<object, bool> action)
    {
      this.conditions.Add(new KeyValuePair<Func<object, bool>, Func<object, bool>>(predicate, action));
      return this;
    }
  }
}
