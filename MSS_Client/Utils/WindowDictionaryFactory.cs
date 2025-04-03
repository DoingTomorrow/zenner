// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.WindowDictionaryFactory
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Client.UI.Desktop.View.Archive;
using MSS.Client.UI.Desktop.View.Jobs;
using MSS.Client.UI.Desktop.View.Meters;
using MSS.Client.UI.Desktop.View.Reporting;
using MSS.Client.UI.Tablet.View.Meters;
using MSS.Client.UI.Tablet.View.Orders;
using MSS.Client.UI.Tablet.View.Structures;
using MSS_Client.ViewModel;
using MSS_Client.ViewModel.Archiving;
using MSS_Client.ViewModel.Configuration;
using MSS_Client.ViewModel.DataCollectors;
using MSS_Client.ViewModel.DataFilters;
using MSS_Client.ViewModel.Download;
using MSS_Client.ViewModel.ExceptionMessageBox;
using MSS_Client.ViewModel.GenericProgressDialog;
using MSS_Client.ViewModel.Jobs;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.NewsAndUpdates;
using MSS_Client.ViewModel.Orders;
using MSS_Client.ViewModel.RadioTest;
using MSS_Client.ViewModel.Reporting;
using MSS_Client.ViewModel.Settings;
using MSS_Client.ViewModel.Startup;
using MSS_Client.ViewModel.Structures;
using MSS_Client.ViewModel.Structures.DeviceViewModels;
using MSS_Client.ViewModel.Synchronization;
using MSS_Client.ViewModel.Users;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS_Client.Utils
{
  public abstract class WindowDictionaryFactory
  {
    protected readonly Dictionary<Type, WindowTypes> windowsDictionary;

    protected WindowDictionaryFactory()
    {
      this.windowsDictionary = new Dictionary<Type, WindowTypes>()
      {
        {
          typeof (MSSSplashScreenViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Startup.MSSSplashScreen),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Startup.MSSSplashScreen)
          }
        },
        {
          typeof (ExceptionMessageBoxViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.ExceptionMessageBox.ExceptionMessageBoxDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.ExceptionMessageBox.ExceptionMessageBoxDialog)
          }
        },
        {
          typeof (GenericMessageViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.GenericMessageDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.GenericMessageDialog)
          }
        },
        {
          typeof (CleanupAppDataMessageViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.GenericMessageConfirmationDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.GenericMessageConfirmationDialog)
          }
        },
        {
          typeof (MSSLicenseCustomerWindowViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.MSSLicenseCustomerWindow),
            Tablet = typeof (MSS.Client.UI.Tablet.View.MSSLicenseCustomerWindow)
          }
        },
        {
          typeof (MSSLoginWindowViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.MSSLoginWindow),
            Tablet = typeof (MSS.Client.UI.Tablet.View.MSSLoginWindow)
          }
        },
        {
          typeof (MSSViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.MSSView),
            Tablet = typeof (MSS.Client.UI.Tablet.View.MSSView)
          }
        },
        {
          typeof (MSSAboutViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.MSSAboutWindow),
            Tablet = typeof (MSS.Client.UI.Tablet.View.MSSAboutWindow)
          }
        },
        {
          typeof (ImportGmmDataViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.ImportGmmDataDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.ImportGmmDataDialog)
          }
        },
        {
          typeof (CreateEditLocationViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.CreateEditLocationDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.CreateEditLocationDialog)
          }
        },
        {
          typeof (CreateEditTenantViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.CreateEditTenantDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.CreateEditTenantDialog)
          }
        },
        {
          typeof (CreateDeviceViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditDeviceDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditDeviceDialog)
          }
        },
        {
          typeof (EditDeviceViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditDeviceDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditDeviceDialog)
          }
        },
        {
          typeof (ReplaceDeviceViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditDeviceDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditDeviceDialog)
          }
        },
        {
          typeof (EditMinomatViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.CreateEditMinomatDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.CreateEditMinomatDialog)
          }
        },
        {
          typeof (EditMinomatMasterViewModel),
          new WindowTypes()
          {
            Tablet = typeof (CreateEditMinomatMasterDialog)
          }
        },
        {
          typeof (EditMinomatSlaveViewModel),
          new WindowTypes()
          {
            Tablet = typeof (CreateEditMinomatSlaveDialog)
          }
        },
        {
          typeof (CreateFixedStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.CreateFixedStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.CreateFixedStructureDialog)
          }
        },
        {
          typeof (CreateLogicalStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.CreateLogicalStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.CreateLogicalStructureDialog)
          }
        },
        {
          typeof (CreatePhysicalStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.CreatePhysicalStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.CreatePhysicalStructureDialog)
          }
        },
        {
          typeof (EditFixedStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditFixedStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditFixedStructureDialog)
          }
        },
        {
          typeof (EditLogicalStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditLogicalStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditLogicalStructureDialog)
          }
        },
        {
          typeof (EditPhysicalStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditPhysicalStructuresDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditPhysicalStructuresDialog)
          }
        },
        {
          typeof (DeleteStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.DeleteStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.DeleteStructureDialog)
          }
        },
        {
          typeof (EditGenericEntityViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.EditGenericEntityDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.EditGenericEntityDialog)
          }
        },
        {
          typeof (SetEvaluationFactorViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.SetEvaluationFactorDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.SetEvaluationFactorDialog)
          }
        },
        {
          typeof (CreateDataCollectorsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataCollectors.CreateDataCollectorDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataCollectors.CreateDataCollectorDialog)
          }
        },
        {
          typeof (StructureMinomatViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataCollectors.StructureMinomatView),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataCollectors.StructureMinomatView)
          }
        },
        {
          typeof (EditDataCollectorViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataCollectors.EditDataCollectorDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataCollectors.EditDataCollectorDialog)
          }
        },
        {
          typeof (DeleteDataCollectorViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataCollectors.DeleteDataCollectorDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataCollectors.DeleteDataCollectorDialog)
          }
        },
        {
          typeof (AddToMasterPoolViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataCollectors.AddDataCollector),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataCollectors.AddDataCollector)
          }
        },
        {
          typeof (RemoveFromMasterPoolViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataCollectors.RemoveDataCollector),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataCollectors.RemoveDataCollector)
          }
        },
        {
          typeof (FilterViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.FiltersDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.FiltersDialog)
          }
        },
        {
          typeof (AddFilterViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.AddFilterDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.AddFilterDialog)
          }
        },
        {
          typeof (UpdateFilterViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.UpdateFilterDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.UpdateFilterDialog)
          }
        },
        {
          typeof (RemoveFilterViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.RemoveFilterDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.RemoveFilterDialog)
          }
        },
        {
          typeof (UpdateRuleViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.UpdateRuleDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.UpdateRuleDialog)
          }
        },
        {
          typeof (RemoveRuleViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.RemoveRuleDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.RemoveRuleDialog)
          }
        },
        {
          typeof (CreateRoleViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.CreateRoleDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.CreateRoleDialog)
          }
        },
        {
          typeof (EditRoleViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.EditRoleDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.EditRoleDialog)
          }
        },
        {
          typeof (DeleteRoleViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.DeleteRoleDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.DeleteRoleDialog)
          }
        },
        {
          typeof (CreateUserViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.CreateUserDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.CreateUserDialog)
          }
        },
        {
          typeof (EditUserViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.EditUserDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.EditUserDialog)
          }
        },
        {
          typeof (DeleteUserViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.DeleteUserDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.DeleteUserDialog)
          }
        },
        {
          typeof (ViewRolePermissionsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Users.ViewRolePermissionsDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Users.ViewRolePermissionsDialog)
          }
        },
        {
          typeof (CreateEditOrderViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.CreateEditReadingOrder),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.CreateEditReadingOrder)
          }
        },
        {
          typeof (DeleteSingleOrderViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.DeleteReadingOrder),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.DeleteReadingOrder)
          }
        },
        {
          typeof (ExecuteReadingOrderViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.ExecuteReadingOrder),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.ExecuteReadingOrder)
          }
        },
        {
          typeof (PrintPreviewViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.PrintPreviewWindow),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.PrintPreviewWindow)
          }
        },
        {
          typeof (StructureOrdersViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.StructureOrdersDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.StructureOrdersDialog)
          }
        },
        {
          typeof (ExecuteInstallationOrderViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.ExecuteInstallationOrderDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.ExecuteInstallationOrderDialog)
          }
        },
        {
          typeof (ReportsForTenantsViewModel),
          new WindowTypes()
          {
            Tablet = typeof (ReportsForTenantsDialog)
          }
        },
        {
          typeof (ShowDataCollectorsForMeterViewModel),
          new WindowTypes()
          {
            Tablet = typeof (ShowDataCollectorsForMeterDialog)
          }
        },
        {
          typeof (ReportsForMinomatsViewModel),
          new WindowTypes()
          {
            Tablet = typeof (ReportsForMinomatsDialog)
          }
        },
        {
          typeof (TestGSMViewModel),
          new WindowTypes() { Tablet = typeof (TestGSMDialog) }
        },
        {
          typeof (AssignMetersViewModel),
          new WindowTypes() { Tablet = typeof (AssignMetersDialog) }
        },
        {
          typeof (RegisterDevicesUserModeViewModel),
          new WindowTypes()
          {
            Tablet = typeof (RegisterDevicesUserModeDialog)
          }
        },
        {
          typeof (ManuallyAssignMetersViewModel),
          new WindowTypes()
          {
            Tablet = typeof (ManuallyAssignMetersDialog)
          }
        },
        {
          typeof (NetworkSetupViewModel),
          new WindowTypes() { Tablet = typeof (NetworkSetupDialog) }
        },
        {
          typeof (RepairModeViewModel),
          new WindowTypes() { Tablet = typeof (RepairModeDialog) }
        },
        {
          typeof (MinomatIdPopupViewModel),
          new WindowTypes() { Tablet = typeof (MinomatIdPopupView) }
        },
        {
          typeof (OrderMessagesViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Orders.OrderMessagesDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Orders.OrderMessagesDialog)
          }
        },
        {
          typeof (ShowRoutingTableViewModel),
          new WindowTypes()
          {
            Tablet = typeof (ShowRoutingTableDialog)
          }
        },
        {
          typeof (RegisteredDevicesForMinomatViewModel),
          new WindowTypes()
          {
            Tablet = typeof (RegisteredDevicesForMinomat)
          }
        },
        {
          typeof (ExportFileSettingsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Reporting.ExportFileSettings),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Reporting.ExportFileSettings)
          }
        },
        {
          typeof (ExportJobViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Reporting.ExportJobDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Reporting.ExportJobDialog)
          }
        },
        {
          typeof (MinomatCommunicationLogDetailsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Reporting.MinomatCommunicationLogDetails),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Reporting.MinomatCommunicationLogDetails)
          }
        },
        {
          typeof (PrintOptionsViewModel),
          new WindowTypes() { Desktop = typeof (PrintOptionsDialog) }
        },
        {
          typeof (ReadingValuesPrintPreviewViewModel),
          new WindowTypes()
          {
            Desktop = typeof (ReadingValuesPrintPreviewDialog)
          }
        },
        {
          typeof (EditServerPathViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Settings.EditServerPath),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Settings.EditServerPath)
          }
        },
        {
          typeof (SendDataBeforeChangeServerViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Settings.SendDataBeforeChangeServer),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Settings.SendDataBeforeChangeServer)
          }
        },
        {
          typeof (ConfigChangeableParamsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Settings.ConfigChangeableParams),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Settings.ConfigChangeableParams)
          }
        },
        {
          typeof (ProfileTypeViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Settings.ProfileTypeDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Settings.ProfileTypeDialog)
          }
        },
        {
          typeof (StructureScanSettingsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Settings.StructureScanSettings),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Settings.StructureScanSettings)
          }
        },
        {
          typeof (MeterPhotosViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Meters.MeterPhotosDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Meters.MeterPhotosDialog)
          }
        },
        {
          typeof (TakePhotoViewModel),
          new WindowTypes()
          {
            Desktop = (Type) null,
            Tablet = typeof (TakePhotoDialog)
          }
        },
        {
          typeof (MeterNotesViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Meters.MeterNotesDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Meters.MeterNotesDialog)
          }
        },
        {
          typeof (AddNoteViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Meters.AddNoteDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Meters.AddNoteDialog)
          }
        },
        {
          typeof (EditNoteViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Meters.EditNoteDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Meters.EditNoteDialog)
          }
        },
        {
          typeof (GenericProgressDialogViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.GenericProgressDialog.GenericProgressDialogView),
            Tablet = typeof (MSS.Client.UI.Tablet.View.GenericProgressDialog.GenericProgressDialogView)
          }
        },
        {
          typeof (DeterminateProgressDialogViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.GenericProgressDialog.DeterminateProgressDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.GenericProgressDialog.DeterminateProgressDialog)
          }
        },
        {
          typeof (DownloadStructuresViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Download.DownloadStructuresDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Download.DownloadStructuresDialog)
          }
        },
        {
          typeof (ShowConflictsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Synchronization.ShowConflictsDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Synchronization.ShowConflictsDialog)
          }
        },
        {
          typeof (FirmwareUpdateViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Configuration.FirmwareUpdateDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Configuration.FirmwareUpdateDialog)
          }
        },
        {
          typeof (DeviceModelChangeableParametersViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Configuration.DeviceModelChangeableParametersDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Configuration.DeviceModelChangeableParametersDialog)
          }
        },
        {
          typeof (ExpertConfigurationViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Configuration.DeviceModelChangeableParametersDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Configuration.DeviceModelChangeableParametersDialog)
          }
        },
        {
          typeof (NewsAndUpdatesViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.NewsAndUpdates.UpdatesAndNews),
            Tablet = typeof (MSS.Client.UI.Tablet.View.NewsAndUpdates.UpdatesAndNews)
          }
        },
        {
          typeof (AddJobDefinitionViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.CreateJobDefinitionDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.CreateJobDefinitionDialog)
          }
        },
        {
          typeof (EditScenarioViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.EditScenarioDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.EditScenarioDialog)
          }
        },
        {
          typeof (AssignStructureMbusViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.AssignStructureMbusDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.AssignStructureDialog)
          }
        },
        {
          typeof (AssignStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.AssignStructureDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.AssignStructureDialog)
          }
        },
        {
          typeof (AddRuleViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.DataFilters.AddRuleDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.DataFilters.AddRuleDialog)
          }
        },
        {
          typeof (MSS_Client.ViewModel.Jobs.RemoveJobDefinition),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.RemoveJobDefinition),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.RemoveJobDefinition)
          }
        },
        {
          typeof (IntervalsViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.AddEditIntervals),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.AddEditIntervals)
          }
        },
        {
          typeof (AddMssReadingJobViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.AddEditJobDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.AddEditJobDialog)
          }
        },
        {
          typeof (RemoveJob),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Jobs.RemoveJobDefinition),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Jobs.RemoveJobDefinition)
          }
        },
        {
          typeof (ViewJobStructureViewModel),
          new WindowTypes()
          {
            Desktop = typeof (ViewJobStructureDialog)
          }
        },
        {
          typeof (JobLogsForJobViewModel),
          new WindowTypes() { Desktop = typeof (JobJogsForJobDialog) }
        },
        {
          typeof (SystemSelectionViewModel),
          new WindowTypes()
          {
            Desktop = typeof (SystemSelectionDialog)
          }
        },
        {
          typeof (ArchivingViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Archive.ArchiveAndDeleteDataDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Archive.ArchiveAndDeleteDataDialog)
          }
        },
        {
          typeof (CreateEditArchiveJobViewModel),
          new WindowTypes()
          {
            Desktop = typeof (CreateEditArchiveJobDialog)
          }
        },
        {
          typeof (DeleteArchiveJobViewModel),
          new WindowTypes()
          {
            Desktop = typeof (DeleteArchiveJobDialog)
          }
        },
        {
          typeof (ViewArchiveTenantViewModel),
          new WindowTypes()
          {
            Desktop = typeof (ViewArchiveTenantDialog)
          }
        },
        {
          typeof (ViewArchiveLocationViewModel),
          new WindowTypes()
          {
            Desktop = typeof (ViewArchiveLocationDialog)
          }
        },
        {
          typeof (ViewArchiveMeterViewModel),
          new WindowTypes()
          {
            Desktop = typeof (ViewArchiveMeterDialog)
          }
        },
        {
          typeof (ViewArchiveMinomatViewModel),
          new WindowTypes()
          {
            Desktop = typeof (ViewArchiveMinomatDialog)
          }
        },
        {
          typeof (MeterReadingValuesViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Meters.MeterReadingValuesDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Meters.MeterReadingValuesDialog)
          }
        },
        {
          typeof (TranslationRulesViewModel),
          new WindowTypes()
          {
            Desktop = typeof (TranslationRulesDialog)
          }
        },
        {
          typeof (ConsumptionViewModel),
          new WindowTypes() { Desktop = typeof (ConsumptionDialog) }
        },
        {
          typeof (RadioTestViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.RadioTest.RadioTestDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.RadioTest.RadioTestDialog)
          }
        },
        {
          typeof (AssignTestRunViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.RadioTest.AssignTestRunDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.RadioTest.AssignTestRunDialog)
          }
        },
        {
          typeof (WarningWithListBoxViewModel),
          new WindowTypes()
          {
            Desktop = typeof (MSS.Client.UI.Desktop.View.Structures.WarningWithListBoxDialog),
            Tablet = typeof (MSS.Client.UI.Tablet.View.Structures.WarningWithListBoxDialog)
          }
        }
      };
    }
  }
}
