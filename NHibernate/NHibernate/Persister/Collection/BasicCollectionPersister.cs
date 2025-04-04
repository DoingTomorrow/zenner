// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.BasicCollectionPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public class BasicCollectionPersister(
    NHibernate.Mapping.Collection collection,
    ICacheConcurrencyStrategy cache,
    Configuration cfg,
    ISessionFactoryImplementor factory) : AbstractCollectionPersister(collection, cache, cfg, factory)
  {
    public override bool CascadeDeleteEnabled => false;

    public override bool IsOneToMany => false;

    public override bool IsManyToMany => this.ElementType.IsEntityType;

    protected override SqlCommandInfo GenerateDeleteString()
    {
      SqlDeleteBuilder sqlDeleteBuilder = new SqlDeleteBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.qualifiedTableName).SetIdentityColumn(this.KeyColumnNames, this.KeyType);
      if (this.HasWhere)
        sqlDeleteBuilder.AddWhereFragment(this.sqlWhereString);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlDeleteBuilder.SetComment("delete collection " + this.Role);
      return sqlDeleteBuilder.ToSqlCommandInfo();
    }

    protected override SqlCommandInfo GenerateInsertRowString()
    {
      SqlInsertBuilder sqlInsertBuilder = new SqlInsertBuilder(this.Factory).SetTableName(this.qualifiedTableName).AddColumns(this.KeyColumnNames, (bool[]) null, this.KeyType);
      if (this.hasIdentifier)
        sqlInsertBuilder.AddColumns(new string[1]
        {
          this.IdentifierColumnName
        }, (bool[]) null, this.IdentifierType);
      if (this.HasIndex)
        sqlInsertBuilder.AddColumns(this.IndexColumnNames, this.indexColumnIsSettable, this.IndexType);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlInsertBuilder.SetComment("insert collection row " + this.Role);
      sqlInsertBuilder.AddColumns(this.ElementColumnNames, this.elementColumnIsSettable, this.ElementType);
      return sqlInsertBuilder.ToSqlCommandInfo();
    }

    protected override SqlCommandInfo GenerateUpdateRowString()
    {
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.qualifiedTableName).AddColumns(this.ElementColumnNames, this.elementColumnIsSettable, this.ElementType);
      if (this.hasIdentifier)
        sqlUpdateBuilder.AddWhereFragment(new string[1]
        {
          this.IdentifierColumnName
        }, this.IdentifierType, " = ");
      else if (this.HasIndex && !this.indexContainsFormula)
      {
        sqlUpdateBuilder.AddWhereFragment(this.KeyColumnNames, this.KeyType, " = ").AddWhereFragment(this.IndexColumnNames, this.IndexType, " = ");
      }
      else
      {
        string[] columnNames = ArrayHelper.Join<string>(this.KeyColumnNames, this.ElementColumnNames, this.elementColumnIsInPrimaryKey);
        SqlType[] types = ArrayHelper.Join<SqlType>(this.KeyType.SqlTypes((IMapping) this.Factory), this.ElementType.SqlTypes((IMapping) this.Factory), this.elementColumnIsInPrimaryKey);
        sqlUpdateBuilder.AddWhereFragment(columnNames, types, " = ");
      }
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment("update collection row " + this.Role);
      return sqlUpdateBuilder.ToSqlCommandInfo();
    }

    protected override SqlCommandInfo GenerateDeleteRowString()
    {
      SqlDeleteBuilder sqlDeleteBuilder = new SqlDeleteBuilder(this.Factory.Dialect, (IMapping) this.Factory);
      sqlDeleteBuilder.SetTableName(this.qualifiedTableName);
      if (this.hasIdentifier)
        sqlDeleteBuilder.AddWhereFragment(new string[1]
        {
          this.IdentifierColumnName
        }, this.IdentifierType, " = ");
      else if (this.HasIndex && !this.indexContainsFormula)
      {
        sqlDeleteBuilder.AddWhereFragment(this.KeyColumnNames, this.KeyType, " = ").AddWhereFragment(this.IndexColumnNames, this.IndexType, " = ");
      }
      else
      {
        string[] columnNames = ArrayHelper.Join<string>(this.KeyColumnNames, this.ElementColumnNames, this.elementColumnIsInPrimaryKey);
        SqlType[] types = ArrayHelper.Join<SqlType>(this.KeyType.SqlTypes((IMapping) this.Factory), this.ElementType.SqlTypes((IMapping) this.Factory), this.elementColumnIsInPrimaryKey);
        sqlDeleteBuilder.AddWhereFragment(columnNames, types, " = ");
      }
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlDeleteBuilder.SetComment("delete collection row " + this.Role);
      return sqlDeleteBuilder.ToSqlCommandInfo();
    }

    public override bool ConsumesEntityAlias() => false;

    public override bool ConsumesCollectionAlias() => true;

    protected override int DoUpdateRows(
      object id,
      IPersistentCollection collection,
      ISessionImplementor session)
    {
      if (ArrayHelper.IsAllFalse(this.elementColumnIsSettable))
        return 0;
      try
      {
        IDbCommand dbCommand = (IDbCommand) null;
        IExpectation expectation = Expectations.AppropriateExpectation(this.UpdateCheckStyle);
        bool canBeBatched = expectation.CanBeBatched;
        IEnumerable enumerable = collection.Entries((ICollectionPersister) this);
        int i1 = 0;
        int num = 0;
        foreach (object entry in enumerable)
        {
          if (collection.NeedsUpdating(entry, i1, this.ElementType))
          {
            int i2 = 0;
            if (canBeBatched)
            {
              if (dbCommand == null)
                dbCommand = session.Batcher.PrepareBatchCommand(this.SqlUpdateRowString.CommandType, this.SqlUpdateRowString.Text, this.SqlUpdateRowString.ParameterTypes);
            }
            else
              dbCommand = session.Batcher.PrepareCommand(this.SqlUpdateRowString.CommandType, this.SqlUpdateRowString.Text, this.SqlUpdateRowString.ParameterTypes);
            try
            {
              int i3 = this.WriteElement(dbCommand, collection.GetElement(entry), i2, session);
              if (this.hasIdentifier)
              {
                this.WriteIdentifier(dbCommand, collection.GetIdentifier(entry, i1), i3, session);
              }
              else
              {
                int i4 = this.WriteKey(dbCommand, id, i3, session);
                if (this.HasIndex && !this.indexContainsFormula)
                  this.WriteIndexToWhere(dbCommand, collection.GetIndex(entry, i1, (ICollectionPersister) this), i4, session);
                else
                  this.WriteElementToWhere(dbCommand, collection.GetSnapshotElement(entry, i1), i4, session);
              }
              if (canBeBatched)
                session.Batcher.AddToBatch(expectation);
              else
                expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
            }
            catch (Exception ex)
            {
              if (canBeBatched)
                session.Batcher.AbortBatch(ex);
              throw;
            }
            finally
            {
              if (!canBeBatched)
                session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
            }
            ++num;
          }
          ++i1;
        }
        return num;
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.SQLExceptionConverter, (Exception) ex, "could not update collection rows: " + MessageHelper.InfoString((ICollectionPersister) this, id), this.SqlUpdateRowString.Text);
      }
    }

    public override string SelectFragment(
      IJoinable rhs,
      string rhsAlias,
      string lhsAlias,
      string entitySuffix,
      string collectionSuffix,
      bool includeCollectionColumns)
    {
      if (rhs != null && this.IsManyToMany && !rhs.IsCollection)
      {
        IAssociationType elementType = (IAssociationType) this.ElementType;
        if (rhs.Equals((object) elementType.GetAssociatedJoinable(this.Factory)))
          return this.ManyToManySelectFragment(rhs, rhsAlias, lhsAlias, collectionSuffix);
      }
      return !includeCollectionColumns ? string.Empty : this.SelectFragment(lhsAlias, collectionSuffix);
    }

    private string ManyToManySelectFragment(
      IJoinable rhs,
      string rhsAlias,
      string lhsAlias,
      string collectionSuffix)
    {
      NHibernate.SqlCommand.SelectFragment selectFragment = this.GenerateSelectFragment(lhsAlias, collectionSuffix);
      string[] keyColumnNames = rhs.KeyColumnNames;
      selectFragment.AddColumns(rhsAlias, keyColumnNames, this.elementColumnAliases);
      this.AppendIndexColumns(selectFragment, lhsAlias);
      this.AppendIdentifierColumns(selectFragment, lhsAlias);
      return selectFragment.ToSqlStringFragment(false);
    }

    protected override ICollectionInitializer CreateCollectionInitializer(
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      return BatchingCollectionInitializer.CreateBatchingCollectionInitializer((IQueryableCollection) this, this.batchSize, this.Factory, enabledFilters);
    }

    public override SqlString FromJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses)
    {
      return SqlString.Empty;
    }

    public override SqlString WhereJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses)
    {
      return SqlString.Empty;
    }

    protected override ICollectionInitializer CreateSubselectInitializer(
      SubselectFetch subselect,
      ISessionImplementor session)
    {
      return (ICollectionInitializer) new SubselectCollectionLoader((IQueryableCollection) this, subselect.ToSubselectString(this.CollectionType.LHSPropertyName), (ICollection<EntityKey>) subselect.Result, subselect.QueryParameters, session.Factory, session.EnabledFilters);
    }

    protected override SqlCommandInfo GenerateIdentityInsertRowString()
    {
      SqlInsertBuilder sqlInsertBuilder = (SqlInsertBuilder) this.identityDelegate.PrepareIdentifierGeneratingInsert();
      sqlInsertBuilder.SetTableName(this.qualifiedTableName).AddColumns(this.KeyColumnNames, (bool[]) null, this.KeyType);
      if (this.HasIndex)
        sqlInsertBuilder.AddColumns(this.IndexColumnNames, (bool[]) null, this.IndexType);
      sqlInsertBuilder.AddColumns(this.ElementColumnNames, this.elementColumnIsSettable, this.ElementType);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlInsertBuilder.SetComment("insert collection row " + this.Role);
      return sqlInsertBuilder.ToSqlCommandInfo();
    }
  }
}
