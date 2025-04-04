// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeRowUpdatingEventArgs
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Data.Common;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeRowUpdatingEventArgs(
    DataRow dataRow,
    IDbCommand command,
    StatementType statementType,
    DataTableMapping tableMapping) : RowUpdatingEventArgs(dataRow, command, statementType, tableMapping)
  {
    public SqlCeCommand Command
    {
      get => (SqlCeCommand) base.Command;
      set => this.Command = (IDbCommand) value;
    }
  }
}
