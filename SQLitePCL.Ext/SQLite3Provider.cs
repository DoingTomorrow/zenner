// Decompiled with JetBrains decompiler
// Type: SQLitePCL.SQLite3Provider
// Assembly: SQLitePCL.Ext, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 28DC4D07-0E35-45C1-8EF3-CED69BFBD581
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.Ext.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace SQLitePCL
{
  internal sealed class SQLite3Provider : ISQLite3Provider
  {
    private static ISQLite3Provider instance = (ISQLite3Provider) new SQLite3Provider();

    private SQLite3Provider()
    {
    }

    internal static ISQLite3Provider Instance => SQLite3Provider.instance;

    public int Sqlite3Open(IntPtr filename, out IntPtr db)
    {
      return SQLite3Provider.NativeMethods.sqlite3_open(filename, out db);
    }

    int ISQLite3Provider.Sqlite3CloseV2(IntPtr db)
    {
      return SQLite3Provider.NativeMethods.sqlite3_close_v2(db);
    }

    int ISQLite3Provider.Sqlite3PrepareV2(
      IntPtr db,
      IntPtr sql,
      int length,
      out IntPtr stm,
      IntPtr tail)
    {
      return SQLite3Provider.NativeMethods.sqlite3_prepare_v2(db, sql, length, out stm, tail);
    }

    int ISQLite3Provider.Sqlite3CreateFunction(
      IntPtr db,
      IntPtr functionName,
      int numArg,
      bool deterministic,
      IntPtr func)
    {
      return SQLite3Provider.NativeMethods.sqlite3_create_function(db, functionName, numArg, deterministic ? 2049 : 1, IntPtr.Zero, func, IntPtr.Zero, IntPtr.Zero);
    }

    int ISQLite3Provider.Sqlite3CreateAggregate(
      IntPtr db,
      IntPtr aggregateName,
      int numArg,
      IntPtr step,
      IntPtr final)
    {
      return SQLite3Provider.NativeMethods.sqlite3_create_function(db, aggregateName, numArg, 1, IntPtr.Zero, IntPtr.Zero, step, final);
    }

    long ISQLite3Provider.Sqlite3LastInsertRowId(IntPtr db)
    {
      return SQLite3Provider.NativeMethods.sqlite3_last_insert_rowid(db);
    }

    IntPtr ISQLite3Provider.Sqlite3Errmsg(IntPtr db)
    {
      return SQLite3Provider.NativeMethods.sqlite3_errmsg(db);
    }

    int ISQLite3Provider.Sqlite3BindInt(IntPtr stm, int paramIndex, int value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_int(stm, paramIndex, value);
    }

    int ISQLite3Provider.Sqlite3BindInt64(IntPtr stm, int paramIndex, long value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_int64(stm, paramIndex, value);
    }

    int ISQLite3Provider.Sqlite3BindText(
      IntPtr stm,
      int paramIndex,
      IntPtr value,
      int length,
      IntPtr destructor)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_text(stm, paramIndex, value, length, destructor);
    }

    int ISQLite3Provider.Sqlite3BindDouble(IntPtr stm, int paramIndex, double value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_double(stm, paramIndex, value);
    }

    int ISQLite3Provider.Sqlite3BindBlob(
      IntPtr stm,
      int paramIndex,
      byte[] value,
      int length,
      IntPtr destructor)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_blob(stm, paramIndex, value, length, destructor);
    }

    int ISQLite3Provider.Sqlite3BindNull(IntPtr stm, int paramIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_null(stm, paramIndex);
    }

    int ISQLite3Provider.Sqlite3BindParameterCount(IntPtr stm)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_parameter_count(stm);
    }

    IntPtr ISQLite3Provider.Sqlite3BindParameterName(IntPtr stm, int paramIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_parameter_name(stm, paramIndex);
    }

    int ISQLite3Provider.Sqlite3BindParameterIndex(IntPtr stm, IntPtr paramName)
    {
      return SQLite3Provider.NativeMethods.sqlite3_bind_parameter_index(stm, paramName);
    }

    int ISQLite3Provider.Sqlite3Step(IntPtr stm) => SQLite3Provider.NativeMethods.sqlite3_step(stm);

    int ISQLite3Provider.Sqlite3ColumnInt(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_int(stm, columnIndex);
    }

    long ISQLite3Provider.Sqlite3ColumnInt64(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_int64(stm, columnIndex);
    }

    IntPtr ISQLite3Provider.Sqlite3ColumnText(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_text(stm, columnIndex);
    }

    double ISQLite3Provider.Sqlite3ColumnDouble(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_double(stm, columnIndex);
    }

    IntPtr ISQLite3Provider.Sqlite3ColumnBlob(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_blob(stm, columnIndex);
    }

    int ISQLite3Provider.Sqlite3ColumnType(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_type(stm, columnIndex);
    }

    int ISQLite3Provider.Sqlite3ColumnBytes(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_bytes(stm, columnIndex);
    }

    int ISQLite3Provider.Sqlite3ColumnCount(IntPtr stm)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_count(stm);
    }

    IntPtr ISQLite3Provider.Sqlite3ColumnName(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_name(stm, columnIndex);
    }

    IntPtr ISQLite3Provider.Sqlite3ColumnOriginName(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_origin_name(stm, columnIndex);
    }

    IntPtr ISQLite3Provider.Sqlite3ColumnTableName(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_table_name(stm, columnIndex);
    }

    IntPtr ISQLite3Provider.Sqlite3ColumnDatabaseName(IntPtr stm, int columnIndex)
    {
      return SQLite3Provider.NativeMethods.sqlite3_column_database_name(stm, columnIndex);
    }

    int ISQLite3Provider.Sqlite3DataCount(IntPtr stm)
    {
      return SQLite3Provider.NativeMethods.sqlite3_data_count(stm);
    }

    int ISQLite3Provider.Sqlite3Reset(IntPtr stm)
    {
      return SQLite3Provider.NativeMethods.sqlite3_reset(stm);
    }

    int ISQLite3Provider.Sqlite3ClearBindings(IntPtr stm)
    {
      return SQLite3Provider.NativeMethods.sqlite3_clear_bindings(stm);
    }

    int ISQLite3Provider.Sqlite3Finalize(IntPtr stm)
    {
      return SQLite3Provider.NativeMethods.sqlite3_finalize(stm);
    }

    int ISQLite3Provider.Sqlite3ValueInt(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_int(value);
    }

    long ISQLite3Provider.Sqlite3ValueInt64(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_int64(value);
    }

    IntPtr ISQLite3Provider.Sqlite3ValueText(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_text(value);
    }

    double ISQLite3Provider.Sqlite3ValueDouble(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_double(value);
    }

    IntPtr ISQLite3Provider.Sqlite3ValueBlob(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_blob(value);
    }

    int ISQLite3Provider.Sqlite3ValueType(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_type(value);
    }

    int ISQLite3Provider.Sqlite3ValueBytes(IntPtr value)
    {
      return SQLite3Provider.NativeMethods.sqlite3_value_bytes(value);
    }

    void ISQLite3Provider.Sqlite3ResultInt(IntPtr context, int value)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_int(context, value);
    }

    void ISQLite3Provider.Sqlite3ResultInt64(IntPtr context, long value)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_int64(context, value);
    }

    void ISQLite3Provider.Sqlite3ResultText(
      IntPtr context,
      IntPtr value,
      int length,
      IntPtr destructor)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_text(context, value, length, destructor);
    }

    void ISQLite3Provider.Sqlite3ResultDouble(IntPtr context, double value)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_double(context, value);
    }

    void ISQLite3Provider.Sqlite3ResultBlob(
      IntPtr context,
      byte[] value,
      int length,
      IntPtr destructor)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_blob(context, value, length, destructor);
    }

    void ISQLite3Provider.Sqlite3ResultNull(IntPtr context)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_null(context);
    }

    void ISQLite3Provider.Sqlite3ResultError(IntPtr context, IntPtr value, int length)
    {
      SQLite3Provider.NativeMethods.sqlite3_result_error(context, value, length);
    }

    IntPtr ISQLite3Provider.Sqlite3AggregateContext(IntPtr context, int length)
    {
      return SQLite3Provider.NativeMethods.sqlite3_aggregate_context(context, length);
    }

    private static class NativeMethods
    {
      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_open(IntPtr filename, out IntPtr db);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_close_v2(IntPtr db);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_prepare_v2(
        IntPtr db,
        IntPtr zSql,
        int nByte,
        out IntPtr ppStmpt,
        IntPtr pzTail);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_create_function(
        IntPtr db,
        IntPtr functionName,
        int nArg,
        int p,
        IntPtr intPtr1,
        IntPtr func,
        IntPtr intPtr2,
        IntPtr intPtr3);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern long sqlite3_last_insert_rowid(IntPtr db);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_errmsg(IntPtr db);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_int(IntPtr stmHandle, int iParam, int value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_int64(IntPtr stmHandle, int iParam, long value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_text(
        IntPtr stmHandle,
        int iParam,
        IntPtr value,
        int length,
        IntPtr destructor);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_double(IntPtr stmHandle, int iParam, double value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_blob(
        IntPtr stmHandle,
        int iParam,
        byte[] value,
        int length,
        IntPtr destructor);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_null(IntPtr stmHandle, int iParam);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_parameter_count(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_bind_parameter_name(IntPtr stmHandle, int iParam);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_bind_parameter_index(IntPtr stmHandle, IntPtr paramName);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_step(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern long sqlite3_column_int64(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_column_text(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_column_blob(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_column_type(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_column_bytes(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_column_count(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_column_name(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_column_origin_name(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_column_table_name(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_column_database_name(IntPtr stmHandle, int iCol);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_data_count(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_reset(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_clear_bindings(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_finalize(IntPtr stmHandle);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_value_int(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern long sqlite3_value_int64(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_value_text(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern double sqlite3_value_double(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_value_blob(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_value_type(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int sqlite3_value_bytes(IntPtr value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_int(IntPtr context, int value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_int64(IntPtr context, long value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_text(
        IntPtr context,
        IntPtr value,
        int length,
        IntPtr destructor);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_double(IntPtr context, double value);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_blob(
        IntPtr context,
        byte[] value,
        int length,
        IntPtr destructor);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_null(IntPtr context);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void sqlite3_result_error(IntPtr context, IntPtr value, int length);

      [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr sqlite3_aggregate_context(IntPtr context, int length);
    }
  }
}
