// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.MSSViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Microsoft.Synchronization;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Languages;
using MSS.Business.Modules.Cleanup;
using MSS.Business.Modules.DataCollectorsManagement;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.OrdersManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.Synchronization;
using MSS.Business.Modules.Synchronization.HandleConflicts;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.Client.UI.Common.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.TechnicalParameters;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.PartialSync.PartialSync.Managers;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Archiving;
using MSS_Client.ViewModel.Configuration;
using MSS_Client.ViewModel.DataCollectors;
using MSS_Client.ViewModel.Download;
using MSS_Client.ViewModel.GenericProgressDialog;
using MSS_Client.ViewModel.Jobs;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.NewsAndUpdates;
using MSS_Client.ViewModel.Orders;
using MSS_Client.ViewModel.Reporting;
using MSS_Client.ViewModel.Settings;
using MSS_Client.ViewModel.Structures;
using MSS_Client.ViewModel.Synchronization;
using MSS_Client.ViewModel.Users;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS_Client.ViewModel
{
  public class MSSViewModel : ViewModelBase
  {
    private ObservableCollection<Language> _items;
    private Language _selectedLanguage;
    private BitmapImage _jobsButtonImage;
    private BitmapImage _archivingButtonImage;
    private BitmapImage _configurationButtonImage;
    private BitmapImage _dataCollectorButtonImage;
    private bool _iToggleStateCulture;
    private BitmapImage _ordersButtonImage;
    private BitmapImage _meterButtonImage;
    private BitmapImage _reportingButtonImage;
    private BitmapImage _structuresButtonImage;
    private BitmapImage _settingsButtonImage;
    private ViewModelBase _userControlToBeAdded;
    private BitmapImage _usersButtonImage;
    private IRepositoryFactory _repositoryFactory;
    private IWindowFactory _windowFactory;
    private string _serverUpToDate;
    private int _pageSize;
    private TechnicalParameter _technicalParameter;
    private const int DAYS_OFFLINE_RED = 45;
    private const int DAYS_OFFLINE_YELLOW = 30;
    private bool _areWindowStateParametersSet = false;
    private Dictionary<string, Func<EModul, EModul, BitmapImage>> _resourceAssignmentsDictionary = new Dictionary<string, Func<EModul, EModul, BitmapImage>>();
    private readonly Dictionary<string, List<BitmapImage>> _moduleImagesDictionary = new Dictionary<string, List<BitmapImage>>()
    {
      {
        nameof (DataCollectors),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/minomat-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/minomat.png"))
        }
      },
      {
        nameof (Jobs),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/import-export-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/import-export.png"))
        }
      },
      {
        nameof (Archiving),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/archiving-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/archiving.png"))
        }
      },
      {
        nameof (Users),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/users-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/users.png"))
        }
      },
      {
        nameof (Reporting),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/reporting-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/reporting.png"))
        }
      },
      {
        nameof (Configuration),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/configuration-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/configuration.png"))
        }
      },
      {
        nameof (Structures),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/structure-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/structure.png"))
        }
      },
      {
        nameof (Orders),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/orders-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/orders.png"))
        }
      },
      {
        nameof (Settings),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/Universal/settings-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/Universal/settings.png"))
        }
      },
      {
        nameof (Meters),
        new List<BitmapImage>()
        {
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/meters-selected.png")),
          new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/SmallIcons/meters.png"))
        }
      }
    };
    private bool _newsButtonVisible;
    private bool _showConflictsButtonVisible;
    private ImageSource _sendImage;
    private ImageSource _newsAndUpdatesImage;
    private bool structuresVisibility;
    private bool settingsVisibility;
    private bool jobsVisibility;
    private bool _usersVisibility;
    private Brush _settingsBackgroundBrush = (Brush) Brushes.Transparent;
    private Brush _settingsForegroundBrush = (Brush) Brushes.Black;
    private bool _isBusy;
    private string _busyContent;
    private WindowState _currentWindowState;
    private double _windowWidth;
    private double _windowHeight;
    private string _dialogTitle;
    private ViewModelBase _messageUserControl;
    private bool allowWindowToClose;
    private byte[] _logoImage;
    private byte[] _imageDaysSinceLicenseIsUsedOffine;

    public MSSViewModel()
    {
      this._serverUpToDate = string.Empty;
      this.DialogTitle = CultureResources.GetValue("MSS_CHANGE_SERVER_TITLE");
      if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
        this.ServerUpToDate = "";
      else if (!SychronizationHelperFactory.GetSynchronizationHelper().IsVersionUpToDateSend(CustomerConfiguration.GetPropertyValue<bool>("IsPartialSync")))
        this.ServerUpToDate = MSS.Localisation.Resources.ITEMS_NOT_SYNCH + " ";
      this.ServerUpToDate += MSS.Localisation.Resources.DATA_WILL_BE_LOST;
      this.IsBusy = false;
    }

    private void UpdateLoggedUserLanguage()
    {
      if (!string.IsNullOrEmpty(MSS.Business.Utils.AppContext.Current.LoggedUser.Language))
        return;
      MSS.Business.Utils.AppContext.Current.LoggedUser.Language = LangEnum.English.ToString();
      if (CultureInfo.CurrentCulture.EnglishName.Contains(LangEnum.German.ToString()))
        MSS.Business.Utils.AppContext.Current.LoggedUser.Language = LangEnum.German.ToString();
      new UsersManager(this._repositoryFactory).SetLanguage(MSS.Business.Utils.AppContext.Current.LoggedUser.Id, MSS.Business.Utils.AppContext.Current.LoggedUser.Language);
    }

    public object DisplayWarning()
    {
      this.IsBusy = true;
      if (CommonExtensions.CheckForInternetConnection())
      {
        MSSUIHelper.ShowWarningDialog(this._windowFactory, "Warning", string.Format("Server {0} is not available ", (object) CustomerConfiguration.GetPropertyValue("LicenseWebApi")), true);
        this.IsBusy = false;
      }
      else
        MSSUIHelper.ShowWarningDialog(this._windowFactory, "Warning", "No internet connection available ", true);
      return (object) null;
    }

    public MSSViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      byte[] logoImage)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.IsLogOutOk = false;
      MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = new SettingsConnectionManager().IsServerAvailableAndStatusAccepted(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
      this.ServerButtonsVisible = MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted;
      this.ShowConflictsButtonVisible = MSS.Business.Utils.AppContext.Current.HasConflicts;
      this.LogoImage = logoImage;
      this._pageSize = Convert.ToInt32(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("PageSize"));
      this.SetWindowState();
      EventPublisher.Register<SelectedTabChanged>(new Action<SelectedTabChanged>(this.UpdateSelectedTab));
      this.SetVisibilitiesAndSelectModule();
      EventPublisher.Register<LocalDatabaseModified>(new Action<LocalDatabaseModified>(this.RefreshSendImage));
      this.RefreshSendImage();
      EventPublisher.Register<StructureUpdated>(new Action<StructureUpdated>(this.RefreshStructure));
      EventPublisher.Register<CleanNews>(new Action<CleanNews>(this.RefreshNewsImage));
      EventPublisher.Register<SyncConflictsStateChanged>(new Action<SyncConflictsStateChanged>(this.ShowConflictsButton));
      EventPublisher.Register<LanguageChangedEvent>(new Action<LanguageChangedEvent>(this.ChangeLanguage));
      this.NewsButtonVisible = true;
      this.RefreshNewsImage();
      this.CloseCommand = (ICommand) new DelegateCommand((Action) (() => { }), (Func<bool>) (() => this.AllowWindowToClose));
      this.CloseFailCommand = (ICommand) new DelegateCommand((Action) (() => this.AllowWindowToClose = MSSUIHelper.ShowSendChangesWarningDialog_AtApplicationClose(this._repositoryFactory, this.IsLogOutOk)), (Func<bool>) (() => true));
      this.UpdateLoggedUserLanguage();
      this.SelectedLanguage = this.Languages.FirstOrDefault<Language>((System.Func<Language, bool>) (it => it.Name == (LangEnum) System.Enum.Parse(typeof (LangEnum), MSS.Business.Utils.AppContext.Current.LoggedUser.Language)));
      this.CurrentSelection = this.ActiveModule;
      this.RegisterStrategies();
      this.InjectUserControl();
      this.InitializeView();
      MSSUIHelper.NotifyUserIfLicenseInvalidOffline(repositoryFactory);
      MSSUIHelper.NotifyUserIfLicenseExpires(repositoryFactory);
      this._technicalParameter = this._repositoryFactory.GetRepository<TechnicalParameter>().FirstOrDefault((Expression<System.Func<TechnicalParameter, bool>>) (tp => true));
    }

    public EModul CurrentSelection { get; set; }

    private void RegisterStrategies()
    {
      TypeHelperExtensionMethods.ForEach<PropertyInfo>(((IEnumerable<PropertyInfo>) this.GetType().GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (_ => _.PropertyType == typeof (BitmapImage))), (Action<PropertyInfo>) (_ => this._resourceAssignmentsDictionary.Add(_.Name, (Func<EModul, EModul, BitmapImage>) ((ActiveModule, CurrentSelection) =>
      {
        List<BitmapImage> source;
        this._moduleImagesDictionary.TryGetValue(_.Name, out source);
        if (!(_.Name == CurrentSelection.ToString()))
          return source.Last<BitmapImage>();
        if (_.Name != ActiveModule.ToString())
          this.InitializeModule(CurrentSelection);
        return source.First<BitmapImage>();
      }))));
    }

    private void PerformPartialSync()
    {
      GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) MSS.Localisation.Resources.MSS_Client_Synchronization_Dialog), (IParameter) new ConstructorArgument("progressDialogMessage", (object) MSS.Localisation.Resources.MSS_CLIENT_Synchronization_Message));
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      if (!CustomerConfiguration.GetPropertyValue<bool>("IsPartialSync"))
        return;
      backgroundWorker.DoWork += (DoWorkEventHandler) ((s, arg) =>
      {
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Users, SyncDirectionOrder.Download);
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Configuration, SyncDirectionOrder.Download);
        Dictionary<Guid, Type> dictionary = new Dictionary<Guid, Type>();
        foreach (ConflictDetails conflictDetails in MSS.Business.Utils.AppContext.Current.SyncConflicts.Values)
        {
          Type databaseMapping = DatabaseConstants.DatabaseMappings[conflictDetails.ConflictInfo.LocalChange.TableName];
          DataRow row = conflictDetails.ConflictInfo.RemoteChange.Rows[0];
          dictionary.Add((Guid) row.ItemArray[0], databaseMapping);
        }
        EventPublisher.Publish<PartialSyncEvent>(new PartialSyncEvent(), (IViewModel) this);
      });
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((s, args) => pd.OnRequestClose(false));
      backgroundWorker.RunWorkerAsync();
      this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
    }

    static MSSViewModel() => MSSViewModel.ConfigureFELanguage();

    private void UpdateSelectedTab(SelectedTabChanged obj) => this.SelectedTab = obj.SelectedTab;

    private void ChangeLanguage(LanguageChangedEvent e)
    {
      this.SelectedLanguage = this.Languages.FirstOrDefault<Language>((System.Func<Language, bool>) (it => it.Name == e.Language));
    }

    private void RefreshNewsImage(CleanNews e) => this.RefreshNewsImage(e.IsRead);

    private void RefreshNewsImage(bool isRead)
    {
      BitmapImage bitmapImage = new BitmapImage(new Uri(!isRead ? "pack://application:,,,/Styles;component/Images/Universal/notification.png" : "pack://application:,,,/Styles;component/Images/Universal/nonotification.png", UriKind.Absolute));
      bitmapImage.Freeze();
      this.NewsAndUpdatesImage = (ImageSource) bitmapImage;
    }

    public bool NewsButtonVisible
    {
      get => this._newsButtonVisible;
      set
      {
        this._newsButtonVisible = value;
        this.OnPropertyChanged(nameof (NewsButtonVisible));
      }
    }

    private void RefreshSendImage(LocalDatabaseModified e)
    {
      MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend = true;
      if (MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
        MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend = !e.IsChanged;
      this.RefreshSendImage();
    }

    private void RefreshSendImage()
    {
      BitmapImage bitmapImage = new BitmapImage(new Uri(MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend ? "pack://application:,,,/Styles;component/Images/Universal/send.png" : "pack://application:,,,/Styles;component/Images/Universal/send-notification.png", UriKind.Absolute));
      bitmapImage.Freeze();
      this.SendImage = (ImageSource) bitmapImage;
    }

    private void ShowConflictsButton(SyncConflictsStateChanged e)
    {
      this.ShowConflictsButtonVisible = MSS.Business.Utils.AppContext.Current.HasConflicts;
    }

    public void RefreshNewsImage()
    {
      bool flag = this._repositoryFactory.GetRepository<MSS.Core.Model.News.News>().Exists((Expression<System.Func<MSS.Core.Model.News.News, bool>>) (x => x.IsNew && x.StartDate.Date <= DateTime.Today && x.EndDate.Date >= DateTime.Today));
      string uriString = flag ? "pack://application:,,,/Styles;component/Images/Universal/notification.png" : "pack://application:,,,/Styles;component/Images/Universal/nonotification.png";
      if (!flag)
        this.NewsButtonVisible = false;
      this.NewsAndUpdatesImage = (ImageSource) new BitmapImage(new Uri(uriString, UriKind.Absolute));
    }

    public ICommand newsAndUpdatesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<NewsAndUpdatesViewModel>());
        });
      }
    }

    private void RefreshStructure(StructureUpdated update)
    {
      if (update.Guid != Guid.Empty)
        this._repositoryFactory.GetRepository<StructureNode>().Refresh((object) update.Guid);
      if (!(update.EntityId != Guid.Empty))
        return;
      switch (update.EntityType)
      {
        case StructureNodeTypeEnum.Location:
          this._repositoryFactory.GetRepository<Location>().Refresh((object) update.EntityId);
          break;
        case StructureNodeTypeEnum.Tenant:
          this._repositoryFactory.GetRepository<Tenant>().Refresh((object) update.EntityId);
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          this._repositoryFactory.GetRepository<Meter>().Refresh((object) update.EntityId);
          break;
      }
    }

    private void SetVisibilitiesAndSelectModule()
    {
      UsersManager usersManager1 = new UsersManager(this._repositoryFactory);
      this.ArchivingVisibility = usersManager1.HasRight(OperationEnum.ArchivingOnDemand.ToString()) || usersManager1.HasRight(OperationEnum.CleanupOnDemand.ToString()) || usersManager1.HasRight(OperationEnum.ArchivedDataView.ToString()) || usersManager1.HasRight(OperationEnum.ArchiveJobView.ToString());
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.InstallationOrderView;
      string operation1 = operationEnum.ToString();
      int num1;
      if (!usersManager2.HasRight(operation1))
      {
        UsersManager usersManager3 = usersManager1;
        operationEnum = OperationEnum.ReadingOrderView;
        string operation2 = operationEnum.ToString();
        num1 = usersManager3.HasRight(operation2) ? 1 : 0;
      }
      else
        num1 = 1;
      this.OrdersVisibility = num1 != 0;
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.ApplicationSettingsUpdate;
      string operation3 = operationEnum.ToString();
      int num2;
      if (!usersManager4.HasRight(operation3))
      {
        UsersManager usersManager5 = usersManager1;
        operationEnum = OperationEnum.ApplicationSettingsChangeServer;
        string operation4 = operationEnum.ToString();
        num2 = usersManager5.HasRight(operation4) ? 1 : 0;
      }
      else
        num2 = 1;
      this.SettingsVisibility = num2 != 0;
      UsersManager usersManager6 = usersManager1;
      operationEnum = OperationEnum.FixedStructureView;
      string operation5 = operationEnum.ToString();
      int num3;
      if (!usersManager6.HasRight(operation5))
      {
        UsersManager usersManager7 = usersManager1;
        operationEnum = OperationEnum.LogicalStructureView;
        string operation6 = operationEnum.ToString();
        if (!usersManager7.HasRight(operation6))
        {
          UsersManager usersManager8 = usersManager1;
          operationEnum = OperationEnum.PhysicalStructureView;
          string operation7 = operationEnum.ToString();
          num3 = usersManager8.HasRight(operation7) ? 1 : 0;
          goto label_10;
        }
      }
      num3 = 1;
label_10:
      this.StructuresVisibility = num3 != 0;
      UsersManager usersManager9 = usersManager1;
      operationEnum = OperationEnum.UserView;
      string operation8 = operationEnum.ToString();
      int num4;
      if (!usersManager9.HasRight(operation8))
      {
        UsersManager usersManager10 = usersManager1;
        operationEnum = OperationEnum.UserRoleView;
        string operation9 = operationEnum.ToString();
        num4 = usersManager10.HasRight(operation9) ? 1 : 0;
      }
      else
        num4 = 1;
      this.UsersVisibility = num4 != 0;
      UsersManager usersManager11 = usersManager1;
      operationEnum = OperationEnum.JobView;
      string operation10 = operationEnum.ToString();
      int num5;
      if (!usersManager11.HasRight(operation10))
      {
        UsersManager usersManager12 = usersManager1;
        operationEnum = OperationEnum.JobLogsView;
        string operation11 = operationEnum.ToString();
        if (!usersManager12.HasRight(operation11))
        {
          UsersManager usersManager13 = usersManager1;
          operationEnum = OperationEnum.JobDefinitionView;
          string operation12 = operationEnum.ToString();
          if (!usersManager13.HasRight(operation12))
          {
            UsersManager usersManager14 = usersManager1;
            operationEnum = OperationEnum.ScenarioView;
            string operation13 = operationEnum.ToString();
            if (!usersManager14.HasRight(operation13))
            {
              UsersManager usersManager15 = usersManager1;
              operationEnum = OperationEnum.ServiceJobView;
              string operation14 = operationEnum.ToString();
              num5 = usersManager15.HasRight(operation14) ? 1 : 0;
              goto label_19;
            }
          }
        }
      }
      num5 = 1;
label_19:
      this.JobsVisibility = num5 != 0;
      UsersManager usersManager16 = usersManager1;
      operationEnum = OperationEnum.AutomatedExportView;
      string operation15 = operationEnum.ToString();
      int num6;
      if (!usersManager16.HasRight(operation15))
      {
        UsersManager usersManager17 = usersManager1;
        operationEnum = OperationEnum.ReadingDataView;
        string operation16 = operationEnum.ToString();
        num6 = usersManager17.HasRight(operation16) ? 1 : 0;
      }
      else
        num6 = 1;
      this.DataAndReportsVisibility = num6 != 0;
      UsersManager usersManager18 = usersManager1;
      operationEnum = OperationEnum.ConfigurationView;
      string operation17 = operationEnum.ToString();
      int num7;
      if (!usersManager18.HasRight(operation17))
      {
        UsersManager usersManager19 = usersManager1;
        operationEnum = OperationEnum.DeviceConfigure;
        string operation18 = operationEnum.ToString();
        if (!usersManager19.HasRight(operation18))
        {
          UsersManager usersManager20 = usersManager1;
          operationEnum = OperationEnum.DeviceRead;
          string operation19 = operationEnum.ToString();
          if (!usersManager20.HasRight(operation19))
          {
            UsersManager usersManager21 = usersManager1;
            operationEnum = OperationEnum.HistoryView;
            string operation20 = operationEnum.ToString();
            num7 = usersManager21.HasRight(operation20) ? 1 : 0;
            goto label_27;
          }
        }
      }
      num7 = 1;
label_27:
      this.ConfigurationVisibility = num7 != 0;
      UsersManager usersManager22 = usersManager1;
      operationEnum = OperationEnum.DataCollectorView;
      string operation21 = operationEnum.ToString();
      this.MinomatsVisibility = usersManager22.HasRight(operation21);
      if (this.StructuresVisibility)
        this.ActiveModule = EModul.Structures;
      else if (this.OrdersVisibility)
        this.ActiveModule = EModul.Orders;
      else if (this.MinomatsVisibility)
        this.ActiveModule = EModul.DataCollectors;
      else if (this.JobsVisibility)
        this.ActiveModule = EModul.Jobs;
      else if (this.DataAndReportsVisibility)
        this.ActiveModule = EModul.Reporting;
      else if (this.ConfigurationVisibility)
        this.ActiveModule = EModul.Configuration;
      else if (this.UsersVisibility)
      {
        this.ActiveModule = EModul.Users;
      }
      else
      {
        if (!this.ArchivingVisibility)
          return;
        this.ActiveModule = EModul.Archiving;
      }
    }

    public void InjectUserControl() => this._userControlToBeAdded = this.GetActiveModuleViewModel();

    private ViewModelBase GetActiveModuleViewModel()
    {
      switch (this.ActiveModule)
      {
        case EModul.Archiving:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<ArchivingViewModel>();
        case EModul.Jobs:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<JobsViewModel>();
        case EModul.Users:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<UsersViewModel>();
        case EModul.Reporting:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<ReportingViewModel>();
        case EModul.Configuration:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<ConfigurationViewModel>();
        case EModul.DataCollectors:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<DataCollectorsViewModel>();
        case EModul.Structures:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<StructuresViewModel>();
        case EModul.Orders:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<OrdersViewModel>();
        default:
          return (ViewModelBase) DIConfigurator.GetConfigurator().Get<StructuresViewModel>();
      }
    }

    private void InitializeView()
    {
      TypeHelperExtensionMethods.ForEach<KeyValuePair<string, Func<EModul, EModul, BitmapImage>>>((IEnumerable<KeyValuePair<string, Func<EModul, EModul, BitmapImage>>>) this._resourceAssignmentsDictionary, (Action<KeyValuePair<string, Func<EModul, EModul, BitmapImage>>>) (_ => this.GetType().GetProperty(_.Key).SetValue((object) this, (object) _.Value(this.ActiveModule, this.CurrentSelection))));
    }

    public ICommand btnHomeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this._windowFactory.CreateNewNonModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSMainWindowViewModel>());
        });
      }
    }

    public ICommand SwitchModuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.CurrentSelection = (EModul) System.Enum.Parse(typeof (EModul), (string) parameter);
          this.InitializeView();
        }));
      }
    }

    private void InitializeModule(EModul selectedModule)
    {
      this.ActiveModule = selectedModule;
      this.SetUserControl = this.GetModuleUserControl(selectedModule);
    }

    protected StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    protected OrdersManager GetOrdersManagerInstance()
    {
      return new OrdersManager(this._repositoryFactory);
    }

    protected DataCollectorsManager GetDataCollectorsManagerInstance()
    {
      return new DataCollectorsManager(this._repositoryFactory);
    }

    public ICommand SearchCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) MSS.Localisation.Resources.MSS_SEARCH_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) MSS.Localisation.Resources.MSS_SEARCH_MESSAGE));
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            string searchText = parameter as string;
            switch (this.ActiveModule)
            {
              case EModul.Users:
                UsersManager usersManager = new UsersManager(this._repositoryFactory);
                RoleManager roleManager = new RoleManager(this._repositoryFactory);
                IEnumerable<UserDTO> userDtos = usersManager.SearchUserDTO(string.Empty).Where<UserDTO>((System.Func<UserDTO, bool>) (x => !x.Username.StartsWith("default")));
                IEnumerable<RoleDTO> roleDtos = roleManager.GetRolesDTO();
                if (searchText != string.Empty)
                {
                  switch (this.SelectedTab)
                  {
                    case ApplicationTabsEnum.UsersUsers:
                      userDtos = usersManager.SearchUserDTO(searchText).Where<UserDTO>((System.Func<UserDTO, bool>) (x => !x.Username.StartsWith("default")));
                      break;
                    case ApplicationTabsEnum.UsersRoles:
                      roleDtos = (IEnumerable<RoleDTO>) roleManager.SearchRoleDTO(searchText).ToList<RoleDTO>();
                      break;
                  }
                }
                MSS.DTO.Message.Message message5 = (MSS.DTO.Message.Message) null;
                if (!userDtos.Any<UserDTO>() || !roleDtos.Any<RoleDTO>())
                  message5 = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Warning,
                    MessageText = MessageCodes.No_Item_found.GetStringValue()
                  };
                switch (this.SelectedTab)
                {
                  case ApplicationTabsEnum.UsersUsers:
                    args.Result = (object) new ActionSearch<UserDTO>()
                    {
                      ObservableCollection = new ObservableCollection<UserDTO>(userDtos),
                      Message = message5,
                      SelectedTab = this.SelectedTab
                    };
                    return;
                  case ApplicationTabsEnum.UsersRoles:
                    args.Result = (object) new ActionSearch<RoleDTO>()
                    {
                      ObservableCollection = new ObservableCollection<RoleDTO>(roleDtos),
                      Message = message5,
                      SelectedTab = this.SelectedTab
                    };
                    return;
                  default:
                    return;
                }
              case EModul.Reporting:
                if (!(searchText != string.Empty))
                  break;
                args.Result = (object) new ActionSearchByText()
                {
                  SearchedText = searchText
                };
                break;
              case EModul.Configuration:
                args.Result = args.Result = (object) new ActionSearch<string>()
                {
                  Message = new MSS.DTO.Message.Message()
                  {
                    MessageText = searchText
                  }
                };
                break;
              case EModul.DataCollectors:
                ObservableCollection<MinomatDTO> observableCollection3 = new ObservableCollection<MinomatDTO>();
                if (searchText != string.Empty)
                {
                  switch (this.SelectedTab)
                  {
                    case ApplicationTabsEnum.DataCollectors:
                      observableCollection3 = (ObservableCollection<MinomatDTO>) this.GetDataCollectorsManagerInstance().GetDataCollectorsItems(searchText);
                      break;
                    case ApplicationTabsEnum.DataCollectorsPool:
                      observableCollection3 = (ObservableCollection<MinomatDTO>) this.GetDataCollectorsManagerInstance().GetDataCollectorsItemsPool(searchText);
                      break;
                  }
                }
                MSS.DTO.Message.Message message6 = (MSS.DTO.Message.Message) null;
                if (!observableCollection3.Any<MinomatDTO>())
                  message6 = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Warning,
                    MessageText = MessageCodes.No_Item_found.GetStringValue()
                  };
                args.Result = (object) new ActionSearch<MinomatDTO>()
                {
                  ObservableCollection = new ObservableCollection<MinomatDTO>((IEnumerable<MinomatDTO>) observableCollection3),
                  Message = message6,
                  SelectedTab = this.SelectedTab
                };
                break;
              case EModul.Structures:
                ObservableCollection<StructureNodeDTO> observableCollection4 = new ObservableCollection<StructureNodeDTO>();
                if (searchText != string.Empty)
                {
                  switch (this.SelectedTab)
                  {
                    case ApplicationTabsEnum.StructuresPhysical:
                      observableCollection4 = this.GetStructuresManagerInstance().GetStructures(searchText, StructureTypeEnum.Physical);
                      break;
                    case ApplicationTabsEnum.StructuresLogical:
                      observableCollection4 = this.GetStructuresManagerInstance().GetStructures(searchText, StructureTypeEnum.Logical);
                      break;
                    case ApplicationTabsEnum.StructuresFixed:
                      observableCollection4 = this.GetStructuresManagerInstance().GetStructures(searchText, StructureTypeEnum.Fixed);
                      break;
                  }
                }
                MSS.DTO.Message.Message message7 = (MSS.DTO.Message.Message) null;
                if (!observableCollection4.Any<StructureNodeDTO>())
                  message7 = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Warning,
                    MessageText = MessageCodes.No_Item_found.GetStringValue()
                  };
                args.Result = (object) new ActionSearch<StructureNodeDTO>()
                {
                  ObservableCollection = new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) observableCollection4),
                  Message = message7,
                  SelectedTab = this.SelectedTab
                };
                break;
              case EModul.Orders:
                IEnumerable<OrderDTO> orderDtos3 = this.GetOrdersManagerInstance().GetInstallationOrdersDTO();
                IEnumerable<OrderDTO> orderDtos4 = this.GetOrdersManagerInstance().GetReadingOrdersDTO();
                if (searchText != string.Empty)
                {
                  switch (this.SelectedTab)
                  {
                    case ApplicationTabsEnum.ReadingOrders:
                      orderDtos4 = this.GetOrdersManagerInstance().GetOrders(searchText, OrderTypeEnum.ReadingOrder);
                      break;
                    case ApplicationTabsEnum.InstallationOrders:
                      orderDtos3 = this.GetOrdersManagerInstance().GetOrders(searchText, OrderTypeEnum.InstallationOrder);
                      break;
                  }
                }
                MSS.DTO.Message.Message message8 = (MSS.DTO.Message.Message) null;
                if (!orderDtos3.Any<OrderDTO>() || !orderDtos4.Any<OrderDTO>())
                  message8 = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Warning,
                    MessageText = MessageCodes.No_Item_found.GetStringValue()
                  };
                switch (this.SelectedTab)
                {
                  case ApplicationTabsEnum.ReadingOrders:
                    args.Result = (object) new ActionSearch<OrderDTO>()
                    {
                      ObservableCollection = new ObservableCollection<OrderDTO>(orderDtos4),
                      Message = message8,
                      SelectedTab = this.SelectedTab
                    };
                    return;
                  case ApplicationTabsEnum.InstallationOrders:
                    args.Result = (object) new ActionSearch<OrderDTO>()
                    {
                      ObservableCollection = new ObservableCollection<OrderDTO>(orderDtos3),
                      Message = message8,
                      SelectedTab = this.SelectedTab
                    };
                    return;
                  default:
                    return;
                }
              default:
                Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), "Not yet working", false)));
                break;
            }
          });
          backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            pd.OnRequestClose(false);
            if (!args.Cancelled && args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
            this.PublishSearchResults(args);
          });
          backgroundWorker.RunWorkerAsync((object) this._repositoryFactory);
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
        }));
      }
    }

    private void PublishSearchResults(RunWorkerCompletedEventArgs args)
    {
      switch (this.ActiveModule)
      {
        case EModul.Users:
          switch (this.SelectedTab)
          {
            case ApplicationTabsEnum.UsersUsers:
              EventPublisher.Publish<ActionSearch<UserDTO>>(args.Result as ActionSearch<UserDTO>, (IViewModel) this);
              return;
            case ApplicationTabsEnum.UsersRoles:
              EventPublisher.Publish<ActionSearch<RoleDTO>>(args.Result as ActionSearch<RoleDTO>, (IViewModel) this);
              return;
            default:
              return;
          }
        case EModul.Reporting:
          EventPublisher.Publish<ActionSearchByText>(args.Result as ActionSearchByText, (IViewModel) this);
          break;
        case EModul.Configuration:
          EventPublisher.Publish<ActionSearch<string>>(args.Result as ActionSearch<string>, (IViewModel) this);
          break;
        case EModul.DataCollectors:
          EventPublisher.Publish<ActionSearch<MinomatDTO>>(args.Result as ActionSearch<MinomatDTO>, (IViewModel) this);
          break;
        case EModul.Structures:
          EventPublisher.Publish<ActionSearch<StructureNodeDTO>>(args.Result as ActionSearch<StructureNodeDTO>, (IViewModel) this);
          break;
        case EModul.Orders:
          EventPublisher.Publish<ActionSearch<OrderDTO>>(args.Result as ActionSearch<OrderDTO>, (IViewModel) this);
          break;
      }
    }

    public ICommand DownloadCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (this.ActiveModule != EModul.Structures)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), "Not implemented yet.", false);
          else if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Server_Not_Available, false);
          else if (!SychronizationHelperFactory.GetSynchronizationHelper().IsVersionUpToDateDownload())
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Not_Up_To_Date, false);
          }
          else
          {
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DownloadStructuresViewModel>((IParameter) new ConstructorArgument("am", (object) this.ActiveModule), (IParameter) new ConstructorArgument("selectedTab", (object) this.SelectedTab)));
            if (newModalDialog.HasValue && !newModalDialog.Value)
              return;
            this.RefreshSelectedView();
            EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
            {
              SelectedTab = this.SelectedTab
            }, (IViewModel) this);
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Download.GetStringValue()
            };
            EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
            {
              Message = message
            }, (IViewModel) this);
          }
        }));
      }
    }

    public ICommand ShowConflictsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Server_Not_Available, false);
          }
          else
          {
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ShowConflictsViewModel>((IParameter) new ConstructorArgument("conflicts", (object) ConflictHelper.GetConflicts())));
            if (newModalDialog.HasValue && !newModalDialog.Value)
              return;
            this.RefreshSelectedView();
            EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
            {
              SelectedTab = this.SelectedTab
            }, (IViewModel) this);
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Download.GetStringValue()
            };
            EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
            {
              Message = message
            }, (IViewModel) this);
          }
        }));
      }
    }

    public bool ShowConflictsButtonVisible
    {
      get => this._showConflictsButtonVisible;
      set
      {
        this._showConflictsButtonVisible = value;
        this.OnPropertyChanged(nameof (ShowConflictsButtonVisible));
      }
    }

    public ICommand SyncCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = new SettingsConnectionManager().IsServerAvailableAndStatusAccepted(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
          if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Server_Not_Available, false);
          }
          else
          {
            GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) MSS.Localisation.Resources.MSS_Client_Synchronization_Dialog), (IParameter) new ConstructorArgument("progressDialogMessage", (object) MSS.Localisation.Resources.MSS_CLIENT_Synchronization_Message));
            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
              WorkerReportsProgress = true,
              WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
            {
              List<object> objectList = (List<object>) args.Argument;
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[0], (SyncDirectionOrder) objectList[4]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[1], (SyncDirectionOrder) objectList[4]);
              new ClientPartialSyncManager().Synchronize(MSS.Business.Utils.AppContext.Current.LoggedUser.Id);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[0], (SyncDirectionOrder) objectList[5]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[1], (SyncDirectionOrder) objectList[5]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[3], (SyncDirectionOrder) objectList[5]);
            });
            backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
            {
              pd.OnRequestClose(false);
              EventPublisher.Publish<SyncConflictsStateChanged>(new SyncConflictsStateChanged(), (IViewModel) this);
              MSS.DTO.Message.Message message = (MSS.DTO.Message.Message) null;
              if (args.Cancelled)
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Warning,
                  MessageText = MSS.Localisation.Resources.MSS_Client_Synchronization_Cancelled
                };
              else if (args.Error != null)
              {
                MSS.Business.Errors.MessageHandler.LogException(args.Error);
                MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
              }
              else if (MSS.Business.Utils.AppContext.Current.HasConflicts)
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Warning,
                  MessageText = MSS.Localisation.Resources.MSS_Client_Synchronization_SucceddedWithConflicts
                };
              else
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Success,
                  MessageText = MSS.Localisation.Resources.MSS_Client_Synchronization_Succedded
                };
              this.RefreshSelectedView();
              if (message == null)
                return;
              EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
              {
                Message = message
              }, (IViewModel) this);
            });
            List<object> objectList1 = new List<object>()
            {
              (object) SyncScopesEnum.Configuration,
              (object) SyncScopesEnum.Users,
              (object) SyncScopesEnum.Application,
              (object) SyncScopesEnum.ReadingValues,
              (object) SyncDirectionOrder.Download,
              (object) SyncDirectionOrder.Upload
            };
            backgroundWorker.RunWorkerAsync((object) objectList1);
            this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
          }
        }));
      }
    }

    public ICommand RemoveOldOrdersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          CleanupModel ordersCleanupModel = new ClientPartialSyncManager().GetOldOrdersCleanupModel(MSS.Business.Utils.AppContext.Current.LoggedUser.Id, new int?(new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory).GetAppParam<int>("CleanupOldOrdersDefinitionInDays")));
          if (ordersCleanupModel != null)
            new DatabaseCleanupManager(this._repositoryFactory, ordersCleanupModel).CleanupDatabaseOfOldClosedOrders();
          this.RefreshSelectedView();
        }));
      }
    }

    public ICommand UpdateCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = new SettingsConnectionManager().IsServerAvailableAndStatusAccepted(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
          if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Server_Not_Available, false);
          else if (MSS.Business.Utils.AppContext.Current.HasConflicts)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_UnsolvedConflicts, false);
          else if (ConfigurationManager.AppSettings["DatabaseEngine"] == "SQLiteDatabase" && !MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Server_Not_UpToDate_Update, false);
          }
          else
          {
            GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) MSS.Localisation.Resources.MSS_Client_Synchronization_Dialog), (IParameter) new ConstructorArgument("progressDialogMessage", (object) MSS.Localisation.Resources.MSS_CLIENT_Synchronization_Message));
            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
              WorkerReportsProgress = true,
              WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
            {
              List<object> objectList = (List<object>) args.Argument;
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[0], (SyncDirectionOrder) objectList[4]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[1], (SyncDirectionOrder) objectList[4]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[2], (SyncDirectionOrder) objectList[4]);
              Dictionary<Guid, Type> conflictsDictionary = new Dictionary<Guid, Type>();
              foreach (ConflictDetails conflictDetails in MSS.Business.Utils.AppContext.Current.SyncConflicts.Values)
              {
                Type databaseMapping = DatabaseConstants.DatabaseMappings[conflictDetails.ConflictInfo.LocalChange.TableName];
                DataRow row = conflictDetails.ConflictInfo.RemoteChange.Rows[0];
                conflictsDictionary.Add((Guid) row.ItemArray[0], databaseMapping);
              }
              using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
                MSS.Business.Utils.AppContext.Current.SyncExtraData = serviceClient.GetSynctonizationExtraInformation(conflictsDictionary);
            });
            backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
            {
              pd.OnRequestClose(false);
              EventPublisher.Publish<SyncConflictsStateChanged>(new SyncConflictsStateChanged(), (IViewModel) this);
              MSS.DTO.Message.Message message = (MSS.DTO.Message.Message) null;
              if (args.Cancelled)
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Warning,
                  MessageText = MSS.Localisation.Resources.MSS_Client_Synchronization_Cancelled
                };
              else if (args.Error != null)
              {
                MSS.Business.Errors.MessageHandler.LogException(args.Error);
                MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
              }
              else if (MSS.Business.Utils.AppContext.Current.HasConflicts)
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Warning,
                  MessageText = MSS.Localisation.Resources.MSS_Client_Synchronization_SucceddedWithConflicts
                };
              else
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Success,
                  MessageText = MSS.Localisation.Resources.MSS_Client_Synchronization_Succedded
                };
              this.RefreshSelectedView();
              if (message == null)
                return;
              EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
              {
                Message = message
              }, (IViewModel) this);
            });
            List<object> objectList1 = new List<object>()
            {
              (object) SyncScopesEnum.Configuration,
              (object) SyncScopesEnum.Users,
              (object) SyncScopesEnum.Application,
              (object) SyncScopesEnum.ReadingValues,
              (object) SyncDirectionOrder.Download
            };
            backgroundWorker.RunWorkerAsync((object) objectList1);
            this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
          }
        }));
      }
    }

    public ICommand SendCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = new SettingsConnectionManager().IsServerAvailableAndStatusAccepted(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
          if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), MSS.Localisation.Resources.MSS_Client_Server_Not_Available, false);
          }
          else
          {
            GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) MSS.Localisation.Resources.MSS_Client_Synchronization_Dialog), (IParameter) new ConstructorArgument("progressDialogMessage", (object) MSS.Localisation.Resources.MSS_CLIENT_Synchronization_Message));
            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
              WorkerReportsProgress = true,
              WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
            {
              List<object> objectList = (List<object>) args.Argument;
              string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[0], (SyncDirectionOrder) objectList[4]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[1], (SyncDirectionOrder) objectList[4]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[2], (SyncDirectionOrder) objectList[4]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[3], (SyncDirectionOrder) objectList[4]);
            });
            backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
            {
              pd.OnRequestClose(false);
              if (args.Cancelled)
                MessageHandlingManager.ShowWarningMessage(MSS.Localisation.Resources.MSS_Client_Synchronization_Cancelled);
              else if (args.Error != null)
              {
                MSS.Business.Errors.MessageHandler.LogException(args.Error);
                MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
              }
              else if (!string.IsNullOrEmpty(args.Result as string))
              {
                MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), args.Result as string, false);
              }
              else
              {
                EventPublisher.Publish<LocalDatabaseModified>(new LocalDatabaseModified()
                {
                  IsChanged = false
                }, (IViewModel) this);
                MessageHandlingManager.ShowSuccessMessage(MSS.Localisation.Resources.MSS_Client_Synchronization_Succedded);
              }
            });
            List<object> objectList1 = new List<object>()
            {
              (object) SyncScopesEnum.Configuration,
              (object) SyncScopesEnum.Users,
              (object) SyncScopesEnum.Application,
              (object) SyncScopesEnum.ReadingValues,
              (object) SyncDirectionOrder.Upload
            };
            backgroundWorker.RunWorkerAsync((object) objectList1);
            this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Send.GetStringValue()
            };
            EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
            {
              Message = message
            }, (IViewModel) this);
          }
        }));
      }
    }

    public ICommand DownloadUsersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate => SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Users, SyncDirectionOrder.Download)));
      }
    }

    public ICommand AboutCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSAboutViewModel>())));
      }
    }

    public bool IsLogOutOk { get; set; }

    public ICommand LogoutCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!MSSUIHelper.ShowSendChangesWarningDialog_AtApplicationClose(this._repositoryFactory))
            return;
          this.IsLogOutOk = true;
          if (MSSUIHelper.ShowSendChangesWarningDialog_AtLogout())
          {
            MSS_Client.Properties.Settings.Default.RememberedUserId = Guid.Empty;
            MSS_Client.Properties.Settings.Default.Save();
            MSS.Business.Utils.AppContext.Current.LoggedUser = (User) null;
            this._windowFactory.CreateNewNonModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSLoginWindowViewModel>());
          }
          else
            this._windowFactory.CreateNewNonModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSSLoginWindowViewModel>());
          this.CloseAllWindows();
        }));
      }
    }

    public string ApplicationName
    {
      get => ConfigurationManager.AppSettings[nameof (ApplicationName)].ToString();
    }

    public ImageSource SendImage
    {
      get => this._sendImage;
      set
      {
        this._sendImage = value;
        this.OnPropertyChanged(nameof (SendImage));
      }
    }

    public ImageSource NewsAndUpdatesImage
    {
      get => this._newsAndUpdatesImage;
      set
      {
        this._newsAndUpdatesImage = value;
        this.OnPropertyChanged(nameof (NewsAndUpdatesImage));
      }
    }

    public bool StructuresVisibility
    {
      get => this.structuresVisibility;
      set
      {
        this.structuresVisibility = value;
        this.OnPropertyChanged(nameof (StructuresVisibility));
      }
    }

    public bool SettingsVisibility
    {
      get => this.settingsVisibility;
      set
      {
        this.settingsVisibility = value;
        this.OnPropertyChanged(nameof (SettingsVisibility));
      }
    }

    public bool DataAndReportsVisibility { get; set; }

    public bool ConfigurationVisibility { get; set; }

    public bool MinomatsVisibility { get; set; }

    public bool OrdersVisibility { get; set; }

    public bool ArchivingVisibility { get; set; }

    public bool JobsVisibility
    {
      get => this.jobsVisibility;
      set
      {
        this.jobsVisibility = value;
        this.OnPropertyChanged(nameof (JobsVisibility));
      }
    }

    public bool UsersVisibility
    {
      get => this._usersVisibility;
      set
      {
        this._usersVisibility = value;
        this.OnPropertyChanged(nameof (UsersVisibility));
      }
    }

    public Brush SettingsButtonBackground
    {
      get => this._settingsBackgroundBrush;
      set
      {
        this._settingsBackgroundBrush = value;
        this.OnPropertyChanged(nameof (SettingsButtonBackground));
      }
    }

    public Brush SettingsButtonForeground
    {
      get => this._settingsForegroundBrush;
      set
      {
        this._settingsForegroundBrush = value;
        this.OnPropertyChanged(nameof (SettingsButtonForeground));
      }
    }

    public ViewModelBase SetUserControl
    {
      get => this._userControlToBeAdded;
      set
      {
        this._userControlToBeAdded.Dispose();
        this._userControlToBeAdded = value;
        this.OnPropertyChanged(nameof (SetUserControl));
      }
    }

    public BitmapImage Meters
    {
      get => this._meterButtonImage;
      set
      {
        this._meterButtonImage = value;
        this.OnPropertyChanged(nameof (Meters));
      }
    }

    public BitmapImage DataCollectors
    {
      get => this._dataCollectorButtonImage;
      set
      {
        this._dataCollectorButtonImage = value;
        this.OnPropertyChanged(nameof (DataCollectors));
      }
    }

    public BitmapImage Jobs
    {
      get => this._jobsButtonImage;
      set
      {
        this._jobsButtonImage = value;
        this.OnPropertyChanged(nameof (Jobs));
      }
    }

    public BitmapImage Archiving
    {
      get => this._archivingButtonImage;
      set
      {
        this._archivingButtonImage = value;
        this.OnPropertyChanged(nameof (Archiving));
      }
    }

    public BitmapImage Orders
    {
      get => this._ordersButtonImage;
      set
      {
        this._ordersButtonImage = value;
        this.OnPropertyChanged(nameof (Orders));
      }
    }

    public BitmapImage Users
    {
      get => this._usersButtonImage;
      set
      {
        this._usersButtonImage = value;
        this.OnPropertyChanged(nameof (Users));
      }
    }

    public BitmapImage Reporting
    {
      get => this._reportingButtonImage;
      set
      {
        this._reportingButtonImage = value;
        this.OnPropertyChanged(nameof (Reporting));
      }
    }

    public BitmapImage Configuration
    {
      get => this._configurationButtonImage;
      set
      {
        this._configurationButtonImage = value;
        this.OnPropertyChanged(nameof (Configuration));
      }
    }

    public BitmapImage Structures
    {
      get => this._structuresButtonImage;
      set
      {
        this._structuresButtonImage = value;
        this.OnPropertyChanged(nameof (Structures));
      }
    }

    public BitmapImage Settings
    {
      get => this._settingsButtonImage;
      set
      {
        this._settingsButtonImage = value;
        this.OnPropertyChanged(nameof (Settings));
      }
    }

    public ObservableCollection<Language> Languages
    {
      get
      {
        if (this._items != null)
          return this._items;
        ObservableCollection<Language> observableCollection = new ObservableCollection<Language>();
        observableCollection.Add(new Language(LangEnum.English, "pack://application:,,,/Styles;component/Images/Universal/english.png"));
        observableCollection.Add(new Language(LangEnum.German, "pack://application:,,,/Styles;component/Images/Universal/german.png"));
        this._items = observableCollection;
        return this._items;
      }
    }

    public Language SelectedLanguage
    {
      get => this._selectedLanguage;
      set
      {
        this._selectedLanguage = value;
        this.OnPropertyChanged(nameof (SelectedLanguage));
        if ("pack://application:,,,/Styles;component/Images/Universal/german.png".Equals(this._selectedLanguage.Image))
        {
          this._iToggleStateCulture = true;
          this.SetCulture();
        }
        else
        {
          if (!"pack://application:,,,/Styles;component/Images/Universal/english.png".Equals(this._selectedLanguage.Image))
            return;
          this._iToggleStateCulture = false;
          this.SetCulture();
        }
      }
    }

    public ApplicationTabsEnum SelectedTab { get; set; }

    public bool ServerButtonsVisible { get; set; }

    public EModul ActiveModule { get; set; }

    public bool IsBusy
    {
      get => this._isBusy;
      set
      {
        if (this._isBusy == value)
          return;
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
      }
    }

    public string BusyContent
    {
      get => this._busyContent;
      set => this._busyContent = value;
    }

    public WindowState CurrentWindowState
    {
      get => this._currentWindowState;
      set
      {
        this._currentWindowState = value;
        this.SaveWindowState();
        this.OnPropertyChanged(nameof (CurrentWindowState));
      }
    }

    public double WindowWidth
    {
      get => this._windowWidth;
      set
      {
        if (this._windowWidth == value)
          return;
        this._windowWidth = value;
        this.SaveWindowState();
        this.OnPropertyChanged(nameof (WindowWidth));
      }
    }

    public double WindowHeight
    {
      get => this._windowHeight;
      set
      {
        if (this._windowHeight == value)
          return;
        this._windowHeight = value;
        this.SaveWindowState();
        this.OnPropertyChanged(nameof (WindowHeight));
      }
    }

    private void SaveWindowState()
    {
      if (!this._areWindowStateParametersSet)
        return;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
      ApplicationParameter appParameter1 = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((System.Func<ApplicationParameter, bool>) (p => p.Parameter == "StartInFullScreen"));
      if (appParameter1 != null)
      {
        appParameter1.Value = (this.CurrentWindowState == WindowState.Maximized).ToString();
        parametersManagement.Update(appParameter1);
      }
      ApplicationParameter appParameter2 = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((System.Func<ApplicationParameter, bool>) (p => p.Parameter == "StartWindowWidth"));
      double num;
      if (appParameter2 != null)
      {
        ApplicationParameter applicationParameter = appParameter2;
        string str;
        if (this.CurrentWindowState != WindowState.Normal)
        {
          str = appParameter2.Value;
        }
        else
        {
          num = this.WindowWidth;
          str = num.ToString();
        }
        applicationParameter.Value = str;
        parametersManagement.Update(appParameter2);
      }
      ApplicationParameter appParameter3 = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((System.Func<ApplicationParameter, bool>) (p => p.Parameter == "StartWindowHeight"));
      if (appParameter3 != null)
      {
        ApplicationParameter applicationParameter = appParameter3;
        string str;
        if (this.CurrentWindowState != WindowState.Normal)
        {
          str = appParameter3.Value;
        }
        else
        {
          num = this.WindowHeight;
          str = num.ToString();
        }
        applicationParameter.Value = str;
        parametersManagement.Update(appParameter3);
      }
    }

    private void SetWindowState()
    {
      ApplicationParameter applicationParameter1 = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((System.Func<ApplicationParameter, bool>) (p => p.Parameter == "StartInFullScreen"));
      this.CurrentWindowState = applicationParameter1 == null ? WindowState.Maximized : (Convert.ToBoolean(applicationParameter1.Value) ? WindowState.Maximized : WindowState.Normal);
      if (this.CurrentWindowState == WindowState.Normal)
      {
        ApplicationParameter applicationParameter2 = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((System.Func<ApplicationParameter, bool>) (p => p.Parameter == "StartWindowWidth"));
        this.WindowWidth = applicationParameter2 != null ? Convert.ToDouble(applicationParameter2.Value) : 1040.0;
        ApplicationParameter applicationParameter3 = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((System.Func<ApplicationParameter, bool>) (p => p.Parameter == "StartWindowHeight"));
        this.WindowHeight = applicationParameter3 != null ? Convert.ToDouble(applicationParameter3.Value) : 770.0;
      }
      else
      {
        this.WindowWidth = 1040.0;
        this.WindowHeight = 770.0;
      }
      this._areWindowStateParametersSet = true;
    }

    private void SetSelectedLeftMenuButtonNew(EModul selectedModule)
    {
      this.InitializeModule(selectedModule);
    }

    private void CloseAllWindows() => this.OnRequestClose(false);

    private ViewModelBase GetModuleUserControl(EModul selectedModule)
    {
      ViewModelBase moduleUserControl = (ViewModelBase) null;
      switch (selectedModule)
      {
        case EModul.Meters:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<MetersViewModel>();
          break;
        case EModul.Archiving:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<ArchivingViewModel>();
          break;
        case EModul.Jobs:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<JobsViewModel>();
          break;
        case EModul.Settings:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<SettingsViewModel>();
          break;
        case EModul.Users:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<UsersViewModel>();
          break;
        case EModul.Reporting:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<ReportingViewModel>();
          break;
        case EModul.Configuration:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<ConfigurationViewModel>();
          break;
        case EModul.DataCollectors:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<DataCollectorsViewModel>();
          break;
        case EModul.Structures:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<StructuresViewModel>();
          break;
        case EModul.Orders:
          moduleUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get<OrdersViewModel>();
          break;
      }
      return moduleUserControl;
    }

    private static void ConfigureFELanguage()
    {
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }

    public void SetCulture()
    {
      if (this._iToggleStateCulture)
      {
        CultureResources.ChangeCulture(new CultureInfo("de"));
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
        this._iToggleStateCulture = false;
        this.OnPropertyChanged("Languages");
      }
      else
      {
        CultureResources.SetDefaultCulture();
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        this._iToggleStateCulture = true;
      }
    }

    private void RefreshSelectedView() => this.RefreshSelectedView(string.Empty);

    private void RefreshSelectedView(string userControlName)
    {
      Type type = Type.GetType(this.SetUserControl.ToString());
      if (userControlName.Equals(type.Name) || userControlName == string.Empty)
        this.SetUserControl = (ViewModelBase) DIConfigurator.GetConfigurator().Get(type);
      EventPublisher.Publish<SelectedTabValue>(new SelectedTabValue()
      {
        Tab = this.SelectedTab
      }, (IViewModel) this);
    }

    public bool IsValidIp(string addr)
    {
      return !string.IsNullOrEmpty(addr) && IPAddress.TryParse(addr, out IPAddress _);
    }

    public bool IsValidPageSize(string pageSize, out int number)
    {
      int result;
      if (!int.TryParse(pageSize, out result))
      {
        number = 0;
        return false;
      }
      number = result;
      return result > 0;
    }

    public string DialogTitle
    {
      get => this._dialogTitle;
      set
      {
        this._dialogTitle = value;
        this.OnPropertyChanged(nameof (DialogTitle));
      }
    }

    public string ServerUpToDate
    {
      get => this._serverUpToDate;
      set
      {
        this._serverUpToDate = value;
        this.OnPropertyChanged(nameof (ServerUpToDate));
      }
    }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public ICommand CloseCommand { get; private set; }

    public ICommand CloseFailCommand { get; private set; }

    public bool AllowWindowToClose
    {
      get => this.allowWindowToClose;
      set
      {
        if (this.allowWindowToClose == value)
          return;
        this.allowWindowToClose = value;
        this.OnPropertyChanged(nameof (AllowWindowToClose));
      }
    }

    public byte[] LogoImage
    {
      get => this._logoImage;
      set
      {
        this._logoImage = value;
        this.OnPropertyChanged(nameof (LogoImage));
      }
    }

    public DateTime? LastConnectionToLicenseServer
    {
      get
      {
        int num = LicenseHelper.DaysSinceTheLicenseIsUsedOffline(this._technicalParameter);
        this.ImageDaysSinceLicenseIsUsedOffline = (num <= 45 ? (num <= 30 || num > 45 ? (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/Settings/light-green.png")) : (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/Settings/light-yellow.png"))) : (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/Settings/light-red.png"))).ToByteArray((BitmapEncoder) new PngBitmapEncoder());
        return (DateTime?) this._technicalParameter?.LastConnectionToLicenseServer;
      }
    }

    public byte[] ImageDaysSinceLicenseIsUsedOffline
    {
      get => this._imageDaysSinceLicenseIsUsedOffine;
      set
      {
        this._imageDaysSinceLicenseIsUsedOffine = value;
        this.OnPropertyChanged(nameof (ImageDaysSinceLicenseIsUsedOffline));
      }
    }
  }
}
