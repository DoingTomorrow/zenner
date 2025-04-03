// Decompiled with JetBrains decompiler
// Type: DeviceCollector.DeviceInfo
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class DeviceInfo
  {
    public DeviceTypes DeviceType = DeviceTypes.None;
    public bool ParameterOk;
    public DateTime LastReadingDate = DateTime.MinValue;
    public string MeterNumber;
    public uint MeterNumberOriginal;
    public string Manufacturer;
    public short ManufacturerCode;
    public byte Version;
    public byte Medium;
    public byte AccessNb;
    public byte Status;
    public int Signature;
    public List<DeviceInfo.MBusParamStruct> ParameterList;
    public DataTable ParameterTable;
    public List<DeviceInfo> SubDevices;
    public byte C_Field;
    public byte A_Field;
    public byte CI_Field;

    public string MediumString => MBusDevice.GetMediaString(this.Medium);

    public List<string> ParameterListWithoutValues { get; set; }

    public DeviceInfo()
    {
      this.ParameterOk = false;
      this.MeterNumber = string.Empty;
      this.Manufacturer = "???";
      this.ManufacturerCode = (short) 0;
      this.Version = (byte) 0;
      this.Medium = (byte) 0;
      this.AccessNb = (byte) 0;
      this.Status = (byte) 0;
      this.Signature = 0;
      this.ParameterList = new List<DeviceInfo.MBusParamStruct>();
      this.SubDevices = new List<DeviceInfo>();
      this.ParameterListWithoutValues = new List<string>();
    }

    public DeviceInfo(DeviceInfo InfoToCopy)
    {
      if (InfoToCopy == null)
        return;
      this.ParameterOk = InfoToCopy.ParameterOk;
      this.MeterNumber = InfoToCopy.MeterNumber;
      this.Manufacturer = InfoToCopy.Manufacturer;
      this.Version = InfoToCopy.Version;
      this.Medium = InfoToCopy.Medium;
      this.AccessNb = InfoToCopy.AccessNb;
      this.Status = InfoToCopy.Status;
      this.Signature = InfoToCopy.Signature;
      this.C_Field = InfoToCopy.C_Field;
      this.A_Field = InfoToCopy.A_Field;
      this.CI_Field = InfoToCopy.CI_Field;
      this.DeviceType = InfoToCopy.DeviceType;
      this.LastReadingDate = InfoToCopy.LastReadingDate;
      this.ManufacturerCode = InfoToCopy.ManufacturerCode;
      this.MeterNumberOriginal = InfoToCopy.MeterNumberOriginal;
      this.ParameterList = new List<DeviceInfo.MBusParamStruct>();
      for (int index = 0; index < InfoToCopy.ParameterList.Count; ++index)
        this.ParameterList.Add(InfoToCopy.ParameterList[index]);
      this.SubDevices = new List<DeviceInfo>();
      if (InfoToCopy.SubDevices.Count > 0)
        this.SubDevices.AddRange((IEnumerable<DeviceInfo>) InfoToCopy.SubDevices);
      this.ParameterListWithoutValues = new List<string>();
      if (InfoToCopy.ParameterListWithoutValues.Count <= 0)
        return;
      this.ParameterListWithoutValues.AddRange((IEnumerable<string>) InfoToCopy.ParameterListWithoutValues);
    }

    public void GenerateParameterTable()
    {
      this.ParameterTable = new DataTable("Parameter");
      this.ParameterTable.Columns.Add("Unit", typeof (string));
      this.ParameterTable.Columns.Add("Value", typeof (string));
      for (int index = 0; index < this.ParameterList.Count; ++index)
      {
        DataRow row = this.ParameterTable.NewRow();
        row["Unit"] = (object) this.ParameterList[index].DefineString;
        row["Value"] = (object) this.ParameterList[index].ValueString;
        this.ParameterTable.Rows.Add(row);
      }
    }

    public string GetParameter(string ParameterName)
    {
      for (int index = 0; index < this.ParameterList.Count; ++index)
      {
        if (this.ParameterList[index].DefineString == ParameterName)
          return this.ParameterList[index].ValueString;
      }
      return string.Empty;
    }

    public bool SetParameter(string ParameterName, string ParameterValue)
    {
      for (int index = 0; index < this.ParameterList.Count; ++index)
      {
        if (this.ParameterList[index].DefineString == ParameterName)
        {
          this.ParameterList[index].ValueString = ParameterValue;
          return true;
        }
      }
      return false;
    }

    public bool EraseParameter(string ParameterName)
    {
      for (int index = 0; index < this.ParameterList.Count; ++index)
      {
        if (this.ParameterList[index].DefineString == ParameterName)
        {
          this.ParameterList.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public string GetZDFParameterString()
    {
      StringBuilder stringBuilder = new StringBuilder(1000);
      if (this.ParameterList.Count > 0)
      {
        for (int index = 0; index < this.ParameterList.Count; ++index)
        {
          if (index != 0)
            stringBuilder.Append(";");
          stringBuilder.Append(this.ParameterList[index].DefineString);
          stringBuilder.Append(";");
          stringBuilder.Append(this.ParameterList[index].ValueString);
        }
      }
      else
        stringBuilder.Append("NoParameter");
      return stringBuilder.ToString();
    }

    public sealed class MBusParamStruct
    {
      public string DefineString;
      public string ValueString;

      public MBusParamStruct(string DefStr, string ValStr)
      {
        this.DefineString = DefStr;
        this.ValueString = ValStr;
      }

      public override string ToString()
      {
        return string.IsNullOrEmpty(this.DefineString) || this.ValueString == null ? base.ToString() : this.DefineString + " " + this.ValueString;
      }
    }
  }
}
