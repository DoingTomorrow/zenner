// Decompiled with JetBrains decompiler
// Type: MinoConnect.FlashMinoConnect
// Assembly: MinoConnect, Version=1.5.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E4D0ECC-943B-4E96-B8E2-CE02CEE9906B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinoConnect.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

#nullable disable
namespace MinoConnect
{
  public sealed class FlashMinoConnect : IDisposable
  {
    private int BLOCK_SIZE = 256;
    private int startAddress;
    private SerialPort Port;

    public string LastError { get; private set; }

    public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    public FlashMinoConnect()
    {
    }

    public FlashMinoConnect(SerialPort port) => this.Port = port;

    public string ReadFirmwareVersion(string portName)
    {
      this.OpenPort(portName);
      this.Port.Write("#ver\r\n");
      Thread.Sleep(600);
      return this.Port.ReadExisting().Replace("|", "\r\n");
    }

    public bool Upgrade(string path) => this.Port != null && this.Upgrade(path, this.Port.PortName);

    public bool Upgrade(string path, string portName)
    {
      this.LastError = string.Empty;
      this.OpenPort(portName);
      byte[] numArray1 = this.ReadFirmware(path);
      this.Port.Write("#update imgclear\r\n");
      Thread.Sleep(200);
      byte[] bytes = new byte[4];
      try
      {
        bytes[0] = (byte) this.Port.ReadByte();
        bytes[1] = (byte) this.Port.ReadByte();
        bytes[2] = (byte) this.Port.ReadByte();
        bytes[3] = (byte) this.Port.ReadByte();
        string str = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        if (!str.StartsWith("OK"))
        {
          this.LastError = "Can not clear the old firmware image on MinoConnect! Response: " + str;
          return false;
        }
      }
      catch (TimeoutException ex)
      {
        return false;
      }
      for (int startAddress = this.startAddress; startAddress < 63488; startAddress += this.BLOCK_SIZE)
      {
        if (this.ProgressChanged != null)
          this.ProgressChanged((object) this, new ProgressChangedEventArgs((startAddress - this.startAddress) * 100 / (63488 - this.startAddress)));
        byte[] numArray2 = new byte[this.BLOCK_SIZE];
        Buffer.BlockCopy((Array) numArray1, startAddress, (Array) numArray2, 0, this.BLOCK_SIZE);
        bool flag1 = true;
        for (int index = 0; index < numArray2.Length; ++index)
        {
          if (numArray2[index] != byte.MaxValue)
          {
            flag1 = false;
            break;
          }
        }
        if (!flag1)
        {
          List<byte> packet = new List<byte>();
          packet.Add((byte) 0);
          packet.Add((byte) 4);
          packet.Add((byte) startAddress);
          packet.Add((byte) (startAddress >> 8));
          packet.Add((byte) (startAddress >> 16));
          packet.Add((byte) (startAddress >> 24));
          packet.AddRange((IEnumerable<byte>) numArray2);
          int num = 0;
          bool flag2;
          do
          {
            flag2 = this.SendUpdateChunkCommand(packet);
            if (!flag2)
            {
              ++num;
              Thread.Sleep(200);
            }
          }
          while (!flag2 && num < 3);
          if (!flag2)
          {
            this.LastError = "Failed to transfer the firmware file!";
            return false;
          }
        }
      }
      ushort num1 = FlashMinoConnect.CalculatesCRC16(numArray1, this.startAddress, 45312 - this.startAddress);
      this.Port.Write("#update go\r\n");
      Thread.Sleep(200);
      this.SendUpdateChunkCommand(new List<byte>()
      {
        (byte) 4,
        (byte) num1,
        (byte) ((uint) num1 >> 8)
      });
      this.Dispose();
      return true;
    }

    private void InitializePort(string portName)
    {
      this.Port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
      this.Port.ReadTimeout = 4000;
      this.Port.WriteTimeout = 4000;
    }

    private void OpenPort(string portName)
    {
      if (this.Port == null)
        this.InitializePort(portName);
      if (this.Port.PortName != portName)
      {
        if (this.Port.IsOpen)
          this.Port.Close();
        this.Port.PortName = portName;
      }
      if (!this.Port.IsOpen)
        this.Port.Open();
      this.Port.ReadTimeout = 4000;
      this.Port.WriteTimeout = 4000;
    }

    internal string ExitFromTransparentMode(string portName)
    {
      this.OpenPort(portName);
      this.Port.Write("fTzhuZl5c39zUNdWq105bmysloncwalnNIK783BH89kirEWmIkPl(!56)bfrtg984!?eV&29IkoPmt!$ymncSrtIopQ'+*bg%ad279vRzOp;-_4y78JI08NJde6HjiOx");
      Thread.Sleep(600);
      return this.Port.ReadExisting();
    }

