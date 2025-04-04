// Decompiled with JetBrains decompiler
// Type: InTheHand.Runtime.InteropServices.Marshal32
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Runtime.InteropServices
{
  internal static class Marshal32
  {
    public static IntPtr ReadIntPtr(IntPtr ptr, int index)
    {
      return IntPtr.Size != 8 ? new IntPtr(Marshal.ReadInt32(ptr, index)) : new IntPtr(Marshal.ReadInt64(ptr, index));
    }

    public static IntPtr ReadIntPtr(byte[] buf, int index)
    {
      return IntPtr.Size != 8 ? new IntPtr(BitConverter.ToInt32(buf, index)) : new IntPtr(BitConverter.ToInt64(buf, index));
    }

    public static void WriteIntPtr(IntPtr ptr, int index, IntPtr value)
    {
      if (IntPtr.Size == 8)
        Marshal.WriteInt64(ptr, index, value.ToInt64());
      else
        Marshal.WriteInt32(ptr, index, value.ToInt32());
    }

    public static void WriteIntPtr(byte[] buf, int index, IntPtr value)
    {
      (IntPtr.Size != 8 ? (Array) BitConverter.GetBytes(value.ToInt32()) : (Array) BitConverter.GetBytes(value.ToInt64())).CopyTo((Array) buf, index);
    }
  }
}
