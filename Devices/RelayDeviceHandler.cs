// Decompiled with JetBrains decompiler
// Type: Devices.RelayDeviceHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  internal sealed class RelayDeviceHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    public override object GetHandler() => (object) this.MyDeviceManager.MyBus;

    public override bool SelectDevice(GlobalDeviceId device)
    {
      return this.MyDeviceManager.MyBus.SetSelectedDeviceBySerialNumber(device.Serialnumber);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      int subDeviceIndex)
    {
      List<DeviceInfo> parameters = this.MyDeviceManager.MyBus.GetParameters();
      if (parameters == null || parameters.Count <= 0)
        return false;
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyDeviceManager.MyBus.GetDeviceCollectorSettings();
      DateTime dateTime1 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.FromTime]);
      DateTime dateTime2 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.ToTime]);
      StringBuilder stringBuilder = new StringBuilder();
      int count = parameters.Count;
      int num1 = 1;
      foreach (DeviceInfo deviceInfo in parameters)
      {
        int num2 = num1 * 100 / count;
        GMM_EventArgs eventMessage = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
        eventMessage.EventMessage = "Progress encode" + num2.ToString() + " %";
        eventMessage.ProgressPercentage = num2;
        this.MyDeviceManager.RaiseEvent(eventMessage);
        if (eventMessage.Cancel)
          return false;
        ++num1;
        if (Util.IsValidTimePoint(deviceInfo.LastReadingDate, dateTime1, dateTime2, true))
        {
          string zdfParameterString = deviceInfo.GetZDFParameterString();
          if (!TranslationRulesManager.Instance.TryParse(zdfParameterString, subDeviceIndex, ref valueList))
            stringBuilder.Append(zdfParameterString + ZR_Constants.SystemNewLine);
        }
      }
      if (stringBuilder.Length > 0)
        ZR_ClassLibMessages.AddWarning("An unknown device was found!", stringBuilder.ToString());
      this.MyDeviceManager.RaiseEvent(new GMM_EventArgs(GMM_EventArgs.MessageType.SimpleMessage));
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      return this.GetValues(ref valueList, 0);
    }

    public override bool ReadAll(List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      return this.MyDeviceManager.MyBus.ScanFromAddress(0);
    }
  }
}
