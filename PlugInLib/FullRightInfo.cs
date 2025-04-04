// Decompiled with JetBrains decompiler
// Type: PlugInLib.FullRightInfo
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace PlugInLib
{
  public class FullRightInfo : IComparer<FullRightInfo>
  {
    private string _right;
    public string[] rightPathParts;
    private bool _defaultValue;
    public List<string> UseFromAssembly = new List<string>();

    public string Right
    {
      get => this._right;
      set
      {
        this._right = value.Trim();
        this.rightPathParts = this._right.Split(new char[1]
        {
          '\\'
        }, StringSplitOptions.RemoveEmptyEntries);
      }
    }

    public string rightName => this.rightPathParts[this.rightPathParts.Length - 1];

    public string rightPath
    {
      get
      {
        return this.rightPathParts.Length > 1 ? this._right.Substring(0, this._right.Length - this.rightName.Length - 1) : "";
      }
    }

    public bool DefaultValue
    {
      set
      {
        this._defaultValue = value;
        this.Enabled = value;
      }
      get => this._defaultValue;
    }

    public bool Enabled { get; set; }

    public string rightDescription { get; set; }

    public bool IsLicenseEnabled { get; set; }

    public bool IsDefaultEnabled { get; set; }

    public bool IsUserEnabled { get; set; }

    public bool IsEditUserEnabled { get; set; }

    public bool IsLicenseRight { get; set; }

    public bool IsScannedRight { get; set; }

    public bool IsCurrentUserRight { get; set; }

    public bool IsEditUserRight { get; set; }

    public bool IsAdded { get; set; }

    public FullRightInfo()
    {
    }

    public FullRightInfo(RightInfo rightInfo)
    {
      this.Right = rightInfo.Right;
      if (this.rightPathParts.Length > 1 && this.rightPathParts[0] == "Setup")
        this.Enabled = rightInfo.Enable;
      this.IsLicenseRight = true;
      this.IsLicenseEnabled = rightInfo.Enable;
    }

    public FullRightInfo(PluginLicenseInfo pluginLicenseInfo)
    {
      if (pluginLicenseInfo.Plugin.Contains("\\"))
      {
        this.Right = pluginLicenseInfo.Plugin;
        this.Right = "Plugin\\" + this.rightName;
      }
      else
        this.Right = "Plugin\\" + pluginLicenseInfo.Plugin;
      this.IsLicenseRight = true;
      this.IsLicenseEnabled = pluginLicenseInfo.Enable;
    }

    public FullRightInfo(DeviceTypeInfo deviceTypeInfo)
    {
      if (deviceTypeInfo.DeviceType.Contains("\\"))
      {
        this.Right = deviceTypeInfo.DeviceType;
        this.Right = "DeviceModel\\" + this.rightName;
      }
      else
        this.Right = "DeviceModel\\" + deviceTypeInfo.DeviceType;
      this.IsLicenseRight = true;
      this.IsLicenseEnabled = deviceTypeInfo.Enable;
    }

    public FullRightInfo(PermissionInfo permInfo) => this.Right = permInfo.PermissionName;

    public FullRightInfo(FullRightInfo rightInfo) => this.Right = rightInfo.Right;

    public string GetExtentionForPath(string basePath)
    {
      if (basePath == null || !this._right.StartsWith(basePath) || this.rightPathParts.Length < 2)
        return (string) null;
      string str = this.rightPathParts[0];
      for (int index = 1; index < this.rightPathParts.Length - 1; ++index)
      {
        if (str == basePath)
          return this.rightPathParts[index];
        str = str + "\\" + this.rightPathParts[index];
      }
      return (string) null;
    }

    public string GetParentPath(string path)
    {
      int length = path.LastIndexOf('\\');
      return length < 0 ? "" : path.Substring(0, length);
    }

    public override string ToString() => this._right + " = " + this.Enabled.ToString();

    public int Compare(FullRightInfo x, FullRightInfo y) => x._right.CompareTo(y._right);
  }
}
