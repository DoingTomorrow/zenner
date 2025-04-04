// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.CSADDR_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  [StructLayout(LayoutKind.Sequential, Size = 24)]
  internal struct CSADDR_INFO
  {
    internal IntPtr localAddr;
    internal int localSize;
    internal IntPtr remoteAddr;
    internal int remoteSize;
    internal SocketType iSocketType;
    internal ProtocolType iProtocol;

    public CSADDR_INFO(
      BluetoothAddress local,
      BluetoothAddress remote,
      SocketType type,
      ProtocolType protocol)
    {
      this.localAddr = IntPtr.Zero;
      this.localSize = 0;
      this.remoteAddr = IntPtr.Zero;
      this.remoteSize = 0;
      this.iSocketType = type;
      this.iProtocol = protocol;
      if (local != (BluetoothAddress) null)
      {
        this.localAddr = Marshal.AllocHGlobal(40);
        Marshal.WriteInt64(this.localAddr, 8, local.ToInt64());
        Marshal.WriteInt16(this.localAddr, 0, (short) 32);
        this.localSize = 40;
      }
      if (!(remote != (BluetoothAddress) null))
        return;
      this.remoteAddr = Marshal.AllocHGlobal(40);
      Marshal.WriteInt64(this.remoteAddr, 8, remote.ToInt64());
      this.remoteSize = 40;
      Marshal.WriteInt16(this.remoteAddr, 0, (short) 32);
    }

    public void Dispose()
    {
      if (this.localAddr != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(this.localAddr);
        this.localAddr = IntPtr.Zero;
      }
      if (!(this.remoteAddr != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this.remoteAddr);
      this.remoteAddr = IntPtr.Zero;
    }
  }
}
