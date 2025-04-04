// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.NativeMethods
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  internal static class NativeMethods
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
    internal static extern int sqlite3_reset(IntPtr stmHandle);

    [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_clear_bindings(IntPtr stmHandle);

    [DllImport("sqlite3.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_finalize(IntPtr stmHandle);
  }
}
