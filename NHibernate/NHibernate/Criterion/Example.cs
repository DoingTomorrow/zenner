// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Example
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class Example : AbstractCriterion
  {
    private readonly object _entity;
    private readonly ISet<string> _excludedProperties = (ISet<string>) new HashedSet<string>();
    private Example.IPropertySelector _selector;
    private bool _isLikeEnabled;
    private char? escapeCharacter;
    private bool _isIgnoreCaseEnabled;
    private MatchMode _matchMode;
    protected static readonly Example.IPropertySelector NotNullOrEmptyString = (Example.IPropertySelector) new Example.NotNullOrEmptyStringPropertySelector();
    protected static readonly Example.IPropertySelector All = (Example.IPropertySelector) new Example.AllPropertySelector();
    protected static readonly Example.IPropertySelector NotNullOrZero = (Example.IPropertySelector) new Example.NotNullOrZeroPropertySelector();

    public virtual Example SetEscapeCharacter(char? escapeCharacter)
    {
      this.escapeCharacter = escapeCharacter;
      return this;
    }

    public Example SetPropertySelector(Example.IPropertySelector selector)
    {
      this._selector = selector;
      return this;
    }

    public Example ExcludeZeroes() => this.SetPropertySelector(Example.NotNullOrZero);

    public Example ExcludeNone()
    {
      this.SetPropertySelector(Example.All);
      return this;
    }

    public Example ExcludeNulls()
    {
      this.SetPropertySelector(Example.NotNullOrEmptyString);
      return this;
    }

    public Example EnableLike(MatchMode matchMode)
    {
      this._isLikeEnabled = true;
      this._matchMode = matchMode;
      return this;
    }

    public Example EnableLike() => this.EnableLike(MatchMode.Exact);

    public Example IgnoreCase()
    {
      this._isIgnoreCaseEnabled = true;
      return this;
    }

    public Example ExcludeProperty(string name)
    {
      this._excludedProperties.Add(name);
      return this;
    }

    public static Example Create(object entity)
    {
      return entity != null ? new Example(entity, Example.NotNullOrEmptyString) : throw new ArgumentNullException(nameof (entity), "null example");
    }

    protected Example(object entity, Example.IPropertySelector selector)
    {
      this._entity = entity;
      this._selector = selector;
    }

    public override string ToString() => "example (" + this._entity + (object) ')';

    private bool IsPropertyIncluded(object value, string name, IType type)
    {
      return !this._excludedProperties.Contains(name) && !type.IsAssociationType && this._selector.Include(value, name, type);
    }

    private object[] GetPropertyValues(
      IEntityPersister persister,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      System.Type type = this._entity.GetType();
      if (type == persister.GetMappedClass(this.GetEntityMode(criteria, criteriaQuery)))
        return persister.GetPropertyValues(this._entity, this.GetEntityMode(criteria, criteriaQuery));
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < persister.PropertyNames.Length; ++index)
      {
        PropertyInfo property = type.GetProperty(persister.PropertyNames[index]);
        if (property != null)
        {
          arrayList.Add(property.GetValue(this._entity, (object[]) null));
        }
        else
        {
          arrayList.Add((object) null);
          this._excludedProperties.Add(persister.PropertyNames[index]);
        }
      }
      return arrayList.ToArray();
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder builder = new SqlStringBuilder();
      builder.Add("(");
      IEntityPersister entityPersister = criteriaQuery.Factory.GetEntityPersister(criteriaQuery.GetEntityName(criteria));
      string[] propertyNames = entityPersister.PropertyNames;
      IType[] propertyTypes = entityPersister.PropertyTypes;
      object[] propertyValues = this.GetPropertyValues(entityPersister, criteria, criteriaQuery);
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        object obj = propertyValues[index];
        string str = propertyNames[index];
        if (index != entityPersister.VersionProperty && this.IsPropertyIncluded(obj, str, propertyTypes[index]))
        {
          if (propertyTypes[index].IsComponentType)
            this.AppendComponentCondition(str, obj, (IAbstractComponentType) propertyTypes[index], criteria, criteriaQuery, enabledFilters, builder);
          else
            this.AppendPropertyCondition(str, obj, criteria, criteriaQuery, enabledFilters, builder);
        }
      }
      if (builder.Count == 1)
        builder.Add("1=1");
      builder.Add(")");
      return builder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      IEntityPersister entityPersister = criteriaQuery.Factory.GetEntityPersister(criteriaQuery.GetEntityName(criteria));
      string[] propertyNames = entityPersister.PropertyNames;
      IType[] propertyTypes = entityPersister.PropertyTypes;
      object[] propertyValues = this.GetPropertyValues(entityPersister, criteria, criteriaQuery);
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        object component = propertyValues[index];
        IType type = propertyTypes[index];
        string str = propertyNames[index];
        if (index != entityPersister.VersionProperty && this.IsPropertyIncluded(component, str, type))
        {
          if (propertyTypes[index].IsComponentType)
            this.AddComponentTypedValues(str, component, (IAbstractComponentType) type, (IList) arrayList, criteria, criteriaQuery);
          else
            this.AddPropertyTypedValue(component, type, (IList) arrayList);
        }
      }
      return (TypedValue[]) arrayList.ToArray(typeof (TypedValue));
    }

    public override IProjection[] GetProjections() => (IProjection[]) null;

    private EntityMode GetEntityMode(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      EntityMode? nullable = criteriaQuery.Factory.GetEntityPersister(criteriaQuery.GetEntityName(criteria)).GuessEntityMode(this._entity);
      return !nullable.HasValue ? EntityMode.Poco : nullable.Value;
    }

    protected void AddPropertyTypedValue(object value, IType type, IList list)
    {
      if (value == null)
        return;
      if (value is string pattern)
      {
        if (this._isIgnoreCaseEnabled)
          pattern = pattern.ToLower();
        if (this._isLikeEnabled)
          pattern = this._matchMode.ToMatchString(pattern);
        value = (object) pattern;
      }
      list.Add((object) new TypedValue(type, value, EntityMode.Poco));
    }

    protected void AddComponentTypedValues(
      string path,
      object component,
      IAbstractComponentType type,
      IList list,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      if (component == null)
        return;
      string[] propertyNames = type.PropertyNames;
      IType[] subtypes = type.Subtypes;
      object[] propertyValues = type.GetPropertyValues(component, this.GetEntityMode(criteria, criteriaQuery));
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        object component1 = propertyValues[index];
        IType type1 = subtypes[index];
        string str = StringHelper.Qualify(path, propertyNames[index]);
        if (this.IsPropertyIncluded(component1, str, type1))
        {
          if (type1.IsComponentType)
            this.AddComponentTypedValues(str, component1, (IAbstractComponentType) type1, list, criteria, criteriaQuery);
          else
            this.AddPropertyTypedValue(component1, type1, list);
        }
      }
    }

    protected void AppendPropertyCondition(
      string propertyName,
      object propertyValue,
      ICriteria criteria,
      ICriteriaQuery cq,
      IDictionary<string, IFilter> enabledFilters,
      SqlStringBuilder builder)
    {
      if (builder.Count > 1)
        builder.Add(" and ");
      ICriterion criterion = propertyValue != null ? this.GetNotNullPropertyCriterion(propertyValue, propertyName) : (ICriterion) new NullExpression(propertyName);
      builder.Add(criterion.ToSqlString(criteria, cq, enabledFilters));
    }

    protected virtual ICriterion GetNotNullPropertyCriterion(
      object propertyValue,
      string propertyName)
    {
      bool flag = propertyValue is string;
      return !this._isLikeEnabled || !flag ? (ICriterion) new SimpleExpression(propertyName, propertyValue, " = ", this._isIgnoreCaseEnabled && flag) : (ICriterion) new LikeExpression(propertyName, propertyValue.ToString(), this._matchMode, this.escapeCharacter, this._isIgnoreCaseEnabled);
    }

    protected void AppendComponentCondition(
      string path,
      object component,
      IAbstractComponentType type,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters,
      SqlStringBuilder builder)
    {
      if (component == null)
        return;
      string[] propertyNames = type.PropertyNames;
      object[] propertyValues = type.GetPropertyValues(component, this.GetEntityMode(criteria, criteriaQuery));
      IType[] subtypes = type.Subtypes;
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        string str = StringHelper.Qualify(path, propertyNames[index]);
        object obj = propertyValues[index];
        if (this.IsPropertyIncluded(obj, str, subtypes[index]))
        {
          IType type1 = subtypes[index];
          if (type1.IsComponentType)
            this.AppendComponentCondition(str, obj, (IAbstractComponentType) type1, criteria, criteriaQuery, enabledFilters, builder);
          else
            this.AppendPropertyCondition(str, obj, criteria, criteriaQuery, enabledFilters, builder);
        }
      }
    }

    public interface IPropertySelector
    {
      bool Include(object propertyValue, string propertyName, IType type);
    }

    [Serializable]
    private class AllPropertySelector : Example.IPropertySelector
    {
      public bool Include(object propertyValue, string propertyName, IType type) => true;
    }

    [Serializable]
    private class NotNullOrZeroPropertySelector : Example.IPropertySelector
    {
      private static bool IsZero(object value)
      {
        if (value is IConvertible)
        {
          if (!(value is string))
          {
            try
            {
              return Convert.ToInt64(value) == 0L;
            }
            catch (FormatException ex)
            {
            }
            catch (InvalidCastException ex)
            {
            }
          }
        }
        return false;
      }

      public bool Include(object propertyValue, string propertyName, IType type)
      {
        return propertyValue != null && !Example.NotNullOrZeroPropertySelector.IsZero(propertyValue);
      }
    }

    [Serializable]
    private class NotNullOrEmptyStringPropertySelector : Example.IPropertySelector
    {
      public bool Include(object propertyValue, string propertyName, IType type)
      {
        return propertyValue != null && propertyValue.ToString().Length > 0;
      }
    }
  }
}
