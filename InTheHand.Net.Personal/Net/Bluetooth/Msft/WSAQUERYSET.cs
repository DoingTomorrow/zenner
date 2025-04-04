// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.WSAQUERYSET
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  [StructLayout(LayoutKind.Sequential, Size = 60)]
  internal struct WSAQUERYSET
  {
    public int dwSize;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpszServiceInstanceName;
    public IntPtr lpServiceClassId;
    private IntPtr lpVersion;
    private IntPtr lpszComment;
    public int dwNameSpace;
    private IntPtr lpNSProviderId;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpszContext;
    private int dwNumberOfProtocols;
    private IntPtr lpafpProtocols;
    private IntPtr lpszQueryString;
    public int dwNumberOfCsAddrs;
    public IntPtr lpcsaBuffer;
    private int dwOutputFlags;
    public IntPtr lpBlob;
  }
}
