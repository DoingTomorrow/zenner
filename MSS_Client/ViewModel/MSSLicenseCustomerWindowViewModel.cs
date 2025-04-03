// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.MSSLicenseCustomerWindowViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Utils;
using MSS.Core.Model.TechnicalParameters;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Commands;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel
{
  internal class MSSLicenseCustomerWindowViewModel : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private ViewModelBase _messageUserControl;
    private bool _isBrowseButtonEnabled;
    private string _customerNumber;

    [Inject]
    public MSSLicenseCustomerWindowViewModel(
      LicenseProblemTypeEnum licenseProblemType,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      switch (licenseProblemType)
      {
        case LicenseProblemTypeEnum.CustomerNumberDoesNotExist:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_CustomerNumberDoesNotExist;
          this.DialogMessage = Resources.MSS_Client_CustomerNumberDoesNotExist_Message;
          break;
        case LicenseProblemTypeEnum.LicenseNotActiveYet:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseNotActiveYet;
          this.DialogMessage = Resources.MSS_Client_LicenseNotActiveYet_Message;
          break;
        case LicenseProblemTypeEnum.LicenseExpired:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseExpired;
          this.DialogMessage = Resources.MSS_Client_LicenseExpired_Message;
          break;
        case LicenseProblemTypeEnum.LicenseDoesNotExist:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseDoesNotExist;
          this.DialogMessage = Resources.MSS_Client_LicenseDoesNotExist_Message;
          break;
        case LicenseProblemTypeEnum.LicenseInvalidSignature:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseInvalidSignature;
          this.DialogMessage = Resources.MSS_Client_LicenseInvalidSignature_Message;
          break;
        case LicenseProblemTypeEnum.LicenseInvalidHardwareKey:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseInvalidHardwareKey;
          this.DialogMessage = Resources.MSS_Client_LicenseInvalidHardwareKey_Message;
          break;
        case LicenseProblemTypeEnum.LicenseInvalidCustomerNumber:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseInvalidCustomerNumber;
          this.DialogMessage = Resources.MSS_Client_LicenseInvalidCustomerNumber_Message;
          break;
        case LicenseProblemTypeEnum.LicenseAvailabilityOfflineExpired:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_LicenseOfflineExpired;
          this.DialogMessage = Resources.MSS_Client_LicenseOfflineExpired_Message;
          break;
        case LicenseProblemTypeEnum.IsMinoConnectNeeded:
          this.DialogTitle = Resources.MSS_Client_UserControl_Dialog_IsMinoConnectNeeded;
          this.DialogMessage = Resources.MSS_Client_IsMinoConnectNeeded_Message;
          break;
      }
      this.IsBrowseButtonEnabled = LicenseHelper.IsCustomerNumberFilled(MSS.Business.Utils.AppContext.Current.TechnicalParameters);
      this.CustomerNumber = MSS.Business.Utils.AppContext.Current.TechnicalParameters.CustomerNumber;
    }

    public ICommand CloseWindowCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(false)));
    }

    public ICommand BrowseWindowCommand
    {
      get
      {
        return (ICommand) new BrowseCommand((ViewModelBase) this, this._repositoryFactory, this._windowFactory);
      }
    }

    public ICommand CheckCustomerCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          try
          {
            IRepository<TechnicalParameter> repository = this._repositoryFactory.GetRepository<TechnicalParameter>();
            TechnicalParameter entity = repository.FirstOrDefault((Expression<Func<TechnicalParameter, bool>>) (f => true));
            entity.CustomerNumber = this.CustomerNumber.Trim();
            repository.Update(entity);
            MSS.Business.Utils.AppContext.Current.TechnicalParameters = entity;
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
            this.IsBrowseButtonEnabled = LicenseHelper.IsCustomerNumberFilled(MSS.Business.Utils.AppContext.Current.TechnicalParameters);
            try
            {
              if (LicenseWebApiHandler.CheckIfCustomerExists(this.CustomerNumber))
              {
                this._repositoryFactory.UpdateLicense();
                MSSUIHelper.UpdateTheApplicationVersionInformation();
              }
            }
            catch (Exception ex)
            {
              MessageHandler.LogException(ex);
            }
            MSS.Business.Utils.AppContext.Current.Operations = LicenseHelper.GetOperations(MSS.Business.Utils.AppContext.Current.LoggedUser, this._repositoryFactory.GetUserRepository().GetOperations(MSS.Business.Utils.AppContext.Current.LoggedUser));
            MSSHelper.InitializeGMMAndSetEquipment();
            MSSUIHelper.ShowApplicationStartWindow(this._repositoryFactory, this._windowFactory);
            this.OnRequestClose(false);
          }
          catch (Exception ex)
          {
            MessageHandler.LogException(ex);
          }
        }));
      }
    }

    public string HardwareKey
    {
      get
      {
        string validHardwareKey = LicenseHelper.GetValidHardwareKey();
        return !string.IsNullOrEmpty(validHardwareKey) ? validHardwareKey : HardwareKeyGenerator.GetHardwareUniqueKey();
      }
    }

    public string DialogTitle { get; private set; }

    public string DialogMessage { get; private set; }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public bool IsBrowseButtonEnabled
    {
      get => this._isBrowseButtonEnabled;
      set
      {
        this._isBrowseButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsBrowseButtonEnabled));
      }
    }

    public string CustomerNumber
    {
      get => this._customerNumber;
      set
      {
        this._customerNumber = value;
        this.OnPropertyChanged(nameof (CustomerNumber));
      }
    }
  }
}
