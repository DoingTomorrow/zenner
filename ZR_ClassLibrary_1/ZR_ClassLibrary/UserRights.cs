// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.UserRights
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class UserRights
  {
    internal UserRights.RightsListEntry[] BaseRightsList = new UserRights.RightsListEntry[24]
    {
      new UserRights.RightsListEntry(UserRights.DemoRights, UserRights.Packages.Demo, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.DeveloperRights, UserRights.Packages.Developer, "GMM"),
      new UserRights.RightsListEntry(UserRights.ZR_FactoryRights, UserRights.Packages.ZennerFactory, "GMM"),
      new UserRights.RightsListEntry(UserRights.AutoDataReaderRights, UserRights.Packages.None, "MeterInstaller"),
      new UserRights.RightsListEntry(UserRights.DesignerRights, UserRights.Packages.Designer, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.MeterConfigRights, UserRights.Packages.None, "Designer"),
      new UserRights.RightsListEntry(UserRights.ExternalTestbenchRights, UserRights.Packages.ExternalTestbench, "S3_Handler"),
      new UserRights.RightsListEntry(UserRights.AllRights, UserRights.Packages.AllRights, "GMM"),
      new UserRights.RightsListEntry(UserRights.AutoReaderRights, UserRights.Packages.None, "DeviceCollector"),
      new UserRights.RightsListEntry(UserRights.ManualDataRights, UserRights.Packages.ManualData, "MeterTypist"),
      new UserRights.RightsListEntry(UserRights.DataRights, UserRights.Packages.None, "MeterInstaller"),
      new UserRights.RightsListEntry(UserRights.CompleteRights, UserRights.Packages.Complete, "GMM"),
      new UserRights.RightsListEntry(UserRights.ExternalWaterTestbenchRights, UserRights.Packages.ExternalWaterTestbench, "GMM"),
      new UserRights.RightsListEntry(UserRights.ServiceManagerRights, UserRights.Packages.ServiceManager, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.ConfigurationManagerRights, UserRights.Packages.ConfigurationManager, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.RadioManagerRights, UserRights.Packages.RadioManager, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.SystemManagerRights, UserRights.Packages.SystemManager, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.GlobalMeterManagerRights, UserRights.Packages.GlobalMeterManager, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.SystemManagerLightRights, UserRights.Packages.SystemManagerLight, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.MinolDeviceBasicRights, UserRights.Packages.MinolDeviceBasic, "GMM"),
      new UserRights.RightsListEntry(UserRights.MinolDeviceFullRights, UserRights.Packages.MinolDeviceFull, "GMM"),
      new UserRights.RightsListEntry(UserRights.MinolDeviceProRights, UserRights.Packages.MinolDevicePro, "GMM"),
      new UserRights.RightsListEntry(UserRights.ConfigurationManagerProRights, UserRights.Packages.ConfigurationManagerPro, "StartWindow"),
      new UserRights.RightsListEntry(UserRights.MinolRights, UserRights.Packages.Minol, "GMM")
    };
    internal UserRights.OptionRightsListEntry[] OptionRightsList = new UserRights.OptionRightsListEntry[11]
    {
      new UserRights.OptionRightsListEntry(UserRights.NoRights, UserRights.PackagesOptions.NoOptions),
      new UserRights.OptionRightsListEntry(UserRights.OptionsGraphicRights, UserRights.PackagesOptions.Graphic),
      new UserRights.OptionRightsListEntry(UserRights.OptionsExportRights, UserRights.PackagesOptions.Export),
      new UserRights.OptionRightsListEntry(UserRights.OptionsGraphicExportRights, UserRights.PackagesOptions.GraphicAndExport),
      new UserRights.OptionRightsListEntry(UserRights.OptionsAlarmRights, UserRights.PackagesOptions.Alarm),
      new UserRights.OptionRightsListEntry(UserRights.OptionsGraphicAlarmRights, UserRights.PackagesOptions.GraphicAndAlarm),
      new UserRights.OptionRightsListEntry(UserRights.OptionsExportAlarmRights, UserRights.PackagesOptions.ExportAndAlarm),
      new UserRights.OptionRightsListEntry(UserRights.OptionsGraphicExportAlarmRights, UserRights.PackagesOptions.ExportAndGraphicAndAlarm),
      new UserRights.OptionRightsListEntry(UserRights.OptionChangeMenuRights, UserRights.PackagesOptions.Designer),
      new UserRights.OptionRightsListEntry(UserRights.OptionSerialBusRights, UserRights.PackagesOptions.DeviceCollector),
      new UserRights.OptionRightsListEntry(UserRights.OptionProfessional, UserRights.PackagesOptions.Professional)
    };
    private static UserRights.Rights[] NoRights = new UserRights.Rights[0];
    private static UserRights.Rights[] OnlyManualRights = new UserRights.Rights[4]
    {
      UserRights.Rights.Developer,
      UserRights.Rights.LanguageTranslator,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] DemoRights = new UserRights.Rights[10]
    {
      UserRights.Rights.Autologin,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.Designer,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] DeveloperRights = new UserRights.Rights[55]
    {
      UserRights.Rights.Developer,
      UserRights.Rights.Administrator,
      UserRights.Rights.LanguageTranslator,
      UserRights.Rights.Database,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.Autologin,
      UserRights.Rights.ChiefOfEnergyTestCenter,
      UserRights.Rights.ChiefOfWaterTestCenter,
      UserRights.Rights.EquipmentCalibration,
      UserRights.Rights.EquipmentCreation,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.PDASynchronizer,
      UserRights.Rights.ZelsiusOperator,
      UserRights.Rights.Designer,
      UserRights.Rights.DesignerChangeMenu,
      UserRights.Rights.ProfessionalConfig,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MeterFactory,
      UserRights.Rights.EHCA_Factory,
      UserRights.Rights.EndTest,
      UserRights.Rights.FactoryPrinter,
      UserRights.Rights.WaterTestbench,
      UserRights.Rights.EnergieTestbench,
      UserRights.Rights.HardwareTest,
      UserRights.Rights.HardwareTestZelsius,
      UserRights.Rights.ZelsiusModuleTest,
      UserRights.Rights.CapsuleTest,
      UserRights.Rights.CompleteTestbench,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MConfigSet2,
      UserRights.Rights.MConfigSet3,
      UserRights.Rights.MConfigSet4,
      UserRights.Rights.WalkBy,
      UserRights.Rights.Radio3,
      UserRights.Rights.MBus,
      UserRights.Rights.MinomatV2,
      UserRights.Rights.MinomatV4,
      UserRights.Rights.Waveflow,
      UserRights.Rights.ISF,
      UserRights.Rights.MinolExpertHandler,
      UserRights.Rights.Configurator,
      UserRights.Rights.WirelessMBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.EDC_Handler,
      UserRights.Rights.S3_Handler,
      UserRights.Rights.EDC_Testbench,
      UserRights.Rights.SmokeDetectorHandler,
      UserRights.Rights.PDC_Handler
    };
    private static UserRights.Rights[] AllRights = new UserRights.Rights[53]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.LanguageTranslator,
      UserRights.Rights.Database,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.Autologin,
      UserRights.Rights.ChiefOfEnergyTestCenter,
      UserRights.Rights.ChiefOfWaterTestCenter,
      UserRights.Rights.EquipmentCalibration,
      UserRights.Rights.EquipmentCreation,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.ZelsiusOperator,
      UserRights.Rights.Designer,
      UserRights.Rights.DesignerChangeMenu,
      UserRights.Rights.ProfessionalConfig,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MeterFactory,
      UserRights.Rights.EHCA_Factory,
      UserRights.Rights.EndTest,
      UserRights.Rights.FactoryPrinter,
      UserRights.Rights.WaterTestbench,
      UserRights.Rights.EnergieTestbench,
      UserRights.Rights.HardwareTest,
      UserRights.Rights.HardwareTestZelsius,
      UserRights.Rights.ZelsiusModuleTest,
      UserRights.Rights.CapsuleTest,
      UserRights.Rights.CompleteTestbench,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MConfigSet2,
      UserRights.Rights.MConfigSet3,
      UserRights.Rights.MConfigSet4,
      UserRights.Rights.WalkBy,
      UserRights.Rights.Radio3,
      UserRights.Rights.MBus,
      UserRights.Rights.MinomatV2,
      UserRights.Rights.MinomatV4,
      UserRights.Rights.Waveflow,
      UserRights.Rights.ISF,
      UserRights.Rights.MinolExpertHandler,
      UserRights.Rights.Configurator,
      UserRights.Rights.WirelessMBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.AutoUpdate,
      UserRights.Rights.EDC_Handler,
      UserRights.Rights.EDC_Testbench,
      UserRights.Rights.SmokeDetectorHandler,
      UserRights.Rights.PDC_Handler
    };
    private static UserRights.Rights[] CompleteRights = new UserRights.Rights[18]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.ZelsiusOperator,
      UserRights.Rights.DesignerChangeMenu,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.ISF,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] ZR_FactoryRights = new UserRights.Rights[37]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.ChiefOfEnergyTestCenter,
      UserRights.Rights.ChiefOfWaterTestCenter,
      UserRights.Rights.EquipmentCalibration,
      UserRights.Rights.EquipmentCreation,
      UserRights.Rights.ZelsiusOperator,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MeterFactory,
      UserRights.Rights.EHCA_Factory,
      UserRights.Rights.EndTest,
      UserRights.Rights.FactoryPrinter,
      UserRights.Rights.WaterTestbench,
      UserRights.Rights.EnergieTestbench,
      UserRights.Rights.HardwareTest,
      UserRights.Rights.HardwareTestZelsius,
      UserRights.Rights.ZelsiusModuleTest,
      UserRights.Rights.CapsuleTest,
      UserRights.Rights.CompleteTestbench,
      UserRights.Rights.Radio3,
      UserRights.Rights.MBus,
      UserRights.Rights.ISF,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.MinolExpertHandler,
      UserRights.Rights.EDC_Handler,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MConfigSet2,
      UserRights.Rights.MConfigSet3,
      UserRights.Rights.MConfigSet4,
      UserRights.Rights.EDC_Testbench,
      UserRights.Rights.MeterProtocol,
      UserRights.Rights.Waveflow,
      UserRights.Rights.WirelessMBus,
      UserRights.Rights.SmokeDetectorHandler,
      UserRights.Rights.PDC_Handler
    };
    private static UserRights.Rights[] AutoDataReaderRights = new UserRights.Rights[13]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] DataRights = new UserRights.Rights[12]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] AutoReaderRights = new UserRights.Rights[6]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Autologin,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] ManualDataRights = new UserRights.Rights[6]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.Autologin,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] MeterConfigRights = new UserRights.Rights[8]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Autologin,
      UserRights.Rights.ZelsiusOperator,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules
    };
    private static UserRights.Rights[] DesignerRights = new UserRights.Rights[10]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Autologin,
      UserRights.Rights.ZelsiusOperator,
      UserRights.Rights.Designer,
      UserRights.Rights.DesignerChangeMenu,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] ServiceManagerRights = new UserRights.Rights[6]
    {
      UserRights.Rights.Autologin,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] ConfigurationManagerRights = new UserRights.Rights[5]
    {
      UserRights.Rights.Autologin,
      UserRights.Rights.Designer,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.Configurator,
      UserRights.Rights.MBus
    };
    private static UserRights.Rights[] ConfigurationManagerProRights = new UserRights.Rights[6]
    {
      UserRights.Rights.Autologin,
      UserRights.Rights.Designer,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.Configurator,
      UserRights.Rights.ProfessionalConfig,
      UserRights.Rights.MBus
    };
    private static UserRights.Rights[] ExternalTestbenchRights = new UserRights.Rights[18]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.ChiefOfEnergyTestCenter,
      UserRights.Rights.ChiefOfWaterTestCenter,
      UserRights.Rights.EquipmentCalibration,
      UserRights.Rights.EquipmentCreation,
      UserRights.Rights.Designer,
      UserRights.Rights.DesignerChangeMenu,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.EnergieTestbench,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.S3_Handler,
      UserRights.Rights.Autologin,
      UserRights.Rights.Configurator,
      UserRights.Rights.ProfessionalConfig
    };
    private static UserRights.Rights[] ExternalWaterTestbenchRights = new UserRights.Rights[12]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.DatabaseManager,
      UserRights.Rights.ChiefOfWaterTestCenter,
      UserRights.Rights.EquipmentCalibration,
      UserRights.Rights.EquipmentCreation,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.WaterTestbench,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] RadioManagerRights = new UserRights.Rights[12]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.PDASynchronizer,
      UserRights.Rights.Waveflow,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] SystemManagerRights = new UserRights.Rights[15]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] SystemManagerLightRights = new UserRights.Rights[15]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MBus,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator
    };
    private static UserRights.Rights[] GlobalMeterManagerRights = new UserRights.Rights[19]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Autologin,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.Designer,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.PDASynchronizer,
      UserRights.Rights.MBus,
      UserRights.Rights.Waveflow,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.Configurator,
      UserRights.Rights.WirelessMBus
    };
    private static UserRights.Rights[] MinolDeviceBasicRights = new UserRights.Rights[6]
    {
      UserRights.Rights.Database,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.Radio3,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MinomatV2
    };
    private static UserRights.Rights[] MinolDeviceProRights = new UserRights.Rights[8]
    {
      UserRights.Rights.Database,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.WalkBy,
      UserRights.Rights.Radio3,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MConfigSet2,
      UserRights.Rights.MinomatV2
    };
    private static UserRights.Rights[] MinolDeviceFullRights = new UserRights.Rights[11]
    {
      UserRights.Rights.Database,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.WalkBy,
      UserRights.Rights.Radio3,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MConfigSet2,
      UserRights.Rights.MConfigSet3,
      UserRights.Rights.MinomatV2,
      UserRights.Rights.MinomatV4
    };
    private static UserRights.Rights[] MinolRights = new UserRights.Rights[33]
    {
      UserRights.Rights.Administrator,
      UserRights.Rights.Database,
      UserRights.Rights.Designer,
      UserRights.Rights.ProfessionalConfig,
      UserRights.Rights.DeviceCollector,
      UserRights.Rights.AsyncCom,
      UserRights.Rights.MeterData,
      UserRights.Rights.MeterDataSynchronizer,
      UserRights.Rights.MeterInstaller,
      UserRights.Rights.MeterReader,
      UserRights.Rights.DesignerChangeMenu,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataExport,
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.PDASynchronizer,
      UserRights.Rights.MBus,
      UserRights.Rights.WirelessMBus,
      UserRights.Rights.Autologin,
      UserRights.Rights.MinolExpertHandler,
      UserRights.Rights.Radio3,
      UserRights.Rights.WalkBy,
      UserRights.Rights.MConfigSet1,
      UserRights.Rights.MConfigSet2,
      UserRights.Rights.MConfigSet3,
      UserRights.Rights.MinomatV2,
      UserRights.Rights.MinomatV4,
      UserRights.Rights.Configurator,
      UserRights.Rights.ISF,
      UserRights.Rights.TranslationRules,
      UserRights.Rights.AutoUpdate,
      UserRights.Rights.EDC_Handler,
      UserRights.Rights.SmokeDetectorHandler,
      UserRights.Rights.PDC_Handler
    };
    private static UserRights.Rights[] OptionsGraphicRights = new UserRights.Rights[1]
    {
      UserRights.Rights.MeterDataGraphics
    };
    private static UserRights.Rights[] OptionsExportRights = new UserRights.Rights[1]
    {
      UserRights.Rights.MeterDataExport
    };
    private static UserRights.Rights[] OptionsGraphicExportRights = new UserRights.Rights[2]
    {
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataExport
    };
    private static UserRights.Rights[] OptionsAlarmRights = new UserRights.Rights[1]
    {
      UserRights.Rights.MeterDataAlarm
    };
    private static UserRights.Rights[] OptionsGraphicAlarmRights = new UserRights.Rights[2]
    {
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataGraphics
    };
    private static UserRights.Rights[] OptionsExportAlarmRights = new UserRights.Rights[2]
    {
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataExport
    };
    private static UserRights.Rights[] OptionsGraphicExportAlarmRights = new UserRights.Rights[3]
    {
      UserRights.Rights.MeterDataAlarm,
      UserRights.Rights.MeterDataGraphics,
      UserRights.Rights.MeterDataExport
    };
    private static UserRights.Rights[] OptionChangeMenuRights = new UserRights.Rights[1]
    {
      UserRights.Rights.DesignerChangeMenu
    };
    private static UserRights.Rights[] OptionProfessional = new UserRights.Rights[1]
    {
      UserRights.Rights.ProfessionalConfig
    };
    private static UserRights.Rights[] OptionSerialBusRights = new UserRights.Rights[1]
    {
      UserRights.Rights.DeviceCollector
    };
    internal ResourceManager MyRes;
    public static byte LICENSE_CODE_VERSION_PC = 1;
    public static byte LICENSE_CODE_VERSION_HANDHEALD = 3;
    public static UserRights GlobalUserRights = (UserRights) null;
    internal ZRDataAdapter User_Adapter;
    internal int UserID_RangeMin = -1;
    internal int UserID_RangeMax = -1;
    private bool dbliz = false;
    private bool ohneliz = true;
    private bool lizok = false;
    public string aktCode = "LZC.";
    public int PackageNumber = -1;
    public string CustomerNameCode = string.Empty;
    internal int OptionNumber = -1;
    public string PackageName = string.Empty;
    public string OptionPackageName = string.Empty;
    public UserRights.Packages Package = UserRights.Packages.None;
    public DateTime DemoEndDate = DateTime.MinValue;
    public UserRights.LastErrorCode LastError = UserRights.LastErrorCode.NoError;
    public string PrivatKey = "12345678";
    public string LoginName = string.Empty;
    public int LoginPersonalNumber = 0;
    internal bool[] ActiveRights;
    internal bool[] BasicRights;
    public bool PluginGMMFlag = false;
    internal string TempName = string.Empty;
    internal string TempRightsString = string.Empty;
    internal string TempPersonalNumber = string.Empty;
    internal string TempKey = string.Empty;
    internal bool[] TempRights;
    internal Schema.GMM_UserDataTable Typed_GMM_UserTable;
    private GMMConfig MyConfig;

    public bool IsDistributionLizense
    {
      get
      {
        int num;
        if (this.PackageName.IndexOf(UserRights.Packages.ConfigurationManager.ToString()) <= -1)
        {
          string packageName1 = this.PackageName;
          UserRights.Packages packages = UserRights.Packages.ServiceManager;
          string str1 = packages.ToString();
          if (packageName1.IndexOf(str1) <= -1)
          {
            string packageName2 = this.PackageName;
            packages = UserRights.Packages.RadioManager;
            string str2 = packages.ToString();
            if (packageName2.IndexOf(str2) <= -1)
            {
              string packageName3 = this.PackageName;
              packages = UserRights.Packages.SystemManager;
              string str3 = packages.ToString();
              if (packageName3.IndexOf(str3) <= -1)
              {
                string packageName4 = this.PackageName;
                packages = UserRights.Packages.SystemManagerLight;
                string str4 = packages.ToString();
                num = packageName4.IndexOf(str4) > -1 ? 1 : 0;
                goto label_6;
              }
            }
          }
        }
        num = 1;
label_6:
        return num != 0;
      }
    }

    public event UserRights.PGMMCheckPermission OnPGMMCheckPermission;

    public UserRights()
    {
      this.MyConfig = (GMMConfig) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.KonfigGroup];
      int length = Util.GetNamesOfEnum(typeof (UserRights.Rights)).Length;
      this.ActiveRights = new bool[length];
      this.BasicRights = new bool[length];
      this.MyRes = new ResourceManager("ZR_ClassLibrary.UserRightsMessages", typeof (UserRights).Assembly);
    }

    public bool IsOneOfThisPackages(UserRights.Packages[] RequestList)
    {
      for (int index = 0; index < RequestList.Length; ++index)
      {
        if (this.Package == RequestList[index])
          return true;
      }
      return false;
    }

    public int NumberOfUsersIntoDb
    {
      get
      {
        this.GarantUserTableLoaded();
        return this.Typed_GMM_UserTable.Count;
      }
    }

    public void LoadFromDatabase()
    {
      this.LoadAllUser();
      ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM DatabaseIdentification WHERE InfoName = 'DatabaseLocationName'", DbBasis.PrimaryDB.GetDbConnection());
      Schema.DatabaseIdentificationDataTable identificationDataTable = new Schema.DatabaseIdentificationDataTable();
      zrDataAdapter1.Fill((DataTable) identificationDataTable);
      if (identificationDataTable.Rows.Count != 1)
        return;
      ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ZRGlobalID  WHERE ZRFieldName = 'UserPersonalNumber' AND DatabaseLocationName = '" + identificationDataTable[0].InfoData + "'", DbBasis.PrimaryDB.GetDbConnection());
      Schema.ZRGlobalIDDataTable globalIdDataTable = new Schema.ZRGlobalIDDataTable();
      zrDataAdapter2.Fill((DataTable) globalIdDataTable);
      if (globalIdDataTable.Rows.Count == 1)
      {
        this.UserID_RangeMin = globalIdDataTable[0].ZRFirstNr;
        this.UserID_RangeMax = globalIdDataTable[0].ZRLastNr;
      }
    }

    public bool NewValueList(int[] wert, string CodeString)
    {
      if (CodeString.Length < 5 || wert.Length < 10)
        return false;
      StringBuilder stringBuilder1 = new StringBuilder(100);
      for (int index = 0; index < wert.Length; ++index)
        stringBuilder1.Append((char) (wert[index] - 19123 - (index + 685) * 5 + (wert.Length - index) * 369));
      string str1 = stringBuilder1.ToString();
      StringBuilder stringBuilder2 = new StringBuilder(100);
      int index1 = 0;
      for (int index2 = 0; index2 < CodeString.Length; ++index2)
      {
        int InChar = (int) str1[index1] % 32 ^ ParameterService.GetIntFromCharacterCode(CodeString[index2]);
        stringBuilder2.Append(ParameterService.GetCharacterCode(InChar));
        ++index1;
        if (index1 >= str1.Length)
          index1 = 0;
      }
      string str2 = stringBuilder2.ToString();
      int num1 = 0;
      string str3;
      try
      {
        for (int index3 = 0; index3 < 6; ++index3)
          num1 += ParameterService.GetIntFromCharacterCode(str2[index3]) << index3 * 5;
        int length = ParameterService.GetIntFromCharacterCode(str2[6]) + (ParameterService.GetIntFromCharacterCode(str2[7]) << 5);
        str3 = str2.Substring(8, length);
      }
      catch
      {
        return false;
      }
      int num2 = 0;
      for (int index4 = 0; index4 < str3.Length; ++index4)
      {
        int fromCharacterCode = ParameterService.GetIntFromCharacterCode(str3[index4]);
        num2 += fromCharacterCode * fromCharacterCode + 74565;
      }
      if (num2 != num1)
        return false;
      bool[] flagArray = new bool[Util.GetNamesOfEnum(typeof (UserRights.Rights)).Length];
      int index5 = 0;
      for (int index6 = 0; index6 < str3.Length; ++index6)
      {
        for (int index7 = 1; index7 < 32 && index5 < flagArray.Length; index7 <<= 1)
        {
          if ((ParameterService.GetIntFromCharacterCode(str3[index6]) & index7) > 0)
            flagArray[index5] = true;
          ++index5;
        }
      }
      this.ActiveRights = flagArray;
      this.ohneliz = false;
      this.dbliz = true;
      this.lizok = true;
      return true;
    }

    public string GetPackageStartComponentName()
    {
      return this.PackageNumber < 0 || this.PackageNumber > this.BaseRightsList.Length - 1 ? string.Empty : this.BaseRightsList[this.PackageNumber].PackageStartComponentName;
    }

    public void SetPackageRights()
    {
      if (this.PackageNumber < 0)
        return;
      if (this.PackageNumber >= this.BaseRightsList.Length || this.BaseRightsList[this.PackageNumber].Package == UserRights.Packages.None)
      {
        int num1 = (int) MessageBox.Show(this.MyRes.GetString("Wrong package number"));
      }
      else
      {
        UserRights.Rights[] availableRights1 = this.BaseRightsList[this.PackageNumber].AvailableRights;
        string str;
        for (int index = 0; index < availableRights1.Length; ++index)
        {
          str = availableRights1[index].ToString();
          this.BasicRights[(int) availableRights1[index]] = true;
        }
        if (this.BaseRightsList[this.PackageNumber].Package == UserRights.Packages.Demo)
        {
          int num2 = DateTime.Now.Year / 10 * 10;
          int num3 = (this.OptionNumber & 960) >> 6;
          if (num3 == 0 && DateTime.Now.Year % 10 == 9)
            num2 += 10;
          this.DemoEndDate = new DateTime(num2 + num3, 1, 1).AddDays((double) ((this.OptionNumber & 63) * 7));
          this.OptionPackageName = this.DemoEndDate.ToShortDateString();
          if (this.DemoEndDate < DateTime.Now)
          {
            int num4 = (int) GMM_MessageBox.ShowMessage("Global Meter Manager", this.MyRes.GetString("DemoVersionExhausted"), true);
            Application.Exit();
            return;
          }
        }
        else
        {
          if (this.OptionNumber >= this.OptionRightsList.Length)
          {
            int num5 = (int) MessageBox.Show("Wrong Option number");
            return;
          }
          UserRights.Rights[] availableRights2 = this.OptionRightsList[this.OptionNumber].AvailableRights;
          for (int index = 0; index < availableRights2.Length; ++index)
          {
            if (!this.BasicRights[(int) availableRights2[index]])
            {
              str = availableRights2[index].ToString();
              this.BasicRights[(int) availableRights2[index]] = true;
            }
          }
        }
        this.ActiveRights = this.BasicRights;
      }
    }

    public void ClearRights()
    {
      this.ActiveRights.Initialize();
      this.TempRights.Initialize();
    }

    internal bool LoadAllUser()
    {
      this.User_Adapter = (ZRDataAdapter) null;
      this.GarantUserTableLoaded();
      return true;
    }

    internal void GarantUserTableLoaded()
    {
      if (this.User_Adapter != null || DbBasis.PrimaryDB == null)
        return;
      this.User_Adapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM GMM_User", DbBasis.PrimaryDB.GetDbConnection());
      this.Typed_GMM_UserTable = new Schema.GMM_UserDataTable();
      this.User_Adapter.Fill((DataTable) this.Typed_GMM_UserTable);
    }

    public bool CheckRight(int PersonelNumber, UserRights.Rights TheRight)
    {
      this.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserPersonalNumber = " + PersonelNumber.ToString());
      return gmmUserRowArray.Length == 1 && (" " + gmmUserRowArray[0].UserRights + " ").IndexOf(" " + ((int) TheRight).ToString() + " ") >= 0;
    }

    public bool CheckRight() => !(this.DemoEndDate == DateTime.MinValue);

    public bool CheckRight(UserRights.Rights TheRight)
    {
      bool flag = !this.ohneliz && this.dbliz && this.lizok && TheRight < (UserRights.Rights) this.ActiveRights.Length && this.ActiveRights[(int) TheRight];
      if (this.PluginGMMFlag)
        flag = this.OnPGMMCheckPermission != null && this.OnPGMMCheckPermission(TheRight.ToString());
      return flag;
    }

    public bool CheckPGMM_Right(string theRight)
    {
      return this.OnPGMMCheckPermission != null && this.OnPGMMCheckPermission(theRight);
    }

    public bool CheckLiz(ref bool[] liz)
    {
      if (!liz[1])
      {
        try
        {
          this.aktCode = this.decodeZennerCode(this.aktCode);
          if (this.localCustomScrable(UserRights.LocalInfoScrable(0) + UserRights.LocalInfoScrable(1) + UserRights.LocalInfoScrable(2)) == this.aktCode)
          {
            liz[3] = false;
            this.ohneliz = false;
            this.dbliz = liz[0];
            liz[2] = true;
          }
          else
            liz[3] = true;
        }
        catch
        {
          liz[3] = false;
        }
      }
      return this.lizok;
    }

    public string scrable(string inText)
    {
      string str = "";
      for (int index = 0; index < inText.Length; ++index)
      {
        char ch = (char) ((uint) inText[index] + 12U);
        str += ch.ToString();
      }
      return str;
    }

    public string decodeZennerCode(string intext)
    {
      if (intext.Length == 0 || intext.Length != 24 && intext.Length != 29)
        return "";
      intext = intext.Replace("-", "");
      if (intext.Length != 20 && intext.Length != 24)
        return "";
      int num = 22;
      if ((int) intext[0] != (int) ParameterService.GetCharacterCode((int) UserRights.LICENSE_CODE_VERSION_PC) && (int) intext[0] != (int) ParameterService.GetCharacterCode((int) UserRights.LICENSE_CODE_VERSION_HANDHEALD))
      {
        num = 18;
        if ((int) intext[0] != (int) ParameterService.GetCharacterCode(0))
          return "";
      }
      if (UserRights.GetStringCS(intext.Substring(0, num)) != intext.Substring(num))
        return "";
      if (num == 22)
        this.CustomerNameCode = intext.Substring(18, 4);
      this.PackageNumber = 0;
      this.PackageNumber += ParameterService.GetIntFromCharacterCode(intext[1]);
      this.PackageNumber += ParameterService.GetIntFromCharacterCode(intext[2]) << 5;
      this.OptionNumber = 0;
      this.OptionNumber += ParameterService.GetIntFromCharacterCode(intext[3]);
      this.OptionNumber += ParameterService.GetIntFromCharacterCode(intext[4]) << 5;
      if (this.PackageNumber < 0 || this.PackageNumber >= this.BaseRightsList.Length || this.OptionNumber < 0 || this.PackageNumber != 0 && this.OptionNumber >= this.OptionRightsList.Length)
      {
        this.PackageNumber = -1;
        this.OptionNumber = -1;
        return "";
      }
      this.Package = this.BaseRightsList[this.PackageNumber].Package;
      this.PackageName = this.Package.ToString();
      if (this.PackageNumber > 0)
        this.OptionPackageName = this.OptionRightsList[this.OptionNumber].PackageOption.ToString();
      return intext.Substring(5, 13);
    }

    public string localCustomScrable(string intext)
    {
      uint[] numArray1 = new uint[intext.Length];
      uint[] numArray2 = new uint[13];
      this.lizok = true;
      for (int index = 0; index < intext.Length; ++index)
        numArray1[index] = (uint) intext[index];
      int index1 = 7;
      int index2 = 0;
      for (int index3 = 0; index3 < 133; ++index3)
      {
        if (index2 >= numArray1.Length)
          index2 = 0;
        if (index1 >= numArray2.Length)
          index1 -= numArray2.Length;
        numArray2[index1] += numArray1[index2];
        numArray2[index1] += (uint) this.PackageNumber;
        numArray2[index1] += (uint) this.OptionNumber;
        numArray2[index1] += (uint) index2;
        ++index2;
        index1 += 3;
      }
      StringBuilder stringBuilder = new StringBuilder(30);
      for (int index4 = 0; index4 < numArray2.Length; ++index4)
      {
        uint num = numArray2[index4];
        uint InChar = 0;
        for (; num > 0U; num >>= 5)
          InChar ^= num & 31U;
        stringBuilder.Append(ParameterService.GetCharacterCode((int) InChar));
      }
      return stringBuilder.ToString();
    }

    public static string LocalInfoScrable(int index)
    {
      string s = "";
      char[] destination = new char[9];
      int num1 = 9;
      SystemValues.AppPathInfo.CopyTo(index * num1, destination, 0, 9);
      for (int index1 = 0; index1 < 9; ++index1)
        s += destination[index1].ToString();
      int num2 = int.Parse(s);
      string str = "";
      int num3 = index >= 2 ? 5 : 6;
      for (int index2 = 0; index2 < num3; ++index2)
      {
        str += ParameterService.GetCharacterCode((int) (ushort) (num2 ^ num2 >> 3)).ToString();
        num2 >>= 5;
      }
      return str;
    }

    public static string GetStringCS(string InputString)
    {
      int InChar = 0;
      for (int index = 0; index < InputString.Length; ++index)
        InChar = InChar + index + (InputString.Length - index) + (int) InputString[index];
      char characterCode = ParameterService.GetCharacterCode(InChar);
      string str1 = "" + characterCode.ToString();
      characterCode = ParameterService.GetCharacterCode(InChar >> 6);
      string str2 = characterCode.ToString();
      return str1 + str2;
    }

    public static string GetSeparatedString(string InputString)
    {
      StringBuilder stringBuilder = new StringBuilder(30);
      for (int index = 0; index < InputString.Length; ++index)
      {
        if (index % 4 == 0 && index > 0)
          stringBuilder.Append('-');
        stringBuilder.Append(InputString[index]);
      }
      return stringBuilder.ToString();
    }

    private static int GetPackageCode(string InString)
    {
      char[] charArray = InString.ToCharArray();
      int num1 = ParameterService.GetIntFromCharacterCode(charArray[5]) + (ParameterService.GetIntFromCharacterCode(charArray[7]) << 5) + (ParameterService.GetIntFromCharacterCode(charArray[14]) << 10);
      charArray[5] = '0';
      charArray[7] = '4';
      charArray[14] = 'W';
      int num2 = 0;
      for (int index = 0; index < 18; ++index)
        num2 = (num2 << 1) + (int) charArray[index];
      int num3 = num2 & (int) short.MaxValue ^ num1 ^ 23130;
      return num3 >> 4 | num3 << 11 & 30720;
    }

    private static string CharArrayToString(char[] TheArray)
    {
      string empty = string.Empty;
      for (int index = 0; index < TheArray.Length; ++index)
        empty += TheArray[index].ToString();
      return empty;
    }

    public bool VerifyUser(string UserName, string Password)
    {
      this.User_Adapter = (ZRDataAdapter) null;
      this.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'");
      this.LastError = UserRights.LastErrorCode.NoError;
      if (gmmUserRowArray.Length < 1)
        return false;
      string userKey = gmmUserRowArray[0].UserKey;
      string PersonalNr = gmmUserRowArray[0].UserPersonalNumber.ToString();
      string userRights = gmmUserRowArray[0].UserRights;
      return !(this.GenerateKey(UserName, PersonalNr, userRights, Password) != userKey);
    }

    public bool SetNewUser(string UserName, string Password)
    {
      this.User_Adapter = (ZRDataAdapter) null;
      this.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray1 = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'");
      this.LastError = UserRights.LastErrorCode.NoError;
      if (gmmUserRowArray1.Length < 1)
      {
        int num = (int) MessageBox.Show(this.MyRes.GetString("User not available"));
        return false;
      }
      string userKey = gmmUserRowArray1[0].UserKey;
      string str = gmmUserRowArray1[0].UserPersonalNumber.ToString();
      string RightsString = gmmUserRowArray1[0].UserRights;
      if (this.GenerateKey(UserName, str, RightsString, Password) != userKey)
      {
        int num = (int) MessageBox.Show(this.MyRes.GetString("Password error"));
        return false;
      }
      Schema.GMM_UserRow[] gmmUserRowArray2 = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'");
      if (gmmUserRowArray2.Length == 1 && !gmmUserRowArray2[0].IsChangedUserRightsNull() && gmmUserRowArray2[0].ChangedUserRights.Length > 0)
      {
        string changedUserRights = gmmUserRowArray2[0].ChangedUserRights;
        if (this.GenerateKey(UserName, str, changedUserRights, "ChangePass") == gmmUserRowArray2[0].ChangedUserKey)
        {
          this.TempName = UserName;
          this.TempPersonalNumber = str;
          this.TempRightsString = changedUserRights;
          RightsString = this.TempRightsString;
          this.TempKey = this.GenerateKey(this.TempName, this.TempPersonalNumber, this.TempRightsString, Password);
          gmmUserRowArray2[0].UserRights = gmmUserRowArray2[0].ChangedUserRights;
          gmmUserRowArray2[0].UserKey = this.TempKey;
        }
        gmmUserRowArray2[0].ChangedUserRights = string.Empty;
        gmmUserRowArray2[0].ChangedUserKey = string.Empty;
        this.User_Adapter.Update((DataTable) this.Typed_GMM_UserTable);
      }
      this.LoginName = UserName;
      this.LoginPersonalNumber = int.Parse(str);
      this.ActiveRights = this.GetRightsFromString(RightsString);
      this.GarantLicenseRights(ref this.ActiveRights);
      ZR_Component.CommonGmmInterface.CreateUserEnabledComponents();
      return true;
    }

    private bool SetAutoLoginUser(string UserName)
    {
      this.GarantUserTableLoaded();
      this.LastError = UserRights.LastErrorCode.NoError;
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'");
      if (gmmUserRowArray.Length != 1)
      {
        int num = (int) MessageBox.Show(this.MyRes.GetString("User not available"));
        return false;
      }
      string userRights = gmmUserRowArray[0].UserRights;
      this.LoginPersonalNumber = gmmUserRowArray[0].UserPersonalNumber;
      this.LoginName = UserName;
      this.ActiveRights = this.GetRightsFromString(userRights);
      this.GarantLicenseRights(ref this.ActiveRights);
      ZR_Component.CommonGmmInterface.CreateUserEnabledComponents();
      return true;
    }

    public bool autoLogin(string theUserName)
    {
      bool flag;
      if (flag = this.checkAutoLoginOfUser(theUserName))
        flag = this.SetAutoLoginUser(theUserName);
      return flag;
    }

    private bool checkAutoLoginOfUser(string TheUsername)
    {
      this.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + TheUsername + "'");
      return gmmUserRowArray.Length == 1 && gmmUserRowArray[0].UserRights.IndexOf(15.ToString()) >= 0;
    }

    internal string GenerateKey(
      string Name,
      string PersonalNr,
      string RightsString,
      string Password)
    {
      StringBuilder stringBuilder = new StringBuilder(150);
      if (RightsString.Length > 154)
      {
        while (true)
        {
          for (int index = 0; index < 150 && index < RightsString.Length; ++index)
            stringBuilder[index] += RightsString[index];
          if (RightsString.Length > 150)
            RightsString = RightsString.Substring(150);
          else
            break;
        }
        RightsString = stringBuilder.ToString();
      }
      string[] strArray = new string[5]
      {
        Name,
        PersonalNr,
        RightsString,
        Password,
        this.PrivatKey
      };
      while (strArray[0].Length < 150 || strArray[1].Length < 150 || strArray[2].Length < 150 || strArray[3].Length < 150 || strArray[4].Length < 150)
      {
        for (int index1 = 0; index1 < 5; ++index1)
        {
          int index2 = index1 + 1;
          if (index2 >= 5)
            index2 = 0;
          if (strArray[index1].Length < 150)
          {
            // ISSUE: explicit reference operation
            ^ref strArray[index1] += strArray[index2];
          }
        }
      }
      stringBuilder.Length = 0;
      int num = 23421;
      for (int index3 = 0; index3 < 150; ++index3)
      {
        num += index3 + 13250;
        for (int index4 = 0; index4 < 5; ++index4)
        {
          if (strArray[index4].Length > index3)
            num += (int) strArray[index4][index3];
        }
        stringBuilder.Append((char) num);
      }
      for (int index = 0; index < stringBuilder.Length; ++index)
      {
        stringBuilder[index] &= '\u007F';
        if (stringBuilder[index] < ' ')
          stringBuilder[index] += ' ';
      }
      return stringBuilder.ToString();
    }

    private void GarantLicenseRights(ref bool[] TheRights)
    {
      if (TheRights[0] && !this.BasicRights[0] && MessageBox.Show(this.MyRes.GetString("Alle Rechte freigeben?"), "Developer login", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
      {
        for (int index = 0; index < TheRights.Length; ++index)
          TheRights[index] = true;
        this.DemoEndDate = DateTime.MinValue;
      }
      else
      {
        for (int index = 0; index < TheRights.Length; ++index)
        {
          if (!this.BasicRights[index])
            TheRights[index] = false;
        }
      }
    }

    internal bool[] GetRightsFromString(string RightsString)
    {
      bool[] rightsFromString = new bool[this.ActiveRights.Length];
      string[] strArray = RightsString.Split(' ');
      for (int index = 0; index < this.ActiveRights.Length; ++index)
        rightsFromString[index] = false;
      try
      {
        for (int index1 = 0; index1 < strArray.Length; ++index1)
        {
          if (strArray[index1].Length > 0)
          {
            int index2 = int.Parse(strArray[index1]);
            if (index2 < rightsFromString.Length)
              rightsFromString[index2] = true;
          }
        }
      }
      catch
      {
        int num = (int) MessageBox.Show(this.MyRes.GetString("Rights string error"));
      }
      if (rightsFromString[22] || rightsFromString[17] || rightsFromString[16])
      {
        rightsFromString[49] = true;
        rightsFromString[57] = true;
        rightsFromString[8] = true;
        rightsFromString[9] = true;
      }
      return rightsFromString;
    }

    public bool GarantFirstStartUser()
    {
      string UserName = "Administrator";
      string Password = "start";
      this.GarantUserTableLoaded();
      if (((Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'")).Length >= 1)
        return true;
      this.TempName = UserName;
      this.TempPersonalNumber = "1";
      List<UserRights.Rights> rightsList = new List<UserRights.Rights>((IEnumerable<UserRights.Rights>) UserRights.OnlyManualRights);
      for (int index = 0; index < this.BasicRights.Length; ++index)
      {
        if (this.BasicRights[index])
        {
          UserRights.Rights rights = (UserRights.Rights) index;
          if (this.CheckRight(UserRights.Rights.Developer) || !rightsList.Contains(rights))
            this.TempRightsString = this.TempRightsString + index.ToString() + " ";
        }
      }
      this.TempRightsString = this.TempRightsString.Trim();
      this.TempKey = this.GenerateKey(this.TempName, this.TempPersonalNumber, this.TempRightsString, Password);
      if (!this.WriteNewUser() || !this.SetNewUser(UserName, Password))
        throw new Exception("Error on start new user.");
      return false;
    }

    internal bool WriteNewUser()
    {
      this.GarantUserTableLoaded();
      if (((Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + this.TempName + "'")).Length != 0)
      {
        int num = (int) MessageBox.Show(this.MyRes.GetString("User already exists"));
        return false;
      }
      Schema.GMM_UserRow row = this.Typed_GMM_UserTable.NewGMM_UserRow();
      row.UserName = this.TempName;
      row.UserPersonalNumber = int.Parse(this.TempPersonalNumber);
      row.UserRights = this.TempRightsString;
      row.UserKey = this.TempKey;
      this.Typed_GMM_UserTable.AddGMM_UserRow(row);
      this.User_Adapter.Update((DataTable) this.Typed_GMM_UserTable);
      this.LoadAllUser();
      return true;
    }

    internal bool WriteChangeUser()
    {
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + this.TempName + "'");
      if (gmmUserRowArray.Length != 1)
      {
        int num = (int) MessageBox.Show(this.MyRes.GetString("User not found"));
        return false;
      }
      gmmUserRowArray[0].ChangedUserRights = this.TempRightsString;
      gmmUserRowArray[0].ChangedUserKey = this.TempKey;
      this.User_Adapter.Update((DataTable) this.Typed_GMM_UserTable);
      return true;
    }

    public int GetUserKeyChecksum(string UserName)
    {
      this.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'");
      if (gmmUserRowArray.Length != 1)
      {
        ZR_ClassLibMessages.AddWarning(this.MyRes.GetString("User not defined") + " User name: " + UserName);
        return 0;
      }
      string userKey = gmmUserRowArray[0].UserKey;
      int userKeyChecksum = 0;
      int num = 0;
      for (int index = 0; index < userKey.Length; ++index)
      {
        num += 7;
        if (num > 25)
          num -= 25;
        userKeyChecksum += (int) userKey[index] << num;
      }
      if (userKeyChecksum == 0)
        userKeyChecksum = 1267494;
      return userKeyChecksum;
    }

    public int GetLockKeyChecksumFromSecondaryDatabase()
    {
      try
      {
        ZRDataAdapter zrDataAdapter = DbBasis.SecondaryDB.ZRDataAdapter("SELECT * FROM GMM_User where UserName = 'ZelsiusLockKey' ", DbBasis.SecondaryDB.GetDbConnection());
        Schema.GMM_UserDataTable gmmUserDataTable = new Schema.GMM_UserDataTable();
        zrDataAdapter.Fill((DataTable) gmmUserDataTable);
        string userKey = gmmUserDataTable[0].UserKey;
        int secondaryDatabase = 0;
        int num = 0;
        for (int index = 0; index < userKey.Length; ++index)
        {
          num += 7;
          if (num > 25)
            num -= 25;
          secondaryDatabase += (int) userKey[index] << num;
        }
        if (secondaryDatabase == 0)
          secondaryDatabase = 1267494;
        return secondaryDatabase;
      }
      catch
      {
        return 0;
      }
    }

    public void NewLicence()
    {
      if (MessageBox.Show(this.MyRes.GetString("Are you sure to delete the old licence?") + "\r\n\r\n" + this.MyRes.GetString("The program will be terminated") + "\r\n" + this.MyRes.GetString("With the next start of the program you have to type in the new licence code."), this.MyRes.GetString("Change licence"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK)
        return;
      this.MyConfig.RemoveValue("GMM", UserRights.GlobalUserRights.scrable("LZC.") + "1");
      this.MyConfig.WriteConfigFile();
      if (this.Typed_GMM_UserTable != null)
      {
        Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.Typed_GMM_UserTable.Select("UserName = 'Administrator'");
        if (gmmUserRowArray.Length == 1)
        {
          string identificationValue = MeterDatabase.TryGetDatabaseIdentificationValue("SingleClientAccess");
          if (!string.IsNullOrEmpty(identificationValue) && Convert.ToBoolean(identificationValue))
          {
            gmmUserRowArray[0].Delete();
            this.User_Adapter.Update((DataTable) this.Typed_GMM_UserTable);
          }
        }
      }
      Application.Exit();
    }

    public enum Packages
    {
      None,
      Developer,
      ZennerFactory,
      AutoDataReader,
      Designer,
      MeterConfig,
      ExternalTestbench,
      AllRights,
      AutoReader,
      ManualData,
      Data,
      Complete,
      ExternalWaterTestbench,
      ServiceManager,
      ConfigurationManager,
      RadioManager,
      SystemManager,
      GlobalMeterManager,
      SystemManagerLight,
      MinolDeviceBasic,
      MinolDeviceFull,
      Demo,
      MinolDevicePro,
      ConfigurationManagerPro,
      Minol,
    }

    public enum PackagesOptions
    {
      NoOptions,
      Graphic,
      Export,
      GraphicAndExport,
      Alarm,
      GraphicAndAlarm,
      ExportAndAlarm,
      ExportAndGraphicAndAlarm,
      Designer,
      DeviceCollector,
      Professional,
    }

    internal struct RightsListEntry
    {
      internal UserRights.Rights[] AvailableRights;
      internal UserRights.Packages Package;
      internal string PackageStartComponentName;

      internal RightsListEntry(
        UserRights.Rights[] AvailableRightsIn,
        UserRights.Packages PackageIn,
        string PackageStartComponentNameIn)
      {
        this.AvailableRights = AvailableRightsIn;
        this.Package = PackageIn;
        this.PackageStartComponentName = PackageStartComponentNameIn;
      }
    }

    internal struct OptionRightsListEntry
    {
      internal UserRights.Rights[] AvailableRights;
      internal UserRights.PackagesOptions PackageOption;

      internal OptionRightsListEntry(
        UserRights.Rights[] AvailableRightsIn,
        UserRights.PackagesOptions PackageOptionIn)
      {
        this.AvailableRights = AvailableRightsIn;
        this.PackageOption = PackageOptionIn;
      }
    }

    public enum Rights
    {
      Developer,
      Administrator,
      Database,
      Designer,
      HardwareTestZelsius,
      EnergieTestbench,
      FactoryPrinter,
      EndTest,
      DeviceCollector,
      AsyncCom,
      MessageLogger,
      ChiefOfEnergyTestCenter,
      ChiefOfWaterTestCenter,
      Logger,
      multidata,
      Autologin,
      RadioTestbench,
      RadioFactory,
      EquipmentCalibration,
      Handler,
      MeterData,
      MeterDataSynchronizer,
      MeterFactory,
      MeterInstaller,
      MeterProtocol,
      MeterReader,
      TestComponents,
      WaterTestbench,
      HardwareTest,
      EHCA_Factory,
      DesignerChangeMenu,
      EquipmentCreation,
      MeterDataGraphics,
      MeterDataExport,
      MeterDataAlarm,
      MeterConfig,
      PPS_Connection,
      PPS_Off,
      ZelsiusModuleTest,
      CompleteTestbench,
      MeterTypist,
      CapsuleTest,
      ZelsiusOperator,
      LanguageTranslator,
      DatabaseManager,
      PDASynchronizer,
      MConfigSet1,
      WalkBy,
      Radio3,
      MBus,
      ProfessionalConfig,
      MConfigSet2,
      MConfigSet3,
      MConfigSet4,
      ISF,
      MinomatV2,
      MinomatV4,
      Waveflow,
      MinolExpertHandler,
      Configurator,
      WirelessMBus,
      TranslationRules,
      AutoUpdate,
      EDC_Handler,
      S3_Handler,
      EDC_Testbench,
      SmokeDetectorHandler,
      PDC_Handler,
    }

    public enum LastErrorCode
    {
      NoError,
      DatabaseError,
    }

    public delegate bool PGMMCheckPermission(string PermissionName);
  }
}
