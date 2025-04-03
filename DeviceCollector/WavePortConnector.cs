// Decompiled with JetBrains decompiler
// Type: DeviceCollector.WavePortConnector
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using NS.Plugin.Wavenis;
using StartupLib;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class WavePortConnector
  {
    private static string InternalWavePortFirmwareVersionString;
    private static Logger logger = LogManager.GetLogger(nameof (WavePortConnector));
    private object MyWavePortObject;
    private object MyWaveFlowObject;
    internal bool PortIsOpen;
    private DeviceCollectorFunctions MyBus;

    internal WavePort MyWavePort
    {
      get => this.MyWavePortObject as WavePort;
      set => this.MyWavePortObject = (object) value;
    }

    internal WaveFlow MyWaveFlow
    {
      get => this.MyWaveFlowObject as WaveFlow;
      set => this.MyWaveFlowObject = (object) value;
    }

    public WavePortConnector(DeviceCollectorFunctions TheBus)
    {
      this.MyBus = TheBus;
      this.PortIsOpen = false;
    }

    public static bool GetWavePortVersion(
      out string WavePortFirmwareVersionString,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      WavePortFirmwareVersionString = WavePortConnector.InternalWavePortFirmwareVersionString;
      if (!(WavePortFirmwareVersionString == string.Empty))
        return true;
      Fehlerstring = "Firmware Version not available!" + ZR_Constants.SystemNewLine + "Please first read any parameter!";
      return false;
    }

    internal bool StartRequest(
      string Address,
      string[] Repeaters,
      string ByteString,
      out string Response,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      Response = string.Empty;
      if (!UserManager.CheckPermission(UserRights.Rights.Waveflow))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Waveflow!");
        return false;
      }
      if (ByteString.Length % 2 != 0)
      {
        Fehlerstring = "Falscher Bytestring!";
        return false;
      }
      int length = ByteString.Length / 2;
      byte[] numArray = new byte[length];
      for (int index = 1; index <= length; ++index)
      {
        int startIndex = 2 * index - 2;
        string s = ByteString.Substring(startIndex, 2);
        numArray[index - 1] = byte.Parse(s, NumberStyles.HexNumber);
      }
      CompletionEventArgs completionEventArgs = this.MyWavePort.Do(new WaveCardRequest()
      {
        Address = Address,
        Buffer = numArray,
        Repeaters = Repeaters
      });
      if (completionEventArgs.Succeeded)
        Response = completionEventArgs.Response;
      else
        WavePortConnector.logger.Debug(completionEventArgs.Error.ToString());
      return true;
    }

    internal bool OpenPort()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Waveflow))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Waveflow!");
        return false;
      }
      if (this.PortIsOpen)
        return true;
      if (this.MyWavePort == null)
        this.MyWavePort = new WavePort();
      try
      {
        short num1 = short.Parse(this.MyBus.MyCom.SingleParameter(CommParameter.Port, string.Empty).ToUpper().Replace("COM", string.Empty));
        this.MyWavePort.LicenseKey = "TCasDbHHKJkVEKrn89ao9w==";
        this.MyWavePort.ConnectionSettings = new int[5]
        {
          (int) num1,
          9600,
          8,
          0,
          1
        };
        if (!this.MyWavePort.Connect())
        {
          this.PortIsOpen = false;
          return false;
        }
        if (!this.MyWavePort.SetRadioTimeout((byte) 40))
        {
          this.PortIsOpen = false;
          return false;
        }
        byte num2 = 0;
        this.MyWavePort.GetSwitchModeStatus(ref num2);
        if (num2 == (byte) 0)
          this.MyWavePort.SetSwitchModeStatus((byte) 1);
        this.MyWaveFlow = this.MyWavePort.Specifics.WaveFlow;
        WaveCard.PhysicalMode physicalMode = (WaveCard.PhysicalMode) 0;
        Version version;
        this.MyWavePort.GetVersion(ref physicalMode, ref version);
        WavePortConnector.InternalWavePortFirmwareVersionString = version.ToString();
        this.PortIsOpen = true;
        return true;
      }
      catch (Exception ex)
      {
        WavePortConnector.logger.Error(ex.Message);
        return false;
      }
    }

    internal void ClosePort()
    {
      if (!this.PortIsOpen)
        return;
      ((Component) this.MyWavePort).Dispose();
      this.MyWavePort = (WavePort) null;
      this.PortIsOpen = false;
    }

    public string GetAppPath()
    {
      return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
    }
  }
}
