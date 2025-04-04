// Decompiled with JetBrains decompiler
// Type: SQLitePCL.ISQLiteStatement
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System;

#nullable disable
namespace SQLitePCL
{
  public interface ISQLiteStatement : IDisposable
  {
    ISQLiteConnection Connection { get; }

    int ColumnCount { get; }

    int DataCount { get; }

    object this[int index] { get; }

    object this[string name] { get; }

    SQLiteType DataType(int index);

    SQLiteType DataType(string name);

    string ColumnName(int index);

    int ColumnIndex(string name);

    SQLiteResult Step();

    long GetInteger(int index);

    long GetInteger(string name);

    double GetFloat(int index);

    double GetFloat(string name);

    string GetText(int index);

    string GetText(string name);

    byte[] GetBlob(int index);

    byte[] GetBlob(string name);

    void Reset();

    void Bind(int index, object value);

    void Bind(string paramName, object value);

    void ClearBindings();
  }
}
