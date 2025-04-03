// Decompiled with JetBrains decompiler
// Type: MSS_Client.App
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Common.Library.NHibernate.Data;
using Microsoft.Synchronization;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.AppParametersManagement;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.NewsAndUpdatesManagement;
using MSS.Business.Modules.Synchronization;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.Client.UI.Desktop.View.ExceptionMessageBox;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.Structures;
using MSS.Core.Model.TechnicalParameters;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS_Client.Utils;
using MSS_Client.ViewModel.ExceptionMessageBox;
using MSS_Client.ViewModel.Orders;
using MSS_Client.ViewModel.Startup;
using MSS_Client.ViewModel.Structures;
using MSSWeb.Common.WebApiUtils;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Linq;
using Ninject;
using Ninject.Syntax;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Docx;
using Telerik.Windows.Documents.FormatProviders.Pdf;
using Telerik.Windows.Documents.FormatProviders.Rtf;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.UI.Extensibility;

#nullable disable
namespace MSS_Client
{
  public partial class App : Application
  {
    private IRepositoryFactory _repositoryFactory;
    private IWindowFactory _windowFactory;
    private bool _configValue;
    private const int SW_RESTORE = 9;
    private bool _contentLoaded;

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);

    public App()
    {
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
      this.Dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(this.OnDispatcherUnhandledException);
      this._configValue = CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode");
    }

