// Decompiled with JetBrains decompiler
// Type: Devices.GmmStructureInfo
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Devices
{
  public static class GmmStructureInfo
  {
    public static List<GmmComponentInfo> ComponentInfos { get; private set; }

    public static void Init(bool isPlugin)
    {
      if (GmmStructureInfo.ComponentInfos != null && GmmStructureInfo.ComponentInfos.Count > 0)
        return;
      GmmStructureInfo.ComponentInfos = new List<GmmComponentInfo>();
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("M8_Handler", new string[1]
      {
        "M8"
      }, CommunicationModels.CommunicationPort, typeof (CommonHandlerWrapper)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("EDCL_Handler", new string[1]
      {
        "EDCL"
      }, CommunicationModels.CommunicationPort, typeof (CommonHandlerWrapper)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("NFCL_Handler", new string[1]
      {
        "NFCL"
      }, CommunicationModels.CommunicationPort, typeof (CommonHandlerWrapper)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("PDCL2_Handler", new string[1]
      {
        "PDCLL"
      }, CommunicationModels.CommunicationPort, typeof (CommonHandlerWrapper)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("S4_Handler", new string[1]
      {
        "IUW"
      }, CommunicationModels.CommunicationPort, typeof (CommonHandlerWrapper)));
      if (isPlugin)
        GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("S3_Handler", new string[2]
        {
          "C5",
          "WR4"
        }, CommunicationModels.CommunicationPort, typeof (Series3DeviceByHandler)));
      else
        GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("S3_Handler", new string[2]
        {
          "C5",
          "WR4"
        }, CommunicationModels.DeviceCollector_AsyncCom, typeof (Series3DeviceByHandler)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("MinolHandler", new string[1]
      {
        "M7"
      }, CommunicationModels.DeviceCollector_AsyncCom, typeof (MinolDeviceHandler)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("GMM_Handler", new string[3]
      {
        "C2",
        "C3",
        "WR3"
      }, CommunicationModels.DeviceCollector_AsyncCom, typeof (Series2DeviceByHandler)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("EDC", new string[1]
      {
        "EDC"
      }, CommunicationModels.DeviceCollector_AsyncCom, typeof (EdcHandler)));
      GmmStructureInfo.ComponentInfos.Add(new GmmComponentInfo("THL_Handler", new string[1]
      {
        "THL"
      }, CommunicationModels.CommunicationPort, typeof (CommonHandlerWrapper)));
    }

    public static GmmComponentInfo GetComponentInfo(string handlerName)
    {
      GmmComponentInfo gmmComponentInfo = (GmmComponentInfo) null;
      if (handlerName != null)
        gmmComponentInfo = GmmStructureInfo.ComponentInfos.FirstOrDefault<GmmComponentInfo>((Func<GmmComponentInfo, bool>) (x => x.HandlerName == handlerName));
      return gmmComponentInfo ?? new GmmComponentInfo((string) null, (string[]) null, CommunicationModels.DeviceCollector_AsyncCom, (Type) null);
    }

    public static string GetMainDeviceNameFromHandlerName(string handlerName)
    {
      return (GmmStructureInfo.ComponentInfos.FirstOrDefault<GmmComponentInfo>((Func<GmmComponentInfo, bool>) (x => x.HandlerName == handlerName)) ?? throw new Exception("Handler name not supported: " + handlerName)).SupportedDevices[0];
    }

    public static string[] GetDeviceNamesFromHandlerName(string handlerName)
    {
      return (GmmStructureInfo.ComponentInfos.FirstOrDefault<GmmComponentInfo>((Func<GmmComponentInfo, bool>) (x => x.HandlerName == handlerName)) ?? throw new Exception("Handler name not supported: " + handlerName)).SupportedDevices;
    }
  }
}
