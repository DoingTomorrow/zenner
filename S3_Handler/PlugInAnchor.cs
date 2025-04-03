// Decompiled with JetBrains decompiler
// Type: S3_Handler.PlugInAnchor
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace S3_Handler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    private S3_HandlerFunctions MyFunctions;
    internal static string[] UsedRights = new string[10]
    {
      S3_HandlerFunctions.Right_AllHandlersEnabled + "|False|Like developer for all Handlers",
      S3_HandlerFunctions.Right_S3_HandlerEnabled + "|False|S3_Handler complete enabled",
      S3_HandlerFunctions.Right_DeviceCollector,
      S3_HandlerFunctions.Right_DesignerChangeMenu,
      S3_HandlerFunctions.Right_ProfessionalConfig,
      S3_HandlerFunctions.Right_ReadOnly + "|False|Write access to devices is blocked",
      S3_HandlerFunctions.Role_Developer,
      S3_HandlerFunctions.Right_VolumeCalibration + "|False|Volume meter calibration enabled",
      S3_HandlerFunctions.Right_TemperatureCalibration + "|False|Volume meter calibration enabled",
      S3_HandlerFunctions.Right_ChangeIdentification + "|False|Change of serial number enabled"
    };

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new S3_HandlerFunctions("PlugIn");
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowS3_HandlerMainWindow("");

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration() => this.MyFunctions.GetReadoutConfiguration();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("S3_Handler", "Configuration", "Change serie3 device settings", "View and change device settings.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
