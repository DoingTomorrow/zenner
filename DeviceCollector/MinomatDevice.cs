// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MinomatDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MinomatDevice : BusDevice
  {
    private string LastErrorString = "";
    internal SortedList<DateTime, List<Minomat.ProcessedData>> readoutValues = new SortedList<DateTime, List<Minomat.ProcessedData>>();
    internal SortedList<OverrideID, ConfigurationParameter> configValues;
    public byte PrimaryDeviceAddress;
    public bool PrimaryAddressOk;
    public bool PrimaryAddressKnown;
    public uint Serialnumber;

    public MinomatDevice(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.MinomatDevice;
      this.Info.Manufacturer = "MINOL";
      this.Info.ManufacturerCode = (short) 3570;
      this.Info.ParameterOk = true;
    }

    public MinomatDevice(MBusDevice TheMBusDevice)
      : base(TheMBusDevice.MyBus)
    {
      this.Info = TheMBusDevice.Info;
      this.Info.Manufacturer = "MINOL";
      this.Info.ManufacturerCode = (short) 3570;
      this.Info.Medium = (byte) 7;
      this.Info.LastReadingDate = SystemValues.DateTimeNow;
      this.LastErrorString = string.Empty;
      this.Info.ParameterOk = true;
      this.DeviceType = DeviceTypes.MinomatDevice;
    }

    internal bool ReadParameters()
    {
      this.Info.ParameterList.Clear();
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", this.Info.LastReadingDate.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern)));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.Info.MeterNumber));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", this.Info.Manufacturer));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", this.Info.Version.ToString()));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RADR", this.PrimaryDeviceAddress.ToString()));
      ((MinomatList) this.MyBus.MyDeviceList).ReadMinomat();
      if (this.configValues == null)
        return true;
      for (int index = 0; index < this.configValues.Count; ++index)
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct(this.configValues.Keys[index].ToString(), this.configValues.Values[index].ParameterValue.ToString()));
      SortedList<DateTime, int> sortedList = new SortedList<DateTime, int>();
      int num1 = 0;
      if (this.readoutValues == null)
        return true;
      for (int index1 = 0; index1 < this.readoutValues.Count; ++index1)
      {
        List<Minomat.ProcessedData> valuesIn = this.readoutValues.Values[index1];
        List<Minomat.ProcessedData> valuesOut;
        if (this.GetParameterFromList(ref valuesIn, "ewWert", out valuesOut))
        {
          if (!sortedList.ContainsKey(this.readoutValues.Keys[index1]))
            sortedList.Add(this.readoutValues.Keys[index1], num1++);
          int num2 = sortedList[this.readoutValues.Keys[index1]];
          for (int index2 = 0; index2 < valuesOut.Count; ++index2)
          {
            switch (valuesOut[index2].DataType)
            {
              case Minomat.DataType.EventData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_ST[" + num2.ToString() + "]", valuesOut[index2].ParameterValue));
                break;
              case Minomat.DataType.MonthlyData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_MO[" + num2.ToString() + "]", valuesOut[index2].ParameterValue));
                break;
              case Minomat.DataType.HalfMonthlyData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_HMO[" + num2.ToString() + "]", valuesOut[index2].ParameterValue));
                break;
            }
          }
        }
        if (this.GetParameterFromList(ref valuesIn, "ewFieldForceSum", out valuesOut))
        {
          if (!sortedList.ContainsKey(this.readoutValues.Keys[index1]))
            sortedList.Add(this.readoutValues.Keys[index1], num1++);
          int num3 = sortedList[this.readoutValues.Keys[index1]];
          string parameterValue = valuesOut[0].ParameterValue;
          string strValue = string.Empty;
          if (this.GetParameterFromList(ref valuesIn, "ewNumberOfReceivedHKVEFrames", out valuesOut))
            strValue = valuesOut[0].ParameterValue;
          int num4 = 0;
          int num5 = 0;
          int num6 = 0;
          if (Util.TryParseToInt32(parameterValue, out num5) && Util.TryParseToInt32(strValue, out num6) && num6 > 0)
            num4 = num5 / num6;
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_RSSI[" + num3.ToString() + "]", num4.ToString()));
        }
        if (this.GetParameterFromList(ref valuesIn, "ewStatus", out valuesOut))
        {
          if (!sortedList.ContainsKey(this.readoutValues.Keys[index1]))
            sortedList.Add(this.readoutValues.Keys[index1], num1++);
          int num7 = sortedList[this.readoutValues.Keys[index1]];
          for (int index3 = 0; index3 < valuesOut.Count; ++index3)
          {
            switch (valuesOut[index3].DataType)
            {
              case Minomat.DataType.EventData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_ST_STATUS[" + num7.ToString() + "]", valuesOut[index3].ParameterValue));
                break;
              case Minomat.DataType.MonthlyData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_MO_STATUS[" + num7.ToString() + "]", valuesOut[index3].ParameterValue));
                break;
              case Minomat.DataType.HalfMonthlyData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_HMO_STATUS[" + num7.ToString() + "]", valuesOut[index3].ParameterValue));
                break;
            }
          }
        }
        if (this.GetParameterFromList(ref valuesIn, "ewStatusDetail", out valuesOut))
        {
          if (!sortedList.ContainsKey(this.readoutValues.Keys[index1]))
            sortedList.Add(this.readoutValues.Keys[index1], num1++);
          int num8 = sortedList[this.readoutValues.Keys[index1]];
          for (int index4 = 0; index4 < valuesOut.Count; ++index4)
          {
            switch (valuesOut[index4].DataType)
            {
              case Minomat.DataType.EventData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_ST_STATUSDETAIL[" + num8.ToString() + "]", valuesOut[index4].ParameterValue));
                break;
              case Minomat.DataType.MonthlyData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_MO_STATUSDETAIL[" + num8.ToString() + "]", valuesOut[index4].ParameterValue));
                break;
              case Minomat.DataType.HalfMonthlyData:
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("M_HMO_STATUSDETAIL[" + num8.ToString() + "]", valuesOut[index4].ParameterValue));
                break;
            }
          }
        }
      }
      for (int index = 0; index < sortedList.Count; ++index)
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("TIMP[" + sortedList.Values[index].ToString() + "]", sortedList.Keys[index].ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern)));
      return true;
    }

    private bool GetParameterFromList(
      ref List<Minomat.ProcessedData> valuesIn,
      string name,
      out List<Minomat.ProcessedData> valuesOut)
    {
      valuesOut = new List<Minomat.ProcessedData>();
      for (int index = 0; index < valuesIn.Count; ++index)
      {
        if (valuesIn[index].ParameterName == name)
          valuesOut.Add(valuesIn[index]);
      }
      return valuesOut.Count > 0;
    }

    internal bool GetDeviceConfiguration(
      out SortedList<OverrideID, ConfigurationParameter> ConfigParamList)
    {
      ConfigParamList = (SortedList<OverrideID, ConfigurationParameter>) null;
      if (this.configValues == null)
        return false;
      ConfigParamList = this.configValues;
      return true;
    }
  }
}
