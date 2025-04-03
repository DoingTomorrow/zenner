// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteNativeModule
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public interface ISQLiteNativeModule
  {
    SQLiteErrorCode xCreate(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError);

    SQLiteErrorCode xConnect(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError);

    SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex);

    SQLiteErrorCode xDisconnect(IntPtr pVtab);

    SQLiteErrorCode xDestroy(IntPtr pVtab);

    SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor);

    SQLiteErrorCode xClose(IntPtr pCursor);

    SQLiteErrorCode xFilter(IntPtr pCursor, int idxNum, IntPtr idxStr, int argc, IntPtr argv);

    SQLiteErrorCode xNext(IntPtr pCursor);

    int xEof(IntPtr pCursor);

    SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index);

    SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId);

    SQLiteErrorCode xUpdate(IntPtr pVtab, int argc, IntPtr argv, ref long rowId);

    SQLiteErrorCode xBegin(IntPtr pVtab);

    SQLiteErrorCode xSync(IntPtr pVtab);

    SQLiteErrorCode xCommit(IntPtr pVtab);

    SQLiteErrorCode xRollback(IntPtr pVtab);

    int xFindFunction(
      IntPtr pVtab,
      int nArg,
      IntPtr zName,
      ref SQLiteCallback callback,
      ref IntPtr pClientData);

    SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew);

    SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint);

    SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint);

    SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint);
  }
}
