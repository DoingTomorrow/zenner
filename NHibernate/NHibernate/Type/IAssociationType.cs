// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.IAssociationType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Type
{
  public interface IAssociationType : IType, ICacheAssembler
  {
    ForeignKeyDirection ForeignKeyDirection { get; }

    bool UseLHSPrimaryKey { get; }

    string LHSPropertyName { get; }

    string RHSUniqueKeyPropertyName { get; }

    IJoinable GetAssociatedJoinable(ISessionFactoryImplementor factory);

    string GetAssociatedEntityName(ISessionFactoryImplementor factory);

    bool IsAlwaysDirtyChecked { get; }

    string GetOnCondition(
      string alias,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters);

    bool IsEmbeddedInXML { get; }
  }
}
