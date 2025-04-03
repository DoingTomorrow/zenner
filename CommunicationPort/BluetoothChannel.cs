// Decompiled with JetBrains decompiler
// Type: CommunicationPort.BluetoothChannel
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using System.Text;

#nullable disable
namespace CommunicationPort
{
  public sealed class BluetoothChannel : BluetoothClient, IChannel, IDisposable
  {
    private object _syncLock = new object();
    private NetworkStream stream;

    public string PortName { get; set; }

    public bool IsOpen => this.Connected;

    public int BytesToRead => this.Available;

    public BluetoothChannel(string portName) => this.PortName = portName;

    public void Open()
    {
      if (this.IsOpen)
        return;
      BluetoothAddress bluetoothAddress = !string.IsNullOrEmpty(this.PortName) ? BluetoothChannel.GetBluetoothAddressByPortName(this.PortName) : throw new ArgumentNullException("PortName");
      if (bluetoothAddress == (BluetoothAddress) null)
        throw new ArgumentException("address");
      this.Connect(bluetoothAddress, (Guid) BluetoothService.L2CapProtocol);
      this.stream = this.GetStream();
    }

    public void Write(byte[] buffer, int offset, int count)
    {
      if (this.stream == null)
        throw new NullReferenceException("stream");
      if (!this.stream.CanWrite)
        return;
      lock (this._syncLock)
      {
        this.stream.Write(buffer, offset, count);
        this.stream.Flush();
      }
    }

    public void Write(string text)
    {
      if (this.stream == null)
        throw new NullReferenceException("stream");
      switch (text)
      {
        case null:
          throw new ArgumentNullException(nameof (text));
        case "":
          break;
        default:
          byte[] bytes = Encoding.ASCII.GetBytes(text);
          if (!this.stream.CanWrite)
            break;
          lock (this._syncLock)
          {
            this.stream.Write(bytes, 0, bytes.Length);
            break;
          }
      }
    }

    public void WriteBaudrateCarrier(int numberOfBytes)
    {
      byte[] buffer = new byte[numberOfBytes];
      for (int index = 0; index < buffer.Length; ++index)
        buffer[index] = (byte) 85;
      this.Write(buffer, 0, buffer.Length);
    }

    public int Read(byte[] buffer, int offset, int count)
    {
      if (this.stream == null)
        throw new NullReferenceException("stream");
      if (!this.stream.CanRead)
        return 0;
      lock (this._syncLock)
        return this.stream.Read(buffer, offset, count);
    }

    public int ReadByte()
    {
      if (this.stream == null)
        throw new NullReferenceException("stream");
      if (!this.stream.CanRead)
        return 0;
      lock (this._syncLock)
        return this.stream.ReadByte();
    }

    public void DiscardInBuffer()
    {
      if (this.stream == null)
        throw new NullReferenceException("stream");
      lock (this._syncLock)
        this.stream.Flush();
    }

    public void DiscardOutBuffer()
    {
      if (this.stream == null)
        throw new NullReferenceException("stream");
      lock (this._syncLock)
        this.stream.Flush();
    }

    public static bool IsBluetooth(string portName)
    {
      if (string.IsNullOrEmpty(portName))
        throw new ArgumentNullException(nameof (portName));
      return BluetoothChannel.GetBluetoothAddressByPortName(portName) != (BluetoothAddress) null;
    }

    private static BluetoothAddress GetBluetoothAddressByPortName(string portName)
    {
      return BluetoothChannel.SearchRegistryForPortName("SYSTEM\\CurrentControlSet\\Enum", portName);
    }

    private static BluetoothAddress SearchRegistryForPortName(string startKey, string portName)
    {
      if (string.IsNullOrEmpty(portName))
        return (BluetoothAddress) null;
      RegistryKey localMachine = Registry.LocalMachine;
      RegistryKey registryKey;
      try
      {
        if (startKey.Contains("ACPI") || startKey.Contains("MS_BTHBRB") || startKey.Contains("Root\\SYSTEM") || startKey.Contains("COMPOSITE_BATTERY") || startKey.Contains("mssmbios") || startKey.Contains("VMUSBCONNECTOR") || startKey.Contains("RDPBUS") || startKey.Contains("blbdrive") || startKey.Contains("volmgr") || startKey.Contains("STORAGE") || startKey.Contains("Enum\\SW") || startKey.Contains("USB") || startKey.Contains("ScFilter") || startKey.Contains("0350278f-3dca-4e62-831d-a41165ff906c"))
          return (BluetoothAddress) null;
        registryKey = localMachine.OpenSubKey(startKey);
        if (registryKey == null)
          return (BluetoothAddress) null;
      }
      catch (SecurityException ex)
      {
        return (BluetoothAddress) null;
      }
      List<string> stringList = new List<string>((IEnumerable<string>) registryKey.GetSubKeyNames());
      if (stringList.Contains("Device Parameters") && startKey != "SYSTEM\\CurrentControlSet\\Enum")
      {
        object obj = Registry.GetValue("HKEY_LOCAL_MACHINE\\" + startKey + "\\Device Parameters", "PortName", (object) null);
        if (obj == null)
          return (BluetoothAddress) null;
        string str = obj.ToString();
        if (!str.StartsWith("COM") || !(portName == str.ToString()))
          return (BluetoothAddress) null;
        string name = registryKey.Name;
        if (name.IndexOf("BTHENUM") > 0)
        {
          int num1 = name.LastIndexOf('&');
          int num2 = name.LastIndexOf('_');
          string bluetoothString = name.Substring(num1 + 1, num2 - num1 - 1);
          if (bluetoothString != "000000000000")
            return BluetoothAddress.Parse(bluetoothString);
        }
      }
      else
      {
        foreach (string str in stringList)
        {
          BluetoothAddress bluetoothAddress = BluetoothChannel.SearchRegistryForPortName(startKey + "\\" + str, portName);
          if (bluetoothAddress != (BluetoothAddress) null)
            return bluetoothAddress;
        }
      }
      return (BluetoothAddress) null;
    }

    public new void Dispose() => base.Dispose();

    void IChannel.Close() => this.Close();
  }
}
