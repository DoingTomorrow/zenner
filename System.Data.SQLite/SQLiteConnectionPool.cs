// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionPool
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace System.Data.SQLite
{
  internal static class SQLiteConnectionPool
  {
    private static readonly object _syncRoot = new object();
    private static ISQLiteConnectionPool _connectionPool = (ISQLiteConnectionPool) null;
    private static SortedList<string, SQLiteConnectionPool.PoolQueue> _queueList = new SortedList<string, SQLiteConnectionPool.PoolQueue>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static int _poolVersion = 1;
    private static int _poolOpened = 0;
    private static int _poolClosed = 0;

    internal static void GetCounts(
      string fileName,
      ref Dictionary<string, int> counts,
      ref int openCount,
      ref int closeCount,
      ref int totalCount)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.GetCounts(fileName, ref counts, ref openCount, ref closeCount, ref totalCount);
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          openCount = SQLiteConnectionPool._poolOpened;
          closeCount = SQLiteConnectionPool._poolClosed;
          if (counts == null)
            counts = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          if (fileName != null)
          {
            SQLiteConnectionPool.PoolQueue poolQueue;
            if (!SQLiteConnectionPool._queueList.TryGetValue(fileName, out poolQueue))
              return;
            Queue<WeakReference> queue = poolQueue.Queue;
            int count = queue != null ? queue.Count : 0;
            counts.Add(fileName, count);
            totalCount += count;
          }
          else
          {
            foreach (KeyValuePair<string, SQLiteConnectionPool.PoolQueue> queue1 in SQLiteConnectionPool._queueList)
            {
              if (queue1.Value != null)
              {
                Queue<WeakReference> queue2 = queue1.Value.Queue;
                int count = queue2 != null ? queue2.Count : 0;
                counts.Add(queue1.Key, count);
                totalCount += count;
              }
            }
          }
        }
      }
    }

    internal static void ClearPool(string fileName)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.ClearPool(fileName);
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          SQLiteConnectionPool.PoolQueue poolQueue;
          if (!SQLiteConnectionPool._queueList.TryGetValue(fileName, out poolQueue))
            return;
          ++poolQueue.PoolVersion;
          Queue<WeakReference> queue = poolQueue.Queue;
          if (queue == null)
            return;
          while (queue.Count > 0)
          {
            WeakReference weakReference = queue.Dequeue();
            if (weakReference != null)
            {
              if (weakReference.Target is SQLiteConnectionHandle target)
                target.Dispose();
              GC.KeepAlive((object) target);
            }
          }
        }
      }
    }

    internal static void ClearAllPools()
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.ClearAllPools();
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          foreach (KeyValuePair<string, SQLiteConnectionPool.PoolQueue> queue1 in SQLiteConnectionPool._queueList)
          {
            if (queue1.Value != null)
            {
              Queue<WeakReference> queue2 = queue1.Value.Queue;
              while (queue2.Count > 0)
              {
                WeakReference weakReference = queue2.Dequeue();
                if (weakReference != null)
                {
                  if (weakReference.Target is SQLiteConnectionHandle target)
                    target.Dispose();
                  GC.KeepAlive((object) target);
                }
              }
              if (SQLiteConnectionPool._poolVersion <= queue1.Value.PoolVersion)
                SQLiteConnectionPool._poolVersion = queue1.Value.PoolVersion + 1;
            }
          }
          SQLiteConnectionPool._queueList.Clear();
        }
      }
    }

    internal static void Add(string fileName, SQLiteConnectionHandle handle, int version)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.Add(fileName, (object) handle, version);
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          SQLiteConnectionPool.PoolQueue queue1;
          if (SQLiteConnectionPool._queueList.TryGetValue(fileName, out queue1) && version == queue1.PoolVersion)
          {
            SQLiteConnectionPool.ResizePool(queue1, true);
            Queue<WeakReference> queue2 = queue1.Queue;
            if (queue2 == null)
              return;
            queue2.Enqueue(new WeakReference((object) handle, false));
            Interlocked.Increment(ref SQLiteConnectionPool._poolClosed);
          }
          else
            handle.Close();
          GC.KeepAlive((object) handle);
        }
      }
    }

    internal static SQLiteConnectionHandle Remove(
      string fileName,
      int maxPoolSize,
      out int version)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
        return connectionPool.Remove(fileName, maxPoolSize, out version) as SQLiteConnectionHandle;
      int poolVersion;
      Queue<WeakReference> collection;
      lock (SQLiteConnectionPool._syncRoot)
      {
        version = SQLiteConnectionPool._poolVersion;
        SQLiteConnectionPool.PoolQueue queue;
        if (!SQLiteConnectionPool._queueList.TryGetValue(fileName, out queue))
        {
          queue = new SQLiteConnectionPool.PoolQueue(SQLiteConnectionPool._poolVersion, maxPoolSize);
          SQLiteConnectionPool._queueList.Add(fileName, queue);
          return (SQLiteConnectionHandle) null;
        }
        version = poolVersion = queue.PoolVersion;
        queue.MaxPoolSize = maxPoolSize;
        SQLiteConnectionPool.ResizePool(queue, false);
        collection = queue.Queue;
        if (collection == null)
          return (SQLiteConnectionHandle) null;
        SQLiteConnectionPool._queueList.Remove(fileName);
        collection = new Queue<WeakReference>((IEnumerable<WeakReference>) collection);
      }
      try
      {
        while (collection.Count > 0)
        {
          WeakReference weakReference = collection.Dequeue();
          if (weakReference != null && weakReference.Target is SQLiteConnectionHandle target)
          {
            GC.SuppressFinalize((object) target);
            try
            {
              GC.WaitForPendingFinalizers();
              if (!target.IsInvalid)
              {
                if (!target.IsClosed)
                {
                  Interlocked.Increment(ref SQLiteConnectionPool._poolOpened);
                  return target;
                }
              }
            }
            finally
            {
              GC.ReRegisterForFinalize((object) target);
            }
            GC.KeepAlive((object) target);
          }
        }
      }
      finally
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          SQLiteConnectionPool.PoolQueue queue1;
          bool flag;
          if (SQLiteConnectionPool._queueList.TryGetValue(fileName, out queue1))
          {
            flag = false;
          }
          else
          {
            flag = true;
            queue1 = new SQLiteConnectionPool.PoolQueue(poolVersion, maxPoolSize);
          }
          Queue<WeakReference> queue2 = queue1.Queue;
          while (collection.Count > 0)
            queue2.Enqueue(collection.Dequeue());
          SQLiteConnectionPool.ResizePool(queue1, false);
          if (flag)
            SQLiteConnectionPool._queueList.Add(fileName, queue1);
        }
      }
      return (SQLiteConnectionHandle) null;
    }

    internal static ISQLiteConnectionPool GetConnectionPool()
    {
      lock (SQLiteConnectionPool._syncRoot)
        return SQLiteConnectionPool._connectionPool;
    }

    internal static void SetConnectionPool(ISQLiteConnectionPool connectionPool)
    {
      lock (SQLiteConnectionPool._syncRoot)
        SQLiteConnectionPool._connectionPool = connectionPool;
    }

    private static void ResizePool(SQLiteConnectionPool.PoolQueue queue, bool add)
    {
      int maxPoolSize = queue.MaxPoolSize;
      if (add && maxPoolSize > 0)
        --maxPoolSize;
      Queue<WeakReference> queue1 = queue.Queue;
      if (queue1 == null)
        return;
      while (queue1.Count > maxPoolSize)
      {
        WeakReference weakReference = queue1.Dequeue();
        if (weakReference != null)
        {
          if (weakReference.Target is SQLiteConnectionHandle target)
            target.Dispose();
          GC.KeepAlive((object) target);
        }
      }
    }

    private sealed class PoolQueue
    {
      internal readonly Queue<WeakReference> Queue = new Queue<WeakReference>();
      internal int PoolVersion;
      internal int MaxPoolSize;

      internal PoolQueue(int version, int maxSize)
      {
        this.PoolVersion = version;
        this.MaxPoolSize = maxSize;
      }
    }
  }
}
