// Decompiled with JetBrains decompiler
// Type: MinomatHandler.RoutingTable
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace MinomatHandler
{
  public sealed class RoutingTable : List<RoutingRow>
  {
    public const string COLUMN_NODE_ID = "Node ID";
    public const string COLUMN_PARENT_ID = "Parent ID";
    public const string COLUMN_HOP_COUNT = "Hop Count";
    public const string COLUMN_RSSI = "RSSI";
    private string header;
    private DataTable table;

    public StateDataOfLPRS StateOfLPRS { get; set; }

    public RoutingTable()
    {
      this.table = new DataTable(nameof (RoutingTable));
      this.table.Columns.Add("Node ID");
      this.table.Columns.Add("Parent ID");
      this.table.Columns.Add("Hop Count");
      this.table.Columns.Add("RSSI");
      this.header = "".PadRight(44, '-') + "\n " + "Node ID".PadRight(10) + "| " + "Parent ID".PadRight(10) + "| " + "Hop Count".PadRight(10) + "| " + "RSSI".PadRight(10) + "\n" + "".PadRight(44, '-') + "\n";
    }

    public DataTable CreateTable()
    {
      this.table.Clear();
      foreach (RoutingRow routingRow in (List<RoutingRow>) this)
        this.table.Rows.Add((object) routingRow.NodeId, (object) routingRow.ParentNodeId, (object) routingRow.HopCount, (object) routingRow.RSSI_dBm);
      return this.table;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.StateOfLPRS != null)
      {
        stringBuilder.Append("\n");
        stringBuilder.Append((object) this.StateOfLPRS);
      }
      stringBuilder.Append("\n");
      stringBuilder.Append(this.header);
      foreach (RoutingRow routingRow in (List<RoutingRow>) this)
      {
        stringBuilder.Append(" ");
        stringBuilder.Append(routingRow.NodeId.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(routingRow.ParentNodeId.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(routingRow.HopCount.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(routingRow.RSSI.ToString().PadRight(10));
        stringBuilder.Append("\n");
      }
      return stringBuilder.ToString();
    }
  }
}
