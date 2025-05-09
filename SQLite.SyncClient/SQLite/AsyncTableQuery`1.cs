﻿// Decompiled with JetBrains decompiler
// Type: SQLite.AsyncTableQuery`1
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable disable
namespace SQLite
{
  public class AsyncTableQuery<T> where T : new()
  {
    private TableQuery<T> _innerQuery;

    public AsyncTableQuery(TableQuery<T> innerQuery) => this._innerQuery = innerQuery;

    public AsyncTableQuery<T> Where(Expression<Func<T, bool>> predExpr)
    {
      return new AsyncTableQuery<T>(this._innerQuery.Where(predExpr));
    }

    public AsyncTableQuery<T> Skip(int n) => new AsyncTableQuery<T>(this._innerQuery.Skip(n));

    public AsyncTableQuery<T> Take(int n) => new AsyncTableQuery<T>(this._innerQuery.Take(n));

    public AsyncTableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
    {
      return new AsyncTableQuery<T>(this._innerQuery.OrderBy<U>(orderExpr));
    }

    public AsyncTableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
    {
      return new AsyncTableQuery<T>(this._innerQuery.OrderByDescending<U>(orderExpr));
    }

    public Task<List<T>> ToListAsync()
    {
      return Task.Factory.StartNew<List<T>>((Func<List<T>>) (() =>
      {
        using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
          return this._innerQuery.ToList<T>();
      }));
    }

    public Task<int> CountAsync()
    {
      return Task.Factory.StartNew<int>((Func<int>) (() =>
      {
        using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
          return this._innerQuery.Count();
      }));
    }

    public Task<T> ElementAtAsync(int index)
    {
      return Task.Factory.StartNew<T>((Func<T>) (() =>
      {
        using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
          return this._innerQuery.ElementAt(index);
      }));
    }

    public Task<T> FirstAsync()
    {
      return Task<T>.Factory.StartNew((Func<T>) (() =>
      {
        using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
          return this._innerQuery.First();
      }));
    }

    public Task<T> FirstOrDefaultAsync()
    {
      return Task<T>.Factory.StartNew((Func<T>) (() =>
      {
        using (((SQLiteConnectionWithLock) this._innerQuery.Connection).Lock())
          return this._innerQuery.FirstOrDefault();
      }));
    }
  }
}
