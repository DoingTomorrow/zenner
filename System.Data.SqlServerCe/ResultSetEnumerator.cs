// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.ResultSetEnumerator
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class ResultSetEnumerator : IEnumerator
  {
    internal SqlCeResultSet _resultset;
    internal SqlCeUpdatableRecord _current;

    public ResultSetEnumerator(SqlCeResultSet resultSet)
    {
      this._resultset = resultSet != null ? resultSet : throw new ArgumentNullException("resultset");
    }

    object IEnumerator.Current => (object) this._current;

    public SqlCeUpdatableRecord Current => this._current;

    public bool MoveNext()
    {
      this._current = (SqlCeUpdatableRecord) null;
      if (!this._resultset.Read())
        return false;
      this._current = this._resultset.GetCurrentRecord();
      return true;
    }

    public void Reset()
    {
      this._current = (SqlCeUpdatableRecord) null;
      this._resultset.ReadFirst();
      this._resultset.ReadPrevious();
    }
  }
}
