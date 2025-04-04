// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.ISqlCommand
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.SqlCommand
{
  public interface ISqlCommand
  {
    SqlType[] ParameterTypes { get; }

    SqlString Query { get; }

    QueryParameters QueryParameters { get; }

    void ResetParametersIndexesForTheCommand(int singleSqlParametersOffset);

    void Bind(
      IDbCommand command,
      IList<Parameter> commandQueryParametersList,
      int singleSqlParametersOffset,
      ISessionImplementor session);

    void Bind(IDbCommand command, ISessionImplementor session);
  }
}
