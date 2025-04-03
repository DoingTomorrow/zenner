// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Settings.EditServerPathViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Configuration;
using System.Net;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Settings
{
  public class EditServerPathViewModel : MSSViewModel
  {
    private string _dialogTitle;
    private readonly IWindowFactory _windowFactory;
    private readonly IRepositoryFactory _repositoryFactory;
    private bool _isServerIpOk;
    private string _submitButtonVisibility;
    private string _serverUrl;
    private string _sqliteSyncWebServiceUrl;
    private bool _isconnectionok;
    private ViewModelBase _messageUserControl;

    [Inject]
    public EditServerPathViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this._sqliteSyncWebServiceUrl = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory).GetAppParam(nameof (SqliteSyncWebServiceUrl)).Value;
      this._serverUrl = "";
      this.DialogTitle = CultureResources.GetValue("MSS_CHANGE_SERVER_TITLE");
      this.IsConnectionOk = true;
      this.SubmitButtonVisibility = "0";
      if ((DatabaseEngineEnum) System.Enum.Parse(typeof (DatabaseEngineEnum), ConfigurationManager.AppSettings["DatabaseEngine"]) != DatabaseEngineEnum.SQLiteDatabase)
        return;
      this.IsSqLiteDb = true;
    }

    public ICommand CancelAndCloseProxyCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.OnRequestClose(false);
          MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = false;
        });
      }
    }

    public ICommand ChangeServerCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          bool flag1 = true;
          bool propertyValue = CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode");
          IPAddress address = (IPAddress) null;
          bool flag2 = IPAddress.TryParse(this.ServerUrl, out address);
          Uri result = (Uri) null;
          bool flag3 = Uri.TryCreate(this.ServerUrl, UriKind.RelativeOrAbsolute, out result);
          if (string.IsNullOrWhiteSpace(this.ServerUrl) || !flag2 || propertyValue && !flag3)
            flag1 = false;
          if (!flag1)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Error_Message_Invalid_IP);
          }
          else
          {
            this.ServerUpToDate += Resources.DATA_WILL_BE_LOST;
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<SendDataBeforeChangeServerViewModel>((IParameter) new ConstructorArgument("serverUrl", (object) this.ServerUrl), (IParameter) new ConstructorArgument("sqliteSyncWebServiceUrl", (object) this.SqliteSyncWebServiceUrl)));
          }
        });
      }
    }

    public ICommand TestServerConnectionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!string.IsNullOrWhiteSpace(this.ServerUrl))
          {
            bool propertyValue = CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode");
            IPAddress address = (IPAddress) null;
            bool flag1 = IPAddress.TryParse(this.ServerUrl, out address);
            Uri result = (Uri) null;
            bool flag2 = Uri.TryCreate(this.ServerUrl, UriKind.RelativeOrAbsolute, out result);
            if (flag1 || propertyValue & flag2)
            {
              try
              {
                if (new SettingsConnectionManager().TestConnectiontoServer(this.ServerUrl))
                {
                  this.IsConnectionOk = false;
                  this.SubmitButtonVisibility = "70";
                  this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation);
                }
                else
                  this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Server_Not_Available);
              }
              catch (Exception ex)
              {
                this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Server_Not_Available);
              }
            }
          }
          MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = false;
        });
      }
    }

    public bool IsServerIpOk
    {
      get => this._isServerIpOk;
      set
      {
        this._isServerIpOk = value;
        this.OnPropertyChanged(nameof (IsServerIpOk));
      }
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

    public string SubmitButtonVisibility
    {
      get => this._submitButtonVisibility;
      set
      {
        this._submitButtonVisibility = value;
        this.OnPropertyChanged(nameof (SubmitButtonVisibility));
      }
    }

    public string ServerUrl
    {
      get => this._serverUrl;
      set
      {
        this._serverUrl = value;
        this.OnPropertyChanged(nameof (ServerUrl));
      }
    }

    public string SqliteSyncWebServiceUrl
    {
      get => this._sqliteSyncWebServiceUrl;
      set
      {
        this._sqliteSyncWebServiceUrl = value;
        this.OnPropertyChanged(nameof (SqliteSyncWebServiceUrl));
      }
    }

    public bool IsConnectionOk
    {
      get => this._isconnectionok;
      set
      {
        this._isconnectionok = value;
        this.IsServerIpOk = value;
        this.OnPropertyChanged(nameof (IsConnectionOk));
      }
    }

    public bool IsSqLiteDb { get; set; }

    public new ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }
  }
}
