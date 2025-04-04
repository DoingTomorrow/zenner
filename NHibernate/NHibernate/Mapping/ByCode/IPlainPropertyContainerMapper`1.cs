// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IPlainPropertyContainerMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IPlainPropertyContainerMapper<TContainer> : 
    IBasePlainPropertyContainerMapper<TContainer>,
    IMinimalPlainPropertyContainerMapper<TContainer>
  {
    void OneToOne<TProperty>(
      Expression<Func<TContainer, TProperty>> property,
      Action<IOneToOneMapper> mapping)
      where TProperty : class;

    void OneToOne<TProperty>(string notVisiblePropertyOrFieldName, Action<IOneToOneMapper> mapping) where TProperty : class;
  }
}
