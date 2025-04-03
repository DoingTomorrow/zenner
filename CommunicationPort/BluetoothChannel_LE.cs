// Decompiled with JetBrains decompiler
// Type: CommunicationPort.BluetoothChannel_LE
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Storage.Streams;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommunicationPort
{
  public sealed class BluetoothChannel_LE : IChannel, IDisposable
  {
    private static Logger MiConLE_Logger = LogManager.GetLogger("MiConLE");
    public static readonly Guid PROTEUS_Base_UUID = new Guid("6E400000-C352-11E5-953D-0002A5D5C51B");
    public static readonly Guid PROTEUS_PrimaryService_UUID = new Guid("6E400001-C352-11E5-953D-0002A5D5C51B");
    public static readonly Guid PROTEUS_TX_CHARACTERISTIC_UUID = new Guid("6E400002-C352-11E5-953D-0002A5D5C51B");
    public static readonly Guid PROTEUS_RX_CHARACTERISTIC_UUID = new Guid("6E400003-C352-11E5-953D-0002A5D5C51B");
    public static readonly int E_BLUETOOTH_ATT_INVALID_PDU = -2140864508;
    public static readonly uint PROTEUS_MTU = 247;
    public static readonly uint PROTEUS_FRAME_SIZE = 5;
    public static readonly uint PROTEUS_MAX_PAYLOAD = BluetoothChannel_LE.PROTEUS_MTU - BluetoothChannel_LE.PROTEUS_FRAME_SIZE;
    private object _syncLock = new object();
    private Queue<byte> ReceivedData;
    internal string WindowsVersion;
    private BluetoothLEDevice SelectedMiCon;
    private GattCharacteristic ToMinoConnect;
    private GattCharacteristic FromMinoConnect;
    public uint MaxDataLength;

    public string PortName { get; set; }

    public bool IsOpen => this.IsConnected;

    public int BytesToRead
    {
      get
      {
        int count = this.ReceivedData.Count;
        if (count > 0 && BluetoothChannel_LE.MiConLE_Logger.IsTraceEnabled)
          BluetoothChannel_LE.MiConLE_Logger.Trace("CheckBytesToRead: " + this.ReceivedData.Count.ToString());
        return count;
      }
    }

    internal event EventHandler OnDataReceived;

    public ulong BTMAC { get; private set; }

    public uint MaxTransmitDataLength { get; private set; }

    public uint MaxReceiveDataLength { get; private set; }

    public bool IsConnected => this.ToMinoConnect != null && this.FromMinoConnect != null;

    private BluetoothChannel_LE()
    {
      this.WindowsVersion = Environment.OSVersion.ToString();
      this.ReceivedData = new Queue<byte>();
      this.MaxTransmitDataLength = BluetoothChannel_LE.PROTEUS_MAX_PAYLOAD;
      this.MaxReceiveDataLength = BluetoothChannel_LE.PROTEUS_MAX_PAYLOAD;
    }

    public BluetoothChannel_LE(ulong BTMAC)
      : this()
    {
      this.PortName = "Mi" + BTMAC.ToString("X06").Substring(6);
      this.BTMAC = BTMAC;
    }

    public BluetoothChannel_LE(string portName)
      : this()
    {
      this.PortName = portName;
    }

    internal async Task ConnectAsync()
    {
      GattDeviceService service = (GattDeviceService) null;
      try
      {
        DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true));
        DeviceInformation deviceInfo = ((IEnumerable<DeviceInformation>) devices).FirstOrDefault<DeviceInformation>((Func<DeviceInformation, bool>) (x => x.Name.StartsWith(this.PortName)));
        if (deviceInfo == null)
          throw new Exception(this.PortName + " was not found!");
        if (this.SelectedMiCon == null)
        {
          BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
          this.SelectedMiCon = bluetoothLeDevice;
          bluetoothLeDevice = (BluetoothLEDevice) null;
        }
        if (this.SelectedMiCon != null)
        {
          service = this.SelectedMiCon.GetGattService(BluetoothChannel_LE.PROTEUS_PrimaryService_UUID);
          this.ToMinoConnect = service != null ? service.GetCharacteristics(BluetoothChannel_LE.PROTEUS_TX_CHARACTERISTIC_UUID).FirstOrDefault<GattCharacteristic>() : throw new Exception("Failed to open the connection with MinoConnect!");
          if (this.ToMinoConnect == null)
            throw new Exception("TX is null.");
          this.FromMinoConnect = service.GetCharacteristics(BluetoothChannel_LE.PROTEUS_RX_CHARACTERISTIC_UUID).FirstOrDefault<GattCharacteristic>();
          if (this.FromMinoConnect == null)
            throw new Exception("RX is null.");
          int attempt = 3;
          GattCommunicationStatus statusStartNotifications = (GattCommunicationStatus) 1;
          do
          {
            try
            {
              statusStartNotifications = await this.FromMinoConnect.WriteClientCharacteristicConfigurationDescriptorAsync((GattClientCharacteristicConfigurationDescriptorValue) 1);
              if (statusStartNotifications > 0)
                BluetoothChannel_LE.MiConLE_Logger.Error(string.Format("Failed to start RX notification! Error: {0}", (object) statusStartNotifications));
            }
            catch (Exception ex)
            {
              BluetoothChannel_LE.MiConLE_Logger.Error("Failed to start RX notification! Error: " + ex.Message);
            }
            await Task.Delay(100);
          }
          while (statusStartNotifications != null && attempt-- > 0);
          if (statusStartNotifications > 0)
            throw new Exception(string.Format("Failed to start RX notification! Error: {0}", (object) statusStartNotifications));
          GattCharacteristic fromMinoConnect = this.FromMinoConnect;
          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>>(new Func<TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>, EventRegistrationToken>(fromMinoConnect.add_ValueChanged), new Action<EventRegistrationToken>(fromMinoConnect.remove_ValueChanged), new TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>((object) this, __methodptr(DataFromMiConReceived)));
          if (!this.IsConnected)
          {
            string m = "Connection to MinoConnect not possible.";
            BluetoothChannel_LE.MiConLE_Logger.Error(m);
            throw new Exception(m);
          }
        }
        devices = (DeviceInformationCollection) null;
        deviceInfo = (DeviceInformation) null;
      }
      catch (Exception ex1)
      {
        try
        {
          service?.Dispose();
          service = (GattDeviceService) null;
        }
        catch (Exception ex2)
        {
          BluetoothChannel_LE.MiConLE_Logger.Error(ex2, "Failed to dispose GATT service!");
        }
        this.FromMinoConnect = (GattCharacteristic) null;
        this.ToMinoConnect = (GattCharacteristic) null;
        string m = "Bluetooth connection to MinoConnect fails";
        BluetoothChannel_LE.MiConLE_Logger.Error(m);
        throw new Exception(m, ex1);
      }
      service = (GattDeviceService) null;
    }

    private void DataFromMiConReceived(GattCharacteristic sender, GattValueChangedEventArgs args)
    {
      DataReader dataReader = DataReader.FromBuffer(args.CharacteristicValue);
      byte[] source = new byte[(int) dataReader.UnconsumedBufferLength];
      dataReader.ReadBytes(source);
      DateTimeOffset timestamp = args.Timestamp;
      byte num = source[0];
      for (int index = 1; index < source.Length; ++index)
        this.ReceivedData.Enqueue(source[index]);
      if (BluetoothChannel_LE.MiConLE_Logger.IsTraceEnabled)
      {
        byte[] array = ((IEnumerable<byte>) source).Skip<byte>(1).ToArray<byte>();
        string str = Utility.ByteArrayToHexString(array) + " ASCII'" + Utility.ByteArrayToAsciiString(array) + "'";
        BluetoothChannel_LE.MiConLE_Logger.Trace("BLE RX (" + array.Length.ToString() + "): " + str);
      }
      if (this.OnDataReceived != null && source.Length > 1)
        this.OnDataReceived((object) this, (EventArgs) new ReceivedCountEventArgs(source.Length - 1));
      switch (num)
      {
        case 1:
          break;
        case 4:
          BluetoothChannel_LE.MiConLE_Logger.Trace("data + high throughput mode");
          break;
        default:
          BluetoothChannel_LE.MiConLE_Logger.Trace("data + other");
          break;
      }
    }

    internal async Task SendData(byte[] sendData)
    {
      if (this.ToMinoConnect == null)
        return;
      int offset = 0;
      try
      {
        while (offset < sendData.Length)
        {
          int blockSize = sendData.Length - offset;
          if ((long) blockSize > (long) this.MaxTransmitDataLength)
            blockSize = (int) this.MaxTransmitDataLength;
          DataWriter writer = new DataWriter();
          byte[] payload = new byte[blockSize + 1];
          payload[0] = (byte) 1;
          Array.ConstrainedCopy((Array) sendData, offset, (Array) payload, 1, blockSize);
          writer.WriteBytes(payload);
          int attempt = 3;
          IBuffer detachedBuffer = writer.DetachBuffer();
          GattCommunicationStatus result;
          do
          {
            result = await this.ToMinoConnect.WriteValueAsync(detachedBuffer, (GattWriteOption) 0);
            if (result > 0)
              BluetoothChannel_LE.MiConLE_Logger.Error("Failed to write chunk! Reason: " + result.ToString());
          }
          while (result != null && attempt-- > 0);
          if (result == 0)
          {
            if (BluetoothChannel_LE.MiConLE_Logger.IsTraceEnabled)
            {
              string traceData = Utility.ByteArrayToHexString(payload) + " ASCII'" + Utility.ByteArrayToAsciiString(payload) + "'";
              BluetoothChannel_LE.MiConLE_Logger.Trace("BLE TX: " + traceData);
              traceData = (string) null;
            }
            offset += blockSize;
            writer = (DataWriter) null;
            payload = (byte[]) null;
            detachedBuffer = (IBuffer) null;
          }
          else
          {
            string m = "TX not successfully. GattCommunicationStatus: " + result.ToString();
            BluetoothChannel_LE.MiConLE_Logger.Error(m);
            throw new Exception(m);
          }
        }
      }
      catch (Exception ex) when (ex.HResult == BluetoothChannel_LE.E_BLUETOOTH_ATT_INVALID_PDU)
      {
        string m = ex.HResult != BluetoothChannel_LE.E_BLUETOOTH_ATT_INVALID_PDU ? "Transmit exception." : "Transmit exception. E_BLUETOOTH_ATT_INVALID_PDU";
        BluetoothChannel_LE.MiConLE_Logger.Error(m);
        throw new Exception(m, ex);
      }
    }

    public void Open()
    {
      if (this.IsOpen)
        return;
      BluetoothChannel_LE.MiConLE_Logger.Trace(nameof (Open));
      Task task = Task.Run((Func<Task>) (() => this.ConnectAsync()));
      if (!task.Wait(5000))
      {
        BluetoothChannel_LE.MiConLE_Logger.Trace("Open timeout");
        task.Dispose();
        throw new Exception("Bluetooth connection error. (timeout)");
      }
    }

    public void Close()
    {
      BluetoothChannel_LE.MiConLE_Logger.Trace(nameof (Close));
      if (this.ToMinoConnect != null)
      {
        if (this.ToMinoConnect.Service != null)
          this.ToMinoConnect.Service.Dispose();
        this.ToMinoConnect = (GattCharacteristic) null;
      }
      if (this.FromMinoConnect != null)
        this.FromMinoConnect = (GattCharacteristic) null;
      if (this.SelectedMiCon == null)
        return;
      this.SelectedMiCon.Dispose();
      this.SelectedMiCon = (BluetoothLEDevice) null;
    }

    public void Write(byte[] buffer, int offset, int count)
    {
      if (this.ToMinoConnect == null)
        throw new NullReferenceException("ToMinoConnect");
      if (offset + count > buffer.Length)
        throw new ArgumentException("offset + count > length");
      byte[] bytesToWrite = new byte[count];
      Buffer.BlockCopy((Array) buffer, offset, (Array) bytesToWrite, 0, count);
      lock (this._syncLock)
      {
        Task task = Task.Run((Func<Task>) (() => this.SendData(bytesToWrite)));
        if (!task.Wait(5000))
        {
          BluetoothChannel_LE.MiConLE_Logger.Trace("Close");
          task.Dispose();
          throw new Exception("Bluetooth write error. (timeout)");
        }
      }
    }

    public void Write(string text)
    {
      if (this.ToMinoConnect == null)
        throw new NullReferenceException("ToMinoConnect");
      switch (text)
      {
        case null:
          throw new ArgumentNullException(nameof (text));
        case "":
          break;
        default:
          byte[] bytesToWrite = Encoding.ASCII.GetBytes(text);
          lock (this._syncLock)
          {
            Task task = Task.Run((Func<Task>) (() => this.SendData(bytesToWrite)));
            if (task.Wait(5000))
              break;
            task.Dispose();
            throw new Exception("Bluetooth write error. (timeout)");
          }
      }
    }

    public void WriteBaudrateCarrier(int numberOfBytes)
    {
      this.Write("#bc " + numberOfBytes.ToString() + "\n");
    }

    internal string GetReceivedDataAsString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (this.ReceivedData.Count > 0)
        stringBuilder.Append((char) this.ReceivedData.Dequeue());
      return stringBuilder.ToString();
    }

    internal List<byte> GetReceivedData()
    {
      List<byte> receivedData = new List<byte>();
      while (this.ReceivedData.Count > 0)
        receivedData.Add(this.ReceivedData.Dequeue());
      return receivedData;
    }

    internal byte[] GetReceivedData(int count)
    {
      if (this.ReceivedData.Count < count)
        throw new Exception("Count bytes not available");
      byte[] receivedData = new byte[count];
      for (int index = 0; index < count; ++index)
        receivedData[index] = this.ReceivedData.Dequeue();
      return receivedData;
    }

    public int Read(byte[] buffer, int offset, int count)
    {
      int count1 = count;
      if (this.ReceivedData.Count < count1)
        count1 = this.ReceivedData.Count;
      if (count1 > buffer.Length - offset)
        count1 = buffer.Length - offset;
      if (count1 < 0)
        count1 = 0;
      for (int index = 0; index < count1; ++index)
        buffer[offset++] = this.ReceivedData.Dequeue();
      if (BluetoothChannel_LE.MiConLE_Logger.IsTraceEnabled)
      {
        byte[] numArray = new byte[count1];
        Buffer.BlockCopy((Array) buffer, offset - count1, (Array) numArray, 0, count1);
        string hexString = Utility.ByteArrayToHexString(numArray);
        BluetoothChannel_LE.MiConLE_Logger.Trace("ReadPoll: " + count1.ToString() + " of " + count.ToString() + ": " + hexString);
      }
      return count;
    }

    public int ReadByte()
    {
      if (this.ReceivedData.Count < 1)
      {
        BluetoothChannel_LE.MiConLE_Logger.Trace("ReadByte, but no data");
        return 0;
      }
      byte num = this.ReceivedData.Dequeue();
      if (BluetoothChannel_LE.MiConLE_Logger.IsTraceEnabled)
        BluetoothChannel_LE.MiConLE_Logger.Trace("ReadByte: 0x" + num.ToString("x02"));
      return (int) num;
    }

    public void DiscardInBuffer()
    {
      BluetoothChannel_LE.MiConLE_Logger.Trace(nameof (DiscardInBuffer));
      this.ReceivedData.Clear();
    }

    public void DiscardOutBuffer()
    {
      BluetoothChannel_LE.MiConLE_Logger.Trace(nameof (DiscardOutBuffer));
    }

    public static bool IsBluetooth(string portName)
    {
      if (string.IsNullOrEmpty(portName))
        throw new ArgumentNullException(nameof (portName));
      return false;
    }

    public void Dispose()
    {
      BluetoothChannel_LE.MiConLE_Logger.Trace(nameof (Dispose));
      this.Close();
    }
  }
}
