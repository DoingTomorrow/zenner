// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.DelayedPostInsertIdentifier
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public class DelayedPostInsertIdentifier
  {
    [ThreadStatic]
    private static long _Sequence;
    private readonly long sequence;

    public DelayedPostInsertIdentifier()
    {
      lock (typeof (DelayedPostInsertIdentifier))
      {
        if (DelayedPostInsertIdentifier._Sequence == long.MaxValue)
          DelayedPostInsertIdentifier._Sequence = 0L;
        this.sequence = DelayedPostInsertIdentifier._Sequence++;
      }
    }

    public override bool Equals(object obj)
    {
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as DelayedPostInsertIdentifier);
    }

    public bool Equals(DelayedPostInsertIdentifier that)
    {
      return that != null && this.sequence == that.sequence;
    }

    public override int GetHashCode() => this.sequence.GetHashCode();

    public override string ToString() => string.Format("<delayed:{0}>", (object) this.sequence);
  }
}
