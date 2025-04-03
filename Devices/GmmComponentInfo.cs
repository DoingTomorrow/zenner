// Decompiled with JetBrains decompiler
// Type: Devices.GmmComponentInfo
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using System;

#nullable disable
namespace Devices
{
  public class GmmComponentInfo
  {
    public string HandlerName { get; internal set; }

    public string[] SupportedDevices { get; internal set; }

    public CommunicationModels CommunicationModel { get; internal set; }

    public Type WrapperType { get; internal set; }

    internal GmmComponentInfo(
      string handlerName,
      string[] supportedDevices,
      CommunicationModels communicationModel,
      Type wrapperType)
    {
      this.HandlerName = handlerName;
      this.SupportedDevices = supportedDevices;
      this.CommunicationModel = communicationModel;
      this.WrapperType = wrapperType;
    }
  }
}
