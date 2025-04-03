// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Synchronization.ShowConflictsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Microsoft.Synchronization;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Synchronization;
using MSS.Business.Modules.Synchronization.HandleConflicts;
using MSS.Business.Utils;
using MSS.Client.UI.Desktop.View.Synchronization;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace MSS_Client.ViewModel.Synchronization
{
  public class ShowConflictsViewModel : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private ProgressDialog _syncProgressDialog;
    private BackgroundWorker _backgroundWorkerSync;
    private readonly IWindowFactory _windowFactory;
    private DataView _displayedFiltersConflicts;
    private DataView _displayedRulesConflicts;
    private DataView _displayedOrdersConflicts;
    private DataView _displayedMetersConflicts;
    private DataView _displayedLocationsConflicts;
    private DataView _displayedTenantsConflicts;
    private DataView _displayedUsersConflicts;
    private DataView _displayedRolesConflicts;
    private DataView _displayedStructureNodesConflicts;
    private DataView _displayedStructureLinksConflicts;

    private ConflictsManager CurrentConflictsManager { get; set; }

    public Dictionary<Guid, string> ExtraInformation => MSS.Business.Utils.AppContext.Current.SyncExtraData;

    [Inject]
    public ShowConflictsViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      Dictionary<Type, DataTable> conflicts)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.CurrentConflictsManager = new ConflictsManager(conflicts);
      DataTable entityConflicts1 = this.CurrentConflictsManager.GetEntityConflicts(typeof (MSS.Core.Model.DataFilters.Filter));
      if (entityConflicts1 != null)
      {
        this.IsFilterTabVisible = true;
        this.DisplayedFiltersConflicts = entityConflicts1.DefaultView;
      }
      else
        this.IsOrdersTabVisible = false;
      DataTable entityConflicts2 = this.CurrentConflictsManager.GetEntityConflicts(typeof (Rules));
      if (entityConflicts2 != null)
      {
        this.IsRuleTabVisible = true;
        this.DisplayedRulesConflicts = entityConflicts2.DefaultView;
      }
      else
        this.IsOrdersTabVisible = false;
      DataTable entityConflicts3 = this.CurrentConflictsManager.GetEntityConflicts(typeof (Order));
      if (entityConflicts3 != null)
      {
        this.IsOrdersTabVisible = true;
        this.DisplayedOrdersConflicts = entityConflicts3.DefaultView;
      }
      else
        this.IsOrdersTabVisible = false;
      DataTable entityConflicts4 = this.CurrentConflictsManager.GetEntityConflicts(typeof (Meter));
      if (entityConflicts4 != null)
      {
        this.IsMetersTabVisible = true;
        this.DisplayedMetersConflicts = entityConflicts4.DefaultView;
      }
      else
        this.IsMetersTabVisible = false;
      DataTable entityConflicts5 = this.CurrentConflictsManager.GetEntityConflicts(typeof (Location));
      if (entityConflicts5 != null)
      {
        this.IsLocationsTabVisible = true;
        this.DisplayedLocationsConflicts = entityConflicts5.DefaultView;
      }
      else
        this.IsLocationsTabVisible = false;
      DataTable entityConflicts6 = this.CurrentConflictsManager.GetEntityConflicts(typeof (Tenant));
      if (entityConflicts6 != null)
      {
        this.IsTenantsTabVisible = true;
        this.DisplayedTenantsConflicts = entityConflicts6.DefaultView;
      }
      else
        this.IsTenantsTabVisible = false;
      DataTable entityConflicts7 = this.CurrentConflictsManager.GetEntityConflicts(typeof (User));
      if (entityConflicts7 != null)
      {
        this.IsUsersTabVisible = true;
        this.DisplayedUsersConflicts = entityConflicts7.DefaultView;
      }
      else
        this.IsUsersTabVisible = false;
      DataTable entityConflicts8 = this.CurrentConflictsManager.GetEntityConflicts(typeof (Role));
      if (entityConflicts8 != null)
      {
        this.IsRolesTabVisible = true;
        this.DisplayedRolesConflicts = entityConflicts8.DefaultView;
      }
      else
        this.IsRolesTabVisible = false;
      DataTable entityConflicts9 = this.CurrentConflictsManager.GetEntityConflicts(typeof (StructureNode));
      if (entityConflicts9 != null)
      {
        this.IsStructureNodesTabVisible = true;
        this.DisplayedStructureNodesConflicts = entityConflicts9.DefaultView;
      }
      else
        this.IsStructureNodesTabVisible = false;
      DataTable entityConflicts10 = this.CurrentConflictsManager.GetEntityConflicts(typeof (StructureNodeLinks));
      if (entityConflicts10 != null)
      {
        this.IsStructureLinksTabVisible = true;
        this.DisplayedStructureLinksConflicts = entityConflicts10.DefaultView;
      }
      else
        this.IsStructureLinksTabVisible = false;
      if (this.IsFilterTabVisible)
        this.IsFilterTabSelected = true;
      else if (this.IsRuleTabVisible)
        this.IsRuleTabSelected = true;
      else if (this.IsOrdersTabVisible)
        this.IsOrdersTabSelected = true;
      else if (this.IsMetersTabVisible)
        this.IsMetersTabSelected = true;
      else if (this.IsLocationsTabVisible)
        this.IsLocationsTabSelected = true;
      else if (this.IsTenantsTabVisible)
        this.IsTenantsTabVisible = true;
      else if (this.IsUsersTabVisible)
        this.IsUsersTabSelected = true;
      else if (this.IsRolesTabVisible)
        this.IsRolesTabSelected = true;
      else if (this.IsStructureNodesTabVisible)
        this.IsStructuresTabSelected = true;
      else
        this.IsStructuresLinksTabSelected = true;
    }

    public DataView DisplayedFiltersConflicts
    {
      get => this._displayedFiltersConflicts;
      set
      {
        this._displayedFiltersConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedFiltersConflicts));
      }
    }

    public DataView DisplayedRulesConflicts
    {
      get => this._displayedRulesConflicts;
      set
      {
        this._displayedRulesConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedRulesConflicts));
      }
    }

    public DataView DisplayedOrdersConflicts
    {
      get => this._displayedOrdersConflicts;
      set
      {
        this._displayedOrdersConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedOrdersConflicts));
      }
    }

    public DataView DisplayedMetersConflicts
    {
      get => this._displayedMetersConflicts;
      set
      {
        this._displayedMetersConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedMetersConflicts));
      }
    }

    public DataView DisplayedLocationsConflicts
    {
      get => this._displayedLocationsConflicts;
      set
      {
        this._displayedLocationsConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedLocationsConflicts));
      }
    }

    public DataView DisplayedTenantsConflicts
    {
      get => this._displayedTenantsConflicts;
      set
      {
        this._displayedTenantsConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedTenantsConflicts));
      }
    }

    public DataView DisplayedUsersConflicts
    {
      get => this._displayedUsersConflicts;
      set
      {
        this._displayedUsersConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedUsersConflicts));
      }
    }

    public DataView DisplayedRolesConflicts
    {
      get => this._displayedRolesConflicts;
      set
      {
        this._displayedRolesConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedRolesConflicts));
      }
    }

    public DataView DisplayedStructureNodesConflicts
    {
      get => this._displayedStructureNodesConflicts;
      set
      {
        this._displayedStructureNodesConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedStructureNodesConflicts));
      }
    }

    public DataView DisplayedStructureLinksConflicts
    {
      get => this._displayedStructureLinksConflicts;
      set
      {
        this._displayedStructureLinksConflicts = value;
        this.OnPropertyChanged(nameof (DisplayedStructureLinksConflicts));
      }
    }

    private void UpdateConflictResolutionValues()
    {
      if (this.DisplayedFiltersConflicts != null)
        this.UpdateValues(this.DisplayedFiltersConflicts);
      if (this.DisplayedRulesConflicts != null)
        this.UpdateValues(this.DisplayedRulesConflicts);
      if (this.DisplayedOrdersConflicts != null)
        this.UpdateValues(this.DisplayedOrdersConflicts);
      if (this.DisplayedMetersConflicts != null)
        this.UpdateValues(this.DisplayedMetersConflicts);
      if (this.DisplayedLocationsConflicts != null)
        this.UpdateValues(this.DisplayedLocationsConflicts);
      if (this.DisplayedTenantsConflicts != null)
        this.UpdateValues(this.DisplayedTenantsConflicts);
      if (this.DisplayedUsersConflicts != null)
        this.UpdateValues(this.DisplayedUsersConflicts);
      if (this.DisplayedRolesConflicts != null)
        this.UpdateValues(this.DisplayedRolesConflicts);
      if (this.DisplayedStructureNodesConflicts != null)
        this.UpdateValues(this.DisplayedStructureNodesConflicts);
      if (this.DisplayedStructureLinksConflicts == null)
        return;
      this.UpdateValues(this.DisplayedStructureLinksConflicts);
    }

    private void UpdateValues(DataView dataView)
    {
      for (int index = 0; index < dataView.Table.Rows.Count; index += 2)
      {
        DataRow row = dataView.Table.Rows[index];
        MSS.Business.Utils.AppContext.Current.SyncConflicts[(Guid) row["Id"]].DestinationWins = row["IconUrl"].ToString() == "pack://application:,,,/Styles;component/Images/Universal/selected_conflict.png";
      }
    }

    public ICommand ApplyChangesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          MSS.Business.Utils.AppContext.Current.HandleConflicts = true;
          this.UpdateConflictResolutionValues();
          if (!MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_Server_Not_Available, false);
            int num = (int) MessageBox.Show(Resources.MSS_Client_Server_Not_Available);
          }
          else
          {
            this._syncProgressDialog = new ProgressDialog();
            this._syncProgressDialog.Cancel += new EventHandler(this.CancelProcess);
            Dispatcher dispatcher = this._syncProgressDialog.Dispatcher;
            this._backgroundWorkerSync = new BackgroundWorker()
            {
              WorkerReportsProgress = true,
              WorkerSupportsCancellation = true
            };
            this._backgroundWorkerSync.DoWork += (DoWorkEventHandler) ((sender, args) =>
            {
              List<object> objectList = (List<object>) args.Argument;
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[0], (SyncDirectionOrder) objectList[3]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[1], (SyncDirectionOrder) objectList[3]);
              SychronizationHelperFactory.GetSynchronizationHelper().SynchronizeScope((SyncScopesEnum) objectList[1], (SyncDirectionOrder) objectList[3]);
            });
            this._backgroundWorkerSync.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
            {
              this._syncProgressDialog.Close();
              this.OnRequestClose(false);
              EventPublisher.Publish<SyncConflictsStateChanged>(new SyncConflictsStateChanged(), (IViewModel) this);
              EventPublisher.Publish<GridShouldBeUpdated>(new GridShouldBeUpdated(), (IViewModel) this);
              MSS.DTO.Message.Message message = (MSS.DTO.Message.Message) null;
              if (args.Cancelled)
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Warning,
                  MessageText = Resources.MSS_Client_Synchronization_Cancelled
                };
              else if (args.Error != null)
              {
                MSS.Business.Errors.MessageHandler.LogException(args.Error);
                MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
              }
              else
                message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Success,
                  MessageText = Resources.MSS_Client_Synchronization_Succedded
                };
              if (message == null)
                return;
              EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
              {
                Message = message
              }, (IViewModel) this);
            });
            this._backgroundWorkerSync.RunWorkerAsync((object) new List<object>()
            {
              (object) SyncScopesEnum.Configuration,
              (object) SyncScopesEnum.Application,
              (object) SyncScopesEnum.Users,
              (object) SyncDirectionOrder.Download
            });
            this._syncProgressDialog.Owner = Application.Current.Windows[0];
            this._syncProgressDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this._syncProgressDialog.ShowDialog();
            MSS.Business.Utils.AppContext.Current.HandleConflicts = false;
          }
        }));
      }
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(false)));
    }

    public bool IsFilterTabSelected { get; set; }

    public bool IsRuleTabSelected { get; set; }

    public bool IsOrdersTabSelected { get; set; }

    public bool IsMetersTabSelected { get; set; }

    public bool IsLocationsTabSelected { get; set; }

    public bool IsTenantsTabSelected { get; set; }

    public bool IsUsersTabSelected { get; set; }

    public bool IsRolesTabSelected { get; set; }

    public bool IsStructuresTabSelected { get; set; }

    public bool IsStructuresLinksTabSelected { get; set; }

    public bool IsFilterTabVisible { get; set; }

    public bool IsRuleTabVisible { get; set; }

    public bool IsOrdersTabVisible { get; set; }

    public bool IsMetersTabVisible { get; set; }

    public bool IsLocationsTabVisible { get; set; }

    public bool IsTenantsTabVisible { get; set; }

    public bool IsUsersTabVisible { get; set; }

    public bool IsRolesTabVisible { get; set; }

    public bool IsStructureNodesTabVisible { get; set; }

    public bool IsStructureLinksTabVisible { get; set; }

    private void CancelProcess(object sender, EventArgs e)
    {
      this._backgroundWorkerSync.CancelAsync();
    }
  }
}
