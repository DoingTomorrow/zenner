// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.SortedMapType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class SortedMapType : MapType
  {
    private readonly IComparer comparer;

    public SortedMapType(
      string role,
      string propertyRef,
      IComparer comparer,
      bool isEmbeddedInXML)
      : base(role, propertyRef, isEmbeddedInXML)
    {
      this.comparer = comparer;
    }

    public IComparer Comparer => this.comparer;

    public override object Instantiate(int anticipatedSize)
    {
      return (object) new SortedList(this.comparer);
    }

    public override System.Type ReturnedClass => typeof (SortedList);
  }
}
