// Decompiled with JetBrains decompiler
// Type: MinomatHandler.TableOfSlaves
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace MinomatHandler
{
  public sealed class TableOfSlaves : List<Slave>
  {
    public const string COLUMN_SLAVE_ID = "Node ID";
    public const string COLUMN_MINOL_ID = "Minol ID";
    private string header;
    private DataTable table;

    public TableOfSlaves()
    {
      this.table = new DataTable(nameof (TableOfSlaves));
      this.table.Columns.Add("Node ID");
      this.table.Columns.Add("Minol ID");
      this.header = "".PadRight(24, '-') + "\n " + "Node ID".PadRight(10) + "| " + "Minol ID".PadRight(10) + "\n" + "".PadRight(24, '-') + "\n";
    }

    public DataTable CreateTable()
    {
      this.table.Clear();
      foreach (Slave slave in (List<Slave>) this)
        this.table.Rows.Add((object) slave.SlaveNodeID, (object) slave.MinolID);
      return this.table;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\n");
      stringBuilder.Append(this.header);
      foreach (Slave slave in (List<Slave>) this)
      {
        stringBuilder.Append(" ");
        stringBuilder.Append(slave.SlaveNodeID.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(slave.MinolID.ToString().PadRight(10));
        stringBuilder.Append("\n");
      }
      return stringBuilder.ToString();
    }
  }
}
