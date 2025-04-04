// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.Sql.SQLQueryParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Loader.Custom.Sql
{
  public class SQLQueryParser
  {
    private readonly ISessionFactoryImplementor factory;
    private readonly string originalQueryString;
    private readonly SQLQueryParser.IParserContext context;
    private long aliasesFound;
    private IEnumerable<IParameterSpecification> parametersSpecifications;

    public SQLQueryParser(
      ISessionFactoryImplementor factory,
      string sqlQuery,
      SQLQueryParser.IParserContext context)
    {
      this.factory = factory;
      this.originalQueryString = sqlQuery;
      this.context = context;
    }

    public bool QueryHasAliases => this.aliasesFound > 0L;

    public SqlString Process() => this.SubstituteParams(this.SubstituteBrackets());

    public IEnumerable<IParameterSpecification> CollectedParametersSpecifications
    {
      get => this.parametersSpecifications;
    }

    private string SubstituteBrackets()
    {
      StringBuilder stringBuilder = new StringBuilder(this.originalQueryString.Length + 20);
      int num1;
      for (int startIndex = 0; startIndex < this.originalQueryString.Length; startIndex = num1 + 1)
      {
        int num2;
        if ((num2 = this.originalQueryString.IndexOf('{', startIndex)) < 0)
        {
          stringBuilder.Append(this.originalQueryString.Substring(startIndex));
          break;
        }
        stringBuilder.Append(this.originalQueryString.Substring(startIndex, num2 - startIndex));
        if ((num1 = this.originalQueryString.IndexOf('}', num2 + 1)) < 0)
          throw new QueryException("Unmatched braces for alias path", this.originalQueryString);
        string aliasName1 = this.originalQueryString.Substring(num2 + 1, num1 - (num2 + 1));
        int length = aliasName1.IndexOf('.');
        if (length == -1)
        {
          if (this.context.IsEntityAlias(aliasName1))
          {
            stringBuilder.Append(aliasName1);
            ++this.aliasesFound;
          }
          else
            stringBuilder.Append('{').Append(aliasName1).Append('}');
        }
        else
        {
          string aliasName2 = aliasName1.Substring(0, length);
          bool flag1 = this.context.IsCollectionAlias(aliasName2);
          bool flag2 = this.context.IsEntityAlias(aliasName2);
          if (flag1)
          {
            string propertyName = aliasName1.Substring(length + 1);
            stringBuilder.Append(this.ResolveCollectionProperties(aliasName2, propertyName));
            ++this.aliasesFound;
          }
          else if (flag2)
          {
            string propertyName = aliasName1.Substring(length + 1);
            stringBuilder.Append(this.ResolveProperties(aliasName2, propertyName));
            ++this.aliasesFound;
          }
          else
            stringBuilder.Append('{').Append(aliasName1).Append('}');
        }
      }
      return stringBuilder.ToString();
    }

    private string ResolveCollectionProperties(string aliasName, string propertyName)
    {
      IDictionary<string, string[]> resultsMapByAlias = this.context.GetPropertyResultsMapByAlias(aliasName);
      ISqlLoadableCollection persisterByAlias = this.context.GetCollectionPersisterByAlias(aliasName);
      string collectionSuffixByAlias = this.context.GetCollectionSuffixByAlias(aliasName);
      if ("*".Equals(propertyName))
      {
        if (resultsMapByAlias.Count != 0)
          throw new QueryException("Using return-propertys together with * syntax is not supported.");
        string str = persisterByAlias.SelectFragment(aliasName, collectionSuffixByAlias);
        ++this.aliasesFound;
        return !persisterByAlias.ElementType.IsEntityType ? str : str + ", " + this.ResolveProperties(aliasName, "*");
      }
      if (propertyName.StartsWith("element."))
      {
        string propertyName1 = propertyName.Substring("element.".Length);
        if (persisterByAlias.ElementType.IsEntityType)
          return this.ResolveProperties(aliasName, propertyName1);
        if (propertyName1 == "*")
          throw new QueryException("Using element.* syntax is only supported for entity elements.");
      }
      string[] propertyColumnAliases;
      if (!resultsMapByAlias.TryGetValue(propertyName, out propertyColumnAliases))
        propertyColumnAliases = persisterByAlias.GetCollectionPropertyColumnAliases(propertyName, collectionSuffixByAlias);
      if (propertyColumnAliases == null || propertyColumnAliases.Length == 0)
        throw new QueryException("No column name found for property [" + propertyName + "] for alias [" + aliasName + "]", this.originalQueryString);
      if (propertyColumnAliases.Length != 1)
        throw new QueryException("SQL queries only support properties mapped to a single column - property [" + propertyName + "] is mapped to " + (object) propertyColumnAliases.Length + " columns.", this.originalQueryString);
      ++this.aliasesFound;
      return propertyColumnAliases[0];
    }

    private string ResolveProperties(string aliasName, string propertyName)
    {
      IDictionary<string, string[]> resultsMapByAlias = this.context.GetPropertyResultsMapByAlias(aliasName);
      ISqlLoadable persisterByAlias = this.context.GetEntityPersisterByAlias(aliasName);
      string entitySuffixByAlias = this.context.GetEntitySuffixByAlias(aliasName);
      if ("*".Equals(propertyName))
      {
        if (resultsMapByAlias.Count != 0)
          throw new QueryException("Using return-propertys together with * syntax is not supported.");
        ++this.aliasesFound;
        return persisterByAlias.SelectFragment(aliasName, entitySuffixByAlias);
      }
      string[] propertyColumnAliases;
      if (!resultsMapByAlias.TryGetValue(propertyName, out propertyColumnAliases))
        propertyColumnAliases = persisterByAlias.GetSubclassPropertyColumnAliases(propertyName, entitySuffixByAlias);
      if (propertyColumnAliases == null || propertyColumnAliases.Length == 0)
        throw new QueryException("No column name found for property [" + propertyName + "] for alias [" + aliasName + "]", this.originalQueryString);
      if (propertyColumnAliases.Length != 1)
        throw new QueryException("SQL queries only support properties mapped to a single column - property [" + propertyName + "] is mapped to " + (object) propertyColumnAliases.Length + " columns.", this.originalQueryString);
      ++this.aliasesFound;
      return propertyColumnAliases[0];
    }

    private SqlString SubstituteParams(string sqlString)
    {
      SQLQueryParser.ParameterSubstitutionRecognizer substitutionRecognizer = new SQLQueryParser.ParameterSubstitutionRecognizer(this.factory);
      ParameterParser.Parse(sqlString, (ParameterParser.IRecognizer) substitutionRecognizer);
      this.parametersSpecifications = (IEnumerable<IParameterSpecification>) substitutionRecognizer.ParametersSpecifications.ToList<IParameterSpecification>();
      return substitutionRecognizer.result.ToSqlString();
    }

    public interface IParserContext
    {
      bool IsEntityAlias(string aliasName);

      ISqlLoadable GetEntityPersisterByAlias(string alias);

      string GetEntitySuffixByAlias(string alias);

      bool IsCollectionAlias(string aliasName);

      ISqlLoadableCollection GetCollectionPersisterByAlias(string alias);

      string GetCollectionSuffixByAlias(string alias);

      IDictionary<string, string[]> GetPropertyResultsMapByAlias(string alias);
    }

    public class ParameterSubstitutionRecognizer : ParameterParser.IRecognizer
    {
      private readonly ISessionFactoryImplementor factory;
      internal SqlStringBuilder result = new SqlStringBuilder();
      internal int parameterCount;
      private readonly List<IParameterSpecification> parametersSpecifications = new List<IParameterSpecification>();
      private int positionalParameterCount;

      public ParameterSubstitutionRecognizer(ISessionFactoryImplementor factory)
      {
        this.factory = factory;
      }

      public IEnumerable<IParameterSpecification> ParametersSpecifications
      {
        get => (IEnumerable<IParameterSpecification>) this.parametersSpecifications;
      }

      public void OutParameter(int position)
      {
        PositionalParameterSpecification parameterSpecification = new PositionalParameterSpecification(1, position, this.positionalParameterCount++);
        Parameter placeholder = Parameter.Placeholder;
        placeholder.BackTrack = (object) parameterSpecification.GetIdsForBackTrack((IMapping) this.factory).First<string>();
        this.parametersSpecifications.Add((IParameterSpecification) parameterSpecification);
        this.result.Add(placeholder);
      }

      public void OrdinalParameter(int position)
      {
        PositionalParameterSpecification parameterSpecification = new PositionalParameterSpecification(1, position, this.positionalParameterCount++);
        Parameter placeholder = Parameter.Placeholder;
        placeholder.BackTrack = (object) parameterSpecification.GetIdsForBackTrack((IMapping) this.factory).First<string>();
        this.parametersSpecifications.Add((IParameterSpecification) parameterSpecification);
        this.result.Add(placeholder);
      }

      public void NamedParameter(string name, int position)
      {
        NamedParameterSpecification parameterSpecification = new NamedParameterSpecification(1, position, name);
        Parameter placeholder = Parameter.Placeholder;
        placeholder.BackTrack = (object) parameterSpecification.GetIdsForBackTrack((IMapping) this.factory).First<string>();
        this.parametersSpecifications.Add((IParameterSpecification) parameterSpecification);
        this.result.Add(placeholder);
      }

      public void JpaPositionalParameter(string name, int position)
      {
        this.NamedParameter(name, position);
      }

      public void Other(char character) => this.result.Add(character.ToString());

      public void Other(string sqlPart) => this.result.Add(sqlPart);
    }
  }
}
