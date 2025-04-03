// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.MSSLoginWindowViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using CA.Cryptography;
using MSS.Business.Errors;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.PartialSync.PartialSync.Managers;
using MSS.Utils.Utils;
using MSS_Client.Properties;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using ZENNER;

#nullable disable
namespace MSS_Client.ViewModel
{
  internal class MSSLoginWindowViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private string _usernameTextValue = string.Empty;
    private bool _isInfoVisible;
    private string _sourceImage;

    [Inject]
    public MSSLoginWindowViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.RememberUsername = false;
      this.IncorrectPasswordErrorMessage = Visibility.Hidden;
      this.IncorrectUsernameErrorMessage = Visibility.Hidden;
      this.SourceImage = "pack://application:,,,/Styles;component/Images/Universal/info.png";
    }

    [Required(ErrorMessage = "MSS_Client_UserControl_Dialog_UsernameValidationMessage")]
    public string UsernameTextValue
    {
      get => this._usernameTextValue;
      set
      {
        this._usernameTextValue = value;
        this.IncorrectPasswordErrorMessage = Visibility.Hidden;
        this.OnPropertyChanged("IncorrectPasswordErrorMessage");
      }
    }

    public Visibility IncorrectPasswordErrorMessage { get; set; }

    public Visibility IncorrectUsernameErrorMessage { get; set; }

    public bool RememberUsername { get; set; }

    public System.Windows.Input.ICommand LoginUserCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          IUserRepository userRepository = this._repositoryFactory.GetUserRepository();
          PasswordManager passwordManager = new PasswordManager();
          PasswordBox passwordBox = parameter as PasswordBox;
          User user = userRepository.FirstOrDefault((Expression<Func<User, bool>>) (usr => usr.Username.ToString() == this.UsernameTextValue.ToString() && usr.IsDeactivated == false));
          if (user == null)
          {
            this.IncorrectUsernameErrorMessage = Visibility.Visible;
            this.OnPropertyChanged("IncorrectUsernameErrorMessage");
          }
          else if (!passwordManager.VerifyPassowrdHashString(passwordBox.Password, user.Password) || user.IsDeactivated)
          {
            if (Settings.Default.NoOfLoginAttempts < (short) 3)
            {
              Settings settings = Settings.Default;
              settings.NoOfLoginAttempts++;
              Settings.Default.LastLoginAttempt = DateTime.Now;
              Settings.Default.Save();
            }
            else if (MSSUIHelper.IsUserAllowedToLogin())
              this.ResetLoginFlags();
            this.IncorrectUsernameErrorMessage = Visibility.Hidden;
            this.OnPropertyChanged("IncorrectUsernameErrorMessage");
            this.IncorrectPasswordErrorMessage = Visibility.Visible;
            this.OnPropertyChanged("IncorrectPasswordErrorMessage");
            if (MSSUIHelper.IsUserAllowedToLogin())
              return;
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_WrongPassword, false);
            this.OnRequestClose(false);
          }
          else
          {
            this.ResetLoginFlags();
            MSS.Business.Utils.AppContext.Current.LoggedUser = user;
            if (CustomerConfiguration.GetPropertyValue<bool>("IsPartialSync"))
            {
              if (this.PartialSyncNotPossible(user))
                return;
              this.ExecuteInitialPartialSync();
            }
            MSS.Business.Utils.AppContext.Current.Operations = LicenseHelper.GetOperations(MSS.Business.Utils.AppContext.Current.LoggedUser, this._repositoryFactory);
            if (this.RememberUsername)
            {
              Settings.Default.RememberedUserId = user.Id;
              Settings.Default.Save();
            }
            this._windowFactory.CreateNewNonModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSViewModel>((IParameter) new ConstructorArgument("logoImage", (object) LicenseHelper.GetLogoImage())));
            this.OnRequestClose(false);
          }
        }));
      }
    }

    private bool PartialSyncNotPossible(User user)
    {
      if (this._repositoryFactory.GetRepository<OrderUser>().Exists((Expression<Func<OrderUser, bool>>) (_ => false)) && (this._repositoryFactory.GetRepository<StructureNodeType>().GetAll_PageCount(1) <= 0 || !MSS.Business.Utils.AppContext.Current.HasServer))
        return true;
      if (this._repositoryFactory.GetRepository<OrderUser>().Exists((Expression<Func<OrderUser, bool>>) (_ => _.User.Id != user.Id)))
      {
        bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CleanupAppDataMessageViewModel>());
        bool flag = false;
        bool? nullable = newModalDialog;
        if (flag == nullable.GetValueOrDefault() && nullable.HasValue)
          return true;
      }
      return false;
    }

    private void ExecuteInitialPartialSync()
    {
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) MSS.Localisation.Resources.MSS_Client_Synchronization_Dialog), (IParameter) new ConstructorArgument("progressDialogMessage", (object) MSS.Localisation.Resources.MSS_CLIENT_Synchronization_Message));
      backgroundWorker.DoWork += (DoWorkEventHandler) ((s, arg) => new ClientPartialSyncManager().ExecuteInitialSynchronization(MSS.Business.Utils.AppContext.Current.LoggedUser.Id));
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((s, args) => pd.OnRequestClose(false));
      backgroundWorker.RunWorkerAsync();
      this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
    }

    public System.Windows.Input.ICommand CloseWindowCommand
    {
      get => (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(false)));
    }

    public System.Windows.Input.ICommand ShowInfoCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          this.IsInfoVisible = !this.IsInfoVisible;
          bool isInfoVisible = this.IsInfoVisible;
          if (isInfoVisible)
          {
            if (!isInfoVisible)
              return;
            this.SourceImage = "pack://application:,,,/Styles;component/Images/Universal/info-deselect.png";
          }
          else
            this.SourceImage = "pack://application:,,,/Styles;component/Images/Universal/info.png";
        }));
      }
    }

    public string HardwareKey => LicenseHelper.GetValidHardwareKey();

    public string VersionNumber => ConfigurationManager.AppSettings["ApplicationVersion"];

    public string GMMMetrologicalCore => GmmInterface.GetMetrologicalCore();

    public string CurrentLicense
    {
      get => LicenseHelper.GetCurrentLicenseType(LicenseHelper.GetValidHardwareKey());
    }

    public bool IsInfoVisible
    {
      get => this._isInfoVisible;
      set
      {
        this._isInfoVisible = value;
        this.OnPropertyChanged(nameof (IsInfoVisible));
      }
    }

    public string SourceImage
    {
      get => this._sourceImage;
      set
      {
        this._sourceImage = value;
        this.OnPropertyChanged(nameof (SourceImage));
      }
    }

    private void ResetLoginFlags()
    {
      Settings.Default.NoOfLoginAttempts = (short) 0;
      Settings.Default.LastLoginAttempt = DateTime.Now;
      Settings.Default.Save();
    }
  }
}
