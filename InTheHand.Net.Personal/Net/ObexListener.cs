// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net
{
  public class ObexListener
  {
    private const int ServiceRecordExpectedPortOffset = 26;
    private static readonly byte[] ServiceRecordExpected = new byte[39]
    {
      (byte) 53,
      (byte) 37,
      (byte) 9,
      (byte) 0,
      (byte) 1,
      (byte) 53,
      (byte) 3,
      (byte) 25,
      (byte) 17,
      (byte) 5,
      (byte) 9,
      (byte) 0,
      (byte) 4,
      (byte) 53,
      (byte) 17,
      (byte) 53,
      (byte) 3,
      (byte) 25,
      (byte) 1,
      (byte) 0,
      (byte) 53,
      (byte) 5,
      (byte) 25,
      (byte) 0,
      (byte) 3,
      (byte) 8,
      (byte) 0,
      (byte) 53,
      (byte) 3,
      (byte) 25,
      (byte) 0,
      (byte) 8,
      (byte) 9,
      (byte) 3,
      (byte) 3,
      (byte) 53,
      (byte) 2,
      (byte) 8,
      byte.MaxValue
    };
    private ObexTransport transport;
    private IrDAListener iListener;
    private BluetoothListener bListener;
    private BluetoothPublicFactory _btFactory;
    private TcpListener tListener;
    private volatile bool listening;

    public ObexListener()
      : this(ObexTransport.Bluetooth)
    {
    }

    public ObexListener(ObexTransport transport)
      : this(transport, (BluetoothPublicFactory) null)
    {
    }

    private ObexListener(ObexTransport transport, BluetoothPublicFactory factory)
    {
      this._btFactory = factory;
      switch (transport)
      {
        case ObexTransport.IrDA:
          this.iListener = new IrDAListener("OBEX");
          break;
        case ObexTransport.Bluetooth:
          ServiceRecord serviceRecord = ObexListener.CreateServiceRecord();
          this.bListener = this._btFactory != null ? this._btFactory.CreateBluetoothListener(BluetoothService.ObexObjectPush, serviceRecord) : new BluetoothListener(BluetoothService.ObexObjectPush, serviceRecord);
          this.bListener.ServiceClass = ServiceClass.ObjectTransfer;
          break;
        case ObexTransport.Tcp:
          this.tListener = new TcpListener(IPAddress.Any, 650);
          break;
        default:
          throw new ArgumentException("Invalid transport specified");
      }
      this.transport = transport;
    }

    internal ObexListener(BluetoothPublicFactory factory)
      : this(ObexTransport.Bluetooth, factory)
    {
    }

    private static ServiceRecord CreateServiceRecord()
    {
      ObexListener.CreateEnglishUtf8PrimaryLanguageServiceElement();
      return new ServiceRecord(new ServiceAttribute[3]
      {
        new ServiceAttribute((ServiceAttributeId) 1, new ServiceElement(ElementType.ElementSequence, new ServiceElement[1]
        {
          new ServiceElement(ElementType.Uuid16, (object) (ushort) 4357)
        })),
        new ServiceAttribute((ServiceAttributeId) 4, ServiceRecordHelper.CreateGoepProtocolDescriptorList()),
        new ServiceAttribute((ServiceAttributeId) 771, new ServiceElement(ElementType.ElementSequence, new ServiceElement[1]
        {
          new ServiceElement(ElementType.UInt8, (object) byte.MaxValue)
        }))
      });
    }

    private static ServiceElement CreateEnglishUtf8PrimaryLanguageServiceElement()
    {
      return LanguageBaseItem.CreateElementSequenceFromList(new LanguageBaseItem[1]
      {
        new LanguageBaseItem("en", (short) 106, (ServiceAttributeId) 256)
      });
    }

    private void TestRecordAsExpected(byte[] serviceRecord_Expected, BluetoothListener bListener)
    {
      serviceRecord_Expected[26] = (byte) bListener.LocalEndPoint.Port;
      byte[] byteArray = bListener.ServiceRecord.ToByteArray();
      ServiceRecord.CreateServiceRecordFromBytes(serviceRecord_Expected);
      ObexListener.Arrays_Equal(serviceRecord_Expected, byteArray);
    }

    internal static void Arrays_Equal(byte[] expected, byte[] actual)
    {
      if (expected.Length != actual.Length)
        throw new InvalidOperationException("diff lengs!!!");
      for (int index = 0; index < expected.Length; ++index)
      {
        if (!expected[index].Equals(actual[index]))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "diff at {0}, x: 0x{1:X2}, y: 0x{2:X2} !!!", (object) index, (object) expected[index], (object) actual[index]));
      }
    }

    public bool Authenticate
    {
      get => this.IsBluetoothListener() && this.bListener.Authenticate;
      set
      {
        if (!this.IsBluetoothListener())
          throw new InvalidOperationException("Setting Authenticate is only supported on Bluetooth ObexListeners.");
        this.bListener.Authenticate = value;
      }
    }

    public bool Encrypt
    {
      get => this.IsBluetoothListener() && this.bListener.Encrypt;
      set
      {
        if (!this.IsBluetoothListener())
          throw new InvalidOperationException("Setting Encrypt is only supported on Bluetooth ObexListeners.");
        this.bListener.Encrypt = value;
      }
    }

    private bool IsBluetoothListener() => this.bListener != null;

    public bool IsListening => this.listening;

    public void Start()
    {
      switch (this.transport)
      {
        case ObexTransport.IrDA:
          this.iListener.Start();
          break;
        case ObexTransport.Bluetooth:
          this.bListener.Start();
          this.TestRecordAsExpected(ObexListener.ServiceRecordExpected, this.bListener);
          break;
        case ObexTransport.Tcp:
          this.tListener.Start();
          break;
      }
      this.listening = true;
    }

    public void Stop()
    {
      this.listening = false;
      switch (this.transport)
      {
        case ObexTransport.IrDA:
          this.iListener.Stop();
          break;
        case ObexTransport.Bluetooth:
          this.bListener.Stop();
          break;
        case ObexTransport.Tcp:
          this.tListener.Stop();
          break;
      }
    }

    public void Close()
    {
      if (!this.listening)
        return;
      this.Stop();
    }

    public ObexListenerContext GetContext()
    {
      if (!this.listening)
        throw new InvalidOperationException("Listener not started");
      try
      {
        SocketClientAdapter s;
        switch (this.transport)
        {
          case ObexTransport.IrDA:
            s = new SocketClientAdapter(this.iListener.AcceptIrDAClient());
            break;
          case ObexTransport.Bluetooth:
            s = new SocketClientAdapter(this.bListener.AcceptBluetoothClient());
            break;
          default:
            s = new SocketClientAdapter(this.tListener.AcceptTcpClient());
            break;
        }
        return new ObexListenerContext((SocketAdapter) s);
      }
      catch
      {
        return (ObexListenerContext) null;
      }
    }
  }
}
