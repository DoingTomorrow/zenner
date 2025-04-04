// Decompiled with JetBrains decompiler
// Type: SQLitePCL.SQLiteConnection
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SQLitePCL
{
  public class SQLiteConnection : ISQLiteConnection, IDisposable
  {
    private static bool temporaryDirectorySet = false;
    private static object syncTDS = new object();
    private IPlatformMarshal platformMarshal;
    private IPlatformStorage platformStorage;
    private ISQLite3Provider sqlite3Provider;
    private IntPtr db;
    private bool disposed;
    private IDictionary<string, Delegate> functionDelegates = (IDictionary<string, Delegate>) new Dictionary<string, Delegate>();
    private IDictionary<string, Delegate> aggregateStepDelegates = (IDictionary<string, Delegate>) new Dictionary<string, Delegate>();
    private IDictionary<string, Delegate> aggregateFinalDelegates = (IDictionary<string, Delegate>) new Dictionary<string, Delegate>();
    private IDictionary<Guid, IDictionary<string, object>> aggregateContextDataDic = (IDictionary<Guid, IDictionary<string, object>>) new Dictionary<Guid, IDictionary<string, object>>();

    public SQLiteConnection(string fileName)
      : this(fileName, true)
    {
    }

    private SQLiteConnection(string fileName, bool setTemporaryDirectory)
    {
      this.platformMarshal = Platform.Instance.PlatformMarshal;
      this.platformStorage = Platform.Instance.PlatformStorage;
      this.sqlite3Provider = Platform.Instance.SQLite3Provider;
      if (setTemporaryDirectory)
        this.SetTemporaryDirectory();
      string managedString = string.Empty;
      if (fileName.Trim().ToLowerInvariant() == ":memory:")
        managedString = ":memory:";
      else if (fileName.Trim() != string.Empty)
        managedString = this.platformStorage.GetLocalFilePath(fileName);
      IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(managedString);
      try
      {
        if (this.sqlite3Provider.Sqlite3Open(nativeUtF8, out this.db) == 0)
          return;
        if (this.db != IntPtr.Zero)
        {
          string managed = this.platformMarshal.MarshalStringNativeUTF8ToManaged(this.sqlite3Provider.Sqlite3Errmsg(this.db));
          this.sqlite3Provider.Sqlite3CloseV2(this.db);
          throw new SQLiteException("Unable to open the database file: " + fileName + " Details: " + managed);
        }
        throw new SQLiteException("Unable to open the database file: " + fileName);
      }
      catch (SQLiteException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new SQLiteException("Unable to open the database file: " + fileName, ex);
      }
      finally
      {
        if (nativeUtF8 != IntPtr.Zero)
          this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
      }
    }

    ~SQLiteConnection() => this.Dispose(false);

    public ISQLiteStatement Prepare(string sql)
    {
      int size;
      IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(sql, out size);
      try
      {
        IntPtr stm;
        if (this.sqlite3Provider.Sqlite3PrepareV2(this.db, nativeUtF8, size, out stm, IntPtr.Zero) != 0)
        {
          string managed = this.platformMarshal.MarshalStringNativeUTF8ToManaged(this.sqlite3Provider.Sqlite3Errmsg(this.db));
          throw new SQLiteException("Unable to prepare the sql statement: " + sql + " Details: " + managed);
        }
        return (ISQLiteStatement) new SQLiteStatement(this, stm);
      }
      catch (SQLiteException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new SQLiteException("Unable to prepare the sql statement: " + sql, ex);
      }
      finally
      {
        if (nativeUtF8 != IntPtr.Zero)
          this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
      }
    }

    public void CreateFunction(
      string name,
      int numberOfArguments,
      Function function,
      bool deterministic)
    {
      name = name.ToUpperInvariant();
      Delegate function1 = this.platformMarshal.ApplyNativeCallingConventionToFunction((FunctionNative) ((context, numberArguments, nativeArguments) =>
      {
        object[] managedArguments = this.ObtainManagedArguments(nativeArguments);
        try
        {
          object result = function(managedArguments);
          this.SetResult(context, result);
        }
        catch (Exception ex)
        {
          this.SetError(context, ex);
        }
      }));
      this.functionDelegates[name] = function1;
      IntPtr nativeFunctionPointer = this.platformMarshal.MarshalDelegateToNativeFunctionPointer(function1);
      IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(name, out int _);
      try
      {
        this.sqlite3Provider.Sqlite3CreateFunction(this.db, nativeUtF8, numberOfArguments, deterministic, nativeFunctionPointer);
      }
      finally
      {
        if (nativeUtF8 != IntPtr.Zero)
          this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
      }
    }

    public void CreateAggregate(
      string name,
      int numberOfArguments,
      AggregateStep step,
      AggregateFinal final)
    {
      name = name.ToUpperInvariant();
      Delegate aggregateStep = this.platformMarshal.ApplyNativeCallingConventionToAggregateStep((AggregateStepNative) ((context, numberArguments, nativeArguments) =>
      {
        IntPtr num = this.sqlite3Provider.Sqlite3AggregateContext(context, 16);
        if (num != IntPtr.Zero)
        {
          byte[] numArray = new byte[16];
          this.platformMarshal.Copy(num, numArray, 0, 16);
          Guid key = new Guid(numArray);
          if (key == Guid.Empty)
          {
            key = Guid.NewGuid();
            this.platformMarshal.Copy(key.ToByteArray(), num, 0, 16);
          }
          object[] managedArguments = this.ObtainManagedArguments(nativeArguments);
          if (!this.aggregateContextDataDic.ContainsKey(key))
            this.aggregateContextDataDic[key] = (IDictionary<string, object>) new Dictionary<string, object>();
          IDictionary<string, object> aggregateContextData = this.aggregateContextDataDic[key];
          try
          {
            step(aggregateContextData, managedArguments);
          }
          catch (Exception ex)
          {
            this.SetError(context, ex);
          }
        }
        else
          this.SetError(context, new Exception("Unable to initialize aggregate context."));
      }));
      this.aggregateStepDelegates[name] = aggregateStep;
      IntPtr nativeFunctionPointer1 = this.platformMarshal.MarshalDelegateToNativeFunctionPointer(aggregateStep);
      Delegate aggregateFinal = this.platformMarshal.ApplyNativeCallingConventionToAggregateFinal((AggregateFinalNative) (context =>
      {
        IDictionary<string, object> aggregateContextData = (IDictionary<string, object>) new Dictionary<string, object>();
        IntPtr source = this.sqlite3Provider.Sqlite3AggregateContext(context, 0);
        if (source != IntPtr.Zero)
        {
          byte[] numArray = new byte[16];
          this.platformMarshal.Copy(source, numArray, 0, 16);
          aggregateContextData = this.aggregateContextDataDic[new Guid(numArray)];
        }
        try
        {
          object result = final(aggregateContextData);
          this.SetResult(context, result);
        }
        catch (Exception ex)
        {
          this.SetError(context, ex);
        }
      }));
      this.aggregateFinalDelegates[name] = aggregateFinal;
      IntPtr nativeFunctionPointer2 = this.platformMarshal.MarshalDelegateToNativeFunctionPointer(aggregateFinal);
      IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(name, out int _);
      try
      {
        this.sqlite3Provider.Sqlite3CreateAggregate(this.db, nativeUtF8, numberOfArguments, nativeFunctionPointer1, nativeFunctionPointer2);
      }
      finally
      {
        if (nativeUtF8 != IntPtr.Zero)
          this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
      }
    }

    public long LastInsertRowId()
    {
      try
      {
        return this.sqlite3Provider.Sqlite3LastInsertRowId(this.db);
      }
      catch (SQLiteException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new SQLiteException("Unable to retrieve the last inserted row id.", ex);
      }
    }

    public string ErrorMessage()
    {
      try
      {
        return this.platformMarshal.MarshalStringNativeUTF8ToManaged(this.sqlite3Provider.Sqlite3Errmsg(this.db));
      }
      catch (SQLiteException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new SQLiteException("Unable to retrieve the error message.", ex);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.sqlite3Provider.Sqlite3CloseV2(this.db);
      this.db = IntPtr.Zero;
      this.disposed = true;
    }

    private object[] ObtainManagedArguments(IntPtr[] nativeArguments)
    {
      object[] managedArguments = (object[]) null;
      if (nativeArguments != null)
      {
        managedArguments = new object[nativeArguments.Length];
        for (int index = 0; index < nativeArguments.Length; ++index)
        {
          IntPtr nativeArgument = nativeArguments[index];
          object destination = (object) null;
          switch (this.sqlite3Provider.Sqlite3ValueType(nativeArgument))
          {
            case 1:
              destination = (object) this.sqlite3Provider.Sqlite3ValueInt64(nativeArgument);
              break;
            case 2:
              destination = (object) this.sqlite3Provider.Sqlite3ValueDouble(nativeArgument);
              break;
            case 3:
              destination = (object) this.platformMarshal.MarshalStringNativeUTF8ToManaged(this.sqlite3Provider.Sqlite3ValueText(nativeArgument));
              break;
            case 4:
              IntPtr source = this.sqlite3Provider.Sqlite3ValueBlob(nativeArgument);
              int length = this.sqlite3Provider.Sqlite3ValueBytes(nativeArgument);
              destination = (object) new byte[length];
              this.platformMarshal.Copy(source, (byte[]) destination, 0, length);
              break;
          }
          managedArguments[index] = destination;
        }
      }
      return managedArguments;
    }

    private void SetResult(IntPtr context, object result)
    {
      switch (result)
      {
        case null:
          this.sqlite3Provider.Sqlite3ResultNull(context);
          break;
        case int num1:
          this.sqlite3Provider.Sqlite3ResultInt(context, num1);
          break;
        case long num2:
          this.sqlite3Provider.Sqlite3ResultInt64(context, num2);
          break;
        case double num3:
          this.sqlite3Provider.Sqlite3ResultDouble(context, num3);
          break;
        case string _:
          int size;
          IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8((string) result, out size);
          try
          {
            this.sqlite3Provider.Sqlite3ResultText(context, nativeUtF8, size - 1, (IntPtr) -1);
            break;
          }
          finally
          {
            if (nativeUtF8 != IntPtr.Zero)
              this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
          }
        case byte[] _:
          this.sqlite3Provider.Sqlite3ResultBlob(context, (byte[]) result, ((byte[]) result).Length, (IntPtr) -1);
          break;
      }
    }

    private void SetError(IntPtr context, Exception ex)
    {
      int size;
      IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(ex.Message, out size);
      try
      {
        this.sqlite3Provider.Sqlite3ResultError(context, nativeUtF8, size - 1);
      }
      finally
      {
        if (nativeUtF8 != IntPtr.Zero)
          this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
      }
    }

    private void SetTemporaryDirectory()
    {
      lock (SQLiteConnection.syncTDS)
      {
        if (SQLiteConnection.temporaryDirectorySet)
          return;
        try
        {
          using (SQLiteConnection sqLiteConnection = new SQLiteConnection(":memory:", false))
          {
            string temporaryDirectoryPath = this.platformStorage.GetTemporaryDirectoryPath();
            using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare(string.Format("PRAGMA temp_store_directory = '{0}';", new object[1]
            {
              (object) temporaryDirectoryPath
            })))
            {
              SQLiteResult sqLiteResult = sqLiteStatement.Step();
              if (sqLiteResult != SQLiteResult.DONE)
              {
                string str = sqLiteConnection.ErrorMessage();
                throw new SQLiteException("Unable to set temporary directory: " + temporaryDirectoryPath + " Result: " + sqLiteResult.ToString() + " Details: " + str);
              }
            }
          }
          SQLiteConnection.temporaryDirectorySet = true;
        }
        catch (Exception ex)
        {
          throw new SQLiteException("Unable to set temporary directory.", ex);
        }
      }
    }
  }
}
