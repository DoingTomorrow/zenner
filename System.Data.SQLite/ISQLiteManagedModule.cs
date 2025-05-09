﻿// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteManagedModule
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public interface ISQLiteManagedModule
  {
    bool Declared { get; }

    string Name { get; }

    SQLiteErrorCode Create(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error);

    SQLiteErrorCode Connect(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error);

    SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index);

    SQLiteErrorCode Disconnect(SQLiteVirtualTable table);

    SQLiteErrorCode Destroy(SQLiteVirtualTable table);

    SQLiteErrorCode Open(SQLiteVirtualTable table, ref SQLiteVirtualTableCursor cursor);

    SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor);

    SQLiteErrorCode Filter(
      SQLiteVirtualTableCursor cursor,
      int indexNumber,
      string indexString,
      SQLiteValue[] values);

    SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor);

    bool Eof(SQLiteVirtualTableCursor cursor);

    SQLiteErrorCode Column(SQLiteVirtualTableCursor cursor, SQLiteContext context, int index);

    SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId);

    SQLiteErrorCode Update(SQLiteVirtualTable table, SQLiteValue[] values, ref long rowId);

    SQLiteErrorCode Begin(SQLiteVirtualTable table);

    SQLiteErrorCode Sync(SQLiteVirtualTable table);

    SQLiteErrorCode Commit(SQLiteVirtualTable table);

    SQLiteErrorCode Rollback(SQLiteVirtualTable table);

    bool FindFunction(
      SQLiteVirtualTable table,
      int argumentCount,
      string name,
      ref SQLiteFunction function,
      ref IntPtr pClientData);

    SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName);

    SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint);

    SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint);

    SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint);
  }
}
