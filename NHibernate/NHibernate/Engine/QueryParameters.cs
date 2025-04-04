// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.QueryParameters
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public sealed class QueryParameters
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (QueryParameters));
    private bool readOnly;

    public QueryParameters()
      : this(ArrayHelper.EmptyTypeArray, ArrayHelper.EmptyObjectArray)
    {
    }

    public QueryParameters(
      IType[] positionalParameterTypes,
      object[] postionalParameterValues,
      object optionalObject,
      string optionalEntityName,
      object optionalObjectId)
      : this(positionalParameterTypes, postionalParameterValues)
    {
      this.OptionalObject = optionalObject;
      this.OptionalId = optionalObjectId;
      this.OptionalEntityName = optionalEntityName;
    }

    public QueryParameters(IType[] positionalParameterTypes, object[] postionalParameterValues)
      : this(positionalParameterTypes, postionalParameterValues, (IDictionary<string, LockMode>) null, (RowSelection) null, false, false, false, (string) null, (string) null, false, (IResultTransformer) null)
    {
    }

    public QueryParameters(
      IType[] positionalParameterTypes,
      object[] postionalParameterValues,
      object[] collectionKeys)
      : this(positionalParameterTypes, postionalParameterValues, (IDictionary<string, TypedValue>) null, collectionKeys)
    {
    }

    public QueryParameters(
      IType[] positionalParameterTypes,
      object[] postionalParameterValues,
      IDictionary<string, TypedValue> namedParameters,
      object[] collectionKeys)
      : this(positionalParameterTypes, postionalParameterValues, namedParameters, (IDictionary<string, LockMode>) null, (RowSelection) null, false, false, false, (string) null, (string) null, collectionKeys, (IResultTransformer) null)
    {
    }

    public QueryParameters(
      IType[] positionalParameterTypes,
      object[] positionalParameterValues,
      IDictionary<string, LockMode> lockModes,
      RowSelection rowSelection,
      bool isReadOnlyInitialized,
      bool readOnly,
      bool cacheable,
      string cacheRegion,
      string comment,
      bool isLookupByNaturalKey,
      IResultTransformer transformer)
      : this(positionalParameterTypes, positionalParameterValues, (IDictionary<string, TypedValue>) null, lockModes, rowSelection, isReadOnlyInitialized, readOnly, cacheable, cacheRegion, comment, (object[]) null, transformer)
    {
      this.NaturalKeyLookup = isLookupByNaturalKey;
    }

    public QueryParameters(
      IDictionary<string, TypedValue> namedParameters,
      IDictionary<string, LockMode> lockModes,
      RowSelection rowSelection,
      bool isReadOnlyInitialized,
      bool readOnly,
      bool cacheable,
      string cacheRegion,
      string comment,
      bool isLookupByNaturalKey,
      IResultTransformer transformer)
      : this(ArrayHelper.EmptyTypeArray, ArrayHelper.EmptyObjectArray, namedParameters, lockModes, rowSelection, isReadOnlyInitialized, readOnly, cacheable, cacheRegion, comment, (object[]) null, transformer)
    {
      this.NaturalKeyLookup = isLookupByNaturalKey;
    }

    public QueryParameters(
      IType[] positionalParameterTypes,
      object[] positionalParameterValues,
      IDictionary<string, TypedValue> namedParameters,
      IDictionary<string, LockMode> lockModes,
      RowSelection rowSelection,
      bool isReadOnlyInitialized,
      bool readOnly,
      bool cacheable,
      string cacheRegion,
      string comment,
      object[] collectionKeys,
      IResultTransformer transformer)
    {
      this.PositionalParameterTypes = positionalParameterTypes ?? new IType[0];
      this.PositionalParameterValues = positionalParameterValues ?? new object[0];
      this.NamedParameters = namedParameters ?? (IDictionary<string, TypedValue>) new Dictionary<string, TypedValue>(1);
      this.LockModes = lockModes;
      this.RowSelection = rowSelection;
      this.Cacheable = cacheable;
      this.CacheRegion = cacheRegion;
      this.Comment = comment;
      this.CollectionKeys = collectionKeys;
      this.IsReadOnlyInitialized = isReadOnlyInitialized;
      this.readOnly = readOnly;
      this.ResultTransformer = transformer;
    }

    public QueryParameters(
      IType[] positionalParameterTypes,
      object[] positionalParameterValues,
      IDictionary<string, TypedValue> namedParameters,
      IDictionary<string, LockMode> lockModes,
      RowSelection rowSelection,
      bool isReadOnlyInitialized,
      bool readOnly,
      bool cacheable,
      string cacheRegion,
      string comment,
      object[] collectionKeys,
      object optionalObject,
      string optionalEntityName,
      object optionalId,
      IResultTransformer transformer)
      : this(positionalParameterTypes, positionalParameterValues, namedParameters, lockModes, rowSelection, isReadOnlyInitialized, readOnly, cacheable, cacheRegion, comment, collectionKeys, transformer)
    {
      this.OptionalEntityName = optionalEntityName;
      this.OptionalId = optionalId;
      this.OptionalObject = optionalObject;
    }

    public bool HasRowSelection => this.RowSelection != null;

    public IDictionary<string, TypedValue> NamedParameters { get; internal set; }

    public IType[] PositionalParameterTypes { get; set; }

    public object[] PositionalParameterValues { get; set; }

    public RowSelection RowSelection { get; set; }

    public IDictionary<string, LockMode> LockModes { get; set; }

    public bool IsReadOnlyInitialized { get; private set; }

    public bool Cacheable { get; set; }

    public string CacheRegion { get; set; }

    public string Comment { get; set; }

    public bool ForceCacheRefresh { get; set; }

    public string OptionalEntityName { get; set; }

    public object OptionalId { get; set; }

    public object OptionalObject { get; set; }

    public object[] CollectionKeys { get; set; }

    public bool Callable { get; set; }

    public bool ReadOnly
    {
      get
      {
        if (!this.IsReadOnlyInitialized)
          throw new InvalidOperationException("cannot call ReadOnly when IsReadOnlyInitialized returns false");
        return this.readOnly;
      }
      set
      {
        this.readOnly = value;
        this.IsReadOnlyInitialized = true;
      }
    }

    public SqlString ProcessedSql { get; internal set; }

    public IEnumerable<IParameterSpecification> ProcessedSqlParameters { get; internal set; }

    public RowSelection ProcessedRowSelection { get; internal set; }

    public bool NaturalKeyLookup { get; set; }

    public IResultTransformer ResultTransformer { get; private set; }

    public bool HasAutoDiscoverScalarTypes { get; set; }

    public void LogParameters(ISessionFactoryImplementor factory)
    {
      Printer printer = new Printer(factory);
      if (this.PositionalParameterValues.Length != 0)
        QueryParameters.log.Debug((object) ("parameters: " + printer.ToString(this.PositionalParameterTypes, this.PositionalParameterValues)));
      if (this.NamedParameters == null)
        return;
      QueryParameters.log.Debug((object) ("named parameters: " + printer.ToString(this.NamedParameters)));
    }

    public void ValidateParameters()
    {
      int length1 = this.PositionalParameterTypes == null ? 0 : this.PositionalParameterTypes.Length;
      int length2 = this.PositionalParameterValues == null ? 0 : this.PositionalParameterValues.Length;
      if (length1 != length2)
        throw new QueryException("Number of positional parameter types (" + (object) length1 + ") does not match number of positional parameter values (" + (object) length2 + ")");
    }

    public QueryParameters CreateCopyUsing(RowSelection selection)
    {
      return new QueryParameters(this.PositionalParameterTypes, this.PositionalParameterValues, this.NamedParameters, this.LockModes, selection, this.IsReadOnlyInitialized, this.readOnly, this.Cacheable, this.CacheRegion, this.Comment, this.CollectionKeys, this.OptionalObject, this.OptionalEntityName, this.OptionalId, this.ResultTransformer)
      {
        ProcessedSql = this.ProcessedSql,
        ProcessedSqlParameters = this.ProcessedSqlParameters != null ? (IEnumerable<IParameterSpecification>) this.ProcessedSqlParameters.ToList<IParameterSpecification>() : (IEnumerable<IParameterSpecification>) null
      };
    }

    public bool IsReadOnly(ISessionImplementor session)
    {
      return !this.IsReadOnlyInitialized ? session.PersistenceContext.DefaultReadOnly : this.ReadOnly;
    }
  }
}
