// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GenericOrderedSetType`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class GenericOrderedSetType<T>(string role, string propertyRef) : GenericSetType<T>(role, propertyRef)
  {
    public override object Instantiate(int anticipatedSize) => (object) new OrderedSet<T>();
  }
}
