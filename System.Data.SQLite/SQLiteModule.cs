// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModule
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  public abstract class SQLiteModule : ISQLiteManagedModule, IDisposable
  {
    private static readonly int DefaultModuleVersion = 2;
    private UnsafeNativeMethods.sqlite3_module nativeModule;
    private UnsafeNativeMethods.xDestroyModule destroyModule;
    private IntPtr disposableModule;
    private Dictionary<IntPtr, SQLiteVirtualTable> tables;
    private Dictionary<IntPtr, SQLiteVirtualTableCursor> cursors;
    private Dictionary<string, SQLiteFunction> functions;
    private bool logErrors;
    private bool logExceptions;
    private bool declared;
    private string name;
    private bool disposed;

    public SQLiteModule(string name)
    {
      this.name = name != null ? name : throw new ArgumentNullException(nameof (name));
      this.tables = new Dictionary<IntPtr, SQLiteVirtualTable>();
      this.cursors = new Dictionary<IntPtr, SQLiteVirtualTableCursor>();
      this.functions = new Dictionary<string, SQLiteFunction>();
    }

    internal bool CreateDisposableModule(IntPtr pDb)
    {
      if (this.disposableModule != IntPtr.Zero)
        return true;
      IntPtr num = IntPtr.Zero;
      try
      {
        num = SQLiteString.Utf8IntPtrFromString(this.name);
        UnsafeNativeMethods.sqlite3_module module = this.AllocateNativeModule();
        this.destroyModule = new UnsafeNativeMethods.xDestroyModule(this.xDestroyModule);
        this.disposableModule = UnsafeNativeMethods.sqlite3_create_disposable_module(pDb, num, ref module, IntPtr.Zero, this.destroyModule);
        return this.disposableModule != IntPtr.Zero;
      }
      finally
      {
        if (num != IntPtr.Zero)
        {
          SQLiteMemory.Free(num);
          IntPtr zero = IntPtr.Zero;
        }
      }
    }

    private void xDestroyModule(IntPtr pClientData) => this.disposableModule = IntPtr.Zero;

    private UnsafeNativeMethods.sqlite3_module AllocateNativeModule()
    {
      return this.AllocateNativeModule(this.GetNativeModuleImpl());
    }

    private UnsafeNativeMethods.sqlite3_module AllocateNativeModule(ISQLiteNativeModule module)
    {
      this.nativeModule = new UnsafeNativeMethods.sqlite3_module();
      this.nativeModule.iVersion = SQLiteModule.DefaultModuleVersion;
      if (module != null)
      {
        this.nativeModule.xCreate = new UnsafeNativeMethods.xCreate(module.xCreate);
        this.nativeModule.xConnect = new UnsafeNativeMethods.xConnect(module.xConnect);
        this.nativeModule.xBestIndex = new UnsafeNativeMethods.xBestIndex(module.xBestIndex);
        this.nativeModule.xDisconnect = new UnsafeNativeMethods.xDisconnect(module.xDisconnect);
        this.nativeModule.xDestroy = new UnsafeNativeMethods.xDestroy(module.xDestroy);
        this.nativeModule.xOpen = new UnsafeNativeMethods.xOpen(module.xOpen);
        this.nativeModule.xClose = new UnsafeNativeMethods.xClose(module.xClose);
        this.nativeModule.xFilter = new UnsafeNativeMethods.xFilter(module.xFilter);
        this.nativeModule.xNext = new UnsafeNativeMethods.xNext(module.xNext);
        this.nativeModule.xEof = new UnsafeNativeMethods.xEof(module.xEof);
        this.nativeModule.xColumn = new UnsafeNativeMethods.xColumn(module.xColumn);
        this.nativeModule.xRowId = new UnsafeNativeMethods.xRowId(module.xRowId);
        this.nativeModule.xUpdate = new UnsafeNativeMethods.xUpdate(module.xUpdate);
        this.nativeModule.xBegin = new UnsafeNativeMethods.xBegin(module.xBegin);
        this.nativeModule.xSync = new UnsafeNativeMethods.xSync(module.xSync);
        this.nativeModule.xCommit = new UnsafeNativeMethods.xCommit(module.xCommit);
        this.nativeModule.xRollback = new UnsafeNativeMethods.xRollback(module.xRollback);
        this.nativeModule.xFindFunction = new UnsafeNativeMethods.xFindFunction(module.xFindFunction);
        this.nativeModule.xRename = new UnsafeNativeMethods.xRename(module.xRename);
        this.nativeModule.xSavepoint = new UnsafeNativeMethods.xSavepoint(module.xSavepoint);
        this.nativeModule.xRelease = new UnsafeNativeMethods.xRelease(module.xRelease);
        this.nativeModule.xRollbackTo = new UnsafeNativeMethods.xRollbackTo(module.xRollbackTo);
      }
      else
      {
        this.nativeModule.xCreate = new UnsafeNativeMethods.xCreate(this.xCreate);
        this.nativeModule.xConnect = new UnsafeNativeMethods.xConnect(this.xConnect);
        this.nativeModule.xBestIndex = new UnsafeNativeMethods.xBestIndex(this.xBestIndex);
        this.nativeModule.xDisconnect = new UnsafeNativeMethods.xDisconnect(this.xDisconnect);
        this.nativeModule.xDestroy = new UnsafeNativeMethods.xDestroy(this.xDestroy);
        this.nativeModule.xOpen = new UnsafeNativeMethods.xOpen(this.xOpen);
        this.nativeModule.xClose = new UnsafeNativeMethods.xClose(this.xClose);
        this.nativeModule.xFilter = new UnsafeNativeMethods.xFilter(this.xFilter);
        this.nativeModule.xNext = new UnsafeNativeMethods.xNext(this.xNext);
        this.nativeModule.xEof = new UnsafeNativeMethods.xEof(this.xEof);
        this.nativeModule.xColumn = new UnsafeNativeMethods.xColumn(this.xColumn);
        this.nativeModule.xRowId = new UnsafeNativeMethods.xRowId(this.xRowId);
        this.nativeModule.xUpdate = new UnsafeNativeMethods.xUpdate(this.xUpdate);
        this.nativeModule.xBegin = new UnsafeNativeMethods.xBegin(this.xBegin);
        this.nativeModule.xSync = new UnsafeNativeMethods.xSync(this.xSync);
        this.nativeModule.xCommit = new UnsafeNativeMethods.xCommit(this.xCommit);
        this.nativeModule.xRollback = new UnsafeNativeMethods.xRollback(this.xRollback);
        this.nativeModule.xFindFunction = new UnsafeNativeMethods.xFindFunction(this.xFindFunction);
        this.nativeModule.xRename = new UnsafeNativeMethods.xRename(this.xRename);
        this.nativeModule.xSavepoint = new UnsafeNativeMethods.xSavepoint(this.xSavepoint);
        this.nativeModule.xRelease = new UnsafeNativeMethods.xRelease(this.xRelease);
        this.nativeModule.xRollbackTo = new UnsafeNativeMethods.xRollbackTo(this.xRollbackTo);
      }
      return this.nativeModule;
    }

    private UnsafeNativeMethods.sqlite3_module CopyNativeModule(
      UnsafeNativeMethods.sqlite3_module module)
    {
      return new UnsafeNativeMethods.sqlite3_module()
      {
        iVersion = module.iVersion,
        xCreate = new UnsafeNativeMethods.xCreate((module.xCreate != null ? module.xCreate : new UnsafeNativeMethods.xCreate(this.xCreate)).Invoke),
        xConnect = new UnsafeNativeMethods.xConnect((module.xConnect != null ? module.xConnect : new UnsafeNativeMethods.xConnect(this.xConnect)).Invoke),
        xBestIndex = new UnsafeNativeMethods.xBestIndex((module.xBestIndex != null ? module.xBestIndex : new UnsafeNativeMethods.xBestIndex(this.xBestIndex)).Invoke),
        xDisconnect = new UnsafeNativeMethods.xDisconnect((module.xDisconnect != null ? module.xDisconnect : new UnsafeNativeMethods.xDisconnect(this.xDisconnect)).Invoke),
        xDestroy = new UnsafeNativeMethods.xDestroy((module.xDestroy != null ? module.xDestroy : new UnsafeNativeMethods.xDestroy(this.xDestroy)).Invoke),
        xOpen = new UnsafeNativeMethods.xOpen((module.xOpen != null ? module.xOpen : new UnsafeNativeMethods.xOpen(this.xOpen)).Invoke),
        xClose = new UnsafeNativeMethods.xClose((module.xClose != null ? module.xClose : new UnsafeNativeMethods.xClose(this.xClose)).Invoke),
        xFilter = new UnsafeNativeMethods.xFilter((module.xFilter != null ? module.xFilter : new UnsafeNativeMethods.xFilter(this.xFilter)).Invoke),
        xNext = new UnsafeNativeMethods.xNext((module.xNext != null ? module.xNext : new UnsafeNativeMethods.xNext(this.xNext)).Invoke),
        xEof = new UnsafeNativeMethods.xEof((module.xEof != null ? module.xEof : new UnsafeNativeMethods.xEof(this.xEof)).Invoke),
        xColumn = new UnsafeNativeMethods.xColumn((module.xColumn != null ? module.xColumn : new UnsafeNativeMethods.xColumn(this.xColumn)).Invoke),
        xRowId = new UnsafeNativeMethods.xRowId((module.xRowId != null ? module.xRowId : new UnsafeNativeMethods.xRowId(this.xRowId)).Invoke),
        xUpdate = new UnsafeNativeMethods.xUpdate((module.xUpdate != null ? module.xUpdate : new UnsafeNativeMethods.xUpdate(this.xUpdate)).Invoke),
        xBegin = new UnsafeNativeMethods.xBegin((module.xBegin != null ? module.xBegin : new UnsafeNativeMethods.xBegin(this.xBegin)).Invoke),
        xSync = new UnsafeNativeMethods.xSync((module.xSync != null ? module.xSync : new UnsafeNativeMethods.xSync(this.xSync)).Invoke),
        xCommit = new UnsafeNativeMethods.xCommit((module.xCommit != null ? module.xCommit : new UnsafeNativeMethods.xCommit(this.xCommit)).Invoke),
        xRollback = new UnsafeNativeMethods.xRollback((module.xRollback != null ? module.xRollback : new UnsafeNativeMethods.xRollback(this.xRollback)).Invoke),
        xFindFunction = new UnsafeNativeMethods.xFindFunction((module.xFindFunction != null ? module.xFindFunction : new UnsafeNativeMethods.xFindFunction(this.xFindFunction)).Invoke),
        xRename = new UnsafeNativeMethods.xRename((module.xRename != null ? module.xRename : new UnsafeNativeMethods.xRename(this.xRename)).Invoke),
        xSavepoint = new UnsafeNativeMethods.xSavepoint((module.xSavepoint != null ? module.xSavepoint : new UnsafeNativeMethods.xSavepoint(this.xSavepoint)).Invoke),
        xRelease = new UnsafeNativeMethods.xRelease((module.xRelease != null ? module.xRelease : new UnsafeNativeMethods.xRelease(this.xRelease)).Invoke),
        xRollbackTo = new UnsafeNativeMethods.xRollbackTo((module.xRollbackTo != null ? module.xRollbackTo : new UnsafeNativeMethods.xRollbackTo(this.xRollbackTo)).Invoke)
      };
    }

    private SQLiteErrorCode CreateOrConnect(
      bool create,
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError)
    {
      try
      {
        string fileName = SQLiteString.StringFromUtf8IntPtr(UnsafeNativeMethods.sqlite3_db_filename(pDb, IntPtr.Zero));
        using (SQLiteConnection connection = new SQLiteConnection(pDb, fileName, false))
        {
          SQLiteVirtualTable table = (SQLiteVirtualTable) null;
          string error = (string) null;
          if (create && this.Create(connection, pAux, SQLiteString.StringArrayFromUtf8SizeAndIntPtr(argc, argv), ref table, ref error) == SQLiteErrorCode.Ok || !create && this.Connect(connection, pAux, SQLiteString.StringArrayFromUtf8SizeAndIntPtr(argc, argv), ref table, ref error) == SQLiteErrorCode.Ok)
          {
            if (table != null)
            {
              pVtab = this.TableToIntPtr(table);
              return SQLiteErrorCode.Ok;
            }
            pError = SQLiteString.Utf8IntPtrFromString("no table was created");
          }
          else
            pError = SQLiteString.Utf8IntPtrFromString(error);
        }
      }
      catch (Exception ex)
      {
        pError = SQLiteString.Utf8IntPtrFromString(ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode DestroyOrDisconnect(bool destroy, IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          if (!destroy || this.Destroy(table) != SQLiteErrorCode.Ok)
          {
            if (!destroy)
            {
              if (this.Disconnect(table) != SQLiteErrorCode.Ok)
                goto label_12;
            }
            else
              goto label_12;
          }
          if (this.tables != null)
            this.tables.Remove(pVtab);
          return SQLiteErrorCode.Ok;
        }
      }
      catch (Exception ex)
      {
        try
        {
          if (this.LogExceptionsNoThrow)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", destroy ? (object) "xDestroy" : (object) "xDisconnect", (object) ex));
        }
        catch
        {
        }
      }
      finally
      {
        this.FreeTable(pVtab);
      }
label_12:
      return SQLiteErrorCode.Error;
    }

    private static bool SetTableError(
      SQLiteModule module,
      IntPtr pVtab,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      try
      {
        if (logErrors)
          SQLiteLog.LogMessage(SQLiteErrorCode.Error, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Virtual table error: {0}", (object) error));
      }
      catch
      {
      }
      bool flag = false;
      IntPtr pMemory1 = IntPtr.Zero;
      try
      {
        if (pVtab == IntPtr.Zero)
          return false;
        int offset = SQLiteMarshal.NextOffsetOf(SQLiteMarshal.NextOffsetOf(0, IntPtr.Size, 4), 4, IntPtr.Size);
        IntPtr pMemory2 = SQLiteMarshal.ReadIntPtr(pVtab, offset);
        if (pMemory2 != IntPtr.Zero)
        {
          SQLiteMemory.Free(pMemory2);
          IntPtr zero = IntPtr.Zero;
          SQLiteMarshal.WriteIntPtr(pVtab, offset, zero);
        }
        if (error == null)
          return true;
        pMemory1 = SQLiteString.Utf8IntPtrFromString(error);
        SQLiteMarshal.WriteIntPtr(pVtab, offset, pMemory1);
        flag = true;
      }
      catch (Exception ex)
      {
        try
        {
          if (logExceptions)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"SetTableError\" method: {0}", (object) ex));
        }
        catch
        {
        }
      }
      finally
      {
        if (!flag && pMemory1 != IntPtr.Zero)
        {
          SQLiteMemory.Free(pMemory1);
          IntPtr zero = IntPtr.Zero;
        }
      }
      return flag;
    }

    private static bool SetTableError(
      SQLiteModule module,
      SQLiteVirtualTable table,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      if (table == null)
        return false;
      IntPtr nativeHandle = table.NativeHandle;
      return !(nativeHandle == IntPtr.Zero) && SQLiteModule.SetTableError(module, nativeHandle, logErrors, logExceptions, error);
    }

    private static bool SetCursorError(
      SQLiteModule module,
      IntPtr pCursor,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      if (pCursor == IntPtr.Zero)
        return false;
      IntPtr pVtab = SQLiteModule.TableFromCursor(module, pCursor);
      return !(pVtab == IntPtr.Zero) && SQLiteModule.SetTableError(module, pVtab, logErrors, logExceptions, error);
    }

    private static bool SetCursorError(
      SQLiteModule module,
      SQLiteVirtualTableCursor cursor,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      if (cursor == null)
        return false;
      IntPtr nativeHandle = cursor.NativeHandle;
      return !(nativeHandle == IntPtr.Zero) && SQLiteModule.SetCursorError(module, nativeHandle, logErrors, logExceptions, error);
    }

    protected virtual ISQLiteNativeModule GetNativeModuleImpl() => (ISQLiteNativeModule) null;

    protected virtual ISQLiteNativeModule CreateNativeModuleImpl()
    {
      return (ISQLiteNativeModule) new SQLiteModule.SQLiteNativeModule(this);
    }

    protected virtual IntPtr AllocateTable()
    {
      return SQLiteMemory.Allocate(Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_vtab)));
    }

    protected virtual void ZeroTable(IntPtr pVtab)
    {
      if (pVtab == IntPtr.Zero)
        return;
      int offset1 = 0;
      SQLiteMarshal.WriteIntPtr(pVtab, offset1, IntPtr.Zero);
      int offset2 = SQLiteMarshal.NextOffsetOf(offset1, IntPtr.Size, 4);
      SQLiteMarshal.WriteInt32(pVtab, offset2, 0);
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, 4, IntPtr.Size);
      SQLiteMarshal.WriteIntPtr(pVtab, offset3, IntPtr.Zero);
    }

    protected virtual void FreeTable(IntPtr pVtab)
    {
      this.SetTableError(pVtab, (string) null);
      SQLiteMemory.Free(pVtab);
    }

    protected virtual IntPtr AllocateCursor()
    {
      return SQLiteMemory.Allocate(Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_vtab_cursor)));
    }

    protected virtual void FreeCursor(IntPtr pCursor) => SQLiteMemory.Free(pCursor);

    private static IntPtr TableFromCursor(SQLiteModule module, IntPtr pCursor)
    {
      return pCursor == IntPtr.Zero ? IntPtr.Zero : Marshal.ReadIntPtr(pCursor);
    }

    protected virtual IntPtr TableFromCursor(IntPtr pCursor)
    {
      return SQLiteModule.TableFromCursor(this, pCursor);
    }

    protected virtual SQLiteVirtualTable TableFromIntPtr(IntPtr pVtab)
    {
      if (pVtab == IntPtr.Zero)
      {
        this.SetTableError(pVtab, "invalid native table");
        return (SQLiteVirtualTable) null;
      }
      SQLiteVirtualTable liteVirtualTable;
      if (this.tables != null && this.tables.TryGetValue(pVtab, out liteVirtualTable))
        return liteVirtualTable;
      this.SetTableError(pVtab, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "managed table for {0} not found", (object) pVtab));
      return (SQLiteVirtualTable) null;
    }

    protected virtual IntPtr TableToIntPtr(SQLiteVirtualTable table)
    {
      if (table == null || this.tables == null)
        return IntPtr.Zero;
      IntPtr intPtr = IntPtr.Zero;
      bool flag = false;
      try
      {
        intPtr = this.AllocateTable();
        if (intPtr != IntPtr.Zero)
        {
          this.ZeroTable(intPtr);
          table.NativeHandle = intPtr;
          this.tables.Add(intPtr, table);
          flag = true;
        }
      }
      finally
      {
        if (!flag && intPtr != IntPtr.Zero)
        {
          this.FreeTable(intPtr);
          intPtr = IntPtr.Zero;
        }
      }
      return intPtr;
    }

    protected virtual SQLiteVirtualTableCursor CursorFromIntPtr(IntPtr pVtab, IntPtr pCursor)
    {
      if (pCursor == IntPtr.Zero)
      {
        this.SetTableError(pVtab, "invalid native cursor");
        return (SQLiteVirtualTableCursor) null;
      }
      SQLiteVirtualTableCursor virtualTableCursor;
      if (this.cursors != null && this.cursors.TryGetValue(pCursor, out virtualTableCursor))
        return virtualTableCursor;
      this.SetTableError(pVtab, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "managed cursor for {0} not found", (object) pCursor));
      return (SQLiteVirtualTableCursor) null;
    }

    protected virtual IntPtr CursorToIntPtr(SQLiteVirtualTableCursor cursor)
    {
      if (cursor == null || this.cursors == null)
        return IntPtr.Zero;
      IntPtr intPtr = IntPtr.Zero;
      bool flag = false;
      try
      {
        intPtr = this.AllocateCursor();
        if (intPtr != IntPtr.Zero)
        {
          cursor.NativeHandle = intPtr;
          this.cursors.Add(intPtr, cursor);
          flag = true;
        }
      }
      finally
      {
        if (!flag && intPtr != IntPtr.Zero)
        {
          this.FreeCursor(intPtr);
          intPtr = IntPtr.Zero;
        }
      }
      return intPtr;
    }

    protected virtual string GetFunctionKey(
      int argumentCount,
      string name,
      SQLiteFunction function)
    {
      return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}:{1}", (object) argumentCount, (object) name);
    }

    protected virtual SQLiteErrorCode DeclareTable(
      SQLiteConnection connection,
      string sql,
      ref string error)
    {
      if (connection == null)
      {
        error = "invalid connection";
        return SQLiteErrorCode.Error;
      }
      SQLiteBase sql1 = connection._sql;
      if (sql1 == null)
      {
        error = "connection has invalid handle";
        return SQLiteErrorCode.Error;
      }
      if (sql != null)
        return sql1.DeclareVirtualTable(this, sql, ref error);
      error = "invalid SQL statement";
      return SQLiteErrorCode.Error;
    }

    protected virtual SQLiteErrorCode DeclareFunction(
      SQLiteConnection connection,
      int argumentCount,
      string name,
      ref string error)
    {
      if (connection == null)
      {
        error = "invalid connection";
        return SQLiteErrorCode.Error;
      }
      SQLiteBase sql = connection._sql;
      if (sql != null)
        return sql.DeclareVirtualFunction(this, argumentCount, name, ref error);
      error = "connection has invalid handle";
      return SQLiteErrorCode.Error;
    }

    protected virtual bool LogErrorsNoThrow
    {
      get => this.logErrors;
      set => this.logErrors = value;
    }

    protected virtual bool LogExceptionsNoThrow
    {
      get => this.logExceptions;
      set => this.logExceptions = value;
    }

    protected virtual bool SetTableError(IntPtr pVtab, string error)
    {
      return SQLiteModule.SetTableError(this, pVtab, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);
    }

    protected virtual bool SetTableError(SQLiteVirtualTable table, string error)
    {
      return SQLiteModule.SetTableError(this, table, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);
    }

    protected virtual bool SetCursorError(SQLiteVirtualTableCursor cursor, string error)
    {
      return SQLiteModule.SetCursorError(this, cursor, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);
    }

    protected virtual bool SetEstimatedCost(SQLiteIndex index, double? estimatedCost)
    {
      if (index == null || index.Outputs == null)
        return false;
      index.Outputs.EstimatedCost = estimatedCost;
      return true;
    }

    protected virtual bool SetEstimatedCost(SQLiteIndex index)
    {
      return this.SetEstimatedCost(index, new double?());
    }

    protected virtual bool SetEstimatedRows(SQLiteIndex index, long? estimatedRows)
    {
      if (index == null || index.Outputs == null)
        return false;
      index.Outputs.EstimatedRows = estimatedRows;
      return true;
    }

    protected virtual bool SetEstimatedRows(SQLiteIndex index)
    {
      return this.SetEstimatedRows(index, new long?());
    }

    protected virtual bool SetIndexFlags(SQLiteIndex index, SQLiteIndexFlags? indexFlags)
    {
      if (index == null || index.Outputs == null)
        return false;
      index.Outputs.IndexFlags = indexFlags;
      return true;
    }

    protected virtual bool SetIndexFlags(SQLiteIndex index)
    {
      return this.SetIndexFlags(index, new SQLiteIndexFlags?());
    }

    public virtual bool LogErrors
    {
      get
      {
        this.CheckDisposed();
        return this.LogErrorsNoThrow;
      }
      set
      {
        this.CheckDisposed();
        this.LogErrorsNoThrow = value;
      }
    }

    public virtual bool LogExceptions
    {
      get
      {
        this.CheckDisposed();
        return this.LogExceptionsNoThrow;
      }
      set
      {
        this.CheckDisposed();
        this.LogExceptionsNoThrow = value;
      }
    }

    private SQLiteErrorCode xCreate(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError)
    {
      return this.CreateOrConnect(true, pDb, pAux, argc, argv, ref pVtab, ref pError);
    }

    private SQLiteErrorCode xConnect(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError)
    {
      return this.CreateOrConnect(false, pDb, pAux, argc, argv, ref pVtab, ref pError);
    }

    private SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          SQLiteIndex index = (SQLiteIndex) null;
          SQLiteIndex.FromIntPtr(pIndex, true, ref index);
          if (this.BestIndex(table, index) == SQLiteErrorCode.Ok)
          {
            SQLiteIndex.ToIntPtr(index, pIndex, true);
            return SQLiteErrorCode.Ok;
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xDisconnect(IntPtr pVtab) => this.DestroyOrDisconnect(false, pVtab);

    private SQLiteErrorCode xDestroy(IntPtr pVtab) => this.DestroyOrDisconnect(true, pVtab);

    private SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          SQLiteVirtualTableCursor cursor = (SQLiteVirtualTableCursor) null;
          if (this.Open(table, ref cursor) == SQLiteErrorCode.Ok)
          {
            if (cursor != null)
            {
              pCursor = this.CursorToIntPtr(cursor);
              if (pCursor != IntPtr.Zero)
                return SQLiteErrorCode.Ok;
              this.SetTableError(pVtab, "no native cursor was created");
            }
            else
              this.SetTableError(pVtab, "no managed cursor was created");
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xClose(IntPtr pCursor)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          if (this.Close(cursor) == SQLiteErrorCode.Ok)
          {
            if (this.cursors != null)
              this.cursors.Remove(pCursor);
            return SQLiteErrorCode.Ok;
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      finally
      {
        this.FreeCursor(pCursor);
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xFilter(
      IntPtr pCursor,
      int idxNum,
      IntPtr idxStr,
      int argc,
      IntPtr argv)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          if (this.Filter(cursor, idxNum, SQLiteString.StringFromUtf8IntPtr(idxStr), SQLiteValue.ArrayFromSizeAndIntPtr(argc, argv)) == SQLiteErrorCode.Ok)
            return SQLiteErrorCode.Ok;
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xNext(IntPtr pCursor)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          if (this.Next(cursor) == SQLiteErrorCode.Ok)
            return SQLiteErrorCode.Ok;
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private int xEof(IntPtr pCursor)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
          return this.Eof(cursor) ? 1 : 0;
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return 1;
    }

    private SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          SQLiteContext context = new SQLiteContext(pContext);
          return this.Column(cursor, context, index);
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
          return this.RowId(cursor, ref rowId);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Update(table, SQLiteValue.ArrayFromSizeAndIntPtr(argc, argv), ref rowId);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xBegin(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Begin(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xSync(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Sync(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xCommit(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Commit(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xRollback(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Rollback(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private int xFindFunction(
      IntPtr pVtab,
      int nArg,
      IntPtr zName,
      ref SQLiteCallback callback,
      ref IntPtr pClientData)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          string name = SQLiteString.StringFromUtf8IntPtr(zName);
          SQLiteFunction function = (SQLiteFunction) null;
          if (this.FindFunction(table, nArg, name, ref function, ref pClientData))
          {
            if (function != null)
            {
              this.functions[this.GetFunctionKey(nArg, name, function)] = function;
              callback = new SQLiteCallback(function.ScalarCallback);
              return 1;
            }
            this.SetTableError(pVtab, "no function was created");
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return 0;
    }

    private SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Rename(table, SQLiteString.StringFromUtf8IntPtr(zNew));
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Savepoint(table, iSavepoint);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Release(table, iSavepoint);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    private SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.RollbackTo(table, iSavepoint);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    public virtual bool Declared
    {
      get
      {
        this.CheckDisposed();
        return this.declared;
      }
      internal set => this.declared = value;
    }

    public virtual string Name
    {
      get
      {
        this.CheckDisposed();
        return this.name;
      }
    }

    public abstract SQLiteErrorCode Create(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error);

    public abstract SQLiteErrorCode Connect(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error);

    public abstract SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index);

    public abstract SQLiteErrorCode Disconnect(SQLiteVirtualTable table);

    public abstract SQLiteErrorCode Destroy(SQLiteVirtualTable table);

    public abstract SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor);

    public abstract SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor);

    public abstract SQLiteErrorCode Filter(
      SQLiteVirtualTableCursor cursor,
      int indexNumber,
      string indexString,
      SQLiteValue[] values);

    public abstract SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor);

    public abstract bool Eof(SQLiteVirtualTableCursor cursor);

    public abstract SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index);

    public abstract SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId);

    public abstract SQLiteErrorCode Update(
      SQLiteVirtualTable table,
      SQLiteValue[] values,
      ref long rowId);

    public abstract SQLiteErrorCode Begin(SQLiteVirtualTable table);

    public abstract SQLiteErrorCode Sync(SQLiteVirtualTable table);

    public abstract SQLiteErrorCode Commit(SQLiteVirtualTable table);

    public abstract SQLiteErrorCode Rollback(SQLiteVirtualTable table);

    public abstract bool FindFunction(
      SQLiteVirtualTable table,
      int argumentCount,
      string name,
      ref SQLiteFunction function,
      ref IntPtr pClientData);

    public abstract SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName);

    public abstract SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint);

    public abstract SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint);

    public abstract SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModule).Name);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this.functions != null)
          this.functions.Clear();
      }
      try
      {
        if (this.disposableModule != IntPtr.Zero)
        {
          UnsafeNativeMethods.sqlite3_dispose_module(this.disposableModule);
          this.disposableModule = IntPtr.Zero;
        }
      }
      catch (Exception ex)
      {
        try
        {
          if (this.LogExceptionsNoThrow)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Dispose\" method: {0}", (object) ex));
        }
        catch
        {
        }
      }
      this.disposed = true;
    }

    ~SQLiteModule() => this.Dispose(false);

    private sealed class SQLiteNativeModule : ISQLiteNativeModule, IDisposable
    {
      private const bool DefaultLogErrors = true;
      private const bool DefaultLogExceptions = true;
      private const string ModuleNotAvailableErrorMessage = "native module implementation not available";
      private SQLiteModule module;
      private bool disposed;

      public SQLiteNativeModule(SQLiteModule module) => this.module = module;

      private static SQLiteErrorCode ModuleNotAvailableTableError(IntPtr pVtab)
      {
        SQLiteModule.SetTableError((SQLiteModule) null, pVtab, true, true, "native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      private static SQLiteErrorCode ModuleNotAvailableCursorError(IntPtr pCursor)
      {
        SQLiteModule.SetCursorError((SQLiteModule) null, pCursor, true, true, "native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      public SQLiteErrorCode xCreate(
        IntPtr pDb,
        IntPtr pAux,
        int argc,
        IntPtr argv,
        ref IntPtr pVtab,
        ref IntPtr pError)
      {
        if (this.module != null)
          return this.module.xCreate(pDb, pAux, argc, argv, ref pVtab, ref pError);
        pError = SQLiteString.Utf8IntPtrFromString("native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      public SQLiteErrorCode xConnect(
        IntPtr pDb,
        IntPtr pAux,
        int argc,
        IntPtr argv,
        ref IntPtr pVtab,
        ref IntPtr pError)
      {
        if (this.module != null)
          return this.module.xConnect(pDb, pAux, argc, argv, ref pVtab, ref pError);
        pError = SQLiteString.Utf8IntPtrFromString("native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      public SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xBestIndex(pVtab, pIndex);
      }

      public SQLiteErrorCode xDisconnect(IntPtr pVtab)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xDisconnect(pVtab);
      }

      public SQLiteErrorCode xDestroy(IntPtr pVtab)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xDestroy(pVtab);
      }

      public SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xOpen(pVtab, ref pCursor);
      }

      public SQLiteErrorCode xClose(IntPtr pCursor)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xClose(pCursor);
      }

      public SQLiteErrorCode xFilter(
        IntPtr pCursor,
        int idxNum,
        IntPtr idxStr,
        int argc,
        IntPtr argv)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xFilter(pCursor, idxNum, idxStr, argc, argv);
      }

      public SQLiteErrorCode xNext(IntPtr pCursor)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xNext(pCursor);
      }

      public int xEof(IntPtr pCursor)
      {
        if (this.module != null)
          return this.module.xEof(pCursor);
        int num = (int) SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
        return 1;
      }

      public SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xColumn(pCursor, pContext, index);
      }

      public SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xRowId(pCursor, ref rowId);
      }

      public SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xUpdate(pVtab, argc, argv, ref rowId);
      }

      public SQLiteErrorCode xBegin(IntPtr pVtab)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xBegin(pVtab);
      }

      public SQLiteErrorCode xSync(IntPtr pVtab)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xSync(pVtab);
      }

      public SQLiteErrorCode xCommit(IntPtr pVtab)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xCommit(pVtab);
      }

      public SQLiteErrorCode xRollback(IntPtr pVtab)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRollback(pVtab);
      }

      public int xFindFunction(
        IntPtr pVtab,
        int nArg,
        IntPtr zName,
        ref SQLiteCallback callback,
        ref IntPtr pClientData)
      {
        if (this.module != null)
          return this.module.xFindFunction(pVtab, nArg, zName, ref callback, ref pClientData);
        int num = (int) SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
        return 0;
      }

      public SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRename(pVtab, zNew);
      }

      public SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xSavepoint(pVtab, iSavepoint);
      }

      public SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRelease(pVtab, iSavepoint);
      }

      public SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRollbackTo(pVtab, iSavepoint);
      }

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      private void CheckDisposed()
      {
        if (this.disposed)
          throw new ObjectDisposedException(typeof (SQLiteModule.SQLiteNativeModule).Name);
      }

      private void Dispose(bool disposing)
      {
        if (this.disposed)
          return;
        if (this.module != null)
          this.module = (SQLiteModule) null;
        this.disposed = true;
      }

      ~SQLiteNativeModule() => this.Dispose(false);
    }
  }
}
