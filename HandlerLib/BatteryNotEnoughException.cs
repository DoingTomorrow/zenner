// Decompiled with JetBrains decompiler
// Type: HandlerLib.BatteryNotEnoughException
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace HandlerLib
{
  public class BatteryNotEnoughException : Exception
  {
    public BatteryEnergyManagement BatteryManagement;

    public BatteryNotEnoughException(BatteryEnergyManagement batteryManagement)
    {
      double num = batteryManagement.RequiredBatteryYears;
      string str1 = num.ToString("0.00");
      num = batteryManagement.MissingBatteryMonth;
      string str2 = num.ToString("0.000");
      // ISSUE: explicit constructor call
      base.\u002Ector("Battery not enough! Required years:" + str1 + "; Missing month:" + str2);
      this.BatteryManagement = batteryManagement;
    }

    protected BatteryNotEnoughException(SerializationInfo info, StreamingContext context)
    {
    }
  }
}
