// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_Component
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_Component
  {
    public static ZR_Component CommonGmmInterface;
    public SortedList<GMM_Components, object> LoadedComponentsList = new SortedList<GMM_Components, object>();
    public object DeviceManager;
    private static string[] ComponentNames;
    private static bool[] InstalledComponents;
    private static bool[] UserEnabledComponents;

    public event ZR_Component.GarantComponentLoadedFunction OnGarantComponentLoaded;

    public ZR_Component()
    {
      if (ZR_Component.ComponentNames != null)
        return;
      ZR_Component.ComponentNames = Util.GetNamesOfEnum(typeof (GMM_Components));
      ZR_Component.InstalledComponents = new bool[ZR_Component.ComponentNames.Length];
      ZR_Component.UserEnabledComponents = new bool[ZR_Component.ComponentNames.Length];
      this.CreateInstalledComponentsInformation();
    }

    public static void Dispose()
    {
      if (ZR_Component.CommonGmmInterface != null)
        ZR_Component.CommonGmmInterface.OnGarantComponentLoaded = (ZR_Component.GarantComponentLoadedFunction) null;
      ZR_Component.ComponentNames = (string[]) null;
      ZR_Component.InstalledComponents = (bool[]) null;
      ZR_Component.UserEnabledComponents = (bool[]) null;
      ZR_Component.CommonGmmInterface = (ZR_Component) null;
    }

    public bool GarantComponentLoaded(GMM_Components TheComponent)
    {
      if (this.IsComponentLoaded(TheComponent))
        return true;
      if (this.OnGarantComponentLoaded == null)
        return false;
      this.OnGarantComponentLoaded(TheComponent);
      return this.IsComponentLoaded(TheComponent);
    }

    public bool IsComponentLoaded(GMM_Components TheComponent)
    {
      return this.LoadedComponentsList.ContainsKey(TheComponent);
    }

    public bool IsComponentInstalled(GMM_Components TheComponent)
    {
      return ZR_Component.InstalledComponents[(int) TheComponent];
    }

    public bool IsComponentUserEnabled(GMM_Components TheComponent)
    {
      return ZR_Component.UserEnabledComponents[(int) TheComponent];
    }

    public string GetInstalledComponentsNameString()
    {
      StringBuilder stringBuilder = new StringBuilder("GMM");
      for (int index = 0; index < ZR_Component.InstalledComponents.Length; ++index)
      {
        if (ZR_Component.InstalledComponents[index])
          stringBuilder.Append(" " + ZR_Component.ComponentNames[index]);
      }
      return stringBuilder.ToString();
    }

    public string[] GetUserEnabledComponentsNameList()
    {
      int length = 0;
      for (int index = 0; index < ZR_Component.UserEnabledComponents.Length; ++index)
      {
        if (ZR_Component.UserEnabledComponents[index])
          ++length;
      }
      string[] componentsNameList = new string[length];
      int num = 0;
      for (int index = 0; index < ZR_Component.InstalledComponents.Length; ++index)
      {
        if (ZR_Component.UserEnabledComponents[index])
          componentsNameList[num++] = ZR_Component.ComponentNames[index];
      }
      return componentsNameList;
    }

    private void CreateInstalledComponentsInformation()
    {
      string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
      try
      {
        ZR_Component.InstalledComponents[0] = true;
        ZR_Component.InstalledComponents[1] = true;
        string str1 = directoryName;
        char directorySeparatorChar = Path.DirectorySeparatorChar;
        string str2 = directorySeparatorChar.ToString();
        if (File.Exists(str1 + str2 + "MeterInstaller.dll"))
          ZR_Component.InstalledComponents[8] = true;
        string str3 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str4 = directorySeparatorChar.ToString();
        if (File.Exists(str3 + str4 + "MeterReader.dll"))
          ZR_Component.InstalledComponents[7] = true;
        string str5 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str6 = directorySeparatorChar.ToString();
        if (File.Exists(str5 + str6 + "MeterData.dll"))
          ZR_Component.InstalledComponents[9] = true;
        string str7 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str8 = directorySeparatorChar.ToString();
        if (File.Exists(str7 + str8 + "Designer.dll"))
          ZR_Component.InstalledComponents[5] = true;
        string str9 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str10 = directorySeparatorChar.ToString();
        if (File.Exists(str9 + str10 + "Configurator.dll"))
          ZR_Component.InstalledComponents[21] = true;
        string str11 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str12 = directorySeparatorChar.ToString();
        if (File.Exists(str11 + str12 + "gmm_handler.dll"))
          ZR_Component.InstalledComponents[6] = true;
        string str13 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str14 = directorySeparatorChar.ToString();
        if (File.Exists(str13 + str14 + "wf_handler.dll"))
          ZR_Component.InstalledComponents[10] = true;
        string str15 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str16 = directorySeparatorChar.ToString();
        if (File.Exists(str15 + str16 + "MinolHandler.dll"))
          ZR_Component.InstalledComponents[20] = true;
        string str17 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str18 = directorySeparatorChar.ToString();
        if (File.Exists(str17 + str18 + "MinomatHandler.dll"))
          ZR_Component.InstalledComponents[24] = true;
        string str19 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str20 = directorySeparatorChar.ToString();
        if (File.Exists(str19 + str20 + "EDC_Handler.dll"))
          ZR_Component.InstalledComponents[25] = true;
        string str21 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str22 = directorySeparatorChar.ToString();
        if (File.Exists(str21 + str22 + "PDC_Handler.dll"))
          ZR_Component.InstalledComponents[28] = true;
        string str23 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str24 = directorySeparatorChar.ToString();
        if (File.Exists(str23 + str24 + "SmokeDetectorHandler.dll"))
          ZR_Component.InstalledComponents[27] = true;
        string str25 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str26 = directorySeparatorChar.ToString();
        if (File.Exists(str25 + str26 + "EDC_Testbench.dll"))
          ZR_Component.InstalledComponents[26] = true;
        string str27 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str28 = directorySeparatorChar.ToString();
        if (File.Exists(str27 + str28 + "S3_Handler.dll"))
          ZR_Component.InstalledComponents[22] = true;
        string str29 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str30 = directorySeparatorChar.ToString();
        if (File.Exists(str29 + str30 + "DeviceCollector.dll"))
          ZR_Component.InstalledComponents[4] = true;
        string str31 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str32 = directorySeparatorChar.ToString();
        if (File.Exists(str31 + str32 + "AsyncCom.dll"))
          ZR_Component.InstalledComponents[3] = true;
        string str33 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str34 = directorySeparatorChar.ToString();
        if (File.Exists(str33 + str34 + "PDASynchronizer.dll"))
          ZR_Component.InstalledComponents[18] = true;
        string str35 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str36 = directorySeparatorChar.ToString();
        if (File.Exists(str35 + str36 + "MeterFactory.dll"))
          ZR_Component.InstalledComponents[11] = true;
        string str37 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str38 = directorySeparatorChar.ToString();
        if (File.Exists(str37 + str38 + "FactoryPrinter.dll"))
          ZR_Component.InstalledComponents[12] = true;
        string str39 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str40 = directorySeparatorChar.ToString();
        if (File.Exists(str39 + str40 + "MeterProtocol.dll"))
          ZR_Component.InstalledComponents[13] = true;
        string str41 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str42 = directorySeparatorChar.ToString();
        if (File.Exists(str41 + str42 + "HardwareTest.dll"))
          ZR_Component.InstalledComponents[16] = true;
        string str43 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str44 = directorySeparatorChar.ToString();
        if (File.Exists(str43 + str44 + "EnergieTestbench.dll"))
          ZR_Component.InstalledComponents[15] = true;
        string str45 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str46 = directorySeparatorChar.ToString();
        if (File.Exists(str45 + str46 + "WaterTestbench.dll"))
          ZR_Component.InstalledComponents[14] = true;
        string str47 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str48 = directorySeparatorChar.ToString();
        if (File.Exists(str47 + str48 + "CapsuleTest.dll"))
          ZR_Component.InstalledComponents[17] = true;
        string str49 = directoryName;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str50 = directorySeparatorChar.ToString();
        if (!File.Exists(str49 + str50 + "TestComponent.dll"))
          return;
        ZR_Component.InstalledComponents[23] = true;
      }
      catch
      {
        int num = (int) MessageBox.Show("DLL Path error");
      }
    }

    public void CreateUserEnabledComponents()
    {
      for (int index = 0; index < ZR_Component.UserEnabledComponents.Length; ++index)
        ZR_Component.UserEnabledComponents[index] = false;
      ZR_Component.UserEnabledComponents[1] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.MeterInstaller) && this.IsComponentInstalled(GMM_Components.MeterInstaller))
        ZR_Component.UserEnabledComponents[8] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.MeterReader) && this.IsComponentInstalled(GMM_Components.MeterReader))
        ZR_Component.UserEnabledComponents[7] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.MeterData) && this.IsComponentInstalled(GMM_Components.MeterData))
        ZR_Component.UserEnabledComponents[9] = true;
      if ((UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Designer) || UserRights.GlobalUserRights.CheckRight(UserRights.Rights.DesignerChangeMenu)) && this.IsComponentInstalled(GMM_Components.Designer))
        ZR_Component.UserEnabledComponents[5] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Configurator) && this.IsComponentInstalled(GMM_Components.Configurator))
        ZR_Component.UserEnabledComponents[21] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Designer) && UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer) && this.IsComponentInstalled(GMM_Components.GMM_Handler))
        ZR_Component.UserEnabledComponents[6] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer) && this.IsComponentInstalled(GMM_Components.WF_Handler))
        ZR_Component.UserEnabledComponents[10] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.MinolExpertHandler) && this.IsComponentInstalled(GMM_Components.MinolHandler))
        ZR_Component.UserEnabledComponents[20] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.EDC_Handler) && this.IsComponentInstalled(GMM_Components.EDC_Handler))
        ZR_Component.UserEnabledComponents[25] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.PDC_Handler) && this.IsComponentInstalled(GMM_Components.PDC_Handler))
        ZR_Component.UserEnabledComponents[28] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.EDC_Testbench) && this.IsComponentInstalled(GMM_Components.EDC_Testbench))
        ZR_Component.UserEnabledComponents[26] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer) || UserRights.GlobalUserRights.CheckRight(UserRights.Rights.ProfessionalConfig) && UserRights.GlobalUserRights.CheckRight(UserRights.Rights.MinomatV4) && this.IsComponentInstalled(GMM_Components.MinomatHandler))
        ZR_Component.UserEnabledComponents[24] = true;
      if ((UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer) || UserRights.GlobalUserRights.CheckRight(UserRights.Rights.S3_Handler)) && this.IsComponentInstalled(GMM_Components.S3_Handler))
        ZR_Component.UserEnabledComponents[22] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.DeviceCollector) && this.IsComponentInstalled(GMM_Components.DeviceCollector))
        ZR_Component.UserEnabledComponents[4] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.AsyncCom) && this.IsComponentInstalled(GMM_Components.AsyncCom))
        ZR_Component.UserEnabledComponents[3] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.PDASynchronizer) && this.IsComponentInstalled(GMM_Components.PDASynchronizer))
        ZR_Component.UserEnabledComponents[18] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.MeterFactory) && this.IsComponentInstalled(GMM_Components.MeterFactory))
        ZR_Component.UserEnabledComponents[11] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.FactoryPrinter) && this.IsComponentInstalled(GMM_Components.FactoryPrinter))
        ZR_Component.UserEnabledComponents[12] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.FactoryPrinter) && this.IsComponentInstalled(GMM_Components.MeterProtocol))
        ZR_Component.UserEnabledComponents[13] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.WaterTestbench) && this.IsComponentInstalled(GMM_Components.WaterTestbench))
        ZR_Component.UserEnabledComponents[14] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.HardwareTest) && this.IsComponentInstalled(GMM_Components.HardwareTest))
        ZR_Component.UserEnabledComponents[16] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.EnergieTestbench) && this.IsComponentInstalled(GMM_Components.EnergieTestbench))
        ZR_Component.UserEnabledComponents[15] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.CapsuleTest) && this.IsComponentInstalled(GMM_Components.CapsuleTest))
        ZR_Component.UserEnabledComponents[17] = true;
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer) && this.IsComponentInstalled(GMM_Components.TestComponent))
        ZR_Component.UserEnabledComponents[23] = true;
      if (!UserRights.GlobalUserRights.CheckRight(UserRights.Rights.SmokeDetectorHandler) || !this.IsComponentInstalled(GMM_Components.SmokeDetectorHandler))
        return;
      ZR_Component.UserEnabledComponents[27] = true;
    }

    public delegate void GarantComponentLoadedFunction(GMM_Components TheComponent);
  }
}
