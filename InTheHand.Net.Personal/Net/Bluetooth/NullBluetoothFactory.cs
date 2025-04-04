// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.NullBluetoothFactory
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal class NullBluetoothFactory : BluetoothFactory
  {
    internal NullBtListener TheBtLsnr { get; private set; }

    internal NullBluetoothFactory() => TestUtilities.IsUnderTestHarness();

    protected override void Dispose(bool disposing)
    {
    }

    protected override IBluetoothRadio GetPrimaryRadio()
    {
      return (IBluetoothRadio) new NullBluetoothFactory.NullRadio();
    }

    protected override IBluetoothRadio[] GetAllRadios()
    {
      return new IBluetoothRadio[1]
      {
        this.GetPrimaryRadio()
      };
    }

    protected override IBluetoothClient GetBluetoothClient()
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    protected override IBluetoothClient GetBluetoothClient(Socket acceptedSocket)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    protected override IBluetoothClient GetBluetoothClientForListener(
      CommonRfcommStream acceptedStream)
    {
      return (IBluetoothClient) new NullBtCli(this, acceptedStream);
    }

    protected override IBluetoothClient GetBluetoothClient(BluetoothEndPoint localEP)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    protected override IBluetoothDeviceInfo GetBluetoothDeviceInfo(BluetoothAddress address)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    protected override IBluetoothListener GetBluetoothListener()
    {
      this.TheBtLsnr = new NullBtListener(this);
      return (IBluetoothListener) this.TheBtLsnr;
    }

    protected override IBluetoothSecurity GetBluetoothSecurity()
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    internal class NullRadio : IBluetoothRadio
    {
      public string Remote => (string) null;

      public virtual ClassOfDevice ClassOfDevice => new ClassOfDevice(16721932U);

      public virtual IntPtr Handle
      {
        get => throw new NotImplementedException("The method or operation is not implemented.");
      }

      public virtual HardwareStatus HardwareStatus => HardwareStatus.Shutdown;

      public virtual int LmpSubversion => 99;

      LmpVersion IBluetoothRadio.LmpVersion => LmpVersion.Unknown;

      public virtual int HciRevision => 99;

      HciVersion IBluetoothRadio.HciVersion => HciVersion.Unknown;

      public virtual BluetoothAddress LocalAddress => BluetoothAddress.Parse("00:11:22:33:44:55");

      public virtual Manufacturer Manufacturer => Manufacturer.AccelSemiconductor;

      public virtual RadioMode Mode
      {
        get => RadioMode.PowerOff;
        set => throw new NotImplementedException("The method or operation is not implemented.");
      }

      public virtual string Name
      {
        get => nameof (NullRadio);
        set => throw new NotImplementedException("The method or operation is not implemented.");
      }

      public virtual Manufacturer SoftwareManufacturer => Manufacturer.AccelSemiconductor;
    }
  }
}
