// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.SQLiteManager
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  internal class SQLiteManager
  {
    private OfflineSchema schema;
    private SQLiteHelper sqliteHelper;
    private string localFilePath;
    private Dictionary<Guid, IEnumerable<SQLiteOfflineEntity>> sentChangesAwaitingResponse;
    private Dictionary<string, TableMapping> mappings;

    public SQLiteManager(OfflineSchema schema, string localfilePath)
    {
      this.schema = schema;
      this.localFilePath = localfilePath;
      this.sqliteHelper = new SQLiteHelper(this.localFilePath, this);
    }

    public TableMapping GetMapping<T>() => this.GetMapping(typeof (T));

    public TableMapping GetMapping(Type type)
    {
      if (this.mappings == null)
        this.mappings = new Dictionary<string, TableMapping>();
      TableMapping mapping;
      if (!this.mappings.TryGetValue(type.FullName, out mapping))
      {
        mapping = new TableMapping(type);
        this.mappings[type.FullName] = mapping;
      }
      return mapping;
    }

    internal bool ScopeTableExist()
    {
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        try
        {
          string str = (string) null;
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("SELECT name FROM sqlite_master WHERE type='table' AND name='ScopeInfoTable'"))
          {
            while (sqLiteStatement.Step() == SQLiteResult.ROW)
              str = (string) sqLiteStatement[0];
          }
          return str == "ScopeInfoTable";
        }
        catch (Exception ex)
        {
          return false;
        }
      }
    }

    internal void CreateTable(Type table) => this.sqliteHelper.CreateTable(table);

    internal void CreateScopeTable() => this.sqliteHelper.CreateTable(typeof (ScopeInfoTable));

    internal SQLiteConfiguration ReadConfiguration(string databaseScopeName)
    {
      SQLiteConfiguration liteConfiguration = new SQLiteConfiguration();
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        string uriString = (string) null;
        List<string> stringList = new List<string>();
        DateTime dateTime = new DateTime(1900, 1, 1);
        byte[] numArray = (byte[]) null;
        bool flag;
        try
        {
          string str1 = databaseScopeName;
          ScopeInfoTable scopeInfoTable = (ScopeInfoTable) null;
          string str2 = (string) null;
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("SELECT name FROM sqlite_master WHERE type='table' AND name='ScopeInfoTable'"))
          {
            if (sqLiteStatement.Step() == SQLiteResult.ROW)
              str2 = sqLiteStatement[0] as string;
          }
          if (str2 == "ScopeInfoTable")
          {
            string sql = "Select * From ScopeInfoTable Where ScopeName = ?;";
            using (ISQLiteStatement stmt = sqLiteConnection.Prepare(sql))
            {
              stmt.Bind(1, (object) str1);
              if (stmt.Step() == SQLiteResult.ROW)
              {
                scopeInfoTable = new ScopeInfoTable();
                scopeInfoTable.ScopeName = (string) SQLiteHelper.ReadCol(stmt, 0, typeof (string));
                scopeInfoTable.ServiceUri = (string) SQLiteHelper.ReadCol(stmt, 1, typeof (string));
                scopeInfoTable.LastSyncDate = (DateTime) SQLiteHelper.ReadCol(stmt, 2, typeof (DateTime));
                scopeInfoTable.AnchorBlob = (byte[]) SQLiteHelper.ReadCol(stmt, 3, typeof (byte[]));
                scopeInfoTable.Configuration = (string) SQLiteHelper.ReadCol(stmt, 4, typeof (string));
              }
            }
          }
          if (scopeInfoTable == null)
            return (SQLiteConfiguration) null;
          XDocument xdocument = XDocument.Parse(scopeInfoTable.Configuration);
          uriString = scopeInfoTable.ServiceUri;
          stringList = xdocument.Descendants().Where<XElement>((Func<XElement, bool>) (tt => tt.Name == (XName) "Types")).Select<XElement, string>((Func<XElement, string>) (tt => tt.Value)).ToList<string>();
          dateTime = scopeInfoTable.LastSyncDate;
          numArray = scopeInfoTable.AnchorBlob;
          flag = true;
        }
        catch
        {
          flag = false;
        }
        if (!flag)
          return (SQLiteConfiguration) null;
        liteConfiguration.ScopeName = databaseScopeName;
        liteConfiguration.ServiceUri = new Uri(uriString);
        liteConfiguration.Types = stringList;
        liteConfiguration.LastSyncDate = dateTime;
        liteConfiguration.AnchorBlob = numArray;
      }
      return liteConfiguration;
    }

    internal void SaveConfiguration(SQLiteConfiguration configuration)
    {
      XElement xelement = new XElement((XName) "ScopeInfoTable");
      foreach (string type in configuration.Types)
        xelement.Add((object) new XElement((XName) "Types", (object) type));
      XDocument xdocument = new XDocument(new object[1]
      {
        (object) xelement
      });
      ScopeInfoTable scopeInfoTable = new ScopeInfoTable()
      {
        ScopeName = configuration.ScopeName,
        ServiceUri = configuration.ServiceUri.AbsoluteUri,
        Configuration = xdocument.ToString(),
        AnchorBlob = configuration.AnchorBlob,
        LastSyncDate = configuration.LastSyncDate
      };
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        try
        {
          string str = (string) null;
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("SELECT name FROM sqlite_master WHERE type='table' AND name='ScopeInfoTable'"))
          {
            if (sqLiteStatement.Step() == SQLiteResult.ROW)
              str = sqLiteStatement[0] as string;
          }
          if (!(str == "ScopeInfoTable"))
            return;
          string sql1 = "Select * From ScopeInfoTable Where ScopeName = ?;";
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare(sql1))
          {
            sqLiteStatement.Bind(1, (object) configuration.ScopeName);
            string sql2 = sqLiteStatement.Step() == SQLiteResult.ROW ? "Update ScopeInfoTable Set ServiceUri = ?, LastSyncDate = ?, Configuration = ?, AnchorBlob = ? Where ScopeName = ?;" : "Insert into ScopeInfoTable (ServiceUri, LastSyncDate, Configuration, AnchorBlob, ScopeName) Values (?, ?, ?, ?, ?);";
            using (ISQLiteStatement stmt = sqLiteConnection.Prepare(sql2))
            {
              SQLiteHelper.BindParameter(stmt, 1, (object) scopeInfoTable.ServiceUri);
              SQLiteHelper.BindParameter(stmt, 2, (object) scopeInfoTable.LastSyncDate);
              SQLiteHelper.BindParameter(stmt, 3, (object) scopeInfoTable.Configuration);
              SQLiteHelper.BindParameter(stmt, 4, (object) scopeInfoTable.AnchorBlob);
              SQLiteHelper.BindParameter(stmt, 5, (object) scopeInfoTable.ScopeName);
              int num = (int) stmt.Step();
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Impossible to save Sync Configuration", ex);
        }
      }
    }

    internal IEnumerable<IOfflineEntity> GetChanges(Guid state, DateTime lastSyncDate)
    {
      IEnumerable<SQLiteOfflineEntity> changes = this.sqliteHelper.GetChanges(this.schema, lastSyncDate);
      if (this.sentChangesAwaitingResponse == null)
        this.sentChangesAwaitingResponse = new Dictionary<Guid, IEnumerable<SQLiteOfflineEntity>>();
      this.sentChangesAwaitingResponse[state] = changes;
      return (IEnumerable<IOfflineEntity>) changes;
    }

    internal IEnumerable<Conflict> UploadSucceeded(Guid state)
    {
      IEnumerable<SQLiteOfflineEntity> source = this.sentChangesAwaitingResponse[state];
      foreach (var data in source.Where<SQLiteOfflineEntity>((Func<SQLiteOfflineEntity, bool>) (e => e.ServiceMetadata.IsTombstone)).GroupBy<SQLiteOfflineEntity, Type>((Func<SQLiteOfflineEntity, Type>) (e => e.GetType())).Select(entitiesTombstone => new
      {
        Type = entitiesTombstone.Key,
        Entities = entitiesTombstone
      }))
        this.sqliteHelper.DeleteTombstoneTrackingEntities(data.Type, data.Entities.ToList<SQLiteOfflineEntity>());
      foreach (var data in source.Where<SQLiteOfflineEntity>((Func<SQLiteOfflineEntity, bool>) (e => !e.ServiceMetadata.IsTombstone)).GroupBy<SQLiteOfflineEntity, Type>((Func<SQLiteOfflineEntity, Type>) (e => e.GetType())).Select(entitiesTombstone => new
      {
        Type = entitiesTombstone.Key,
        Entities = entitiesTombstone
      }))
        this.sqliteHelper.UpdateDirtyTrackingEntities(data.Type, data.Entities.ToList<SQLiteOfflineEntity>());
      this.sentChangesAwaitingResponse.Remove(state);
      return (IEnumerable<Conflict>) new List<Conflict>();
    }

    internal void SaveDownloadedChanges(IEnumerable<SQLiteOfflineEntity> entities)
    {
      if (entities == null || !entities.Any<SQLiteOfflineEntity>())
        return;
      foreach (var data in entities.GroupBy<SQLiteOfflineEntity, Type>((Func<SQLiteOfflineEntity, Type>) (e => e.GetType())).Select(entitiesPerGroup => new
      {
        Type = entitiesPerGroup.Key,
        Entities = entitiesPerGroup
      }))
        this.sqliteHelper.MergeEntities(data.Type, data.Entities.ToList<SQLiteOfflineEntity>());
    }
  }
}