    private void OnDispatcherUnhandledException(
      object sender,
      DispatcherUnhandledExceptionEventArgs e)
    {
      if (this._windowFactory != null)
      {
        this._windowFactory.CreateNewModalDialog((IViewModel) new ExceptionMessageBoxViewModel(e.Exception.Message, true));
      }
      else
      {
        ExceptionMessageBoxDialog messageBoxDialog1 = new ExceptionMessageBoxDialog();
        messageBoxDialog1.DataContext = (object) new ExceptionMessageBoxViewModel(e.Exception.Message, true);
        ExceptionMessageBoxDialog messageBoxDialog2 = messageBoxDialog1;
        if (Application.Current.Windows[0] != messageBoxDialog2)
        {
          messageBoxDialog2.Owner = Application.Current.Windows[0];
          messageBoxDialog2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        else
          messageBoxDialog2.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        messageBoxDialog2.ShowDialog();
      }
      MessageHandler.LogException(e.Exception);
      e.Handled = true;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      Process[] processesByName = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
      if (processesByName.Length > 1)
      {
        Process currentProcess = Process.GetCurrentProcess();
        int index = 0;
        if (processesByName[0].Id == currentProcess.Id)
          index = 1;
        IntPtr mainWindowHandle = processesByName[index].MainWindowHandle;
        if (App.IsIconic(mainWindowHandle))
          App.ShowWindowAsync(mainWindowHandle, 9);
        App.SetForegroundWindow(mainWindowHandle);
        Application.Current.Shutdown();
      }
      else
        base.OnStartup(e);
    }

    private void OnStartup(object sender1, StartupEventArgs e)
    {
      bool isError = false;
      MSSSplashScreenViewModel viewModelSplash = (MSSSplashScreenViewModel) null;
      Task task = new Task((Action) (() =>
      {
        try
        {
          this.InitializeNInject();
          this.RegisterResources();
          this._windowFactory = DIConfigurator.GetConfigurator().Get<IWindowFactory>();
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() => viewModelSplash = MSSUIHelper.ShowSplashScreen(this._windowFactory)));
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() => viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_Nhibernate));
          AppDomain.CurrentDomain.SetData("DataDirectory", (object) System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData));
          MSSHelper.InitializeNHibernateFactoryForMSSDatabase();
          HibernateMultipleDatabasesManager.FluentConfiguration.ExposeConfiguration((Action<Configuration>) (cfg =>
          {
            cfg.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[1]
            {
              (IPreInsertEventListener) new NHibernateEventListener()
            };
            cfg.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[1]
            {
              (IPreUpdateEventListener) new NHibernateEventListener()
            };
            cfg.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[1]
            {
              (IPostInsertEventListener) new NHibernateEventListener()
            };
            cfg.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[1]
            {
              (IPostUpdateEventListener) new NHibernateEventListener()
            };
            cfg.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[1]
            {
              (IPostDeleteEventListener) new NHibernateEventListener()
            };
          }));
          HibernateMultipleDatabasesManager.FluentConfiguration.BuildSessionFactory();
          try
          {
            foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<string>) Directory.GetFiles(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "MSS\\Logs"))).Select<string, FileInfo>((Func<string, FileInfo>) (file => new FileInfo(file))).Where<FileInfo>((Func<FileInfo, bool>) (fi => fi.LastWriteTime < DateTime.Now.AddMonths(-1))))
              fileSystemInfo.Delete();
          }
          catch (Exception ex)
          {
            MessageHandler.LogException(ex);
          }
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_EnvSettings;
            viewModelSplash.CurrentProgress = 30.0;
          }));
          this._repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
          this.SetEnvSettings();
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_InitializeSynchronization;
            viewModelSplash.CurrentProgress = 40.0;
          }));
          this.InitializeSyncronization();
          this.CheckSendStatus();
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_InitializeTelerik;
            viewModelSplash.CurrentProgress = 60.0;
          }));
          this.InitializeTelerikCompont();
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_LoadUserSettings;
            viewModelSplash.CurrentProgress = 70.0;
          }));
          if (!this._repositoryFactory.GetSession().IsOpen)
            this._repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
          List<Operation> operations = this._repositoryFactory.GetUserRepository().GetOperations(MSS.Business.Utils.AppContext.Current.LoggedUser);
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_UpdateLicenseFromServer;
            viewModelSplash.CurrentProgress = 80.0;
          }));
          if (LicenseHelper.IsCustomerNumberFilled(MSS.Business.Utils.AppContext.Current.TechnicalParameters))
          {
            this._repositoryFactory.UpdateLicense();
            MSSUIHelper.UpdateTheApplicationVersionInformation();
            MSS.Business.Utils.AppContext.Current.Operations = LicenseHelper.GetOperations(MSS.Business.Utils.AppContext.Current.LoggedUser, operations);
          }
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_LoaderGMM;
            viewModelSplash.CurrentProgress = 85.0;
          }));
          if (!this._repositoryFactory.GetSession().IsOpen)
            this._repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
          MSSHelper.InitializeGMMAndSetEquipment();
          MSS.Business.Utils.AppContext.Current.IsMinoConnectConnected = MSSHelper.IsMinoConnectConnected();
          Task.Run((Action) (() => MSS.Business.Utils.AppContext.Current.MinoConnectDeviceNames = EquipmentHelper.DiscoverMiCons()));
          if (!this._repositoryFactory.GetSession().IsOpen)
            this._repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() =>
          {
            viewModelSplash.Message = MSS.Localisation.Resources.MSS_Client_SplashScreen_LoadingModule_News;
            viewModelSplash.CurrentProgress = 90.0;
          }));
          List<NewsAndUpdatesSerializable> newsAndUpdates = this.GetNewsAndUpdates();
          try
          {
            IRepositoryFactory repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
            try
            {
              new NewsAndUpdatesmanager(repositoryFactory).InsertNews((IEnumerable<NewsAndUpdatesSerializable>) newsAndUpdates);
            }
            catch (Exception ex)
            {
              if (repositoryFactory.GetSession().IsOpen)
                ((IDisposable) repositoryFactory.GetSession()).Dispose();
              MessageHandler.LogException(ex);
            }
          }
          catch (Exception ex)
          {
            MessageHandler.LogException(ex);
          }
          Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate) (() => viewModelSplash.CurrentProgress = 100.0));
        }
        catch (Exception ex)
        {
          isError = true;
          MessageHandler.LogException(ex);
        }
      }));
      task.ContinueWith((Action<Task>) (t =>
      {
        if (isError)
        {
          MSSUIHelper.ShowErrorWindow(this._windowFactory, "MSS_Client_Exception_AppStart");
        }
        else
        {
          try
          {
            MSSUIHelper.ShowApplicationStartWindow(this._repositoryFactory, this._windowFactory);
          }
          catch (Exception ex)
          {
            MessageHandler.LogException(ex);
            MSSUIHelper.ShowErrorWindow(this._windowFactory, "MSS_Client_Exception_AppStart");
          }
        }
        viewModelSplash.OnRequestClose(false);
      }), System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
      task.Start();
    }

    private List<NewsAndUpdatesSerializable> GetNewsAndUpdates()
    {
      List<NewsAndUpdatesSerializable> newsAndUpdates = new List<NewsAndUpdatesSerializable>();
      try
      {
        newsAndUpdates = NewsAndUpdatesWepApiHandler.GetNewsAndUpdates(MSS.Business.Utils.AppContext.Current.TechnicalParameters.CustomerNumber);
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
      }
      return newsAndUpdates;
    }

    private void InitializeTelerikCompont()
    {
      RadCompositionInitializer.Catalog = (ComposablePartCatalog) new TypeCatalog(new Type[5]
      {
        typeof (XamlFormatProvider),
        typeof (RtfFormatProvider),
        typeof (DocxFormatProvider),
        typeof (PdfFormatProvider),
        typeof (HtmlFormatProvider)
      });
    }

    private void RegisterResources()
    {
      if (this._configValue)
      {
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/System.Windows.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/Telerik.Windows.Controls.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/Telerik.Windows.Controls.Data.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/Telerik.Windows.Controls.DataVisualization.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/Telerik.Windows.Controls.Input.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/Telerik.Windows.Controls.Navigation.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8Touch;component/Themes/telerik.windows.controls.gridview.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/Styles;component/Resources/AppResourcesTablet.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("ViewsResourcesTablet.xaml", UriKind.RelativeOrAbsolute)
        });
      }
      else
      {
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8;component/Themes/System.Windows.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8;component/Themes/telerik.windows.controls.data.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8;component/Themes/Telerik.Windows.Controls.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8;component/Themes/Telerik.Windows.Controls.Input.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8;component/Themes/Telerik.Windows.Controls.Navigation.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("/Telerik.Windows.Themes.Windows8;component/Themes/telerik.windows.controls.gridview.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/Styles;component/Resources/AppResources.xaml", UriKind.RelativeOrAbsolute)
        });
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("ViewsResources.xaml", UriKind.RelativeOrAbsolute)
        });
      }
    }

    private void InitializeNInject()
    {
      IKernel diConfig = DIConfigurator.GetConfigurator();
      diConfig.Bind<IUIPrinter>().To<UIPrinter>().Named("UIPrinter");
      TypeHelperExtensionMethods.ForEach<Type>(((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).Where<Type>((Func<Type, bool>) (t => typeof (IViewModel).IsAssignableFrom(t) && !t.IsInterface)), (Action<Type>) (t => diConfig.Bind(t).ToSelf()));
      if (this._configValue)
        diConfig.Bind(typeof (IWindowFactory)).To(typeof (TabletWindowFactory));
      else
        diConfig.Bind(typeof (IWindowFactory)).To(typeof (DesktopWindowFactory));
      diConfig.Bind<EditFixedStructureViewModel>().ToConstructor<EditFixedStructureViewModel>((Expression<Func<IConstructorArgumentSyntax, EditFixedStructureViewModel>>) (x => new EditFixedStructureViewModel(x.Inject<StructureNodeDTO>(), x.Inject<bool>(), x.Inject<bool>(), x.Inject<IRepositoryFactory>(), x.Inject<IWindowFactory>()))).Named("EditFixedStructureForStructureViewModel");
      diConfig.Bind<EditFixedStructureViewModel>().ToConstructor<EditFixedStructureViewModel>((Expression<Func<IConstructorArgumentSyntax, EditFixedStructureViewModel>>) (x => new EditFixedStructureViewModel(x.Inject<StructureNodeDTO>(), x.Inject<bool>(), x.Inject<bool>(), x.Inject<OrderDTO>(), x.Inject<IRepositoryFactory>(), x.Inject<IWindowFactory>()))).Named("EditFixedStructureForOrderViewModel");
      diConfig.Bind<ExecuteInstallationOrderViewModel>().ToConstructor<ExecuteInstallationOrderViewModel>((Expression<Func<IConstructorArgumentSyntax, ExecuteInstallationOrderViewModel>>) (_ => new ExecuteInstallationOrderViewModel(_.Inject<string>(), _.Inject<StructureNodeDTO>(), _.Inject<IRepositoryFactory>(), _.Inject<IWindowFactory>()))).Named("ExecuteInstallationOrderViewModel");
      diConfig.Bind<RepairModeViewModel>().ToSelf().Named("RepairModeViewModel");
    }

    private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
    {
      MessageHandler.LogException(e.Exception);
    }

    private string FlattenException(Exception exception)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (; exception != null; exception = exception.InnerException)
      {
        stringBuilder.AppendLine(exception.Message);
        stringBuilder.AppendLine(exception.StackTrace);
      }
      return stringBuilder.ToString();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      string exceptionMessage = this.FlattenException(e.ExceptionObject as Exception);
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        ExceptionMessageBoxDialog messageBoxDialog = new ExceptionMessageBoxDialog()
        {
          DataContext = (object) new ExceptionMessageBoxViewModel(exceptionMessage, true)
        };
        messageBoxDialog.Owner = Application.Current.Windows[0];
        messageBoxDialog.ShowDialog();
      }), DispatcherPriority.Send);
    }

    private void SetEnvSettings()
    {
      MSS.Business.Utils.AppContext.Current.TechnicalParameters = this._repositoryFactory.GetRepository<TechnicalParameter>().GetAll().First<TechnicalParameter>();
      if (string.IsNullOrEmpty(MSS_Client.Properties.Settings.Default.MSSId))
      {
        MSS_Client.Properties.Settings.Default.MSSId = Guid.NewGuid().ToString();
        MSS_Client.Properties.Settings.Default.Save();
      }
      MSS.Business.Utils.AppContext.Current.MSSClientId = MSS_Client.Properties.Settings.Default.MSSId;
      if (MSS_Client.Properties.Settings.Default.RememberedUserId != Guid.Empty)
      {
        User user = this._repositoryFactory.GetUserRepository().SearchWithFetch<IList<UserRole>>((Expression<Func<User, bool>>) (u => u.Id == MSS_Client.Properties.Settings.Default.RememberedUserId), (Expression<Func<User, IList<UserRole>>>) (user1 => user1.UserRoles)).FirstOrDefault<User>();
        if (user != null)
          MSS.Business.Utils.AppContext.Current.LoggedUser = user;
      }
      MSS.Business.Utils.AppContext.Current.Initialize(this._repositoryFactory.GetRepository<ApplicationParameter>().GetAll());
      if (!MSS.Business.Utils.AppContext.Current.HasServer)
        return;
      MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = new SettingsConnectionManager().IsServerAvailableAndStatusAccepted(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp"));
    }

    private void CheckSendStatus()
    {
      try
      {
        MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend = true;
        if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
          return;
        MSS.Business.Utils.AppContext.Current.IsClientUpToDateSend &= SychronizationHelperFactory.GetSynchronizationHelper().IsVersionUpToDateSend(CustomerConfiguration.GetPropertyValue<bool>("IsPartialSync"));
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
      }
    }

    public void InitializeSyncronization()
    {
      if (this._repositoryFactory.GetRepository<StructureNodeType>().GetAll().Count > 0 || !MSS.Business.Utils.AppContext.Current.HasServer)
        return;
      if (CustomerConfiguration.GetPropertyValue<bool>("IsPartialSync"))
      {
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Configuration, SyncDirectionOrder.Download);
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Users, SyncDirectionOrder.Download);
      }
      else
      {
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Configuration, SyncDirectionOrder.Download);
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Users, SyncDirectionOrder.Download);
        SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope(SyncScopesEnum.Application, SyncDirectionOrder.Download);
      }
      if (!this._repositoryFactory.GetSession().IsOpen)
        this._repositoryFactory = DIConfigurator.GetConfigurator().Get<IRepositoryFactory>();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      this.Startup += new StartupEventHandler(this.OnStartup);
      Application.LoadComponent((object) this, new Uri("/MSS_Client;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
