// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.SafetyEnumerable`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public class SafetyEnumerable<T> : IEnumerable<T>, IEnumerable
  {
    private readonly IEnumerable collection;

    public SafetyEnumerable(IEnumerable collection) => this.collection = collection;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      foreach (object element in this.collection)
      {
        if (element == null)
          yield return default (T);
        else if (typeof (T).IsAssignableFrom(element.GetType()))
          yield return (T) element;
      }
    }

    public IEnumerator GetEnumerator() => (IEnumerator) ((IEnumerable<T>) this).GetEnumerator();
  }
}
