// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.EnumerableImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Impl
{
  public class EnumerableImpl : IEnumerable, IEnumerator, IDisposable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (EnumerableImpl));
    private IDataReader _reader;
    private IEventSource _session;
    private IType[] _types;
    private bool _single;
    private object _currentResult;
    private bool _hasNext;
    private bool _startedReading;
    private string[][] _names;
    private IDbCommand _cmd;
    private bool _readOnly;
    private int _currentRow = -1;
    private HolderInstantiator _holderInstantiator;
    private RowSelection _selection;
    private bool _isAlreadyDisposed;

    public EnumerableImpl(
      IDataReader reader,
      IDbCommand cmd,
      IEventSource session,
      bool readOnly,
      IType[] types,
      string[][] columnNames,
      RowSelection selection,
      HolderInstantiator holderInstantiator)
    {
      this._reader = reader;
      this._cmd = cmd;
      this._session = session;
      this._readOnly = readOnly;
      this._types = types;
      this._names = columnNames;
      this._selection = selection;
      this._holderInstantiator = holderInstantiator;
      this._single = this._types.Length == 1;
    }

    public IEnumerator GetEnumerator()
    {
      this.Reset();
      return (IEnumerator) this;
    }

    public object Current => this._currentResult;

    public bool MoveNext()
    {
      bool hasNext;
      try
      {
        hasNext = this._reader.Read();
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this._session.Factory.SQLExceptionConverter, (Exception) ex, "Error executing Enumerable() query", new SqlString(this._cmd.CommandText));
      }
      this.PostMoveNext(hasNext);
      return this._hasNext;
    }

    private void PostNext()
    {
      EnumerableImpl.log.Debug((object) "attempting to retrieve next results");
      try
      {
        if (!this._reader.Read())
        {
          EnumerableImpl.log.Debug((object) "exhausted results");
          this._currentResult = (object) null;
          this._session.Batcher.CloseCommand(this._cmd, this._reader);
        }
        else
          EnumerableImpl.log.Debug((object) "retrieved next results");
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this._session.Factory.SQLExceptionConverter, (Exception) ex, "Error executing Enumerable() query", new SqlString(this._cmd.CommandText));
      }
    }

    private void PostMoveNext(bool hasNext)
    {
      this._startedReading = true;
      this._hasNext = hasNext;
      ++this._currentRow;
      if (this._selection != null && this._selection.MaxRows != RowSelection.NoValue)
        this._hasNext = this._hasNext && this._currentRow < this._selection.MaxRows;
      bool defaultReadOnly = this._session.DefaultReadOnly;
      this._session.DefaultReadOnly = this._readOnly;
      try
      {
        if (!this._hasNext)
        {
          EnumerableImpl.log.Debug((object) "exhausted results");
          this._currentResult = (object) null;
          this._session.Batcher.CloseCommand(this._cmd, this._reader);
        }
        else
        {
          EnumerableImpl.log.Debug((object) "retrieving next results");
          bool isRequired = this._holderInstantiator.IsRequired;
          if (this._single && !isRequired)
          {
            this._currentResult = this._types[0].NullSafeGet(this._reader, this._names[0], (ISessionImplementor) this._session, (object) null);
          }
          else
          {
            object[] row = new object[this._types.Length];
            for (int index = 0; index < this._types.Length; ++index)
              row[index] = this._types[index].NullSafeGet(this._reader, this._names[index], (ISessionImplementor) this._session, (object) null);
            if (isRequired)
              this._currentResult = this._holderInstantiator.Instantiate(row);
            else
              this._currentResult = (object) row;
          }
        }
      }
      finally
      {
        this._session.DefaultReadOnly = defaultReadOnly;
      }
    }

    public void Reset()
    {
    }

    ~EnumerableImpl() => this.Dispose(false);

    public void Dispose()
    {
      EnumerableImpl.log.Debug((object) "running EnumerableImpl.Dispose()");
      this.Dispose(true);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (this._isAlreadyDisposed)
        return;
      if (isDisposing && (this._hasNext || !this._startedReading))
      {
        this._currentResult = (object) null;
        this._session.Batcher.CloseCommand(this._cmd, this._reader);
      }
      this._isAlreadyDisposed = true;
      GC.SuppressFinalize((object) this);
    }
  }
}
