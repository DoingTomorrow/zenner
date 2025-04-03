// Decompiled with JetBrains decompiler
// Type: ZENNER.DeviceMode
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System;

#nullable disable
namespace ZENNER
{
  [Serializable]
  public enum DeviceMode : byte
  {
    OperationMode = 0,
    DeliveryMode = 1,
    TemperatureCalibrationMode = 3,
    VolumeCalibrationMode = 4,
    DeliveryMode8 = 8,
    DeliveryMode9 = 9,
  }
}
