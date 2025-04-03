// Decompiled with JetBrains decompiler
// Type: HandlerLib.SENSUSConnector
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class SENSUSConnector
  {
    internal static Logger SENSUSConnector_Logger = LogManager.GetLogger("FirmwareUpdateToolSENSUS");
    internal ConfigList configList;
    internal CommunicationPortFunctions myPort;
    private readonly uint[] waitTimeArray = new uint[8]
    {
      200U,
      300U,
      500U,
      700U,
      1000U,
      1400U,
      1800U,
      2200U
    };
    public static byte[] CMD_READ_VOL_FORMAT = new byte[7]
    {
      (byte) 6,
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 61,
      (byte) 170
    };
    public static byte[] CMD_SET_FORMAT = new byte[7]
    {
      (byte) 6,
      (byte) 0,
      (byte) 8,
      (byte) 1,
      (byte) 1,
      (byte) 9,
      (byte) 101
    };
    public static byte[] CMD_GET_VERSION = new byte[7]
    {
      (byte) 85,
      (byte) 90,
      (byte) 165,
      (byte) 165,
      (byte) 90,
      (byte) 161,
      (byte) 117
    };
    public static byte[] CMD_GO = new byte[7]
    {
      (byte) 4,
      (byte) 90,
      (byte) 165,
      (byte) 165,
      (byte) 90,
      (byte) 194,
      (byte) 202
    };
    public static string CMD_GET_VERSION_STR = "555aa5a55aa175";
    public static string CMD_GO_STR = "045AA5A55AC2CA";
    public static string CMD_WRITE_MEMORY = "01#ADDRESS#000000#DATA##CRC16#";
    public static string CMD_READ_MEMORY = "02#ADDRESS##CRC16#";
    public static string CMD_READ_ADDRESS = "03#ADDRESS##CRC16#";
    public static byte ResponseACK = 229;
    public static byte ResponseNACK = 26;
    public static byte ResponseFORMAT = 6;
    public static int FW_ERR_NONE = 0;
    public static int FW_ERR_PARITY = 1;
    public static int FW_ERR_STOP = 2;
    public static int FW_ERR_CRC = 16;
    public static int FW_ERR_CMD = 32;
    public static int FW_ERR_ADR = 48;
    private string stringVersion;
    public byte[] Response;

    public SENSUSSTATE Status { get; private set; }

    public FirmwareVersion FwV { get; private set; }

    public string HV { get; private set; }

    public string UID { get; private set; }

    public SENSUSConnector()
      : this((CommunicationPortFunctions) null, string.Empty, (ConfigList) null)
    {
    }

    public SENSUSConnector(string strVersion)
      : this((CommunicationPortFunctions) null, strVersion, (ConfigList) null)
    {
    }

    public SENSUSConnector(ConfigList configList)
      : this((CommunicationPortFunctions) null, string.Empty, configList)
    {
    }

    public SENSUSConnector(CommunicationPortFunctions Port)
      : this(Port, string.Empty, (ConfigList) null)
    {
    }

    public SENSUSConnector(CommunicationPortFunctions Port, ConfigList configList)
      : this(Port, string.Empty, configList)
    {
    }

    public SENSUSConnector(
      CommunicationPortFunctions Port,
      string strVersion,
      ConfigList configList)
    {
      this.myPort = Port;
      this.stringVersion = strVersion;
      this.parseVersion();
      this.configList = configList;
    }

    public void SetConfigurationList(ConfigList configList) => this.configList = configList;

    public void SetPortFunctions(CommunicationPortFunctions Port) => this.myPort = Port;

    public void SetFirmwareVersion(string fwVersion)
    {
      this.stringVersion = fwVersion;
      this.parseVersion();
    }

    public bool isSensusConnectorReady() => this.myPort != null && this.configList != null;

    public bool isSensusConnectorOnline()
    {
      FirmwareVersion fwV;
      int num;
      if (this.FwV.Major > (byte) 0)
      {
        fwV = this.FwV;
        if (fwV.Minor >= (byte) 0)
        {
          num = 1;
          goto label_6;
        }
      }
      fwV = this.FwV;
      if (fwV.Major == (byte) 0)
      {
        fwV = this.FwV;
        num = fwV.Minor > (byte) 0 ? 1 : 0;
      }
      else
        num = 0;
label_6:
      return num != 0;
    }

    private void parseVersion()
    {
      if (string.IsNullOrEmpty(this.stringVersion))
        return;
      int startIndex1 = this.stringVersion.IndexOf("FV") + 2;
      int length1 = this.stringVersion.IndexOf(";", startIndex1) - startIndex1;
      string str = this.stringVersion.Substring(startIndex1, length1);
      if (!string.IsNullOrEmpty(str) && (str.ElementAt<char>(6) != '2' || str.ElementAt<char>(7) != '6'))
        str = str.Remove(6, 2) + "26";
      this.FwV = new FirmwareVersion(str);
      int startIndex2 = this.stringVersion.IndexOf("HV") + 2;
      int length2 = this.stringVersion.IndexOf(";", startIndex2) - startIndex2;
      this.HV = this.stringVersion.Substring(startIndex2, length2);
      int startIndex3 = this.stringVersion.IndexOf("UID") + 3;
      int length3 = this.stringVersion.Length - startIndex3;
      this.UID = this.stringVersion.Substring(startIndex3, length3);
    }

    public override string ToString()
    {
      return "" + "\rFW version: " + this.FwV.ToString() + "\rHV version: " + this.HV + "\rARM UID:    " + this.UID;
    }

    internal async Task InitMinoConnect(ProgressHandler progress, CancellationToken cancelToken)
    {
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - Init MinoConnect...");
      progress.Report(10, " -> Init MinoConnect...");
      CommunicationByMinoConnect MiCon = this.myPort.GetCommunicationByMinoConnect();
      MiCon.WriteCommand("#com off\r\n");
      MiCon.WriteCommand("#comcl\r\n");
      MiCon.WriteCommand("#broff\r\n");
      MiCon.WriteCommand("#ver\r\n");
      MiCon.WriteCommand("#com rs485 9600 8e1\r\n");
      MiCon.WriteCommand("#3von\r\n");
      MiCon.WriteCommand("#apo 0\r\n");
      MiCon.WriteCommand("#out 1\r\n");
      MiCon.WriteCommand("#fs\r\n");
      await Task.Delay(1);
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - Init MinoConnect - DONE.");
      progress.Report(100, " -> Init MinoConnect - DONE.");
      MiCon = (CommunicationByMinoConnect) null;
    }

    internal async Task WakeUpSensusModuleStartEnd(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort delay = 500)
    {
      CommunicationByMinoConnect MiCon = this.myPort.GetCommunicationByMinoConnect();
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - WakeUp Module");
      progress.Report(10, "Sensus - WakeUp Module");
      MiCon.WriteCommand("#485ton\r\n");
      MiCon.WriteCommand("#bron\r\n");
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus clock low");
      progress.Report(50, "Sensus clock low");
      await Task.Delay((int) delay);
      MiCon.WriteCommand("#485toff\r\n");
      MiCon.WriteCommand("#broff\r\n");
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus clock high");
      progress.Report(100, "Sensus clock high");
      MiCon = (CommunicationByMinoConnect) null;
    }

    internal async Task StartWakeUpSensusModule(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort delay = 1)
    {
      CommunicationByMinoConnect MiCon = this.myPort.GetCommunicationByMinoConnect();
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - WakeUp Module START");
      progress.Report(10, "Sensus - WakeUp Module START");
      MiCon.WriteCommand("#485ton\r\n");
      MiCon.WriteCommand("#bron\r\n");
      await Task.Delay((int) delay);
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus clock low");
      progress.Report(100, "Sensus clock low");
      MiCon = (CommunicationByMinoConnect) null;
    }

    internal async Task EndWakeUpSensusModule(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort delay = 1)
    {
      CommunicationByMinoConnect MiCon = this.myPort.GetCommunicationByMinoConnect();
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - WakeUp Module END");
      progress.Report(10, "Sensus - WakeUp Module END");
      MiCon.WriteCommand("#485toff\r\n");
      MiCon.WriteCommand("#broff\r\n");
      await Task.Delay((int) delay);
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus clock high");
      progress.Report(100, "Sensus clock high");
      MiCon = (CommunicationByMinoConnect) null;
    }

    public async Task<byte[]> ReadVolFormatSensusModule(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      ushort responseByteCount = 6;
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - read volume format ...");
      await this.StartWakeUpSensusModule(progress, cancelToken, (ushort) 500);
      await this.EndWakeUpSensusModule(progress, cancelToken, (ushort) 1500);
      this.myPort.Write(SENSUSConnector.CMD_GET_VERSION);
      await Task.Delay(500);
      this.myPort.DiscardInBuffer();
      this.myPort.Write(SENSUSConnector.CMD_READ_VOL_FORMAT);
      byte[] response = await this.handleResponseFromSensus(progress, cancelToken, responseByteCount);
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - read - DONE.");
      progress.Report(100, "read... DONE.");
      byte[] numArray = response;
      response = (byte[]) null;
      return numArray;
    }

    private async Task<byte[]> SetFormatToSensusConnector(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte[] command,
      ushort responseCount = 6,
      ushort delay = 200)
    {
      ushort responseByteCount = responseCount;
      progress.Report(10, "Set format to Sensus Connector...");
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - set format ...");
      progress.Report(20, "WakeUp sensus module ...");
      await this.StartWakeUpSensusModule(progress, cancelToken, (ushort) 500);
      await this.EndWakeUpSensusModule(progress, cancelToken, (ushort) 1500);
      progress.Report(30, "WakeUp done...");
      this.myPort.Write(SENSUSConnector.CMD_GET_VERSION);
      progress.Report(40, "write command to SENSUSConnector ...");
      await Task.Delay((int) delay);
      this.myPort.Write(command);
      progress.Report(60, "received response ... check");
      byte[] response = await this.handleResponseFromSensus(progress, cancelToken, responseByteCount);
      SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - set format - DONE.");
      progress.Report(100, "check ... DONE.");
      byte[] sensusConnector = response;
      response = (byte[]) null;
      return sensusConnector;
    }

    public async Task<byte[]> SetFormat(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte usFormat,
      byte usTrunc)
    {
      progress.Report("Sensus - set format ");
      Logger sensusConnectorLogger = SENSUSConnector.SENSUSConnector_Logger;
      SENSUSVOLFORMAT sensusvolformat = (SENSUSVOLFORMAT) usFormat;
      string message1 = "Sensus - set " + sensusvolformat.ToString() + " trunc " + ((SENSUSVOLFORMATTRUNCATE) usTrunc).ToString();
      sensusConnectorLogger.Trace(message1);
      SENSUSConnector.CMD_SET_FORMAT[2] = usFormat;
      SENSUSConnector.CMD_SET_FORMAT[3] = usTrunc;
      ushort crc16 = Util.CalculatesCRC16(SENSUSConnector.CMD_SET_FORMAT, 0, 5);
      SENSUSConnector.CMD_SET_FORMAT[5] = BitConverter.GetBytes(crc16)[0];
      SENSUSConnector.CMD_SET_FORMAT[6] = BitConverter.GetBytes(crc16)[1];
      byte[] response = await this.SetFormatToSensusConnector(progress, cancelToken, SENSUSConnector.CMD_SET_FORMAT);
      if (response != null)
      {
        if ((int) response[1] == (int) SENSUSConnector.CMD_SET_FORMAT[2] && (int) response[2] == (int) SENSUSConnector.CMD_SET_FORMAT[3])
        {
          SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - set Format - DONE.");
          ProgressHandler progressHandler = progress;
          string[] strArray = new string[5]
          {
            "Sensus - set ",
            null,
            null,
            null,
            null
          };
          sensusvolformat = (SENSUSVOLFORMAT) usFormat;
          strArray[1] = sensusvolformat.ToString();
          strArray[2] = " trunc ";
          strArray[3] = ((SENSUSVOLFORMATTRUNCATE) usTrunc).ToString();
          strArray[4] = " - DONE.";
          string message2 = string.Concat(strArray);
          progressHandler.Report(100, message2);
        }
        else
        {
          SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - set Format - ERROR. (" + Util.ByteArrayToHexString(response) + ")");
          progress.Report(0, "Sensus - set Format - ERROR.");
        }
      }
      else
      {
        SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus - set Format - ERROR. --- no response");
        progress.Report(0, "Sensus - set Format - ERROR. --- no response ");
      }
      byte[] numArray = response;
      response = (byte[]) null;
      return numArray;
    }

    private async Task<byte[]> handleResponseFromSensus(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort responseBytes = 53)
    {
      int maxRepeats = this.configList.MaxRequestRepeat;
      byte[] response = (byte[]) null;
      bool byteCntChanged = false;
      SENSUSConnector.SENSUSConnector_Logger.Trace("handle response from SENSUS connector asynchron.");
      for (int w = 0; w < maxRepeats; ++w)
      {
        progress.Report(65 + w * 5, "handling response ...");
        if (!cancelToken.IsCancellationRequested)
        {
          try
          {
            byte[] responseH = new byte[1];
            responseH = this.myPort.ReadHeader(1);
            if (this.myPort.BytesToRead > 0 && this.myPort.BytesToRead < (int) responseBytes)
            {
              responseBytes = (ushort) (this.myPort.BytesToRead - 1);
              byteCntChanged = true;
            }
            byte[] responseE = new byte[(int) responseBytes];
            if (responseBytes > (ushort) 0)
              responseE = this.myPort.ReadEnd((int) responseBytes);
            response = new byte[(int) responseBytes + 1];
            Buffer.BlockCopy((Array) responseH, 0, (Array) response, 0, 1);
            Buffer.BlockCopy((Array) responseE, 0, (Array) response, 1, responseE.Length);
            responseH = (byte[]) null;
            responseE = (byte[]) null;
          }
          catch (TimeoutException ex)
          {
            progress.Report(71, "timeout!!! ");
            if (response != null)
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout but received: " + Util.ByteArrayToHexString(response));
            else
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout without data");
            response = (byte[]) null;
          }
          if (response == null)
          {
            progress.Report(71, "no response");
            SENSUSConnector.SENSUSConnector_Logger.Trace("no response");
            await Task.Delay(this.configList.WaitBeforeRepeatTime);
          }
          else
            break;
        }
        else
          break;
      }
      if (response != null)
      {
        if (response.Length != (int) responseBytes && !byteCntChanged)
        {
          string msg = "Internal error. ReadHeader management problem";
          progress.Report(msg);
          throw new Exception(msg);
        }
        string strResponse = Util.ByteArrayToHexString(response);
        progress.Report(80, "received: " + strResponse);
        SENSUSConnector.SENSUSConnector_Logger.Trace("received: " + strResponse);
        return response;
      }
      string msg1 = "no response, could not connect to SENSUSConnector!!!";
      progress.Report(90, msg1);
      SENSUSConnector.SENSUSConnector_Logger.Trace(msg1);
      msg1 = (string) null;
      return response;
    }

    public async Task<byte[]> readDataFromSensusAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      int lenHeader,
      int lenData,
      string command,
      bool getDataOnly = false)
    {
      byte[] cmd = Util.HexStringToByteArray(command);
      byte[] numArray = await this.readDataFromSensusAsync(progress, cancelToken, lenHeader, lenData, cmd, getDataOnly);
      cmd = (byte[]) null;
      return numArray;
    }

    public async Task<byte[]> readDataFromSensusAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      int lenHeader,
      int lenData,
      byte[] cmd,
      bool getDataOnly = false)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      int lenCRC = 2;
      int maxRepeats = this.configList.MaxRequestRepeat;
      byte[] response = (byte[]) null;
      byte[] header = new byte[lenHeader];
      byte[] CRC = new byte[lenCRC];
      SENSUSConnector.SENSUSConnector_Logger.Trace("read SENSUS connector asynchron.");
      this.myPort.DiscardInBuffer();
      for (int w = 0; w < maxRepeats && !cancelToken.IsCancellationRequested; ++w)
      {
        SENSUSConnector.SENSUSConnector_Logger.Trace("read SENSUS version (" + w.ToString() + 1.ToString() + ") ");
        if (!this.myPort.IsOpen)
        {
          this.myPort.Open();
          SENSUSConnector.SENSUSConnector_Logger.Trace("open port OK.");
        }
        SENSUSConnector.SENSUSConnector_Logger.Trace("sending: " + Util.ByteArrayToHexString(cmd));
        this.myPort.Write(cmd);
        await Task.Delay(this.configList.RecTime_BeforFirstByte);
        for (int r = 0; r < 3; ++r)
        {
          try
          {
            header = this.myPort.ReadHeader(lenHeader);
            SENSUSConnector.SENSUSConnector_Logger.Trace("received header: " + Util.ByteArrayToHexString(header));
            if (header != null)
              break;
          }
          catch (Exception ex)
          {
            SENSUSConnector.SENSUSConnector_Logger.Trace("ERROR: received header: " + Util.ByteArrayToHexString(header));
            SENSUSConnector.SENSUSConnector_Logger.Trace("ERROR: " + ex.Message);
            header = (byte[]) null;
          }
        }
        if (header != null)
        {
          byte[] data = this.myPort.ReadEnd(lenData);
          Buffer.BlockCopy((Array) data, data.Length - 2, (Array) CRC, 0, 2);
          if (getDataOnly)
          {
            response = new byte[lenData - lenCRC];
            Buffer.BlockCopy((Array) data, 0, (Array) response, 0, response.Length);
          }
          else
          {
            response = new byte[lenData + lenHeader];
            Buffer.BlockCopy((Array) header, 0, (Array) response, 0, header.Length);
            Buffer.BlockCopy((Array) data, 0, (Array) response, header.Length, data.Length);
          }
          SENSUSConnector.SENSUSConnector_Logger.Trace("received data: " + Util.ByteArrayToHexString(response));
          if (response == null)
            data = (byte[]) null;
          else
            break;
        }
        SENSUSConnector.SENSUSConnector_Logger.Trace("repeat reading data from SENSUS");
        await Task.Delay(this.configList.WaitBeforeRepeatTime);
      }
      byte[] numArray = response;
      response = (byte[]) null;
      header = (byte[]) null;
      CRC = (byte[]) null;
      return numArray;
    }

    public async Task<bool> keepAliveSensusAsync()
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      SENSUSConnector.SENSUSConnector_Logger.Trace("keep alive SENSUS connector asynchron.");
      if (this.Status != SENSUSSTATE.KEEP_ALIVE)
        return false;
      SENSUSConnector.SENSUSConnector_Logger.Trace("sending " + SENSUSConnector.CMD_GET_VERSION?.ToString());
      this.myPort.Write(SENSUSConnector.CMD_GET_VERSION);
      await Task.Delay(1000);
      byte[] response = this.myPort.ReadExisting();
      SENSUSConnector.SENSUSConnector_Logger.Trace("received: " + Util.ByteArrayToHexString(response));
      return response != null;
    }

    public async Task<bool> doSensusConnectionTestAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      int iDelay)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      string msg = "read version of SENSUS connector asynchron.";
      SENSUSConnector.SENSUSConnector_Logger.Trace(msg);
      msg = "sending " + SENSUSConnector.CMD_GET_VERSION?.ToString();
      SENSUSConnector.SENSUSConnector_Logger.Trace(msg);
      this.myPort.Write(SENSUSConnector.CMD_GET_VERSION);
      msg = "wait for receiving first byte: " + this.configList.RecTime_BeforFirstByte.ToString();
      SENSUSConnector.SENSUSConnector_Logger.Trace(msg);
      byte[] response = (byte[]) null;
      try
      {
        response = this.myPort.ReadHeader(47);
      }
      catch (TimeoutException ex)
      {
        msg = msg + "--> ERROR: " + ex.Message;
      }
      msg = msg + "received: " + Util.ByteArrayToHexString(response);
      SENSUSConnector.SENSUSConnector_Logger.Trace(msg);
      if (response != null && response.Length == 47)
      {
        byte[] responseData = new byte[response.Length - 2];
        Buffer.BlockCopy((Array) response, 0, (Array) responseData, 0, responseData.Length);
        string strResponse = Util.ByteArrayToString(responseData);
        this.SetFirmwareVersion(strResponse);
        SENSUSConnector.SENSUSConnector_Logger.Trace("\rresponse: " + this.ToString());
        responseData = (byte[]) null;
        strResponse = (string) null;
      }
      else if (response != null && response.Length != 47)
        SENSUSConnector.SENSUSConnector_Logger.Trace("\r!--> wrong response, pleas check connection! ");
      else if (response == null)
        SENSUSConnector.SENSUSConnector_Logger.Trace("\r!--> if this happens again, please try to re-plug Sensus to MinoConnect! ");
      await Task.Delay(iDelay);
      bool flag = response != null;
      msg = (string) null;
      response = (byte[]) null;
      return flag;
    }

    public async Task<string> readSensusVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      bool isNewConnection = true)
    {
      SENSUSConnector sensusTemp = await this.readSensusConnectorAsync(progress, cancelToken, isNewConnection);
      string str = sensusTemp.ToString();
      sensusTemp = (SENSUSConnector) null;
      return str;
    }

    public async Task<SENSUSConnector> readSensusConnectorAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      bool isNewConnection = true)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      int responseBytes = 47;
      if (this.myPort == null)
        throw new Exception("\rPort is not available!");
      SENSUSConnector.SENSUSConnector_Logger.Trace("read SENSUS connector asynchron.");
      byte[] cmd = (byte[]) null;
      byte[] response = (byte[]) null;
      int maxW = this.configList.MaxRequestRepeat;
      for (int w = 0; w < maxW; ++w)
      {
        SENSUSConnector.SENSUSConnector_Logger.Trace("read SENSUS version (" + w.ToString() + 1.ToString() + ") ");
        this.GuaranteePortOpen();
        CommunicationByMinoConnect MiCon = this.myPort.GetCommunicationByMinoConnect();
        MiCon.WriteCommand("#485ton\r\n");
        MiCon.WriteCommand("#bron\r\n");
        await Task.Delay(1000);
        SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus clock low");
        MiCon.WriteCommand("#485toff\r\n");
        MiCon.WriteCommand("#broff\r\n");
        await Task.Delay(1000);
        SENSUSConnector.SENSUSConnector_Logger.Trace("Sensus clock high");
        for (int i = 0; i < ((IEnumerable<uint>) this.waitTimeArray).Count<uint>(); ++i)
        {
          this.myPort.DiscardInBuffer();
          int waitTime = (int) this.waitTimeArray[i];
          SENSUSConnector.SENSUSConnector_Logger.Trace("sending: " + Util.ByteArrayToHexString(cmd));
          this.myPort.Write(SENSUSConnector.CMD_GET_VERSION);
          SENSUSConnector.SENSUSConnector_Logger.Trace("Delay(" + i.ToString() + "): " + waitTime.ToString());
          await Task.Delay(waitTime);
          try
          {
            response = this.myPort.ReadHeader(responseBytes);
          }
          catch (TimeoutException ex)
          {
            if (response != null)
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout but received: " + Util.ByteArrayToHexString(response));
            else
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout without data");
            response = (byte[]) null;
          }
          if (response == null)
          {
            SENSUSConnector.SENSUSConnector_Logger.Trace("no response");
          }
          else
          {
            SENSUSConnector.SENSUSConnector_Logger.Trace(" ... response after " + waitTime.ToString() + " ms.");
            break;
          }
        }
        if (response == null)
          MiCon = (CommunicationByMinoConnect) null;
        else
          break;
      }
      if (response != null)
      {
        if (response.Length != responseBytes)
          throw new Exception("Internal error. ReadHeader management problem");
        byte[] responseData = new byte[responseBytes - 2];
        byte[] responseDataCRC = new byte[2];
        Buffer.BlockCopy((Array) response, 0, (Array) responseData, 0, responseBytes - 2);
        Buffer.BlockCopy((Array) response, responseBytes - 2, (Array) responseDataCRC, 0, 2);
        string strResponse = Util.ByteArrayToString(responseData);
        SENSUSConnector.SENSUSConnector_Logger.Trace("received: " + strResponse);
        if (!string.IsNullOrEmpty(strResponse))
        {
          if (isNewConnection)
          {
            SENSUSConnector.SENSUSConnector_Logger.Trace("create LOCAL SENSUSConnector object!");
            SENSUSConnector localSensusConnector = new SENSUSConnector(strResponse);
            return localSensusConnector;
          }
          this.SetFirmwareVersion(strResponse);
          return this;
        }
        SENSUSConnector.SENSUSConnector_Logger.Trace("no response, could not connect to SENSUSConnector!!!");
        throw new Exception("\rERROR, could not connect to SENSUS connector ...");
      }
      string message = "\r\rNo response from device, please unplug and reconnect MinoConnect!!!";
      message += "\r +---------------------------------------------------------------+";
      message += "\r | After reconnect there is a time gap of 30 seconds to update!  |";
      message += "\r +---------------------------------------------------------------+";
      throw new Exception(message);
    }

    public async Task initSensusFirmwareAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      if (this.myPort == null || this.FwV.Version <= 0U)
        throw new Exception("\rNo connection to sensus device, please reconnect!!!");
      string msg = "initialize new firmware ...";
      progress.Report(msg);
      SENSUSConnector.SENSUSConnector_Logger.Trace(msg);
      this.Status = SENSUSSTATE.INIT;
      SENSUSConnector.SENSUSConnector_Logger.Trace("sending command: " + SENSUSConnector.CMD_GO?.ToString());
      this.myPort.Write(SENSUSConnector.CMD_GO);
      await Task.Delay(1200);
      SENSUSConnector.SENSUSConnector_Logger.Trace("send command OK.");
      SENSUSConnector.SENSUSConnector_Logger.Trace("wait 2 seconds for init is done..");
      await Task.Delay(1200);
      progress.Report("New firmware initialized! ");
      msg = (string) null;
    }

    public async Task<byte[]> readSensusAdressAsync(
      uint address,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      if (this.myPort == null || this.FwV.Version <= 0U)
        throw new Exception("\rNo connection to sensus device, please reconnect!!!");
      if (address >= 0U && address < 2576980377U)
      {
        this.Status = SENSUSSTATE.READING;
        List<byte> cmdBuilder = new List<byte>()
        {
          (byte) 3
        };
        cmdBuilder.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
        ushort crc16 = Util.CalculatesCRC16(cmdBuilder.ToArray<byte>());
        cmdBuilder.AddRange((IEnumerable<byte>) BitConverter.GetBytes(crc16));
        byte[] cmdComplete = cmdBuilder.ToArray<byte>();
        int maxW = this.configList.MaxRequestRepeat;
        for (int i = 0; i < maxW; ++i)
        {
          SENSUSConnector.SENSUSConnector_Logger.Trace("sending command: " + cmdComplete?.ToString());
          this.GuaranteePortOpen();
          this.myPort.Write(cmdComplete);
          byte[] response = new byte[522];
          try
          {
            this.myPort.ReadHeader(response.Length);
            SENSUSConnector.SENSUSConnector_Logger.Trace("response: " + Util.ByteArrayToHexString(response));
          }
          catch (TimeoutException ex)
          {
            if (response != null)
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout but received: " + Util.ByteArrayToHexString(response));
            else
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout without data");
            response = (byte[]) null;
          }
          if (response != null)
          {
            byte[] data = new byte[512];
            Buffer.BlockCopy((Array) data, 8, (Array) response, 0, data.Length);
            SENSUSConnector.SENSUSConnector_Logger.Trace("response data: " + Util.ByteArrayToHexString(response));
            return data;
          }
          await Task.Delay(this.configList.WaitBeforeRepeatTime);
          response = (byte[]) null;
        }
        throw new Exception("\rNo data received from device on address: " + address.ToString("x8"));
      }
      throw new Exception("\rAdress out of range!!!");
    }

    public async Task<byte[]> readSensusMemoryAsync(
      uint address,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      await Task.Delay(1);
      if (this.myPort == null || this.FwV.Version <= 0U)
        throw new Exception("\rNo connection to sensus device, please reconnect!!!");
      if (address >= 0U && address < 2576980377U)
      {
        List<byte> cmdBuilder = new List<byte>()
        {
          (byte) 2
        };
        cmdBuilder.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
        ushort crc16 = Util.CalculatesCRC16(cmdBuilder.ToArray<byte>());
        cmdBuilder.AddRange((IEnumerable<byte>) BitConverter.GetBytes(crc16));
        byte[] cmdComplete = cmdBuilder.ToArray<byte>();
        int maxW = this.configList.MaxRequestRepeat;
        for (int i = 0; i < maxW; ++i)
        {
          SENSUSConnector.SENSUSConnector_Logger.Trace("sending command: " + cmdComplete?.ToString());
          this.GuaranteePortOpen();
          this.myPort.Write(cmdComplete);
          byte[] response = new byte[522];
          try
          {
            response = this.myPort.ReadHeader(response.Length);
            SENSUSConnector.SENSUSConnector_Logger.Trace("response header: " + Util.ByteArrayToHexString(response));
          }
          catch (TimeoutException ex)
          {
            if (response != null)
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout but received: " + Util.ByteArrayToHexString(response));
            else
              SENSUSConnector.SENSUSConnector_Logger.Trace("timeout without data");
            response = (byte[]) null;
          }
          if (response != null)
          {
            byte[] data = new byte[512];
            SENSUSConnector.SENSUSConnector_Logger.Trace("response: " + Util.ByteArrayToHexString(response));
            Buffer.BlockCopy((Array) response, 8, (Array) data, 0, data.Length);
            return data;
          }
          response = (byte[]) null;
        }
        throw new Exception("\rNo data received from device on address: " + address.ToString("x8"));
      }
      throw new Exception("\rAdress out of range!!!");
    }

    public async Task<byte[]> writeSensusMemoryAsync(
      uint address,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      bool SlowWrite = false;
      if (this.myPort == null || this.FwV.Version <= 0U)
        throw new Exception("\rNo connection to sensus device, please reconnect!!!");
      if (address < 0U || address >= 2576980377U)
        throw new Exception("\rAdress out of range!!!");
      await Task.Delay(1);
      progress.Report("Writing at address: " + address.ToString("x8"));
      this.myPort.DiscardInBuffer();
      this.Status = SENSUSSTATE.WRITING;
      int maxW = this.configList.MaxRequestRepeat;
      byte[] response = (byte[]) null;
      bool anyResponse = false;
      List<byte> cmdBuilder = new List<byte>() { (byte) 1 };
      cmdBuilder.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      cmdBuilder.AddRange((IEnumerable<byte>) new byte[3]);
      cmdBuilder.AddRange((IEnumerable<byte>) data);
      ushort crc16 = Util.CalculatesCRC16(cmdBuilder.ToArray<byte>());
      cmdBuilder.AddRange((IEnumerable<byte>) BitConverter.GetBytes(crc16));
      byte[] cmdComplete = cmdBuilder.ToArray<byte>();
      for (int w = 0; w < maxW; ++w)
      {
        if (w == maxW - 1 & anyResponse && !SlowWrite)
        {
          SlowWrite = true;
          SENSUSConnector.SENSUSConnector_Logger.Warn("Switched to SlowWrite. Wait 500ms and restart repeats.");
          await Task.Delay(500);
          w = 0;
        }
        if (w > 0)
          SENSUSConnector.SENSUSConnector_Logger.Warn("Repeat: " + w.ToString());
        if (!SlowWrite)
        {
          this.myPort.Write(cmdComplete);
        }
        else
        {
          byte[] singleByte = new byte[1];
          int[] sendTimes = new int[cmdComplete.Length];
          DateTime NextTime = DateTime.MinValue;
          for (int i = 0; i < cmdComplete.Length; ++i)
          {
            DateTime TimeNow;
            do
            {
              TimeNow = DateTime.Now;
            }
            while (TimeNow < NextTime);
            NextTime = TimeNow.AddMilliseconds(5.0);
            sendTimes[i] = TimeNow.Millisecond;
            singleByte[0] = cmdComplete[i];
            this.myPort.Write(singleByte);
          }
          singleByte = (byte[]) null;
          sendTimes = (int[]) null;
        }
        response = (byte[]) null;
        try
        {
          response = this.myPort.ReadHeader(3);
          if (response == null || response.Length != 3)
          {
            string err = "Internal error. Illegal answer length";
            SENSUSConnector.SENSUSConnector_Logger.Error(err);
            throw new Exception(err);
          }
          anyResponse = true;
          if ((int) response[0] != (int) SENSUSConnector.ResponseACK)
          {
            await Task.Delay(20);
            byte[] additionalBytes = this.myPort.ReadExisting();
            if (additionalBytes != null && additionalBytes.Length != 0)
            {
              Array.Resize<byte>(ref response, response.Length + additionalBytes.Length);
              Buffer.BlockCopy((Array) additionalBytes, 0, (Array) response, 3, additionalBytes.Length);
            }
            additionalBytes = (byte[]) null;
          }
          SENSUSConnector.SENSUSConnector_Logger.Trace("Response: " + Util.ByteArrayToHexString(response));
          ushort CRCRet = BitConverter.ToUInt16(response, response.Length - 2);
          ushort CRCCheck = Util.CalculatesCRC16(response, 0, response.Length - 2);
          if ((int) CRCRet != (int) CRCCheck)
            SENSUSConnector.SENSUSConnector_Logger.Warn(" --> CRC is wrong !!!");
          else if ((int) response[0] != (int) SENSUSConnector.ResponseACK)
          {
            if ((int) response[0] == (int) SENSUSConnector.ResponseNACK)
            {
              byte lowNibble = (byte) ((uint) response[1] & 15U);
              byte highNibble = (byte) ((uint) response[1] & 240U);
              if (((uint) lowNibble & 1U) > 0U)
                SENSUSConnector.SENSUSConnector_Logger.Warn("Response: NACK received. Parity bit error");
              if (((uint) lowNibble & 2U) > 0U)
                SENSUSConnector.SENSUSConnector_Logger.Warn("Response: NACK received. Stop bit error");
              switch (highNibble)
              {
                case 16:
                  SENSUSConnector.SENSUSConnector_Logger.Warn("Response: NACK received. CRC error");
                  break;
                case 48:
                  SENSUSConnector.SENSUSConnector_Logger.Warn("Response: NACK received. Address error");
                  break;
                default:
                  if (((int) lowNibble & 3) == 0)
                  {
                    SENSUSConnector.SENSUSConnector_Logger.Warn("Response: NACK received");
                    break;
                  }
                  break;
              }
              if (response.Length == 6)
              {
                ushort errorIndex = BitConverter.ToUInt16(response, 2);
                SENSUSConnector.SENSUSConnector_Logger.Warn("Receive error index: " + errorIndex.ToString());
              }
            }
            else
              SENSUSConnector.SENSUSConnector_Logger.Warn("Unexpected response");
          }
          else
            goto label_48;
        }
        catch (TimeoutException ex)
        {
          if (response != null)
          {
            anyResponse = true;
            SENSUSConnector.SENSUSConnector_Logger.Warn("timeout but received: " + Util.ByteArrayToHexString(response));
          }
          else
            SENSUSConnector.SENSUSConnector_Logger.Warn("timeout without data");
          response = (byte[]) null;
        }
        await Task.Delay(this.configList.WaitBeforeRepeatTime);
      }
      SENSUSConnector.SENSUSConnector_Logger.Error("Write memory error");
label_48:
      return response;
    }

    public SortedDictionary<uint, byte[]> prepareDataForSensusWrite(
      SortedDictionary<uint, byte[]> firmware,
      int cmdSize = 512)
    {
      SortedDictionary<uint, byte[]> sortedDictionary = new SortedDictionary<uint, byte[]>();
      SENSUSConnector.SENSUSConnector_Logger.Trace("prepare data for size: " + cmdSize.ToString());
      int length1 = firmware.Values.First<byte[]>().Length;
      uint num = firmware.Keys.Last<uint>();
      if (cmdSize % length1 != 0)
        throw new Exception("\rPrepareData: not valid command size given!");
      uint key1 = 0;
      byte[] dst = new byte[cmdSize];
      for (int index = 0; index < dst.Length; ++index)
        dst[index] = (byte) 0;
      foreach (uint key2 in firmware.Keys)
      {
        SENSUSConnector.SENSUSConnector_Logger.Trace("prepare of address: " + key2.ToString());
        if (key1 == 0U)
          key1 = key2;
        byte[] src = firmware[key2];
        int dstOffset1 = (int) key2 - (int) key1;
        int length2 = src.Length;
        if (dstOffset1 < cmdSize && dstOffset1 + length2 <= cmdSize || (int) key2 == (int) num)
          Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset1, length2);
        else if (dstOffset1 < cmdSize && dstOffset1 + length2 > cmdSize)
        {
          int count1 = (int) ((long) (dstOffset1 + length2) - ((long) key2 + (long) cmdSize));
          int count2 = length2 - count1;
          Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset1, count2);
          sortedDictionary.Add(key1, dst);
          key1 = (uint) ((ulong) key2 + (ulong) cmdSize + 1UL);
          dst = new byte[cmdSize];
          for (int index = 0; index < dst.Length; ++index)
            dst[index] = (byte) 0;
          int dstOffset2 = 0;
          Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset2, count1);
        }
        else if (dstOffset1 >= cmdSize && length2 > 0)
        {
          sortedDictionary.Add(key1, dst);
          key1 = key2;
          dst = new byte[cmdSize];
          for (int index = 0; index < dst.Length; ++index)
            dst[index] = (byte) 0;
          int dstOffset3 = 0;
          Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset3, length2);
        }
      }
      return sortedDictionary;
    }

    internal void GuaranteePortOpen()
    {
      if (!this.isSensusConnectorReady())
        throw new Exception("SensusConnector not ready, check Port and ConfigurationList!");
      if (this.myPort.IsOpen)
        return;
      SENSUSConnector.SENSUSConnector_Logger.Trace("open port (" + this.myPort.PortType.ToString() + ")");
      this.myPort.Open();
      SENSUSConnector.SENSUSConnector_Logger.Trace("open port OK.");
    }

    private void myPort_OnResponse(object sender, byte[] e)
    {
      Type type = sender.GetType();
      SENSUSConnector.SENSUSConnector_Logger.Trace("On_Response called.");
      SENSUSConnector.SENSUSConnector_Logger.Trace("On_Response - " + e.ToString());
      SENSUSConnector.SENSUSConnector_Logger.Trace("On_Response - sender: " + type.FullName);
    }
  }
}
