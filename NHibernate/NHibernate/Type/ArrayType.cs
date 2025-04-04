// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ArrayType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class ArrayType : CollectionType
  {
    private readonly System.Type elementClass;
    private readonly System.Type arrayClass;

    public ArrayType(string role, string propertyRef, System.Type elementClass, bool isEmbeddedInXML)
      : base(role, propertyRef, isEmbeddedInXML)
    {
      this.elementClass = elementClass;
      this.arrayClass = Array.CreateInstance(elementClass, 0).GetType();
    }

    public override System.Type ReturnedClass => this.arrayClass;

    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentArrayHolder(session, persister);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      ISessionImplementor session)
    {
      base.NullSafeSet(st, (object) session.PersistenceContext.GetCollectionHolder(value), index, session);
    }

    public override IEnumerable GetElementsIterator(object collection)
    {
      return (IEnumerable) (Array) collection;
    }

    public override IPersistentCollection Wrap(ISessionImplementor session, object array)
    {
      return (IPersistentCollection) new PersistentArrayHolder(session, array);
    }

    public override bool IsArrayType => true;

    public override object InstantiateResult(object original)
    {
      return (object) Array.CreateInstance(this.elementClass, ((Array) original).Length);
    }

    public override object Instantiate(int anticipatedSize) => throw new NotSupportedException();

    public override bool HasHolder(EntityMode entityMode) => true;

    public override object IndexOf(object collection, object element)
    {
      Array array = (Array) collection;
      int length = array.Length;
      for (int index = 0; index < length; ++index)
      {
        if (array.GetValue(index) == element)
          return (object) index;
      }
      return (object) null;
    }

    protected internal override bool InitializeImmediately(EntityMode entityMode) => true;

    public override object ReplaceElements(
      object original,
      object target,
      object owner,
      IDictionary copyCache,
      ISessionImplementor session)
    {
      Array array1 = (Array) original;
      Array array2 = (Array) target;
      int length = array1.Length;
      if (length != array2.Length)
        array2 = (Array) this.InstantiateResult(original);
      IType elementType = this.GetElementType(session.Factory);
      for (int index = 0; index < length; ++index)
        array2.SetValue(elementType.Replace(array1.GetValue(index), (object) null, session, owner, copyCache), index);
      return (object) array2;
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      if (value == null)
        return "null";
      Array array = (Array) value;
      int length = array.Length;
      IList list = (IList) new ArrayList(length);
      IType elementType = this.GetElementType(factory);
      for (int index = 0; index < length; ++index)
        list.Add((object) elementType.ToLoggableString(array.GetValue(index), factory));
      return CollectionPrinter.ToString(list);
    }
  }
}
