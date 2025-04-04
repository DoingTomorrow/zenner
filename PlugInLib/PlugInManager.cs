// Decompiled with JetBrains decompiler
// Type: PlugInLib.PlugInManager
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace PlugInLib
{
  public class PlugInManager
  {
    private static Logger logger = LogManager.GetLogger(nameof (PlugInManager));
    private List<string> IgnorDlls;
    public SortedList<string, PlugInAssamblyInfo> AvailablePlugInList;
    public SortedList<string, string> ExceptionDLLs;
    public SortedList<string, int> NoPluginDLLs;
    public StringBuilder RightWarnings = new StringBuilder();

    public PlugInManager()
    {
      this.IgnorDlls = new List<string>();
      this.IgnorDlls.Add("MSP430FPA");
      this.IgnorDlls.Add("Aga.Controls");
      this.IgnorDlls.Add("PlugInLib");
      this.IgnorDlls.Add("AxInterop.MSForms");
      this.IgnorDlls.Add("Interop.MSForms");
      this.IgnorDlls.Add("Microsoft.Office.Interop.Excel");
      this.IgnorDlls.Add("Microsoft.Vbe.Interop");
      this.IgnorDlls.Add("Mono.Security");
      this.IgnorDlls.Add("Nitrosoft.WSN");
      this.IgnorDlls.Add("NLog");
      this.IgnorDlls.Add("Npgsql");
      this.IgnorDlls.Add("NS.Plugin.Wavenis");
      this.IgnorDlls.Add("NTFS");
      this.IgnorDlls.Add("office");
      this.IgnorDlls.Add("OpenNETCF.Desktop.Communication");
      this.IgnorDlls.Add("SQLite");
      this.IgnorDlls.Add("libad4");
      this.IgnorDlls.Add("System.Data.SQLite");
      this.IgnorDlls.Add("System.Windows.Forms.DataVisualization");
      this.IgnorDlls.Add("GMM_Interface");
      this.IgnorDlls.Add("Microsoft.ReportViewer.Common");
      this.IgnorDlls.Add("MSP430FPA1");
      this.IgnorDlls.Add("MSPFET430UIF1");
      this.IgnorDlls.Add("Telerik");
      this.IgnorDlls.Add("Devart");
      this.IgnorDlls.Add("ZR_ClassLibrary");
      this.IgnorDlls.Add("System");
      this.IgnorDlls.Add("Styles");
      this.IgnorDlls.Add("RestSharp");
      this.IgnorDlls.Add("Microsoft");
      this.IgnorDlls.Add("CommonLibrary");
      this.IgnorDlls.Add("GmmDbLib");
      this.IgnorDlls.Add("AutoMapper");
      this.IgnorDlls.Add("StartupLib");
      this.IgnorDlls.Add("NHibernate");
      this.IgnorDlls.Add("Interop");
      this.IgnorDlls.Add("ICSharpCode");
      this.IgnorDlls.Add("Excel");
      this.IgnorDlls.Add("Common.Library");
      this.IgnorDlls.Add("Devices");
      this.IgnorDlls.Add("GmmInterface");
      this.IgnorDlls.Add("HandlerLib");
      this.IgnorDlls.Add("sqlceme35.dll");
    }

    public SortedList<string, PlugInAssamblyInfo> GetPlugInList(string FolderName)
    {
      this.AvailablePlugInList = new SortedList<string, PlugInAssamblyInfo>();
      this.ExceptionDLLs = new SortedList<string, string>();
      this.NoPluginDLLs = new SortedList<string, int>();
      foreach (string file in Directory.GetFiles(FolderName, "*.dll"))
      {
        string dll = Path.GetFileName(file);
        if (!this.IgnorDlls.Exists((Predicate<string>) (item => dll.StartsWith(item))))
        {
          try
          {
            Assembly assembly = Assembly.LoadFrom(file);
            string str1 = assembly.FullName.Split(',')[0];
            Type[] types;
            try
            {
              types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
              types = ex.Types;
            }
            Type type = (Type) null;
            string str2 = (string) null;
            foreach (Type element in types)
            {
              try
              {
                PlugInManager.logger.Trace("Try load IGmmPlugIn from Type: " + element.ToString());
                if (element.GetInterface("ILicensePlugIn") != (Type) null)
                {
                  type = element;
                  foreach (Attribute customAttribute in Attribute.GetCustomAttributes((MemberInfo) element))
                  {
                    if (customAttribute is ComponentPathAttribute)
                    {
                      str2 = ((ComponentPathAttribute) customAttribute).ComponentPath;
                      PlugInManager.logger.Trace("Add PluginPath: " + str2);
                      break;
                    }
                  }
                  if (element.GetInterface("IGmmPlugIn") != (Type) null)
                  {
                    type = element;
                    break;
                  }
                  break;
                }
              }
              catch (Exception ex)
              {
                PlugInManager.logger.Trace("Exception:", ex.ToString());
              }
            }
            if (type != (Type) null && str2 != null)
            {
              PlugInAssamblyInfo plugInAssamblyInfo = new PlugInAssamblyInfo();
              plugInAssamblyInfo.FileName = file;
              plugInAssamblyInfo.GmmName = assembly.FullName.Split(',')[0];
              plugInAssamblyInfo.assambly = assembly;
              plugInAssamblyInfo.type = type;
              plugInAssamblyInfo.plugInPath = str2;
              this.AvailablePlugInList.Add(plugInAssamblyInfo.GmmName, plugInAssamblyInfo);
            }
            else
              this.NoPluginDLLs.Add(dll, 0);
          }
          catch (Exception ex)
          {
            this.ExceptionDLLs.Add(dll, ex.Message);
            PlugInManager.logger.ErrorException(ex.Message, ex);
          }
        }
      }
      return this.AvailablePlugInList;
    }

    public List<FullRightInfo> GetRightsList(out SortedList<string, string> openExceptions)
    {
      this.RightWarnings.Clear();
      if (this.AvailablePlugInList == null)
        throw new Exception("Required rights without scanned plugins");
      List<FullRightInfo> allRighs = new List<FullRightInfo>();
      openExceptions = new SortedList<string, string>();
      foreach (PlugInAssamblyInfo plugInAssamblyInfo in (IEnumerable<PlugInAssamblyInfo>) this.AvailablePlugInList.Values)
      {
        int length1 = this.RightWarnings.Length;
        try
        {
          ILicensePlugIn instance = (ILicensePlugIn) plugInAssamblyInfo.assambly.CreateInstance(plugInAssamblyInfo.type.ToString());
          foreach (string usedRight in instance.GetPluginInfo().UsedRights)
          {
            char[] chArray = new char[1]{ '|' };
            string[] strArray = usedRight.Split(chArray);
            if (strArray.Length != 0)
            {
              int length2 = strArray[0].LastIndexOf('\\');
              string rightPath;
              string str;
              if (length2 >= 0)
              {
                rightPath = strArray[0].Substring(0, length2);
                str = strArray[0].Substring(length2 + 1);
              }
              else
              {
                rightPath = !(instance is IGmmPlugIn) ? "MSS_Right" : "NoPath";
                str = strArray[0];
              }
              string rightName = str.Trim();
              if (rightName.Length != 0 && !(rightName == "Demo"))
              {
                FullRightInfo fullRightInfo = PlugInManager.AddSpecialRight(rightPath, rightName, allRighs);
                if (strArray.Length > 1)
                {
                  fullRightInfo.DefaultValue = bool.Parse(strArray[1]);
                  if (strArray.Length > 2)
                    fullRightInfo.rightDescription = strArray[2];
                }
                fullRightInfo.UseFromAssembly.Add(plugInAssamblyInfo.GmmName);
              }
            }
          }
          PlugInManager.AddSpecialRight("Plugin", plugInAssamblyInfo.GmmName, allRighs).UseFromAssembly.Add(plugInAssamblyInfo.GmmName);
        }
        catch (Exception ex)
        {
          openExceptions.Add(plugInAssamblyInfo.GmmName, ex.ToString());
        }
        if (length1 != this.RightWarnings.Length)
          this.RightWarnings.Insert(length1, "Plugin: " + plugInAssamblyInfo.GmmName + Environment.NewLine);
      }
      PlugInManager.AddSpecialRight("Role", "Developer", allRighs);
      PlugInManager.AddSpecialRight("Role", "Administrator", allRighs);
      PlugInManager.AddSpecialRight("Role", "UserManager", allRighs);
      PlugInManager.AddSpecialRight("Role", "ChiefOfEnergyTestCenter", allRighs);
      PlugInManager.AddSpecialRight("Role", "ChiefOfWaterTestCenter", allRighs);
      PlugInManager.AddSpecialRight("Right", "Autologin", allRighs);
      PlugInManager.AddSpecialRight("Setup", "UseUserLogin", allRighs);
      PlugInManager.AddSpecialRight("Setup", "UsePureLicenseRights", allRighs);
      PlugInManager.AddSpecialRight("Setup", "EnableUnknownComponents", allRighs);
      PlugInManager.AddSpecialRight("Setup", "EnableAllConfigurationParameters", allRighs);
      PlugInManager.AddSpecialRight("Setup", "EnableAllTypeModels", allRighs);
      PlugInManager.AddSpecialRight("Setup", "DisableAllPlugins", allRighs);
      string[] names = Enum.GetNames(typeof (OldRights));
      for (int index = 0; index < names.Length; ++index)
      {
        if (!(names[index] == "Demo"))
          PlugInManager.AddSpecialRight("CRight", names[index], allRighs);
      }
      foreach (FullRightInfo fullRightInfo in allRighs)
      {
        if (fullRightInfo.rightPath == "NoPath")
          fullRightInfo.Right = "Right\\" + fullRightInfo.rightName;
      }
      allRighs.Sort((IComparer<FullRightInfo>) new FullRightInfo());
      return allRighs;
    }

    public static FullRightInfo AddSpecialRight(string rightWithPath, List<FullRightInfo> allRighs)
    {
      int length = rightWithPath.LastIndexOf("\\");
      return length >= 0 ? PlugInManager.AddSpecialRight(rightWithPath.Substring(0, length), rightWithPath.Substring(length + 1), allRighs) : PlugInManager.AddSpecialRight("NoPath", rightWithPath, allRighs);
    }

    public static FullRightInfo AddSpecialRight(
      string rightPath,
      string rightName,
      List<FullRightInfo> allRighs)
    {
      string fullRight = rightPath + "\\" + rightName;
      FullRightInfo fullRightInfo1 = allRighs.Find((Predicate<FullRightInfo>) (item => item.Right == fullRight));
      if (fullRightInfo1 != null)
        return fullRightInfo1;
      List<FullRightInfo> all = allRighs.FindAll((Predicate<FullRightInfo>) (item => item.rightName == rightName));
      if (all != null)
      {
        foreach (FullRightInfo fullRightInfo2 in all)
        {
          int num;
          switch (rightPath)
          {
            case "CRight":
            case "NoPath":
              num = 0;
              break;
            case "Right":
              num = !(fullRightInfo2.rightPath == "Role") ? 1 : 0;
              break;
            default:
              num = 1;
              break;
          }
          if (num == 0)
            return fullRightInfo2;
          if (fullRightInfo2.rightPath == "CRight" || fullRightInfo2.rightPath == "NoPath" || rightPath == "Role" && fullRightInfo2.rightPath == "Right")
          {
            fullRightInfo2.Right = fullRight;
            return fullRightInfo2;
          }
        }
      }
      FullRightInfo fullRightInfo3 = new FullRightInfo();
      fullRightInfo3.Right = fullRight;
      allRighs.Add(fullRightInfo3);
      return fullRightInfo3;
    }
  }
}
