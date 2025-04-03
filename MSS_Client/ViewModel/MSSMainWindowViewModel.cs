// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.MSSMainWindowViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Languages;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Properties;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel
{
  public class MSSMainWindowViewModel : ViewModelBase
  {
    private bool iToggleStateCulture;
    private readonly IRepository<RoleOperation> _roleOperationRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private byte[] _logoImage;

    [Inject]
    public MSSMainWindowViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._userRepository = repositoryFactory.GetUserRepository();
      this._userRoleRepository = repositoryFactory.GetRepository<UserRole>();
      this._roleOperationRepository = repositoryFactory.GetRepository<RoleOperation>();
      this._roleRepository = repositoryFactory.GetRepository<Role>();
      this.LogoImage = LicenseHelper.GetLogoImage();
      if ((LangEnum) Enum.Parse(typeof (LangEnum), MSS.Business.Utils.AppContext.Current.LoggedUser.Language) == LangEnum.German)
        this.iToggleStateCulture = true;
      this.CheckUserRights();
      this.SetCulture();
    }

    private void CheckUserRights()
    {
      UsersManager usersManager1 = new UsersManager(this._repositoryFactory);
      bool flag1 = usersManager1.HasRight(OperationEnum.ArchivingOnDemand.ToString()) || usersManager1.HasRight(OperationEnum.CleanupOnDemand.ToString()) || usersManager1.HasRight(OperationEnum.ArchivedDataView.ToString()) || usersManager1.HasRight(OperationEnum.ArchiveJobView.ToString());
      this.ArchivingVisibility = "0";
      this.OrdersVisibility = usersManager1.HasRight(OperationEnum.InstallationOrderView.ToString()) || usersManager1.HasRight(OperationEnum.ReadingOrderView.ToString()) ? "*" : "0";
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.ApplicationSettingsUpdate;
      string operation1 = operationEnum.ToString();
      int num1;
      if (!usersManager2.HasRight(operation1))
      {
        UsersManager usersManager3 = usersManager1;
        operationEnum = OperationEnum.ApplicationSettingsChangeServer;
        string operation2 = operationEnum.ToString();
        num1 = usersManager3.HasRight(operation2) ? 1 : 0;
      }
      else
        num1 = 1;
      bool flag2 = num1 != 0;
      this.SettingsVisibility = "0";
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.FixedStructureView;
      string operation3 = operationEnum.ToString();
      int num2;
      if (!usersManager4.HasRight(operation3))
      {
        UsersManager usersManager5 = usersManager1;
        operationEnum = OperationEnum.LogicalStructureView;
        string operation4 = operationEnum.ToString();
        if (!usersManager5.HasRight(operation4))
        {
          UsersManager usersManager6 = usersManager1;
          operationEnum = OperationEnum.PhysicalStructureView;
          string operation5 = operationEnum.ToString();
          num2 = usersManager6.HasRight(operation5) ? 1 : 0;
          goto label_7;
        }
      }
      num2 = 1;
label_7:
      this.StructuresVisibility = num2 != 0 ? "*" : "0";
      UsersManager usersManager7 = usersManager1;
      operationEnum = OperationEnum.JobView;
      string operation6 = operationEnum.ToString();
      int num3;
      if (!usersManager7.HasRight(operation6))
      {
        UsersManager usersManager8 = usersManager1;
        operationEnum = OperationEnum.JobLogsView;
        string operation7 = operationEnum.ToString();
        if (!usersManager8.HasRight(operation7))
        {
          UsersManager usersManager9 = usersManager1;
          operationEnum = OperationEnum.JobDefinitionView;
          string operation8 = operationEnum.ToString();
          if (!usersManager9.HasRight(operation8))
          {
            UsersManager usersManager10 = usersManager1;
            operationEnum = OperationEnum.ScenarioView;
            string operation9 = operationEnum.ToString();
            if (!usersManager10.HasRight(operation9))
            {
              UsersManager usersManager11 = usersManager1;
              operationEnum = OperationEnum.ServiceJobView;
              string operation10 = operationEnum.ToString();
              num3 = usersManager11.HasRight(operation10) ? 1 : 0;
              goto label_13;
            }
          }
        }
      }
      num3 = 1;
label_13:
      this.JobsVisibility = num3 != 0 ? "*" : "0";
      UsersManager usersManager12 = usersManager1;
      operationEnum = OperationEnum.UserView;
      string operation11 = operationEnum.ToString();
      int num4;
      if (!usersManager12.HasRight(operation11))
      {
        UsersManager usersManager13 = usersManager1;
        operationEnum = OperationEnum.UserRoleView;
        string operation12 = operationEnum.ToString();
        num4 = usersManager13.HasRight(operation12) ? 1 : 0;
      }
      else
        num4 = 1;
      bool flag3 = num4 != 0;
      this.UsersVisibility = "0";
      UsersManager usersManager14 = usersManager1;
      operationEnum = OperationEnum.AutomatedExportView;
      string operation13 = operationEnum.ToString();
      int num5;
      if (!usersManager14.HasRight(operation13))
      {
        UsersManager usersManager15 = usersManager1;
        operationEnum = OperationEnum.ReadingDataView;
        string operation14 = operationEnum.ToString();
        num5 = usersManager15.HasRight(operation14) ? 1 : 0;
      }
      else
        num5 = 1;
      bool flag4 = num5 != 0;
      this.DataAndReportsVisibility = "0";
      UsersManager usersManager16 = usersManager1;
      operationEnum = OperationEnum.ConfigurationView;
      string operation15 = operationEnum.ToString();
      int num6;
      if (!usersManager16.HasRight(operation15))
      {
        UsersManager usersManager17 = usersManager1;
        operationEnum = OperationEnum.DeviceRead;
        string operation16 = operationEnum.ToString();
        if (!usersManager17.HasRight(operation16))
        {
          UsersManager usersManager18 = usersManager1;
          operationEnum = OperationEnum.DeviceConfigure;
          string operation17 = operationEnum.ToString();
          if (!usersManager18.HasRight(operation17))
          {
            UsersManager usersManager19 = usersManager1;
            operationEnum = OperationEnum.HistoryView;
            string operation18 = operationEnum.ToString();
            num6 = usersManager19.HasRight(operation18) ? 1 : 0;
            goto label_24;
          }
        }
      }
      num6 = 1;
label_24:
      this.ConfigurationVisibility = num6 != 0 ? "*" : "0";
      UsersManager usersManager20 = usersManager1;
      operationEnum = OperationEnum.DataCollectorView;
      string operation19 = operationEnum.ToString();
      this.MinomatsVisibility = usersManager20.HasRight(operation19) ? "*" : "0";
    }

    public ICommand btnModulesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          string name = (parameter as Button).Name;
          EModul emodul = EModul.Meters;
          switch (name)
          {
            case "btnArchiving":
              emodul = EModul.Archiving;
              break;
            case "btnConfiguration":
              emodul = EModul.Configuration;
              break;
            case "btnDataCollectors":
              emodul = EModul.DataCollectors;
              break;
            case "btnJobs":
              emodul = EModul.Jobs;
              break;
            case "btnMeters":
              emodul = EModul.Meters;
              break;
            case "btnOrders":
              emodul = EModul.Orders;
              break;
            case "btnReporting":
              emodul = EModul.Reporting;
              break;
            case "btnSettings":
              emodul = EModul.Settings;
              break;
            case "btnStructures":
              emodul = EModul.Structures;
              break;
            case "btnUsers":
              emodul = EModul.Users;
              break;
          }
          this._windowFactory.CreateNewNonModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSViewModel>((IParameter) new ConstructorArgument("activeModule", (object) emodul), (IParameter) new ConstructorArgument("logoImage", (object) this.LogoImage)));
          this.CloseAllWindows();
        }));
      }
    }

    public ICommand LogoutCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!MSSUIHelper.ShowSendChangesWarningDialog_AtLogout())
            return;
          Settings.Default.RememberedUserId = Guid.Empty;
          Settings.Default.Save();
          MSS.Business.Utils.AppContext.Current.LoggedUser = (User) null;
          this._windowFactory.CreateNewNonModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSLoginWindowViewModel>());
          this.CloseAllWindows();
        }));
      }
    }

    private void CloseAllWindows() => this.OnRequestClose(false);

    public void SetCulture()
    {
      if (this.iToggleStateCulture)
      {
        CultureResources.ChangeCulture(new CultureInfo("de"));
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
      }
      if (!this.iToggleStateCulture)
      {
        CultureResources.SetDefaultCulture();
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        this.iToggleStateCulture = true;
      }
      else
        this.iToggleStateCulture = false;
    }

    public string StructuresVisibility { get; set; }

    public string JobsVisibility { get; set; }

    public string SettingsVisibility { get; set; }

    public string UsersVisibility { get; set; }

    public string DataAndReportsVisibility { get; set; }

    public string ConfigurationVisibility { get; set; }

    public string MinomatsVisibility { get; set; }

    public string OrdersVisibility { get; set; }

    public string ArchivingVisibility { get; set; }

    public string ApplicationName => ConfigurationManager.AppSettings[nameof (ApplicationName)];

    public byte[] LogoImage
    {
      get => this._logoImage;
      set
      {
        this._logoImage = value;
        this.OnPropertyChanged(nameof (LogoImage));
      }
    }
  }
}
