// Decompiled with JetBrains decompiler
// Type: GmmDbLib.ZRDataAdapter
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public class ZRDataAdapter : DbDataAdapter
  {
    internal BaseDbConnection baseDbConnection;
    internal DbDataAdapter commonAdapter;

    public ZRDataAdapter(BaseDbConnection baseDbConnection, DbDataAdapter commonAdapter)
      : base(commonAdapter)
    {
      this.commonAdapter = commonAdapter;
      this.baseDbConnection = baseDbConnection;
    }

    [Obsolete]
    public void Fill(DataTable MyDataTable, IDbTransaction Transaction)
    {
      this.commonAdapter.SelectCommand.Transaction = (DbTransaction) Transaction;
      this.commonAdapter.Fill(MyDataTable);
    }

    [Obsolete]
    public virtual bool Fill(DataTable MyDataTable, out string Fehlerstring)
    {
      Fehlerstring = "";
      try
      {
        this.Fill(MyDataTable);
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
      }
      return false;
    }

    [Obsolete]
    public new int Update(DataTable MyDataTable)
    {
      DbCommandBuilder commandBuilder = this.baseDbConnection.ProviderFactory.CreateCommandBuilder();
      commandBuilder.QuotePrefix = "[";
      commandBuilder.QuoteSuffix = "]";
      commandBuilder.DataAdapter = this.commonAdapter;
      this.commonAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
      this.commonAdapter.InsertCommand = commandBuilder.GetInsertCommand();
      this.commonAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
      return this.commonAdapter.Update(MyDataTable);
    }

    [Obsolete]
    public int Update(DataTable MyDataTable, IDbTransaction Transaction)
    {
      if (this.commonAdapter.SelectCommand.Transaction == null)
        this.commonAdapter.SelectCommand.Transaction = (DbTransaction) Transaction;
      DbCommandBuilder commandBuilder = this.baseDbConnection.ProviderFactory.CreateCommandBuilder();
      commandBuilder.QuotePrefix = "[";
      commandBuilder.QuoteSuffix = "]";
      commandBuilder.DataAdapter = this.commonAdapter;
      this.commonAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
      this.commonAdapter.InsertCommand = commandBuilder.GetInsertCommand();
      this.commonAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
      return this.commonAdapter.Update(MyDataTable);
    }

    public virtual int Update(DataTable MyDataTable, bool IgnoreUpdateErrors)
    {
      try
      {
        return base.Update(MyDataTable);
      }
      catch (Exception ex)
      {
        if (!IgnoreUpdateErrors)
          throw ex;
      }
      return 0;
    }
  }
}
