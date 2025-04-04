// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Enhanced.SequenceStructure
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Id.Enhanced
{
  public class SequenceStructure : IDatabaseStructure
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (SequenceStructure));
    private readonly int _incrementSize;
    private readonly int _initialValue;
    private readonly string _sequenceName;
    private readonly SqlString _sql;
    private int _accessCounter;
    private bool _applyIncrementSizeToSourceValues;

    public SequenceStructure(
      NHibernate.Dialect.Dialect dialect,
      string sequenceName,
      int initialValue,
      int incrementSize)
    {
      this._sequenceName = sequenceName;
      this._initialValue = initialValue;
      this._incrementSize = incrementSize;
      this._sql = new SqlString(dialect.GetSequenceNextValString(sequenceName));
    }

    public string Name => this._sequenceName;

    public int IncrementSize => this._incrementSize;

    public IAccessCallback BuildCallback(ISessionImplementor session)
    {
      return (IAccessCallback) new SequenceStructure.SequenceAccessCallback(session, this);
    }

    public void Prepare(IOptimizer optimizer)
    {
      this._applyIncrementSizeToSourceValues = optimizer.ApplyIncrementSizeToSourceValues;
    }

    public string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
    {
      int incrementSize = this._applyIncrementSizeToSourceValues ? this._incrementSize : 1;
      if (this._initialValue > 1 || incrementSize > 1)
        return dialect.GetCreateSequenceStrings(this._sequenceName, this._initialValue, incrementSize);
      return new string[1]
      {
        dialect.GetCreateSequenceString(this._sequenceName)
      };
    }

    public string[] SqlDropStrings(NHibernate.Dialect.Dialect dialect)
    {
      return dialect.GetDropSequenceStrings(this._sequenceName);
    }

    public int TimesAccessed => this._accessCounter;

    private class SequenceAccessCallback : IAccessCallback
    {
      private readonly SequenceStructure _owner;
      private readonly ISessionImplementor _session;

      public SequenceAccessCallback(ISessionImplementor session, SequenceStructure owner)
      {
        this._session = session;
        this._owner = owner;
      }

      public virtual long GetNextValue()
      {
        ++this._owner._accessCounter;
        try
        {
          IDbCommand cmd = this._session.Batcher.PrepareCommand(CommandType.Text, this._owner._sql, SqlTypeFactory.NoTypes);
          IDataReader reader = (IDataReader) null;
          try
          {
            reader = this._session.Batcher.ExecuteReader(cmd);
            try
            {
              reader.Read();
              long int64 = Convert.ToInt64(reader.GetValue(0));
              if (SequenceStructure.Log.IsDebugEnabled)
                SequenceStructure.Log.Debug((object) ("Sequence value obtained: " + (object) int64));
              return int64;
            }
            finally
            {
              try
              {
                reader.Close();
              }
              catch
              {
              }
            }
          }
          finally
          {
            this._session.Batcher.CloseCommand(cmd, reader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this._session.Factory.SQLExceptionConverter, (Exception) ex, "could not get next sequence value", this._owner._sql);
        }
      }
    }
  }
}
