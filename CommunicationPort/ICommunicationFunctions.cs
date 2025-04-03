// Decompiled with JetBrains decompiler
// Type: CommunicationPort.ICommunicationFunctions
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using ZENNER.CommonLibrary;

#nullable disable
namespace CommunicationPort
{
  public interface ICommunicationFunctions
  {
    void RaiseMessageEvent(string theMessage);

    void RaiseAliveEvent(int aliveCounter);

    void RaiseBatteryLowEvent();

    void RaiseKeyEvent();

    ConfigList GetReadoutConfiguration();

    string TransceiverDeviceInfo { get; set; }
  }
}
