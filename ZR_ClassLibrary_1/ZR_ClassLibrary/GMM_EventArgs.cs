// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMM_EventArgs
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.ComponentModel;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class GMM_EventArgs : CancelEventArgs
  {
    public string EventMessage;
    public string InfoText;
    public int InfoNumber;
    public GMM_EventArgs.MessageType TheMessageType;
    public GMM_EventArgs.MessageLevel TheMessageLevel = GMM_EventArgs.MessageLevel.standard;
    public DeviceListStatus deviceListStatus = new DeviceListStatus();

    public int ProgressPercentage { get; set; }

    public GMM_EventArgs(string message)
    {
      this.Cancel = false;
      this.EventMessage = message;
      this.TheMessageType = GMM_EventArgs.MessageType.SimpleMessage;
    }

    public GMM_EventArgs(GMM_EventArgs.MessageType TheMessageType)
    {
      this.Cancel = false;
      this.EventMessage = string.Empty;
      this.InfoNumber = 0;
      this.TheMessageType = TheMessageType;
    }

    public GMM_EventArgs(string EventMessage, int InfoNumber)
    {
      this.Cancel = false;
      this.EventMessage = EventMessage;
      this.InfoNumber = InfoNumber;
      this.TheMessageType = GMM_EventArgs.MessageType.StandardMessage;
    }

    public GMM_EventArgs(string EventMessage, GMM_EventArgs.MessageType TheMessageType)
    {
      this.Cancel = false;
      this.EventMessage = EventMessage;
      this.TheMessageType = TheMessageType;
    }

    public GMM_EventArgs(
      string EventMessage,
      int InfoNumber,
      GMM_EventArgs.MessageType TheMessageType)
    {
      this.Cancel = false;
      this.EventMessage = EventMessage;
      this.InfoNumber = InfoNumber;
      this.TheMessageType = TheMessageType;
    }

    public GMM_EventArgs(
      string EventMessage,
      string EventInfo,
      int InfoNumber,
      GMM_EventArgs.MessageType TheMessageType)
    {
      this.Cancel = false;
      this.EventMessage = EventMessage;
      this.InfoText = EventInfo;
      this.InfoNumber = InfoNumber;
      this.TheMessageType = TheMessageType;
    }

    public GMM_EventArgs(
      string EventMessage,
      int InfoNumber,
      GMM_EventArgs.MessageType TheMessageType,
      GMM_EventArgs.MessageLevel TheMessageLevel)
    {
      this.Cancel = false;
      this.EventMessage = EventMessage;
      this.InfoNumber = InfoNumber;
      this.TheMessageType = TheMessageType;
      this.TheMessageLevel = TheMessageLevel;
    }

    public enum MessageType
    {
      StandardMessage,
      Alive,
      PrimaryAddressMessage,
      ScanAddressMessage,
      EndMessage,
      MinomatErrorMessage,
      MinomatAliveMessage,
      MinomatConnectingMessage,
      KeyReceived,
      StatusError,
      StatusThreadStopped,
      StatusChanged,
      MinoConnectPlugMessage,
      Overload,
      BatteryLow,
      MessageAndProgressPercentage,
      SimpleMessage,
      Wait,
      WalkByPacketReceived,
      TestStepDone,
      LanguageTranslation,
      C5_FirmwareUpdate,
    }

    public enum ConnectionEventParameter
    {
      NotConnected,
      Connected,
    }

    public enum MessageLevel
    {
      standard,
      debug,
      info,
    }
  }
}
