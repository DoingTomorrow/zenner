// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Settings.SendDataBeforeChangeServerViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using MSS.Business.Modules.Cleanup;
using MSS.Business.Modules.Synchronization;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Settings
{
  internal class SendDataBeforeChangeServerViewModel : MSSViewModel
  {
    private string _serverUrl;
    private string _sqliteSyncWebServiceUrl;
    private string _dialogTitle;
    private IRepositoryFactory _repositoryFactory;

    public SendDataBeforeChangeServerViewModel(
      IRepositoryFactory repositoryFactory,
      string serverUrl,
      string sqliteSyncWebServiceUrl)
    {
      this._repositoryFactory = repositoryFactory;
      this._serverUrl = serverUrl;
      this._sqliteSyncWebServiceUrl = sqliteSyncWebServiceUrl;
      this.DialogTitle = CultureResources.GetValue("MSS_CHANGE_SERVER_TITLE");
    }

    public new string DialogTitle
    {
      get => this._dialogTitle;
      set
      {
        this._dialogTitle = value;
        this.OnPropertyChanged(nameof (DialogTitle));
      }
    }

    public ICommand DeleteAllItemsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          using (ServiceClient serviceClient = new ServiceClient(this._serverUrl))
            serviceClient.InsertNewClient(MSS.Business.Utils.AppContext.Current.MSSClientId, MSS.Business.Utils.AppContext.Current.LoggedUser.Username, MSS.Business.Utils.AppContext.Current.LoggedUser.Id);
          DatabaseCleanupManager databaseCleanupManager = new DatabaseCleanupManager(this._repositoryFactory);
          bool propertyValue = CustomerConfiguration.GetPropertyValue<bool>("IsPartialSync");
          databaseCleanupManager.CleanupDatabaseOnChangingServer(propertyValue);
          MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
          if (ConfigurationManager.AppSettings["DatabaseEngine"] == "MSSQLDatabase")
            this._repositoryFactory.GetSynchronizationRepository().CleanMSSQLSyncTables();
          else if (!string.IsNullOrEmpty(parametersManagement.GetAppParam("ServerIp").Value))
            this._repositoryFactory.GetSynchronizationRepository().CleanSQLiteSyncTables(propertyValue);
          ApplicationParameter appParam1 = parametersManagement.GetAppParam("ServerIp");
          appParam1.Value = this._serverUrl;
          this._repositoryFactory.GetRepository<ApplicationParameter>().Update(appParam1);
          ApplicationParameter appParam2 = parametersManagement.GetAppParam("SqliteSyncWebServiceUrl");
          appParam2.Value = this._sqliteSyncWebServiceUrl;
          this._repositoryFactory.GetRepository<ApplicationParameter>().Update(appParam2);
          this.ProvisionClientDatabase(this._serverUrl);
          if (Application.ResourceAssembly.Location != null)
            Process.Start(Application.ResourceAssembly.Location);
          Application.Current.Shutdown();
        });
      }
    }

    private void ProvisionClientDatabase(string serverIp)
    {
      string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
      if (!(appSetting == "MSSQLDatabase"))
        return;
      using (SqlConnection sqlConnection = new SqlConnection(NHibernateConfigurationHelper.GetPropertyValue(appSetting, "connection.connection_string")))
      {
        this.ProvisionClientDB(sqlConnection, SyncScopesEnum.Configuration, serverIp);
        this.ProvisionClientDB(sqlConnection, SyncScopesEnum.Application, serverIp);
        this.ProvisionClientDB(sqlConnection, SyncScopesEnum.Users, serverIp);
        this.ProvisionClientDB(sqlConnection, SyncScopesEnum.ReadingValues, serverIp);
      }
    }

    private void ProvisionClientDB(
      SqlConnection sqlConnection,
      SyncScopesEnum syncScope,
      string serverIp)
    {
      SqlSyncScopeProvisioning scopeProvisioning = new SqlSyncScopeProvisioning(sqlConnection);
      if (scopeProvisioning.ScopeExists(syncScope.ToString()))
        return;
      SynchronizationServiceClient synchronizationServiceClient = new SynchronizationServiceClient(syncScope.ToString(), serverIp);
      DbSyncScopeDescription scopeDescription = synchronizationServiceClient.GetScopeDescription(DatabaseEngineEnum.MSSQLDatabase.ToString());
      synchronizationServiceClient.Dispose();
      scopeProvisioning.PopulateFromScopeDescription(scopeDescription);
      scopeProvisioning.Apply();
    }
  }
}
