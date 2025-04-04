// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothComponent
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class BluetoothComponent : Component
  {
    private readonly BluetoothClient m_cli;

    public BluetoothComponent()
      : this(new BluetoothClient())
    {
    }

    public BluetoothComponent(BluetoothClient cli)
    {
      this.m_cli = cli != null ? cli : throw new ArgumentNullException(nameof (cli));
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        this.m_cli.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public event EventHandler<DiscoverDevicesEventArgs> DiscoverDevicesComplete;

    protected void OnDiscoveryComplete(DiscoverDevicesEventArgs e)
    {
      EventHandler<DiscoverDevicesEventArgs> discoverDevicesComplete = this.DiscoverDevicesComplete;
      if (discoverDevicesComplete == null)
        return;
      discoverDevicesComplete((object) this, e);
    }

    public event EventHandler<DiscoverDevicesEventArgs> DiscoverDevicesProgress;

    protected void OnDiscoveryProgress(DiscoverDevicesEventArgs e)
    {
      EventHandler<DiscoverDevicesEventArgs> discoverDevicesProgress = this.DiscoverDevicesProgress;
      if (discoverDevicesProgress == null)
        return;
      discoverDevicesProgress((object) this, e);
    }

    public void DiscoverDevicesAsync(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      object state)
    {
      AsyncOperation operation = AsyncOperationManager.CreateOperation(state);
      this.DoRemembered(authenticated, remembered, discoverableOnly, operation);
      this.m_cli.BeginDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, new AsyncCallback(this.HandleDiscoComplete), (object) operation, new BluetoothClient.LiveDiscoveryCallback(this.HandleDiscoNewDevice), (object) operation);
    }

    private void DoRemembered(
      bool authenticated,
      bool remembered,
      bool discoverableOnly,
      AsyncOperation asyncOp)
    {
      if (!authenticated && !remembered || discoverableOnly)
        return;
      BluetoothDeviceInfo[] devices = this.m_cli.DiscoverDevices((int) byte.MaxValue, authenticated, remembered, false, false);
      if (devices.Length == 0)
        return;
      DiscoverDevicesEventArgs devicesEventArgs = new DiscoverDevicesEventArgs(devices, asyncOp.UserSuppliedState);
      SendOrPostCallback d = (SendOrPostCallback) (args => this.OnDiscoveryProgress((DiscoverDevicesEventArgs) args));
      asyncOp.Post(d, (object) devicesEventArgs);
    }

    private void HandleDiscoComplete(IAsyncResult ar)
    {
      AsyncOperation asyncState = (AsyncOperation) ar.AsyncState;
      DiscoverDevicesEventArgs devicesEventArgs;
      try
      {
        devicesEventArgs = new DiscoverDevicesEventArgs(this.m_cli.EndDiscoverDevices(ar), asyncState.UserSuppliedState);
      }
      catch (Exception ex)
      {
        devicesEventArgs = new DiscoverDevicesEventArgs(ex, asyncState.UserSuppliedState);
      }
      SendOrPostCallback d = (SendOrPostCallback) (args => this.OnDiscoveryComplete((DiscoverDevicesEventArgs) args));
      asyncState.PostOperationCompleted(d, (object) devicesEventArgs);
    }

    private void HandleDiscoNewDevice(IBluetoothDeviceInfo newDevice, object state)
    {
      AsyncOperation asyncOperation = (AsyncOperation) state;
      DiscoverDevicesEventArgs devicesEventArgs = new DiscoverDevicesEventArgs(new BluetoothDeviceInfo[1]
      {
        new BluetoothDeviceInfo(newDevice)
      }, asyncOperation.UserSuppliedState);
      SendOrPostCallback d = (SendOrPostCallback) (args => this.OnDiscoveryProgress((DiscoverDevicesEventArgs) args));
      asyncOperation.Post(d, (object) devicesEventArgs);
    }
  }
}
