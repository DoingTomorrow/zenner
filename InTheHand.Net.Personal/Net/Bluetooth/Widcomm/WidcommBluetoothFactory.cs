// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothFactory
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBluetoothFactory : WidcommBluetoothFactoryBase
  {
    private static IBtIf s_btIf;
    private static WidcommBtInterface s_btInterface;
    private static WidcommBluetoothSecurity m_btSecurity;
    private static WidcommPortSingleThreader _st;
    internal volatile bool _seenStackDownEvent;
    private List<object> _thingsToKeepAlive = new List<object>();

    public WidcommBluetoothFactory()
    {
      this.DoPowerDownUpReset = true;
      this.EnsureLoaded();
      this.GetPrimaryRadio();
    }

    public bool DoPowerDownUpReset { get; set; }

    internal override void EnsureLoaded()
    {
      if (WidcommBluetoothFactory._st != null)
        WidcommBtInterface.IsWidcommSingleThread(WidcommBluetoothFactory._st);
      lock (typeof (WidcommBluetoothFactory))
      {
        bool seenStackDownEvent = this._seenStackDownEvent;
        this._seenStackDownEvent = false;
        if (seenStackDownEvent)
        {
          if (!this.DoPowerDownUpReset)
          {
            MiscUtils.Trace_WriteLine("Ignoring stack/radio shutdown due to DoPowerDownUpReset=false.");
          }
          else
          {
            MiscUtils.Trace_WriteLine("Restarting due to stack/radio shutdown.");
            bool respectLocks = true;
            ThreadStart dlgt = (ThreadStart) (() => this.Dispose(true, respectLocks));
            WidcommPortSingleThreader singleThreader = this.GetSingleThreader();
            if (singleThreader != null && !WidcommBtInterface.IsWidcommSingleThread(singleThreader))
            {
              respectLocks = false;
              singleThreader.AddCommand<WidcommPortSingleThreader.MiscNoReturnCommand>(new WidcommPortSingleThreader.MiscNoReturnCommand(dlgt)).WaitCompletion();
              WidcommBluetoothFactory.MemoryBarrier();
            }
            else
              dlgt();
            Thread.Sleep(5000);
          }
        }
        if (WidcommBluetoothFactory.s_btIf != null)
          return;
        if (WidcommBluetoothFactory._st == null)
          WidcommBluetoothFactory._st = new WidcommPortSingleThreader();
        Func<IBtIf> dlgt1 = (Func<IBtIf>) (() => (IBtIf) new WidcommBtIf(this));
        WidcommPortSingleThreader singleThreader1 = this.GetSingleThreader();
        IBtIf btIf = singleThreader1 == null || WidcommBtInterface.IsWidcommSingleThread(singleThreader1) ? dlgt1() : singleThreader1.AddCommand<WidcommPortSingleThreader.MiscReturnCommand<IBtIf>>(new WidcommPortSingleThreader.MiscReturnCommand<IBtIf>(dlgt1)).WaitCompletion();
        if (singleThreader1 != null)
        {
          btIf = (IBtIf) new WidcommStBtIf((WidcommBluetoothFactoryBase) this, btIf);
          MiscUtils.Trace_WriteLine("IBtIf using WidcommStBtIf.");
        }
        WidcommBtInterface widcommBtInterface = new WidcommBtInterface(btIf, (WidcommBluetoothFactoryBase) this);
        WidcommBluetoothFactory.s_btIf = btIf;
        WidcommBluetoothFactory.s_btInterface = widcommBtInterface;
      }
    }

    protected override void Dispose(bool disposing) => this.Dispose(disposing, true);

    private void Dispose(bool disposing, bool respectLocks)
    {
      bool flag = false;
      IDisposable btInterface;
      IDisposable disposable;
      try
      {
        if (respectLocks)
        {
          Monitor.Enter((object) typeof (WidcommBluetoothFactory));
          flag = true;
        }
        btInterface = (IDisposable) WidcommBluetoothFactory.s_btInterface;
        WidcommBluetoothFactory.s_btIf = (IBtIf) null;
        WidcommBluetoothFactory.s_btInterface = (WidcommBtInterface) null;
        WidcommBluetoothFactory.m_btSecurity = (WidcommBluetoothSecurity) null;
        disposable = (IDisposable) null;
        if (disposing)
          this._thingsToKeepAlive.Clear();
      }
      finally
      {
        if (flag)
          Monitor.Exit((object) typeof (WidcommBluetoothFactory));
      }
      btInterface?.Dispose();
      disposable?.Dispose();
    }

    [Obsolete("_untested_")]
    internal override void AddThingsToKeepAlive<TObject>(TObject o)
    {
      lock (typeof (WidcommBluetoothFactory))
        this._thingsToKeepAlive.Add((object) o);
    }

    internal override WidcommPortSingleThreader GetSingleThreader() => WidcommBluetoothFactory._st;

    internal override bool IsWidcommSingleThread()
    {
      return WidcommPortSingleThreader.IsWidcommSingleThread(this.GetSingleThreader());
    }

    protected override IBluetoothClient GetBluetoothClient()
    {
      this.EnsureLoaded();
      return (IBluetoothClient) new WidcommBluetoothClient((WidcommBluetoothFactoryBase) this);
    }

    protected override IBluetoothClient GetBluetoothClient(Socket acceptedSocket)
    {
      throw new NotSupportedException("Cannot create a BluetoothClient from a Socket on the Widcomm stack.");
    }

    protected override IBluetoothClient GetBluetoothClientForListener(CommonRfcommStream strm)
    {
      return (IBluetoothClient) new WidcommBluetoothClient((WidcommRfcommStreamBase) strm, (WidcommBluetoothFactoryBase) this);
    }

    protected override IBluetoothClient GetBluetoothClient(BluetoothEndPoint localEP)
    {
      this.EnsureLoaded();
      return (IBluetoothClient) new WidcommBluetoothClient(localEP, (WidcommBluetoothFactoryBase) this);
    }

    protected override IL2CapClient GetL2CapClient()
    {
      this.EnsureLoaded();
      return (IL2CapClient) new WidcommL2CapClient((WidcommBluetoothFactoryBase) this);
    }

    protected override IBluetoothListener GetBluetoothListener()
    {
      this.EnsureLoaded();
      return (IBluetoothListener) new WidcommBluetoothListener((WidcommBluetoothFactoryBase) this);
    }

    internal override WidcommBtInterface GetWidcommBtInterface()
    {
      this.EnsureLoaded();
      return WidcommBluetoothFactory.s_btInterface;
    }

    internal override WidcommRfcommStreamBase GetWidcommRfcommStream()
    {
      this.EnsureLoaded();
      return (WidcommRfcommStreamBase) new WidcommRfcommStream(this.GetWidcommRfcommPort(), this.GetWidcommRfCommIf(), (WidcommBluetoothFactoryBase) this);
    }

    internal override WidcommRfcommStreamBase GetWidcommRfcommStreamWithoutRfcommIf()
    {
      this.EnsureLoaded();
      return (WidcommRfcommStreamBase) new WidcommRfcommStream(this.GetWidcommRfcommPort(), (IRfCommIf) null, (WidcommBluetoothFactoryBase) this);
    }

    internal override IRfcommPort GetWidcommRfcommPort()
    {
      this.EnsureLoaded();
      return (IRfcommPort) new WidcommRfcommPort();
    }

    internal override IRfCommIf GetWidcommRfCommIf()
    {
      this.EnsureLoaded();
      IRfCommIf child = (IRfCommIf) new WidcommRfCommIf();
      if (this.GetSingleThreader() != null)
      {
        child = (IRfCommIf) new WidcommStRfCommIf((WidcommBluetoothFactoryBase) this, child);
        MiscUtils.Trace_WriteLine("IRfCommIf using WidcommStRfCommIf.");
      }
      return child;
    }

    protected override IBluetoothDeviceInfo GetBluetoothDeviceInfo(BluetoothAddress address)
    {
      this.EnsureLoaded();
      return (IBluetoothDeviceInfo) WidcommBluetoothDeviceInfo.CreateFromGivenAddress(address, (WidcommBluetoothFactoryBase) this);
    }

    protected override IBluetoothRadio GetPrimaryRadio()
    {
      this.EnsureLoaded();
      return (IBluetoothRadio) new WidcommBluetoothRadio((WidcommBluetoothFactoryBase) this);
    }

    protected override IBluetoothRadio[] GetAllRadios()
    {
      this.EnsureLoaded();
      return new IBluetoothRadio[1]
      {
        this.GetPrimaryRadio()
      };
    }

    internal override ISdpService GetWidcommSdpService()
    {
      this.EnsureLoaded();
      return (ISdpService) new SdpService();
    }

    protected override IBluetoothSecurity GetBluetoothSecurity()
    {
      this.EnsureLoaded();
      if (WidcommBluetoothFactory.m_btSecurity == null)
        WidcommBluetoothFactory.m_btSecurity = new WidcommBluetoothSecurity((WidcommBluetoothFactoryBase) this);
      return (IBluetoothSecurity) WidcommBluetoothFactory.m_btSecurity;
    }

    internal static WidcommBluetoothFactoryBase GetWidcommIfExists()
    {
      foreach (BluetoothFactory factory in (IEnumerable<BluetoothFactory>) BluetoothFactory.Factories)
      {
        if (factory is WidcommBluetoothFactoryBase widcommIfExists)
          return widcommIfExists;
      }
      throw new InvalidOperationException("No Widcomm.");
    }

    private static void MemoryBarrier() => Thread.MemoryBarrier();
  }
}
