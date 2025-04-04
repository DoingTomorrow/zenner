// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.ICriteriaQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  public interface ICriteriaQuery
  {
    ISessionFactoryImplementor Factory { get; }

    string GetColumn(ICriteria criteria, string propertyPath);

    string[] GetColumns(ICriteria criteria, string propertyPath);

    IType GetType(ICriteria criteria, string propertyPath);

    string[] GetColumnAliasesUsingProjection(ICriteria criteria, string propertyPath);

    string[] GetColumnsUsingProjection(ICriteria criteria, string propertyPath);

    IType GetTypeUsingProjection(ICriteria criteria, string propertyPath);

    TypedValue GetTypedValue(ICriteria criteria, string propertyPath, object value);

    string GetEntityName(ICriteria criteria);

    string GetEntityName(ICriteria criteria, string propertyPath);

    string GetSQLAlias(ICriteria subcriteria);

    string GetSQLAlias(ICriteria criteria, string propertyPath);

    string GetPropertyName(string propertyName);

    string[] GetIdentifierColumns(ICriteria subcriteria);

    IType GetIdentifierType(ICriteria subcriteria);

    TypedValue GetTypedIdentifierValue(ICriteria subcriteria, object value);

    string GenerateSQLAlias();

    int GetIndexForAlias();

    IEnumerable<Parameter> NewQueryParameter(TypedValue parameter);

    ICollection<IParameterSpecification> CollectedParameterSpecifications { get; }

    ICollection<NamedParameter> CollectedParameters { get; }

    Parameter CreateSkipParameter(int value);

    Parameter CreateTakeParameter(int value);
  }
}
