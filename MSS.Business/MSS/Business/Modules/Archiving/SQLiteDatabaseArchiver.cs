// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.SQLiteDatabaseArchiver
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Archiving;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  internal class SQLiteDatabaseArchiver : GenericArchiver
  {
    private SQLiteConnection SourceConnection { get; set; }

    private SQLiteConnection DestinationConnection { get; set; }

    private SQLiteTransaction Transaction { get; set; }

    public SQLiteDatabaseArchiver(
      string connStringMSS,
      string connStringArchive,
      ISessionFactory sessionFactory)
    {
      this.ConnStringMSS = connStringMSS;
      this.ConnStringArchive = connStringArchive;
      this.SessionFactory = sessionFactory;
      this.InitializeTypesConverter();
    }

    internal override void SaveArchiveInformation(ArchiveDetailsADO archiveDetails)
    {
      this.ArchiveId = Guid.NewGuid();
      SQLiteCommand sqLiteCommand = new SQLiteCommand("INSERT INTO t_ArchiveInformation ([Id],[ArchiveName],[DateTime],[StartTime],[EndTime],[ArchivedEntities]) VALUES (:Id,:ArchiveName,:DateTime,:StartTime,:EndTime,:ArchivedEntities)", this.DestinationConnection);
      sqLiteCommand.Parameters.AddWithValue("Id", (object) this.ArchiveId);
      sqLiteCommand.Parameters.AddWithValue("ArchiveName", (object) archiveDetails.ArchiveName);
      sqLiteCommand.Parameters.AddWithValue("DateTime", (object) DateTime.Now);
      sqLiteCommand.Parameters.AddWithValue("StartTime", (object) archiveDetails.StartTime);
      sqLiteCommand.Parameters.AddWithValue("EndTime", (object) archiveDetails.EndTime);
      sqLiteCommand.Parameters.AddWithValue("ArchivedEntities", (object) JsonConvert.SerializeObject((object) archiveDetails.ArchivedEntities));
      sqLiteCommand.Transaction = this.Transaction;
      sqLiteCommand.ExecuteNonQuery();
    }

    internal override void Initialize()
    {
      this.SourceConnection = new SQLiteConnection(this.ConnStringMSS);
      this.DestinationConnection = new SQLiteConnection(this.ConnStringArchive);
      this.SourceConnection.Open();
      this.DestinationConnection.Open();
      this.Transaction = this.DestinationConnection.BeginTransaction();
    }

    internal override void Commit()
    {
      if (this.DestinationConnection.State != ConnectionState.Open)
        this.DestinationConnection.Open();
      this.Transaction.Commit();
      if (this.SourceConnection.State == ConnectionState.Open)
        this.SourceConnection.Close();
      if (this.DestinationConnection.State != ConnectionState.Open)
        return;
      this.DestinationConnection.Close();
    }

    internal override void Error()
    {
      if (this.DestinationConnection.State != ConnectionState.Open)
        this.DestinationConnection.Open();
      this.Transaction.Rollback();
      if (this.SourceConnection.State == ConnectionState.Open)
        this.SourceConnection.Close();
      if (this.DestinationConnection.State != ConnectionState.Open)
        return;
      this.DestinationConnection.Close();
    }

    protected override string GetSelectStatement<SourceEntity>(string tableName)
    {
      return string.Format("SELECT * FROM {0}", (object) tableName);
    }

    protected override string GetInsertStatement<T>(string tableName)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      PropertyInfo[] properties = this.GetProperties<T>();
      string str1 = ((IEnumerable<PropertyInfo>) properties).Aggregate<PropertyInfo, string>(empty2, (Func<string, PropertyInfo, string>) ((current, property) => current + property.Name + ","));
      string str2 = str1.Substring(0, str1.Length - 1);
      string str3 = ((IEnumerable<PropertyInfo>) properties).Aggregate<PropertyInfo, string>(empty1, (Func<string, PropertyInfo, string>) ((current, property) => current + ":" + property.Name + ","));
      string str4 = str3.Substring(0, str3.Length - 1);
      return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", (object) tableName, (object) str2, (object) str4);
    }

    protected override void UpdateArchiveInformationId(string tableName, Guid archiveJobId)
    {
      SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format("UPDATE {0} SET " + tableName + "_ArchiveJobId = :ArchiveJobId WHERE " + tableName + "_ArchiveJobId IS NULL", (object) tableName), this.DestinationConnection);
      sqLiteCommand.Parameters.AddWithValue("ArchiveJobId", (object) archiveJobId);
      sqLiteCommand.Transaction = this.Transaction;
      sqLiteCommand.ExecuteNonQuery();
    }

    protected override void ArchiveEntity<SourceEntity, DestinationEntity>(
      DatasourcesInfo dsInfo,
      ArchiveJob archiveJob)
    {
      SQLiteCommand sqLiteCommand1 = new SQLiteCommand(this.GetSelectStatement<SourceEntity>(this.GetSourceName(dsInfo)), this.SourceConnection);
      DataTable table;
      using (SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter())
      {
        sqLiteDataAdapter.SelectCommand = sqLiteCommand1;
        DataSet dataSet = new DataSet();
        sqLiteDataAdapter.Fill(dataSet, dsInfo.SourceTableName);
        table = dataSet.Tables[0];
      }
      SQLiteCommand sqLiteCommand2 = new SQLiteCommand(this.GetSelectStatement<DestinationEntity>(dsInfo.DestinationTableName), this.DestinationConnection);
      SQLiteCommand sqLiteCommand3 = new SQLiteCommand(this.GetInsertStatement<DestinationEntity>(dsInfo.DestinationTableName), this.DestinationConnection);
      List<SQLiteParameter> list = ((IEnumerable<PropertyInfo>) this.GetProperties<DestinationEntity>()).Select<PropertyInfo, SQLiteParameter>((System.Func<PropertyInfo, SQLiteParameter>) (property => new SQLiteParameter(":" + property.Name, this.GetDbType(property), property.Name))).ToList<SQLiteParameter>();
      using (SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter())
      {
        sqLiteDataAdapter.SelectCommand = sqLiteCommand2;
        sqLiteDataAdapter.InsertCommand = sqLiteCommand3;
        sqLiteDataAdapter.InsertCommand.Parameters.AddRange(list.ToArray());
        using (SQLiteCommandBuilder liteCommandBuilder = new SQLiteCommandBuilder())
        {
          liteCommandBuilder.DataAdapter = sqLiteDataAdapter;
          using (sqLiteDataAdapter.InsertCommand = (SQLiteCommand) liteCommandBuilder.GetInsertCommand().Clone())
          {
            liteCommandBuilder.DataAdapter = (SQLiteDataAdapter) null;
            foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
              row.SetAdded();
            sqLiteDataAdapter.Update(table);
          }
        }
      }
      this.UpdateArchiveInformationId(dsInfo.SourceTableName, archiveJob.Id);
    }
  }
}