    private byte[] ReadFirmware(string path)
    {
      this.startAddress = -1;
      byte[] destinationArray = new byte[1048576];
      for (int index = 0; index < destinationArray.Length; ++index)
        destinationArray[index] = byte.MaxValue;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          string str = streamReader.ReadLine();
          if (!str.StartsWith("@"))
            throw new ArgumentException("The firmware file is not valid!");
          while (!string.IsNullOrEmpty(str))
          {
            if (str.StartsWith("@"))
            {
              int int32 = Convert.ToInt32(str.Substring(1), 16);
              if (this.startAddress == -1)
                this.startAddress = int32;
              for (str = streamReader.ReadLine().Trim(); !str.StartsWith("@") && !str.StartsWith("q"); str = streamReader.ReadLine().Trim())
              {
                string[] strArray = str.Split(' ');
                byte[] sourceArray = new byte[strArray.Length];
                for (int index = 0; index < strArray.Length; ++index)
                  sourceArray[index] = Convert.ToByte(strArray[index], 16);
                Array.Copy((Array) sourceArray, 0, (Array) destinationArray, int32, sourceArray.Length);
                int32 += sourceArray.Length;
              }
            }
            if (str.StartsWith("q"))
              break;
          }
        }
      }
      return destinationArray;
    }

    private bool SendUpdateChunkCommand(List<byte> packet)
    {
      if (this.Port == null || !this.Port.IsOpen)
        return false;
      ushort num = FlashMinoConnect.CalculatesCRC16(packet.ToArray());
      packet.Add((byte) num);
      packet.Add((byte) ((uint) num >> 8));
      byte[] stuffedBuffer = this.GetStuffedBuffer(packet);
      this.Port.Write("#update chunk\r\n");
      Thread.Sleep(200);
      this.Port.Write(new byte[1]{ (byte) 170 }, 0, 1);
      this.Port.Write(stuffedBuffer, 0, stuffedBuffer.Length);
      this.Port.Write(new byte[1]{ (byte) 205 }, 0, 1);
      byte[] bytes = new byte[4];
      try
      {
        bytes[0] = (byte) this.Port.ReadByte();
        bytes[1] = (byte) this.Port.ReadByte();
        bytes[2] = (byte) this.Port.ReadByte();
        bytes[3] = (byte) this.Port.ReadByte();
        string str = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        bool flag = str.StartsWith("OK");
        if (!flag)
          this.LastError = "Wrong responce while #update chunk command. Responce: " + str;
        return flag;
      }
      catch (TimeoutException ex)
      {
        return false;
      }
    }

    private byte[] GetStuffedBuffer(List<byte> buffer)
    {
      List<byte> byteList = new List<byte>();
      foreach (byte num1 in buffer)
      {
        int num2;
        switch (num1)
        {
          case 170:
          case 205:
            num2 = 1;
            break;
          default:
            num2 = num1 == (byte) 92 ? 1 : 0;
            break;
        }
        if (num2 != 0)
        {
          byteList.Add((byte) 92);
          byteList.Add(~num1);
        }
        else
          byteList.Add(num1);
      }
      return byteList.ToArray();
    }

    public static string ByteArrayToHexString(byte[] buffer)
    {
      return FlashMinoConnect.ByteArrayToHexString(buffer, 0, buffer.Length);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex, int length)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = startIndex; index < startIndex + length; ++index)
        stringBuilder.Append(buffer[index].ToString("X2"));
      return stringBuilder.ToString();
    }

    public static ushort CalculatesCRC16(byte[] data)
    {
      return FlashMinoConnect.CalculatesCRC16(data, 0, data.Length);
    }

    public static ushort CalculatesCRC16(byte[] data, int offset, int length)
    {
      ushort num = ushort.MaxValue;
      for (int index1 = offset; index1 < length; ++index1)
      {
        num ^= (ushort) ((uint) data[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((ushort) ((uint) num & 32768U) > (ushort) 0)
            num = (ushort) ((uint) (ushort) ((uint) num << 1) ^ 4129U);
          else
            num <<= 1;
        }
      }
      return num;
    }

    public void Dispose()
    {
      if (this.Port == null)
        return;
      try
      {
        if (this.Port.IsOpen)
          this.Port.Close();
        this.Port.Dispose();
        this.Port = (SerialPort) null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      catch
      {
      }
    }
  }
}
