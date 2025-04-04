// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.IVersionType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections;

#nullable disable
namespace NHibernate.Type
{
  public interface IVersionType : IType, ICacheAssembler
  {
    object Next(object current, ISessionImplementor session);

    object Seed(ISessionImplementor session);

    bool IsEqual(object x, object y);

    IComparer Comparator { get; }

    object FromStringValue(string xml);
  }
}
