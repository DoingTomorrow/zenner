// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.ITuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Tuple
{
  public interface ITuplizer
  {
    Type MappedClass { get; }

    object[] GetPropertyValues(object entity);

    void SetPropertyValues(object entity, object[] values);

    object GetPropertyValue(object entity, int i);

    object Instantiate();

    bool IsInstance(object obj);
  }
}
