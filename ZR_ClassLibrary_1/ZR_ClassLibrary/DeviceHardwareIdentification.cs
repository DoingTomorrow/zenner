// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DeviceHardwareIdentification
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public class DeviceHardwareIdentification
  {
    public DHI_VolumeMeterType VolumeMeterType = DHI_VolumeMeterType.NONE;
    public DHI_VolumeMeterBounding VolumeMeterBounding = DHI_VolumeMeterBounding.NONE;
    public List<DHI_CommunicationInterface> CommunicationInterfaces = new List<DHI_CommunicationInterface>();
    public DHI_TempSensorType TempSensorType = DHI_TempSensorType.NONE;
    public DHI_DeviceType DeviceType = DHI_DeviceType.NONE;
  }
}
