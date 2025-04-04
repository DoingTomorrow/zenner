// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.Values.List`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using Iesi.Collections;
using Iesi.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Testing.Values
{
  public class List<T, TListElement> : Property<T, IEnumerable<TListElement>>
  {
    private readonly IEnumerable<TListElement> _expected;
    private Action<T, Accessor, IEnumerable<TListElement>> _valueSetter;

    public List(Accessor property, IEnumerable<TListElement> value)
      : base(property, value)
    {
      this._expected = value;
    }

    public override Action<T, Accessor, IEnumerable<TListElement>> ValueSetter
    {
      get
      {
        return this._valueSetter != null ? this._valueSetter : (Action<T, Accessor, IEnumerable<TListElement>>) ((target, propertyAccessor, value) =>
        {
          object obj;
          if (propertyAccessor.PropertyType.IsAssignableFrom(typeof (ISet<TListElement>)))
            obj = (object) new HashedSet<TListElement>((ICollection<TListElement>) this.Expected.ToList<TListElement>());
          else if (propertyAccessor.PropertyType.IsAssignableFrom(typeof (ISet)))
            obj = (object) new HashedSet((ICollection) this.Expected);
          else if (propertyAccessor.PropertyType.IsArray)
          {
            obj = (object) Array.CreateInstance(typeof (TListElement), this.Expected.Count<TListElement>());
            Array.Copy((Array) this.Expected, (Array) obj, this.Expected.Count<TListElement>());
          }
          else
            obj = (object) new List<TListElement>(this.Expected);
          propertyAccessor.SetValue((object) target, obj);
        });
      }
      set => this._valueSetter = value;
    }

    protected IEnumerable<TListElement> Expected => this._expected;

    public override void CheckValue(object target)
    {
      this.AssertGenericListMatches(this.PropertyAccessor.GetValue(target) as IEnumerable, this.Expected);
    }

    private void AssertGenericListMatches(
      IEnumerable actualEnumerable,
      IEnumerable<TListElement> expectedEnumerable)
    {
      if (actualEnumerable == null)
        throw new ArgumentNullException(nameof (actualEnumerable), "Actual and expected are not equal (actual was null).");
      if (expectedEnumerable == null)
        throw new ArgumentNullException(nameof (expectedEnumerable), "Actual and expected are not equal (expected was null).");
      List<object> objectList = new List<object>();
      foreach (object actual in actualEnumerable)
        objectList.Add(actual);
      List<TListElement> list = expectedEnumerable.ToList<TListElement>();
      if (objectList.Count != list.Count)
        throw new ApplicationException(string.Format("Actual count ({0}) does not equal expected count ({1})", (object) objectList.Count, (object) list.Count));
      Func<object, object, bool> func = this.EntityEqualityComparer != null ? (Func<object, object, bool>) ((a, b) => this.EntityEqualityComparer.Equals(a, b)) : new Func<object, object, bool>(object.Equals);
      for (int index = 0; index < objectList.Count; ++index)
      {
        if (!func(objectList[index], (object) list[index]))
          throw new ApplicationException(string.Format("Expected '{0}' but got '{1}' at position {2}", (object) list[index], objectList[index], (object) index));
      }
    }
  }
}
