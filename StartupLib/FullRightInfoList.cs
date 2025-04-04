// Decompiled with JetBrains decompiler
// Type: StartupLib.FullRightInfoList
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using PlugInLib;
using System;
using System.Collections.Generic;

#nullable disable
namespace StartupLib
{
  public class FullRightInfoList
  {
    private bool isEditUserList;

    public List<FullRightInfo> RightsList { get; private set; }

    public UserInfo CurrentUser { get; private set; }

    public bool UserIsDeveloper { get; private set; }

    public bool UserHasDeveloperOption { get; private set; }

    public bool UseOnlyLicenseRights { get; set; }

    public FullRightInfoList() => this.RightsList = new List<FullRightInfo>();

    public List<FullRightInfo> AddLicenseRights()
    {
      foreach (RightInfo right in LicenseManager.CurrentLicense.Rights)
        this.RightsList.Add(new FullRightInfo(right));
      bool flag1 = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.Right == "Setup\\EnableAllConfigurationParameters" && item.IsLicenseEnabled)) >= 0;
      bool flag2 = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.Right == "Setup\\EnableAllTypeModels" && item.IsLicenseEnabled)) >= 0;
      if (this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.Right == "Setup\\DisableAllPlugins" && item.IsLicenseEnabled)) < 0 && LicenseManager.CurrentLicense.Plugins != null)
      {
        foreach (PluginLicenseInfo plugin in LicenseManager.CurrentLicense.Plugins)
          this.RightsList.Add(new FullRightInfo(plugin));
      }
      if (!flag2 && LicenseManager.CurrentLicense.DeviceTypes != null)
      {
        foreach (DeviceTypeInfo deviceType in LicenseManager.CurrentLicense.DeviceTypes)
          this.RightsList.Add(new FullRightInfo(deviceType));
      }
      this.UseOnlyLicenseRights = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.rightName == "UsePureLicenseRights" && item.Enabled)) >= 0;
      return this.RightsList;
    }

    public void AddScannedRights()
    {
      if (PlugInLoader.availableLibraries.LibraryRights == null)
        return;
      foreach (FullRightInfo libraryRight in PlugInLoader.availableLibraries.LibraryRights)
      {
        FullRightInfo fullRightInfo = PlugInManager.AddSpecialRight(libraryRight.Right, this.RightsList);
        fullRightInfo.IsScannedRight = true;
        fullRightInfo.IsDefaultEnabled = libraryRight.DefaultValue;
      }
    }

    public void AddCurrentUserRights(int UserId)
    {
      this.CurrentUser = UserManager.GetUser(UserId);
      foreach (PermissionInfo permission in this.CurrentUser.Permissions)
      {
        if (permission.PermissionName == "EnableExtendedRights")
          permission.PermissionName = "DbRight\\EnableExtendedRights";
        else if (permission.PermissionName.EndsWith("Developer"))
          permission.PermissionName = "Role\\Developer";
        FullRightInfo fullRightInfo = PlugInManager.AddSpecialRight(permission.PermissionName, this.RightsList);
        fullRightInfo.IsCurrentUserRight = permission.PermissionValue;
        fullRightInfo.IsUserEnabled = permission.PermissionValue;
      }
    }

    public void AddEditUserRights(UserInfo activeUserInfo)
    {
      this.isEditUserList = true;
      if (activeUserInfo == null)
        return;
      foreach (PermissionInfo permission in activeUserInfo.Permissions)
      {
        FullRightInfo fullRightInfo = PlugInManager.AddSpecialRight(permission.PermissionName, this.RightsList);
        fullRightInfo.IsEditUserRight = true;
        fullRightInfo.IsEditUserEnabled = permission.PermissionValue;
      }
    }

    public void FinalWorkAndSort()
    {
      bool flag1 = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.Right == "Setup\\EnableAllConfigurationParameters" && item.Enabled)) >= 0;
      bool flag2 = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.Right == "Setup\\EnableAllTypeModels" && item.Enabled)) >= 0;
      bool flag3 = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.Right == "Setup\\DisableAllPlugins" && item.Enabled)) >= 0;
      if (flag1)
        this.RightsList.RemoveAll((Predicate<FullRightInfo>) (item => item.Right.StartsWith("Right\\Configurator\\")));
      if (flag2)
        this.RightsList.RemoveAll((Predicate<FullRightInfo>) (item => item.Right.StartsWith("ModelType")));
      if (flag3)
        this.RightsList.RemoveAll((Predicate<FullRightInfo>) (item => item.Right.StartsWith("Plugin")));
      FullRightInfo fullRightInfo = this.RightsList.Find((Predicate<FullRightInfo>) (item => item.rightName == "EnableExtendedRights"));
      if (fullRightInfo == null)
        this.RightsList.Add(new FullRightInfo()
        {
          Right = "DbRight\\EnableExtendedRights"
        });
      else
        fullRightInfo.Right = "DbRight\\EnableExtendedRights";
      foreach (FullRightInfo rights in this.RightsList)
      {
        if (rights.rightPath == "NoPath")
          rights.Right = "Right\\" + rights.rightName;
      }
      this.RightsList.Sort((IComparer<FullRightInfo>) new FullRightInfo());
    }

    public void SetEnabledRights()
    {
      bool flag1 = this.RightsList.FindIndex((Predicate<FullRightInfo>) (item => item.rightName == "EnableExtendedRights" && item.IsUserEnabled)) >= 0;
      this.UserIsDeveloper = false;
      this.UserHasDeveloperOption = false;
      FullRightInfo fullRightInfo = this.RightsList.Find((Predicate<FullRightInfo>) (item => item.rightName == "Developer"));
      if (fullRightInfo != null)
      {
        this.UserIsDeveloper = fullRightInfo.IsLicenseEnabled || flag1 && fullRightInfo.IsUserEnabled || !flag1 && fullRightInfo.IsUserEnabled && UserManager.DeveloperChosenOnStart;
        this.UserHasDeveloperOption = !this.UserIsDeveloper && fullRightInfo.IsUserEnabled && !flag1;
      }
      foreach (FullRightInfo rights in this.RightsList)
      {
        bool flag2 = !(rights.rightPathParts[0] == "Setup") ? (!(rights.Right == "Right\\ReadOnly") ? (!this.UseOnlyLicenseRights ? this.UserIsDeveloper || (!flag1 ? rights.IsUserEnabled && rights.IsLicenseEnabled : rights.IsUserEnabled) : rights.IsLicenseEnabled || this.UserIsDeveloper) : (rights.IsLicenseEnabled || rights.IsUserEnabled) && !this.UserIsDeveloper) : rights.IsLicenseEnabled;
        rights.Enabled = !this.isEditUserList ? flag2 : rights.IsEditUserEnabled;
      }
    }

    public bool IsEditByUserEnabled(FullRightInfo userRightInfo)
    {
      return userRightInfo.IsUserEnabled || UserManager.CheckPermission("Developer") || this.UserHasDeveloperOption && UserManager.DeveloperChosenOnStart;
    }
  }
}
