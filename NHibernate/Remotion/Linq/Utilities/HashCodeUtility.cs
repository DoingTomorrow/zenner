// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.HashCodeUtility
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Remotion.Linq.Utilities
{
  public static class HashCodeUtility
  {
    public static int GetHashCodeOrZero(object valueOrNull)
    {
      return valueOrNull == null ? 0 : valueOrNull.GetHashCode();
    }

    public static int GetHashCodeForSequence<T>(IEnumerable<T> sequence)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<T>>(nameof (sequence), sequence);
      return sequence.Aggregate<T, int>(0, (Func<int, T, int>) ((totalHashCode, item) => totalHashCode ^ HashCodeUtility.GetHashCodeOrZero((object) item)));
    }
  }
}
