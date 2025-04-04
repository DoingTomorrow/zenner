// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NestedSelects.Tuple
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.NestedSelects
{
  internal class Tuple : IEquatable<Tuple>
  {
    public static readonly ConstructorInfo Constructor = typeof (Tuple).GetConstructor(new Type[1]
    {
      typeof (object[])
    });
    public static readonly PropertyInfo ItemsProperty = typeof (Tuple).GetProperty(nameof (Items));
    private readonly object[] _items;

    public Tuple(object[] items)
    {
      this._items = items != null ? items : throw new ArgumentNullException(nameof (items));
    }

    public object[] Items => this._items;

    public bool Equals(Tuple other)
    {
      if (other == null)
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return other._items.Length == this._items.Length && ((IEnumerable<object>) this._items).SequenceEqual<object>((IEnumerable<object>) other._items);
    }

    public override bool Equals(object obj) => this.Equals(obj as Tuple);

    public override int GetHashCode()
    {
      int length = this._items.Length;
      if (length == 0)
        return 0;
      int num = length;
      object objA = this._items[0];
      return object.ReferenceEquals(objA, (object) null) ? num : objA.GetHashCode() * 397 ^ num;
    }
  }
}
