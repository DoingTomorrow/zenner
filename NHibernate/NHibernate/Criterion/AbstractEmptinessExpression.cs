// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.AbstractEmptinessExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class AbstractEmptinessExpression : AbstractCriterion
  {
    private readonly TypedValue[] NO_VALUES = new TypedValue[0];
    private readonly string propertyName;

    protected abstract bool ExcludeEmpty { get; }

    protected AbstractEmptinessExpression(string propertyName) => this.propertyName = propertyName;

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.NO_VALUES;
    }

    public override sealed string ToString()
    {
      return this.propertyName + (this.ExcludeEmpty ? " is not empty" : " is empty");
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      string entityName = criteriaQuery.GetEntityName(criteria, this.propertyName);
      string propertyName = criteriaQuery.GetPropertyName(this.propertyName);
      string sqlAlias = criteriaQuery.GetSQLAlias(criteria, this.propertyName);
      ISessionFactoryImplementor factory = criteriaQuery.Factory;
      IQueryableCollection queryableCollection = this.GetQueryableCollection(entityName, propertyName, factory);
      string[] keyColumnNames = queryableCollection.KeyColumnNames;
      string[] identifierColumnNames = ((ILoadable) factory.GetEntityPersister(entityName)).IdentifierColumnNames;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("(select 1 from ").Append(queryableCollection.TableName).Append(" where ").Append((object) new ConditionalFragment().SetTableAlias(sqlAlias).SetCondition(identifierColumnNames, keyColumnNames).ToSqlStringFragment());
      if (queryableCollection.HasWhere)
        stringBuilder.Append(" and (").Append(queryableCollection.GetSQLWhereString(queryableCollection.TableName)).Append(") ");
      stringBuilder.Append(")");
      return new SqlString((object[]) new string[2]
      {
        this.ExcludeEmpty ? "exists" : "not exists",
        stringBuilder.ToString()
      });
    }

    protected IQueryableCollection GetQueryableCollection(
      string entityName,
      string actualPropertyName,
      ISessionFactoryImplementor factory)
    {
      IType type = ((IPropertyMapping) factory.GetEntityPersister(entityName)).ToType(actualPropertyName);
      string role = type.IsCollectionType ? ((CollectionType) type).Role : throw new MappingException("Property path [" + entityName + "." + actualPropertyName + "] does not reference a collection");
      try
      {
        return (IQueryableCollection) factory.GetCollectionPersister(role);
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException("collection role is not queryable: " + role, (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new QueryException("collection role not found: " + role, ex);
      }
    }
  }
}
