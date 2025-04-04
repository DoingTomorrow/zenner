// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.NullBtListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal class NullBtListener : CommonBluetoothListener
  {
    private volatile bool _disposed;
    private volatile bool _disposedMore;
    private Queue<NullBtListener.LsnrSetting> _portSettings = new Queue<NullBtListener.LsnrSetting>();

    internal NullBtListener(NullBluetoothFactory fcty)
      : base((BluetoothFactory) fcty)
    {
    }

    internal void AddPortSettings(NullBtListener.LsnrSetting[] settings)
    {
      foreach (NullBtListener.LsnrSetting setting in settings)
        this._portSettings.Enqueue(setting);
    }

    protected override CommonRfcommStream GetNewPort()
    {
      NullBtListener.LsnrSetting lsnrSetting = this._portSettings.Dequeue();
      LsnrCommands cmds = new LsnrCommands();
      switch (lsnrSetting)
      {
        case NullBtListener.LsnrSetting.None:
          return (CommonRfcommStream) new NullRfcommStream(cmds);
        case NullBtListener.LsnrSetting.ErrorOnOpenServer:
          cmds.NextOpenServerShouldFail = true;
          goto case NullBtListener.LsnrSetting.None;
        case NullBtListener.LsnrSetting.ConnectsImmediately:
          cmds.NextPortShouldConnectImmediately = true;
          goto case NullBtListener.LsnrSetting.None;
        case NullBtListener.LsnrSetting.ErrorConnectsImmediately:
          throw new NotImplementedException();
        default:
          throw new ArgumentException("Unknown LsnrSetting value: " + (object) lsnrSetting);
      }
    }

    protected override void SetupListener(
      BluetoothEndPoint bep,
      int scn,
      out BluetoothEndPoint liveLocalEP)
    {
      liveLocalEP = new BluetoothEndPoint(BluetoothAddress.None, Guid.Empty, 25);
    }

    protected override void AddCustomServiceRecord(
      ref ServiceRecord fullServiceRecord,
      int livePort)
    {
    }

    protected override void AddSimpleServiceRecord(
      out ServiceRecord fullServiceRecord,
      int livePort,
      Guid serviceClass,
      string serviceName)
    {
      fullServiceRecord = NullBtListener.CreateSimpleServiceRecord(serviceClass, serviceName);
      this.AddCustomServiceRecord(ref fullServiceRecord, livePort);
    }

    private static ServiceRecord CreateSimpleServiceRecord(Guid serviceClass, string serviceName)
    {
      ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
      serviceRecordBuilder.AddServiceClass(serviceClass);
      serviceRecordBuilder.ServiceName = serviceName;
      return serviceRecordBuilder.ServiceRecord;
    }

    protected override bool IsDisposed => this._disposed;

    protected override void OtherDispose(bool disposing) => this._disposed = true;

    protected override void OtherDisposeMore() => this._disposedMore = true;

    internal bool AllDisposed => this._disposed && this._disposedMore;

    internal enum LsnrSetting
    {
      None,
      ErrorOnOpenServer,
      ConnectsImmediately,
      ErrorConnectsImmediately,
    }
  }
}
