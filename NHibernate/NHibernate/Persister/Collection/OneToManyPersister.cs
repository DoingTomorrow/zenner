// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.OneToManyPersister
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
using NHibernate.Loader.Entity;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public class OneToManyPersister : AbstractCollectionPersister
  {
    private readonly bool _cascadeDeleteEnabled;
    private readonly bool _keyIsNullable;
    private readonly bool _keyIsUpdateable;

    public OneToManyPersister(
      NHibernate.Mapping.Collection collection,
      ICacheConcurrencyStrategy cache,
      Configuration cfg,
      ISessionFactoryImplementor factory)
      : base(collection, cache, cfg, factory)
    {
      this._cascadeDeleteEnabled = collection.Key.IsCascadeDeleteEnabled && factory.Dialect.SupportsCascadeDelete;
      this._keyIsNullable = collection.Key.IsNullable;
      this._keyIsUpdateable = collection.Key.IsUpdateable;
    }

    protected override bool RowDeleteEnabled => this._keyIsUpdateable && this._keyIsNullable;

    protected override bool RowInsertEnabled => this._keyIsUpdateable;

    public override bool CascadeDeleteEnabled => this._cascadeDeleteEnabled;

    public override bool IsOneToMany => true;

    public override bool IsManyToMany => false;

    protected override SqlCommandInfo GenerateDeleteString()
    {
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.qualifiedTableName).AddColumns(this.KeyColumnNames, "null").SetIdentityColumn(this.KeyColumnNames, this.KeyType);
      if (this.HasIndex)
        sqlUpdateBuilder.AddColumns(this.IndexColumnNames, "null");
      if (this.HasWhere)
        sqlUpdateBuilder.AddWhereFragment(this.sqlWhereString);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment("delete one-to-many " + this.Role);
      return sqlUpdateBuilder.ToSqlCommandInfo();
    }

    protected override SqlCommandInfo GenerateInsertRowString()
    {
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory);
      sqlUpdateBuilder.SetTableName(this.qualifiedTableName).AddColumns(this.KeyColumnNames, this.KeyType);
      if (this.HasIndex && !this.indexContainsFormula)
        sqlUpdateBuilder.AddColumns(this.IndexColumnNames, this.IndexType);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment("create one-to-many row " + this.Role);
      sqlUpdateBuilder.SetIdentityColumn(this.ElementColumnNames, this.ElementType);
      return sqlUpdateBuilder.ToSqlCommandInfo();
    }

    protected override SqlCommandInfo GenerateUpdateRowString() => (SqlCommandInfo) null;

    protected override SqlCommandInfo GenerateDeleteRowString()
    {
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory);
      sqlUpdateBuilder.SetTableName(this.qualifiedTableName).AddColumns(this.KeyColumnNames, "null");
      if (this.HasIndex && !this.indexContainsFormula)
        sqlUpdateBuilder.AddColumns(this.IndexColumnNames, "null");
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment("delete one-to-many row " + this.Role);
      sqlUpdateBuilder.AddWhereFragment(this.KeyColumnNames, this.KeyType, " = ").AddWhereFragment(this.ElementColumnNames, this.ElementType, " = ");
      return sqlUpdateBuilder.ToSqlCommandInfo();
    }

    public override bool ConsumesEntityAlias() => true;

    public override bool ConsumesCollectionAlias() => true;

    protected override int DoUpdateRows(
      object id,
      IPersistentCollection collection,
      ISessionImplementor session)
    {
      try
      {
        int num = 0;
        if (this.RowDeleteEnabled)
        {
          bool flag = true;
          IDbCommand dbCommand = (IDbCommand) null;
          try
          {
            int i1 = 0;
            IEnumerable enumerable = collection.Entries((ICollectionPersister) this);
            IExpectation expectation = Expectations.None;
            foreach (object entry in enumerable)
            {
              if (collection.NeedsUpdating(entry, i1, this.ElementType))
              {
                if (dbCommand == null)
                {
                  SqlCommandInfo sqlDeleteRowString = this.SqlDeleteRowString;
                  if (this.DeleteCallable)
                  {
                    expectation = Expectations.AppropriateExpectation(this.DeleteCheckStyle);
                    flag = expectation.CanBeBatched;
                    dbCommand = flag ? session.Batcher.PrepareBatchCommand(this.SqlDeleteRowString.CommandType, sqlDeleteRowString.Text, this.SqlDeleteRowString.ParameterTypes) : session.Batcher.PrepareCommand(this.SqlDeleteRowString.CommandType, sqlDeleteRowString.Text, this.SqlDeleteRowString.ParameterTypes);
                  }
                  else
                    dbCommand = session.Batcher.PrepareBatchCommand(this.SqlDeleteRowString.CommandType, sqlDeleteRowString.Text, this.SqlDeleteRowString.ParameterTypes);
                }
                int i2 = this.WriteKey(dbCommand, id, 0, session);
                this.WriteElementToWhere(dbCommand, collection.GetSnapshotElement(entry, i1), i2, session);
                if (flag)
                  session.Batcher.AddToBatch(expectation);
                else
                  expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
                ++num;
              }
              ++i1;
            }
          }
          catch (Exception ex)
          {
            if (flag)
              session.Batcher.AbortBatch(ex);
            throw;
          }
          finally
          {
            if (!flag && dbCommand != null)
              session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
          }
        }
        if (this.RowInsertEnabled)
        {
          IExpectation expectation = Expectations.AppropriateExpectation(this.InsertCheckStyle);
          bool canBeBatched = expectation.CanBeBatched;
          SqlCommandInfo sqlInsertRowString = this.SqlInsertRowString;
          IDbCommand dbCommand = (IDbCommand) null;
          try
          {
            int i3 = 0;
            foreach (object entry in collection.Entries((ICollectionPersister) this))
            {
              if (collection.NeedsUpdating(entry, i3, this.ElementType))
              {
                if (canBeBatched)
                {
                  if (dbCommand == null)
                    dbCommand = session.Batcher.PrepareBatchCommand(this.SqlInsertRowString.CommandType, sqlInsertRowString.Text, this.SqlInsertRowString.ParameterTypes);
                }
                else
                  dbCommand = session.Batcher.PrepareCommand(this.SqlInsertRowString.CommandType, sqlInsertRowString.Text, this.SqlInsertRowString.ParameterTypes);
                int i4 = this.WriteKey(dbCommand, id, 0, session);
                if (this.HasIndex && !this.indexContainsFormula)
                  i4 = this.WriteIndexToWhere(dbCommand, collection.GetIndex(entry, i3, (ICollectionPersister) this), i4, session);
                this.WriteElementToWhere(dbCommand, collection.GetElement(entry), i4, session);
                if (canBeBatched)
                  session.Batcher.AddToBatch(expectation);
                else
                  expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
                ++num;
              }
              ++i3;
            }
          }
          catch (Exception ex)
          {
            if (canBeBatched)
              session.Batcher.AbortBatch(ex);
            throw;
          }
          finally
          {
            if (!canBeBatched && dbCommand != null)
              session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
          }
        }
        return num;
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.SQLExceptionConverter, (Exception) ex, "could not update collection rows: " + MessageHelper.InfoString((ICollectionPersister) this, id));
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
      StringBuilder stringBuilder = new StringBuilder();
      if (includeCollectionColumns)
        stringBuilder.Append(this.SelectFragment(lhsAlias, collectionSuffix)).Append(", ");
      IOuterJoinLoadable elementPersister = (IOuterJoinLoadable) this.ElementPersister;
      return stringBuilder.Append(elementPersister.SelectFragment(lhsAlias, entitySuffix)).ToString();
    }

    protected override NHibernate.SqlCommand.SelectFragment GenerateSelectFragment(
      string alias,
      string columnSuffix)
    {
      IOuterJoinLoadable elementPersister = (IOuterJoinLoadable) this.ElementPersister;
      NHibernate.SqlCommand.SelectFragment selectFragment = new NHibernate.SqlCommand.SelectFragment(this.Dialect).SetSuffix(columnSuffix);
      string[] keyColumnNames = this.KeyColumnNames;
      string[] keyColumnAliases = this.KeyColumnAliases;
      for (int index = 0; index < keyColumnNames.Length; ++index)
      {
        string str = keyColumnNames[index];
        string tableAliasForColumn = elementPersister.GenerateTableAliasForColumn(alias, str);
        selectFragment.AddColumn(tableAliasForColumn, str, keyColumnAliases[index]);
      }
      return selectFragment;
    }

    protected override ICollectionInitializer CreateCollectionInitializer(
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      return BatchingCollectionInitializer.CreateBatchingOneToManyInitializer(this, this.batchSize, this.Factory, enabledFilters);
    }

    public override SqlString FromJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses)
    {
      return ((IJoinable) this.ElementPersister).FromJoinFragment(alias, innerJoin, includeSubclasses);
    }

    public override SqlString WhereJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses)
    {
      return ((IJoinable) this.ElementPersister).WhereJoinFragment(alias, innerJoin, includeSubclasses);
    }

    public override string TableName => ((IJoinable) this.ElementPersister).TableName;

    protected override string FilterFragment(string alias)
    {
      string str = base.FilterFragment(alias);
      if (this.ElementPersister is IJoinable elementPersister)
        str += elementPersister.OneToManyFilterFragment(alias);
      return str;
    }

    protected override ICollectionInitializer CreateSubselectInitializer(
      SubselectFetch subselect,
      ISessionImplementor session)
    {
      return (ICollectionInitializer) new SubselectOneToManyLoader((IQueryableCollection) this, subselect.ToSubselectString(this.CollectionType.LHSPropertyName), (ICollection<EntityKey>) subselect.Result, subselect.QueryParameters, session.Factory, session.EnabledFilters);
    }

    public override object GetElementByIndex(
      object key,
      object index,
      ISessionImplementor session,
      object owner)
    {
      return new CollectionElementLoader((IQueryableCollection) this, this.Factory, session.EnabledFilters).LoadElement(session, key, this.IncrementIndexByBase(index)) ?? this.NotFoundObject;
    }

    protected override SqlCommandInfo GenerateIdentityInsertRowString()
    {
      throw new NotSupportedException("Identity insert is not needed for one-to-many association");
    }
  }
}
