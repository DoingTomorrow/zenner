// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DatabaseSelectorViewModel
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

#nullable disable
namespace GmmDbLib
{
  public class DatabaseSelectorViewModel : DependencyObject
  {
    public static readonly DependencyProperty SelectedDatabaseInstanceProperty = DependencyProperty.Register(nameof (SelectedDatabaseInstanceProperty), typeof (DbInstances), typeof (DatabaseSelectorViewModel), (PropertyMetadata) new FrameworkPropertyMetadata((object) DbInstances.Primary, new PropertyChangedCallback(DatabaseSelectorViewModel.DatabaseInstanceChanged)));
    public static readonly DependencyProperty SelectedDatabaseTypeProperty = DependencyProperty.Register(nameof (SelectedDatabaseTypeProperty), typeof (MeterDbTypes), typeof (DatabaseSelectorViewModel), (PropertyMetadata) new FrameworkPropertyMetadata((object) MeterDbTypes.Access, new PropertyChangedCallback(DatabaseSelectorViewModel.DatabaseTypeChanged)));
    public static readonly DependencyProperty URLorPathProperty = DependencyProperty.Register(nameof (URLorPathProperty), typeof (string), typeof (DatabaseSelectorViewModel), (PropertyMetadata) new FrameworkPropertyMetadata((object) "", new PropertyChangedCallback(DatabaseSelectorViewModel.URLoPathChanged)));
    public static readonly DependencyProperty DatabaseNameProperty = DependencyProperty.Register(nameof (DatabaseNameProperty), typeof (string), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(nameof (UserNameProperty), typeof (string), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof (PasswordProperty), typeof (string), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty DatabaseInfoProperty = DependencyProperty.Register(nameof (DatabaseInfoProperty), typeof (string), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty PathRequestedProperty = DependencyProperty.Register(nameof (PathRequestedProperty), typeof (bool), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty URL_RequestedProperty = DependencyProperty.Register(nameof (URL_RequestedProperty), typeof (bool), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty DatabaseNameRequestedProperty = DependencyProperty.Register(nameof (DatabaseNameRequestedProperty), typeof (bool), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty UserNameRequestedProperty = DependencyProperty.Register(nameof (UserNameRequestedProperty), typeof (bool), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty PasswordRequestedProperty = DependencyProperty.Register(nameof (PasswordRequestedProperty), typeof (bool), typeof (DatabaseSelectorViewModel));
    public static readonly DependencyProperty SelectionOkProperty = DependencyProperty.Register(nameof (SelectionOkProperty), typeof (bool), typeof (DatabaseSelectorViewModel));
    private DbConnectionInfo workConnectionInfo;
    private List<DbConnectionInfo> AllConnectionInfos;
    private DbConnectionInfo ActiveConnectionInfo;
    private DbConnectionInfo localPrimaryDbConnectionInfo;
    private DbConnectionInfo localSecundaryDbConnectionInfo;

    public static void DatabaseInstanceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs args)
    {
      DbInstances newValue = (DbInstances) args.NewValue;
      ((DatabaseSelectorViewModel) d).SetDbInstance(newValue);
    }

    public static void DatabaseTypeChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs args)
    {
      MeterDbTypes newValue = (MeterDbTypes) args.NewValue;
      MeterDbTypes oldValue = (MeterDbTypes) args.OldValue;
      ((DatabaseSelectorViewModel) d).SetDbType(newValue);
    }

    public List<DbInstances> DatabaseInstances { get; set; }

    public DbInstances SelectedDatabaseInstance
    {
      get
      {
        return (DbInstances) this.GetValue(DatabaseSelectorViewModel.SelectedDatabaseInstanceProperty);
      }
      set
      {
        this.SetValue(DatabaseSelectorViewModel.SelectedDatabaseInstanceProperty, (object) value);
      }
    }

    public List<MeterDbTypes> DatabaseTypes { get; set; }

    public MeterDbTypes SelectedDatabaseType
    {
      get => (MeterDbTypes) this.GetValue(DatabaseSelectorViewModel.SelectedDatabaseTypeProperty);
      set => this.SetValue(DatabaseSelectorViewModel.SelectedDatabaseTypeProperty, (object) value);
    }

    public string URLorPath
    {
      get => (string) this.GetValue(DatabaseSelectorViewModel.URLorPathProperty);
      set => this.SetValue(DatabaseSelectorViewModel.URLorPathProperty, (object) value);
    }

    public string DatabaseName
    {
      get => (string) this.GetValue(DatabaseSelectorViewModel.DatabaseNameProperty);
      set => this.SetValue(DatabaseSelectorViewModel.DatabaseNameProperty, (object) value);
    }

    public string UserName
    {
      get => (string) this.GetValue(DatabaseSelectorViewModel.UserNameProperty);
      set => this.SetValue(DatabaseSelectorViewModel.UserNameProperty, (object) value);
    }

    public string Password
    {
      get => (string) this.GetValue(DatabaseSelectorViewModel.PasswordProperty);
      set => this.SetValue(DatabaseSelectorViewModel.PasswordProperty, (object) value);
    }

    public string DatabaseInfo
    {
      get => (string) this.GetValue(DatabaseSelectorViewModel.DatabaseInfoProperty);
      set => this.SetValue(DatabaseSelectorViewModel.DatabaseInfoProperty, (object) value);
    }

    public bool PathRequested
    {
      get => (bool) this.GetValue(DatabaseSelectorViewModel.PathRequestedProperty);
      set => this.SetValue(DatabaseSelectorViewModel.PathRequestedProperty, (object) value);
    }

    public bool URL_Requested
    {
      get => (bool) this.GetValue(DatabaseSelectorViewModel.URL_RequestedProperty);
      set => this.SetValue(DatabaseSelectorViewModel.URL_RequestedProperty, (object) value);
    }

    public bool DatabaseNameRequested
    {
      get => (bool) this.GetValue(DatabaseSelectorViewModel.DatabaseNameRequestedProperty);
      set => this.SetValue(DatabaseSelectorViewModel.DatabaseNameRequestedProperty, (object) value);
    }

    public bool UserNameRequested
    {
      get => (bool) this.GetValue(DatabaseSelectorViewModel.UserNameRequestedProperty);
      set => this.SetValue(DatabaseSelectorViewModel.UserNameRequestedProperty, (object) value);
    }

    public bool PasswordRequested
    {
      get => (bool) this.GetValue(DatabaseSelectorViewModel.PasswordRequestedProperty);
      set => this.SetValue(DatabaseSelectorViewModel.PasswordRequestedProperty, (object) value);
    }

    public bool SelectionOk
    {
      get => (bool) this.GetValue(DatabaseSelectorViewModel.SelectionOkProperty);
      set => this.SetValue(DatabaseSelectorViewModel.SelectionOkProperty, (object) value);
    }

    public DatabaseSelectorViewModel(
      DbConnectionInfo activeConnectionInfo,
      List<DbConnectionInfo> allConnectionInfos)
    {
      this.AllConnectionInfos = allConnectionInfos;
      this.ActiveConnectionInfo = activeConnectionInfo;
      this.DatabaseInstances = new List<DbInstances>();
      foreach (DbInstances dbInstances in (DbInstances[]) Enum.GetValues(typeof (DbInstances)))
        this.DatabaseInstances.Add(dbInstances);
      this.DatabaseTypes = new List<MeterDbTypes>();
      foreach (MeterDbTypes meterDbTypes in (MeterDbTypes[]) Enum.GetValues(typeof (MeterDbTypes)))
        this.DatabaseTypes.Add(meterDbTypes);
      this.localPrimaryDbConnectionInfo = allConnectionInfos[0];
      this.localSecundaryDbConnectionInfo = allConnectionInfos[1];
      this.workConnectionInfo = new DbConnectionInfo(activeConnectionInfo);
      this.SetConnectionInfo(this.workConnectionInfo);
    }

    public List<string> GetDbFileExtentions()
    {
      List<string> dbFileExtentions = new List<string>();
      switch (this.SelectedDatabaseType)
      {
        case MeterDbTypes.Access:
          dbFileExtentions.Add("mdb");
          break;
        case MeterDbTypes.SQLite:
          dbFileExtentions.Add("db3");
          break;
        case MeterDbTypes.LocalDB:
          dbFileExtentions.Add("mdf");
          break;
        case MeterDbTypes.Microsoft_SQL_Compact:
          dbFileExtentions.Add("sdf");
          break;
      }
      return dbFileExtentions;
    }

    private void SetDbInstance(DbInstances newDbInstance)
    {
      if (this.workConnectionInfo != null)
      {
        if (this.workConnectionInfo.DbInstance == DbInstances.Primary)
          this.localPrimaryDbConnectionInfo = this.workConnectionInfo;
        else
          this.localSecundaryDbConnectionInfo = this.workConnectionInfo;
      }
      switch (newDbInstance)
      {
        case DbInstances.Primary:
          if (this.localPrimaryDbConnectionInfo != null)
          {
            this.workConnectionInfo = this.localPrimaryDbConnectionInfo;
            break;
          }
          this.workConnectionInfo = new DbConnectionInfo();
          this.workConnectionInfo.DbInstance = DbInstances.Primary;
          break;
        case DbInstances.Secundary:
          if (this.localSecundaryDbConnectionInfo != null)
          {
            this.workConnectionInfo = this.localSecundaryDbConnectionInfo;
          }
          else
          {
            this.workConnectionInfo = new DbConnectionInfo();
            this.workConnectionInfo.DbInstance = DbInstances.Secundary;
          }
          break;
      }
      this.SetConnectionInfo(this.workConnectionInfo);
    }

    private void SetConnectionInfo(DbConnectionInfo workConnectionInfo)
    {
      this.SelectedDatabaseInstance = workConnectionInfo.DbInstance;
      this.SelectedDatabaseType = workConnectionInfo.DbType;
      this.SetDbType(workConnectionInfo.DbType);
      this.URLorPath = workConnectionInfo.UrlOrPath;
      this.DatabaseName = workConnectionInfo.DatabaseName;
      this.UserName = workConnectionInfo.UserName;
      this.Password = workConnectionInfo.Password;
    }

    private void SetDbType(MeterDbTypes newDbType)
    {
      this.URLorPath = string.Empty;
      this.DatabaseInfo = string.Empty;
      switch (newDbType)
      {
        case MeterDbTypes.Access:
          this.PathRequested = true;
          this.URL_Requested = false;
          this.DatabaseNameRequested = false;
          this.UserNameRequested = false;
          this.PasswordRequested = false;
          break;
        case MeterDbTypes.NPGSQL:
          this.PathRequested = false;
          this.URL_Requested = true;
          this.DatabaseNameRequested = true;
          this.UserNameRequested = true;
          this.PasswordRequested = true;
          break;
        case MeterDbTypes.SQLite:
          this.PathRequested = true;
          this.URL_Requested = false;
          this.DatabaseNameRequested = false;
          this.UserNameRequested = false;
          this.PasswordRequested = false;
          break;
        case MeterDbTypes.MSSQL:
          this.PathRequested = false;
          this.URL_Requested = true;
          this.DatabaseNameRequested = true;
          this.UserNameRequested = true;
          this.PasswordRequested = true;
          break;
        case MeterDbTypes.LocalDB:
          this.PathRequested = true;
          this.URL_Requested = false;
          this.DatabaseNameRequested = false;
          this.UserNameRequested = false;
          this.PasswordRequested = false;
          break;
        case MeterDbTypes.Microsoft_SQL_Compact:
          this.PathRequested = true;
          this.URL_Requested = false;
          this.DatabaseNameRequested = false;
          this.UserNameRequested = false;
          this.PasswordRequested = false;
          break;
      }
    }

    public static void URLoPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
    {
      ((DatabaseSelectorViewModel) d).CheckSetOk();
    }

    public void CheckSetOk()
    {
      if (this.PathRequested)
      {
        if (File.Exists(this.URLorPath))
          this.SelectionOk = true;
        else
          this.SelectionOk = false;
      }
      else
        this.SelectionOk = true;
    }

    public string TestDatabaseConnection()
    {
      this.refreshConnectionInfo();
      this.DatabaseInfo = new BaseDbConnection(this.workConnectionInfo).ConnectDatabase();
      return this.DatabaseInfo;
    }

    public DbConnectionInfo SaveDatabaseConnection()
    {
      this.refreshConnectionInfo();
      if (this.workConnectionInfo.DbInstance == DbInstances.Primary)
      {
        DbInstance.PrimaryDb = new BaseDbConnection(this.workConnectionInfo);
        return DbInstance.PrimaryDb.ConnectionInfo;
      }
      DbInstance.SecundaryDb = new BaseDbConnection(this.workConnectionInfo);
      return DbInstance.SecundaryDb.ConnectionInfo;
    }

    private void refreshConnectionInfo()
    {
      this.workConnectionInfo.ConnectionString = string.Empty;
      this.workConnectionInfo.DbType = this.SelectedDatabaseType;
      this.workConnectionInfo.UrlOrPath = this.URLorPath;
      this.workConnectionInfo.DatabaseName = this.DatabaseName;
      this.workConnectionInfo.UserName = this.UserName;
      this.workConnectionInfo.Password = this.Password;
    }
  }
}
