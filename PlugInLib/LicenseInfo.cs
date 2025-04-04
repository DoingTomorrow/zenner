// Decompiled with JetBrains decompiler
// Type: PlugInLib.LicenseInfo
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace PlugInLib
{
  [Serializable]
  public class LicenseInfo
  {
    public Guid Id;
    public string Name;
    public string HardwareKey;
    public string CurrentLicenseType;
    public string AboutText;
    public List<PluginLicenseInfo> Plugins;
    public List<RightInfo> Rights;
    public List<DeviceTypeInfo> DeviceTypes;
    public string Customer;
    public string ValidityPeriod;
    public int ServerConnectionInterval;
    public int ServerConnectionTolerance;
    public bool IsMinoConnectedNeeded;
    public bool DisplayEvaluationFactor;
    public bool IsServerLicense;
    public bool IsStandaloneClient;
    public bool IsClientLicense;
    public byte[] LogoImage;
    public int LicenseGeneratorID;
    [XmlIgnore]
    public DateTime LicenseGenerationDate;
    [XmlIgnore]
    public DateTime? ValidityStartDate;

    [XmlElement(ElementName = "LicenseGenerationDate")]
    public string XmlLicenseGenerationDate
    {
      get
      {
        return XmlConvert.ToString(this.LicenseGenerationDate, XmlDateTimeSerializationMode.Unspecified);
      }
      set => this.LicenseGenerationDate = DateTime.Parse(value);
    }

    [XmlElement(ElementName = "ValidityStartDate")]
    public string XmlValidityStartDate
    {
      get
      {
        return this.ValidityStartDate.HasValue ? XmlConvert.ToString(this.ValidityStartDate.Value, XmlDateTimeSerializationMode.Unspecified) : (string) null;
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          this.ValidityStartDate = new DateTime?();
        else
          this.ValidityStartDate = new DateTime?(DateTime.Parse(value));
      }
    }

    public LicenseInfo()
    {
      this.Id = Guid.NewGuid();
      this.Name = string.Empty;
      this.HardwareKey = string.Empty;
      this.CurrentLicenseType = string.Empty;
      this.AboutText = string.Empty;
      this.Plugins = new List<PluginLicenseInfo>();
      this.Rights = new List<RightInfo>();
      this.DeviceTypes = new List<DeviceTypeInfo>();
      this.Customer = string.Empty;
      this.ServerConnectionInterval = 0;
      this.ServerConnectionTolerance = 0;
      this.IsMinoConnectedNeeded = false;
      this.DisplayEvaluationFactor = false;
      this.IsServerLicense = false;
      this.IsClientLicense = false;
      this.IsStandaloneClient = false;
      this.LogoImage = (byte[]) null;
      this.LicenseGenerationDate = DateTime.MinValue;
      this.LicenseGeneratorID = HardwareKeyGenerator.GetHardwareUniqueKey().GetHashCode();
      this.ValidityStartDate = new DateTime?();
      this.ValidityPeriod = string.Empty;
    }

    public SortedList<string, RightInfo> GetPureEnabledRights()
    {
      SortedList<string, RightInfo> pureEnabledRights = new SortedList<string, RightInfo>();
      foreach (RightInfo right in this.Rights)
      {
        if (right.Enable)
        {
          string purePermissionName = LicenseInfo.GetPurePermissionName(right.Right);
          if (pureEnabledRights.ContainsKey(purePermissionName))
            throw new Exception("Illegal license file. Right not unique: " + purePermissionName);
          pureEnabledRights.Add(purePermissionName, right);
        }
      }
      foreach (PluginLicenseInfo plugin in this.Plugins)
      {
        if (plugin.Enable)
        {
          string purePermissionName = LicenseInfo.GetPurePermissionName(plugin.Plugin);
          RightInfo rightInfo = new RightInfo();
          rightInfo.Right = plugin.Plugin;
          rightInfo.Enable = plugin.Enable;
          if (pureEnabledRights.ContainsKey(purePermissionName))
            throw new Exception("Illegal license file. Plugin not unique or right with same name: " + purePermissionName);
          pureEnabledRights.Add(purePermissionName, rightInfo);
        }
      }
      return pureEnabledRights;
    }

    public SortedList<string, RightInfo> GetPureRights()
    {
      SortedList<string, RightInfo> pureRights = new SortedList<string, RightInfo>();
      foreach (RightInfo right in this.Rights)
      {
        string purePermissionName = LicenseInfo.GetPurePermissionName(right.Right);
        if (pureRights.ContainsKey(purePermissionName))
          throw new Exception("Illegal license file. Right not unique: " + purePermissionName);
        pureRights.Add(purePermissionName, right);
      }
      foreach (PluginLicenseInfo plugin in this.Plugins)
      {
        string purePermissionName = LicenseInfo.GetPurePermissionName(plugin.Plugin);
        RightInfo rightInfo = new RightInfo();
        rightInfo.Right = plugin.Plugin;
        rightInfo.Enable = plugin.Enable;
        if (pureRights.ContainsKey(purePermissionName))
          throw new Exception("Illegal license file. Plugin not unique or right with same name: " + purePermissionName);
        pureRights.Add(purePermissionName, rightInfo);
      }
      return pureRights;
    }

    public static string GetPurePermissionName(string fullPermissionName)
    {
      int num = fullPermissionName.LastIndexOf('\\');
      return num >= 0 ? fullPermissionName.Substring(num + 1) : fullPermissionName;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.GetHeaderInfo());
      stringBuilder.AppendLine("Plugins:");
      if (this.Plugins != null)
      {
        foreach (PluginLicenseInfo plugin in this.Plugins)
          stringBuilder.Append(plugin.Plugin).Append(" Enabled: ").AppendLine(plugin.Enable.ToString());
      }
      stringBuilder.AppendLine("Rights:");
      if (this.Rights != null)
      {
        foreach (RightInfo right in this.Rights)
          stringBuilder.Append(right.Right).Append(" Enabled: ").AppendLine(right.Enable.ToString());
      }
      stringBuilder.AppendLine("Device types:");
      if (this.DeviceTypes != null)
      {
        foreach (DeviceTypeInfo deviceType in this.DeviceTypes)
          stringBuilder.Append(deviceType.DeviceType).Append(" Enabled: ").AppendLine(deviceType.Enable.ToString());
      }
      return stringBuilder.ToString();
    }

    public string GetHeaderInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("License Info:");
      if (string.IsNullOrEmpty(this.Name))
        stringBuilder.Append("Name: ").AppendLine(this.Name);
      if (string.IsNullOrEmpty(this.HardwareKey))
        stringBuilder.Append("HardwareKey: ").AppendLine(this.HardwareKey);
      if (string.IsNullOrEmpty(this.CurrentLicenseType))
        stringBuilder.Append("CurrentLicenseType: ").AppendLine(this.CurrentLicenseType);
      if (string.IsNullOrEmpty(this.AboutText))
        stringBuilder.Append("AboutText: ").AppendLine(this.AboutText);
      if (string.IsNullOrEmpty(this.Customer))
        stringBuilder.Append("Customer: ").AppendLine(this.Customer);
      if (string.IsNullOrEmpty(this.ValidityPeriod))
        stringBuilder.Append("ValidityPeriod: ").AppendLine(this.ValidityPeriod);
      stringBuilder.AppendLine("License generation time: " + LicenseManager.CurrentLicense.LicenseGenerationDate.ToShortDateString() + " " + LicenseManager.CurrentLicense.LicenseGenerationDate.ToShortTimeString());
      stringBuilder.AppendLine("License generator ID: " + (object) LicenseManager.CurrentLicense.LicenseGeneratorID);
      return stringBuilder.ToString();
    }
  }
}
