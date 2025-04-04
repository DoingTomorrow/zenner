// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.SdpDiscoveryRecordsBuffer
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class SdpDiscoveryRecordsBuffer : SdpDiscoveryRecordsBufferBase
  {
    private const int SizeOfOneRecord = 127;
    private IntPtr m_pBuffer;
    private int m_filledCount;
    private readonly WidcommBluetoothFactoryBase _fcty;

    internal SdpDiscoveryRecordsBuffer(
      WidcommBluetoothFactoryBase fcty,
      IntPtr pList,
      int recordCount,
      ServiceDiscoveryParams requestArgs)
      : base(requestArgs)
    {
      if (pList == IntPtr.Zero)
      {
        GC.SuppressFinalize((object) this);
        throw new ArgumentException("The native pointer pList is NULL.");
      }
      this.m_filledCount = recordCount;
      this.m_pBuffer = pList;
      this._fcty = fcty;
    }

    internal IntPtr Buffer
    {
      get
      {
        this.EnsureNotDisposed();
        return this.m_pBuffer;
      }
    }

    public override int RecordCount
    {
      get
      {
        this.EnsureNotDisposed();
        return this.m_filledCount != -1 ? this.m_filledCount : throw new InvalidOperationException("Buffer not yet filled.");
      }
    }

    public override int[] Hack_GetPorts()
    {
      return this.GetPorts(new SdpDiscoveryRecordsBuffer.GetPortsMethod(SdpDiscoveryRecordsBuffer.NativeMethods.SdpDiscoveryRec_GetRfcommPorts));
    }

    public override int[] Hack_GetPsms()
    {
      return this.GetPorts(new SdpDiscoveryRecordsBuffer.GetPortsMethod(SdpDiscoveryRecordsBuffer.NativeMethods.SdpDiscoveryRec_GetL2CapPsms));
    }

    private int[] GetPorts(SdpDiscoveryRecordsBuffer.GetPortsMethod doGet)
    {
      this.EnsureNotDisposed();
      int[] ports = new int[this.RecordCount];
      for (int index = 0; index < ports.Length; ++index)
        ports[index] = -5;
      doGet(this.Buffer, ports.Length, ports);
      return ports;
    }

    protected override SdpDiscoveryRecordsBufferBase.SimpleInfo[] GetSimpleInfo()
    {
      SdpDiscoveryRecordsBufferBase.SimpleInfo[] simpleInfo = new SdpDiscoveryRecordsBufferBase.SimpleInfo[this.RecordCount];
      GCHandle gcHandle = GCHandle.Alloc((object) simpleInfo, GCHandleType.Pinned);
      try
      {
        SdpDiscoveryRecordsBuffer.NativeMethods.SdpDiscoveryRec_GetSimpleInfo(this.Buffer, simpleInfo.Length, gcHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof (SdpDiscoveryRecordsBufferBase.SimpleInfo)));
      }
      finally
      {
        gcHandle.Free();
      }
      return simpleInfo;
    }

    ~SdpDiscoveryRecordsBuffer() => this.Dispose(false);

    protected override void Dispose(bool disposing)
    {
      if (!(this.m_pBuffer != IntPtr.Zero))
        return;
      SdpDiscoveryRecordsBuffer.NativeMethods.SdpDiscoveryRec_DeleteArray(this.m_pBuffer);
      this.m_pBuffer = IntPtr.Zero;
      this.m_filledCount = -1;
    }

    protected override void EnsureNotDisposed()
    {
      if (this.m_pBuffer == IntPtr.Zero)
        throw new ObjectDisposedException(nameof (SdpDiscoveryRecordsBuffer));
    }

    private static class NativeMethods
    {
      [DllImport("32feetWidcomm")]
      internal static extern void SdpDiscoveryRec_GetRfcommPorts(
        IntPtr p_list,
        int recordCount,
        [Out] int[] ports);

      [DllImport("32feetWidcomm")]
      internal static extern void SdpDiscoveryRec_GetL2CapPsms(
        IntPtr p_list,
        int recordCount,
        [Out] int[] ports);

      [DllImport("32feetWidcomm")]
      internal static extern void SdpDiscoveryRec_GetSimpleInfo(
        IntPtr p_list,
        int recordCount,
        IntPtr info,
        int cb);

      [DllImport("32feetWidcomm")]
      internal static extern void SdpDiscoveryRec_DeleteArray(IntPtr p_list);
    }

    private delegate void GetPortsMethod(IntPtr p_list, int recordCount, [Out] int[] ports);
  }
}
