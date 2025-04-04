// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.BTHNS_SETBLOB
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal class BTHNS_SETBLOB : BTHNS_BLOB
  {
    private const int BTH_SDP_VERSION = 1;
    private GCHandle pVersionHandle;
    private GCHandle pRecordHandle;
    private static readonly int Offset_pSdpVersion_0 = 0;
    private static readonly int Offset_pRecordHandle_4 = IntPtr.Size;
    private static readonly int Offset_fCodService_8 = 2 * IntPtr.Size;
    private static readonly int Offset_ulRecordLength_32 = 2 * IntPtr.Size + 24;
    private static readonly int StructLength_36 = 2 * IntPtr.Size + 28;
    private static bool s_doneAssert;

    public BTHNS_SETBLOB(byte[] record)
    {
      this.m_data = new byte[BTHNS_SETBLOB.StructLength_36 + record.Length];
      this.pVersionHandle = GCHandle.Alloc((object) 1, GCHandleType.Pinned);
      this.pRecordHandle = GCHandle.Alloc((object) (IntPtr) 0, GCHandleType.Pinned);
      IntPtr num1 = this.pVersionHandle.AddrOfPinnedObject();
      IntPtr num2 = this.pRecordHandle.AddrOfPinnedObject();
      Marshal32.WriteIntPtr(this.m_data, BTHNS_SETBLOB.Offset_pSdpVersion_0, num1);
      Marshal32.WriteIntPtr(this.m_data, BTHNS_SETBLOB.Offset_pRecordHandle_4, num2);
      BitConverter.GetBytes(record.Length).CopyTo((Array) this.m_data, BTHNS_SETBLOB.Offset_ulRecordLength_32);
      Buffer.BlockCopy((Array) record, 0, (Array) this.m_data, BTHNS_SETBLOB.StructLength_36, record.Length);
    }

    public IntPtr Handle
    {
      get
      {
        return Marshal32.ReadIntPtr(Marshal32.ReadIntPtr(this.m_data, BTHNS_SETBLOB.Offset_pRecordHandle_4), 0);
      }
      set
      {
        Marshal32.WriteIntPtr(Marshal32.ReadIntPtr(this.m_data, BTHNS_SETBLOB.Offset_pRecordHandle_4), 0, value);
      }
    }

    public uint CodService
    {
      get => BitConverter.ToUInt32(this.m_data, BTHNS_SETBLOB.Offset_fCodService_8);
      set
      {
        BitConverter.GetBytes(value).CopyTo((Array) this.m_data, BTHNS_SETBLOB.Offset_fCodService_8);
      }
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.pVersionHandle.IsAllocated)
          this.pVersionHandle.Free();
        if (!this.pRecordHandle.IsAllocated)
          return;
        this.pRecordHandle.Free();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    [Conditional("DEBUG")]
    public static void AssertCheckLayout()
    {
      if (BTHNS_SETBLOB.s_doneAssert)
        return;
      BTHNS_SETBLOB.s_doneAssert = true;
      int size = IntPtr.Size;
    }
  }
}
