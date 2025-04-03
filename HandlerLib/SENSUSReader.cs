// Decompiled with JetBrains decompiler
// Type: HandlerLib.SENSUSReader
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class SENSUSReader
  {
    internal static Logger SensusReader_Logger = LogManager.GetLogger("Sensus_SensusReader");
    internal string[] strBuffer;
    internal int strBufferPos;
    internal static readonly string SENSUS_CMD_VALUES = "sensus values\r\n";
    internal static readonly string SENSUS_CMD_SERIALNO = "version ?\r\n";
    internal static readonly string SENSUS_CMD_PROG = "sensus prog\r\n";
    internal static readonly int bufferSize = 10;
    internal string SerialPortID_STD = "ZR_SENSUSREADER_V4_DEV1";
    internal string SerialPortID_AtLEAST = "ZR_SENSUSREADER";
    internal string SerialPortID = "ZR_SENSUSREADER_V4_DEV1";
    internal string SerialPortIDFunc = "ZR_SENSUSREADER_V4_DEV2";
    internal bool newDataAvail;
    internal bool isFunctionPort = false;
    internal int localTimeOut = 3600;
    internal static readonly string SENSUS_SERIALNUMBER_FOR_FIRMWARE_UPDATE = "FDEV5A78787801";
    internal static volatile bool checkLocked = false;

    public SerialPort localPort { get; private set; }

    public string MyPortName { get; private set; }

    public string FWversion { get; private set; }

    internal FirmwareVersion zFWVersion { get; private set; }

    public byte[] actualByteBuffer { get; private set; }

    public SENSUSReader(string serialPortName = null, bool isFunctionPort = false)
    {
      this.strBuffer = new string[SENSUSReader.bufferSize];
      this.strBufferPos = -1;
      this.isFunctionPort = isFunctionPort;
      this.FWversion = "0";
      this.newDataAvail = false;
      this.SerialPortID = serialPortName;
      this.doInitialize(this.SerialPortID);
    }

    internal bool doInitialize(string serialPortName = null)
    {
      try
      {
        SENSUSReader.SensusReader_Logger.Debug(nameof (doInitialize));
        if (this.localPort != null)
        {
          this.Close();
          this.localPort = (SerialPort) null;
          Task.Delay(this.localTimeOut);
        }
        this.SerialPortID = serialPortName != null ? serialPortName : this.SerialPortID_STD;
        if (this.isFunctionPort)
          this.SerialPortID = serialPortName != null ? serialPortName : this.SerialPortIDFunc;
        this.MyPortName = this.GetComPortFromIdentification(this.SerialPortID);
        if (string.IsNullOrEmpty(this.MyPortName) && !string.IsNullOrEmpty(serialPortName))
          this.MyPortName = serialPortName;
        else if (string.IsNullOrEmpty(this.MyPortName) && string.IsNullOrEmpty(serialPortName))
          this.MyPortName = this.GetComPortFromIdentification(this.SerialPortID_AtLEAST);
        this.localPort = new SerialPort(this.MyPortName);
        this.localPort.Open();
        this.localPort.RtsEnable = true;
        this.localPort.DiscardInBuffer();
        this.localPort.DiscardOutBuffer();
        this.localPort.DataReceived -= new SerialDataReceivedEventHandler(this.MyPort_DataReceived);
        this.localPort.DataReceived += new SerialDataReceivedEventHandler(this.MyPort_DataReceived);
        this.localPort.ErrorReceived -= new SerialErrorReceivedEventHandler(this.LocalPort_ErrorReceived);
        this.localPort.ErrorReceived += new SerialErrorReceivedEventHandler(this.LocalPort_ErrorReceived);
        this.localPort.ReadTimeout = this.localTimeOut;
        this.localPort.WriteTimeout = 777;
        if (this.localPort != null)
          this.localPort.Close();
        return true;
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case UnauthorizedAccessException _:
            throw new Exception("\nPort: " + this.MyPortName + " is actually used by another process!!!");
          case ArgumentException _:
            throw new Exception("\nPort: " + this.MyPortName + " could not be found on this computer!!!");
          case NullReferenceException _:
            throw new Exception("\nThe equipment value of SerialPortID could not be found, please check.");
          default:
            if (ex.Message.Contains("NULL"))
              throw ex;
            if (ex.Message.Length > 0)
              throw ex;
            return false;
        }
      }
    }

    private string GetComPortFromIdentification(string identification)
    {
      string empty = string.Empty;
      List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
      if (string.IsNullOrEmpty(identification))
        throw new Exception("Identification string of SerialPort is NULL !!!");
      try
      {
        foreach (ValueItem valueItem in availableComPorts)
        {
          if (valueItem.Info.Contains(identification))
            return valueItem.Value;
        }
        return empty;
      }
      catch
      {
        return empty;
      }
    }

    public bool Close()
    {
      SENSUSReader.SensusReader_Logger.Debug("Close Port");
      if (this.localPort == null)
        return false;
      this.localPort.Close();
      return true;
    }

    private async void LocalPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      SENSUSReader.SensusReader_Logger.Debug("Port_ErrorReceived");
      SENSUSReader.SensusReader_Logger.Debug("Data: " + e.EventType.ToString());
      string str = await this.checkDataReceived();
      throw new Exception(" receiving data error, please check connection!");
    }

    public async void MyPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      SENSUSReader.SensusReader_Logger.Debug("Port_DataReceived");
      if (e.EventType == SerialData.Chars)
      {
        SENSUSReader.SensusReader_Logger.Debug("Data: " + e.EventType.ToString());
        string str = await this.checkDataReceived();
      }
      else
      {
        if (e.EventType != SerialData.Eof)
          return;
        SENSUSReader.SensusReader_Logger.Debug("Data: EOF!");
      }
    }

    public async Task<string> checkDataReceived()
    {
      if (!SENSUSReader.checkLocked)
      {
        SENSUSReader.checkLocked = true;
        try
        {
          string val = string.Empty;
          this.newDataAvail = false;
          await Task.Delay(10);
          if (this.localPort.BytesToRead > 0)
          {
            byte[] buffer = new byte[this.localPort.BytesToRead];
            this.localPort.Read(buffer, 0, buffer.Length);
            if (buffer != null)
            {
              val = Util.ByteArrayToString(buffer);
              SENSUSReader.SensusReader_Logger.Debug(" -> Data: " + val);
              if ((long) this.strBufferPos == (long) (uint) (this.strBuffer.Length - 1))
              {
                this.strBuffer = new string[SENSUSReader.bufferSize];
                this.strBufferPos = -1;
              }
              ++this.strBufferPos;
              this.strBuffer[this.strBufferPos] = val;
              this.actualByteBuffer = new byte[buffer.Length];
              Buffer.BlockCopy((Array) buffer, 0, (Array) this.actualByteBuffer, 0, buffer.Length);
              this.newDataAvail = true;
            }
            buffer = (byte[]) null;
          }
          return val;
        }
        finally
        {
          SENSUSReader.checkLocked = false;
        }
      }
      else
      {
        this.newDataAvail = false;
        return string.Empty;
      }
    }

    private async Task<string> checkNewDataFromBuffer()
    {
      string retVal = (string) null;
      if (this.newDataAvail)
      {
        retVal = this.strBuffer[this.strBufferPos];
        this.newDataAvail = false;
        await Task.Delay(10);
      }
      string str = retVal;
      retVal = (string) null;
      return str;
    }

    public async Task<string> setProgrammingMode()
    {
      SENSUSReader.SensusReader_Logger.Debug("Set programming mode!");
      if (this.localPort != null && !this.localPort.IsOpen)
        this.localPort.Open();
      if (this.localPort != null)
      {
        this.localPort.Write(SENSUSReader.SENSUS_CMD_PROG);
        await Task.Delay(this.localPort.ReadTimeout);
      }
      string str = await this.checkNewDataFromBuffer();
      return str;
    }

    public async Task<FirmwareVersion> readSerialNumber()
    {
      try
      {
        SENSUSReader.SensusReader_Logger.Debug("read serial number!");
        if (this.localPort != null && !this.localPort.IsOpen)
          this.localPort.Open();
        for (int i = 1; i < 10; ++i)
        {
          if (this.localPort != null)
          {
            this.localPort.Write(SENSUSReader.SENSUS_CMD_SERIALNO);
            await Task.Delay(520 * i);
            if (this.newDataAvail)
              break;
          }
        }
        string valHex = await this.checkNewDataFromBuffer();
        if (!string.IsNullOrEmpty(valHex) && valHex.Contains("FWversion:"))
        {
          valHex = valHex.Replace("FWversion:", "").Trim();
          uint version = new FirmwareVersion(valHex).Version;
          this.zFWVersion = new FirmwareVersion(version);
        }
        valHex = (string) null;
      }
      catch (Exception ex)
      {
        this.zFWVersion = new FirmwareVersion(0U);
      }
      return this.zFWVersion;
    }

    public async Task<string> readData()
    {
      try
      {
        for (int i = 1; i < 5; ++i)
        {
          SENSUSReader.SensusReader_Logger.Debug("read data from SENSUS!");
          if (this.localPort != null && !this.localPort.IsOpen)
            this.localPort.Open();
          if (this.localPort != null)
          {
            this.localPort.Write(SENSUSReader.SENSUS_CMD_VALUES);
            await Task.Delay(520 * i);
            if (this.newDataAvail)
              break;
          }
        }
      }
      catch (Exception ex)
      {
        return "Error while reading data! \n" + ex.Message;
      }
      string str = await this.checkNewDataFromBuffer();
      return str;
    }

    public async Task<string> sendCommand(string COMMAND)
    {
      SENSUSReader.SensusReader_Logger.Debug("send command: " + COMMAND);
      this.actualByteBuffer = new byte[0];
      if (this.localPort != null && !this.localPort.IsOpen)
        this.localPort.Open();
      if (this.localPort == null || string.IsNullOrEmpty(COMMAND))
        return string.Empty;
      this.localPort.Write(COMMAND);
      await Task.Delay(this.localPort.ReadTimeout + 333);
      string str = await this.checkNewDataFromBuffer();
      return str;
    }

    public async Task<bool> checkSENSUSConnectorOK()
    {
      SENSUSReader.SensusReader_Logger.Debug("check SENSUS ...");
      string data = await this.readData();
      bool flag = data.Contains("Data not available") || data.Contains("V;");
      data = (string) null;
      return flag;
    }
  }
}
