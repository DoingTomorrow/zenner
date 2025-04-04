// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.BluezSocketOptionHelper
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal class BluezSocketOptionHelper : ISocketOptionHelper
  {
    private readonly Socket _sock;
    private StackConsts.RFCOMM_LM _linkModeSetting;

    internal BluezSocketOptionHelper(Socket sock) => this._sock = sock;

    private StackConsts.RFCOMM_LM ReadLinkMode()
    {
      StackConsts.RFCOMM_LM socketOption = (StackConsts.RFCOMM_LM) this._sock.GetSocketOption((SocketOptionLevel) 18, SocketOptionName.TypeOfService);
      Console.WriteLine("Read: {0} 0x{0:X}", (object) socketOption);
      return socketOption;
    }

    private void SetOrClear(StackConsts.RFCOMM_LM bit, bool value)
    {
      this._linkModeSetting &= ~bit;
      if (value)
        this._linkModeSetting |= bit;
      Console.WriteLine("Setting: {0} 0x{0:X}", (object) this._linkModeSetting);
      this._sock.SetSocketOption((SocketOptionLevel) 18, SocketOptionName.TypeOfService, (int) this._linkModeSetting);
    }

    private static bool IsSet(StackConsts.RFCOMM_LM value, StackConsts.RFCOMM_LM bit)
    {
      return (StackConsts.RFCOMM_LM) 0 != (value & bit);
    }

    public bool Authenticate
    {
      get => BluezSocketOptionHelper.IsSet(this.ReadLinkMode(), StackConsts.RFCOMM_LM.Auth);
      set => this.SetOrClear(StackConsts.RFCOMM_LM.Auth, value);
    }

    public bool Encrypt
    {
      get => BluezSocketOptionHelper.IsSet(this.ReadLinkMode(), StackConsts.RFCOMM_LM.Encrypt);
      set => this.SetOrClear(StackConsts.RFCOMM_LM.Encrypt, value);
    }

    public void SetPin(BluetoothAddress device, string pin) => throw new NotImplementedException();
  }
}
