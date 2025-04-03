// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.MSSQLDatabaseArchiver
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Archiving;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  internal class MSSQLDatabaseArchiver : GenericArchiver
  {
    private SqlConnection SourceConnection { get; set; }

    private SqlConnection DestinationConnection { get; set; }

    private SqlTransaction Transaction { get; set; }

    public MSSQLDatabaseArchiver(
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
      SqlCommand sqlCommand = new SqlCommand("INSERT INTO t_ArchiveInformation ([Id],[ArchiveName],[DateTime],[StartTime],[EndTime],[ArchivedEntities]) VALUES (@Id,@ArchiveName,@DateTime,@StartTime,@EndTime,@ArchivedEntities)", this.DestinationConnection);
      sqlCommand.Parameters.AddWithValue("Id", (object) this.ArchiveId);
      sqlCommand.Parameters.AddWithValue("ArchiveName", (object) archiveDetails.ArchiveName);
      sqlCommand.Parameters.AddWithValue("DateTime", (object) DateTime.Now);
      sqlCommand.Parameters.AddWithValue("StartTime", (object) archiveDetails.StartTime);
      sqlCommand.Parameters.AddWithValue("EndTime", (object) archiveDetails.EndTime);
      sqlCommand.Parameters.AddWithValue("ArchivedEntities", (object) JsonConvert.SerializeObject((object) archiveDetails.ArchivedEntities));
      sqlCommand.Transaction = this.Transaction;
      sqlCommand.ExecuteNonQuery();
    }

    internal override void Initialize()
    {
      this.SourceConnection = new SqlConnection(this.ConnStringMSS);
      this.DestinationConnection = new SqlConnection(this.ConnStringArchive);
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
      throw new NotImplementedException();
    }

    protected override void UpdateArchiveInformationId(string tableName, Guid archiveJobId)
    {
      SqlCommand sqlCommand1 = new SqlCommand(string.Format("UPDATE {0} SET ArchiveJobId = @archiveJobId WHERE ArchiveJobId IS NULL", (object) tableName), this.DestinationConnection);
      sqlCommand1.CommandTimeout = 600;
      SqlCommand sqlCommand2 = sqlCommand1;
      sqlCommand2.Parameters.AddWithValue(nameof (archiveJobId), (object) archiveJobId);
      sqlCommand2.Transaction = this.Transaction;
      sqlCommand2.ExecuteNonQuery();
    }

    protected override void ArchiveEntity<SourceEntity, DestinationEntity>(
      DatasourcesInfo dsInfo,
      ArchiveJob archiveJob)
    {
      string sourceName = this.GetSourceName(dsInfo);
      if (archiveJob.DeleteAfterArchive)
        this.SaveItemsIds(sourceName);
      SqlCommand sqlCommand = new SqlCommand(this.GetSelectStatement<SourceEntity>(sourceName), this.SourceConnection);
      sqlCommand.CommandTimeout = 600;
      SqlDataReader reader = sqlCommand.ExecuteReader();
      SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(this.DestinationConnection, SqlBulkCopyOptions.Default, this.Transaction)
      {
        DestinationTableName = dsInfo.DestinationTableName,
        BatchSize = 1000,
        BulkCopyTimeout = 0
      };
      foreach (PropertyInfo property in this.GetProperties<DestinationEntity>())
        sqlBulkCopy.ColumnMappings.Add(property.Name, property.Name);
      sqlBulkCopy.WriteToServer((DbDataReader) reader);
      sqlBulkCopy.Close();
      reader.Close();
      this.UpdateArchiveInformationId(dsInfo.SourceTableName, archiveJob.Id);
    }

    protected void SaveItemsIds(string tableName)
    {
      new SqlCommand("truncate table temp_processing_items", this.SourceConnection).ExecuteNonQuery();
      SqlCommand sqlCommand = new SqlCommand("insert into temp_processing_items select id from " + tableName, this.SourceConnection);
      sqlCommand.CommandTimeout = 600;
      sqlCommand.ExecuteNonQuery();
    }
  }
}
