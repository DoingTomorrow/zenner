// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.NullRfcommStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal class NullRfcommStream : CommonRfcommStream
  {
    private readonly LsnrCommands _cmds;

    internal NullRfcommStream(LsnrCommands cmds) => this._cmds = cmds;

    internal void Do_HandleCONNECTED() => this.HandleCONNECTED("testconnected");

    protected override void RemovePortRecords()
    {
    }

    protected override void DoOtherPreDestroy(bool disposing)
    {
    }

    protected override void DoPortClose(bool disposing)
    {
    }

    protected override void DoPortDestroy(bool disposing)
    {
    }

    protected override void DoOtherSetup(BluetoothEndPoint bep, int scn)
    {
    }

    protected override void AddPortRecords()
    {
    }

    protected override void DoOpenClient(int scn, BluetoothAddress addressToConnect)
    {
    }

    protected override void DoOpenServer(int scn)
    {
      if (this._cmds.NextPortShouldConnectImmediately)
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          this.Do_HandleCONNECTED();
        });
      else if (this._cmds.NextOpenServerShouldFail)
        throw new MulticastNotSupportedException("DoOpenServer FAILURE!");
    }

    protected override bool TryBondingIf_inLock(
      BluetoothAddress addressToConnect,
      int ocScn,
      out Exception err)
    {
      err = (Exception) null;
      return false;
    }

    protected override void DoWrite(byte[] p_data, ushort len_to_write, out ushort p_len_written)
    {
      p_len_written = len_to_write;
    }
  }
}
