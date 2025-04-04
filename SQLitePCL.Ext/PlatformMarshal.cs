// Decompiled with JetBrains decompiler
// Type: SQLitePCL.PlatformMarshal
// Assembly: SQLitePCL.Ext, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 28DC4D07-0E35-45C1-8EF3-CED69BFBD581
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.Ext.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace SQLitePCL
{
  internal sealed class PlatformMarshal : IPlatformMarshal
  {
    private static IPlatformMarshal instance = (IPlatformMarshal) new PlatformMarshal();

    private PlatformMarshal()
    {
    }

    internal static IPlatformMarshal Instance => PlatformMarshal.instance;

    void IPlatformMarshal.CleanUpStringNativeUTF8(IntPtr nativeString)
    {
      Marshal.FreeHGlobal(nativeString);
    }

    IntPtr IPlatformMarshal.MarshalStringManagedToNativeUTF8(string managedString)
    {
      return ((IPlatformMarshal) this).MarshalStringManagedToNativeUTF8(managedString, out int _);
    }

    IntPtr IPlatformMarshal.MarshalStringManagedToNativeUTF8(string managedString, out int size)
    {
      IntPtr nativeUtF8 = IntPtr.Zero;
      size = 0;
      if (managedString != null)
      {
        byte[] bytes = Encoding.UTF8.GetBytes(managedString);
        size = bytes.Length + 1;
        nativeUtF8 = Marshal.AllocHGlobal(size);
        Marshal.Copy(bytes, 0, nativeUtF8, bytes.Length);
        Marshal.WriteByte(nativeUtF8, size - 1, (byte) 0);
      }
      return nativeUtF8;
    }

    string IPlatformMarshal.MarshalStringNativeUTF8ToManaged(IntPtr nativeString)
    {
      string managed = (string) null;
      if (nativeString != IntPtr.Zero)
      {
        int nativeUtF8Size = ((IPlatformMarshal) this).GetNativeUTF8Size(nativeString);
        byte[] numArray = new byte[nativeUtF8Size - 1];
        Marshal.Copy(nativeString, numArray, 0, nativeUtF8Size - 1);
        managed = Encoding.UTF8.GetString(numArray, 0, numArray.Length);
      }
      return managed;
    }

    int IPlatformMarshal.GetNativeUTF8Size(IntPtr nativeString)
    {
      int ofs = 0;
      if (nativeString != IntPtr.Zero)
      {
        while (Marshal.ReadByte(nativeString, ofs) > (byte) 0)
          ++ofs;
        ++ofs;
      }
      return ofs;
    }

    Delegate IPlatformMarshal.ApplyNativeCallingConventionToFunction(FunctionNative function)
    {
      return (Delegate) new FunctionNativeCdecl(function.Invoke);
    }

    Delegate IPlatformMarshal.ApplyNativeCallingConventionToAggregateStep(AggregateStepNative step)
    {
      return (Delegate) new AggregateStepNativeCdecl(step.Invoke);
    }

    Delegate IPlatformMarshal.ApplyNativeCallingConventionToAggregateFinal(
      AggregateFinalNative final)
    {
      return (Delegate) new AggregateFinalNativeCdecl(final.Invoke);
    }

    IntPtr IPlatformMarshal.MarshalDelegateToNativeFunctionPointer(Delegate del)
    {
      return Marshal.GetFunctionPointerForDelegate(del);
    }

    void IPlatformMarshal.Copy(IntPtr source, byte[] destination, int startIndex, int length)
    {
      Marshal.Copy(source, destination, startIndex, length);
    }

    void IPlatformMarshal.Copy(byte[] source, IntPtr destination, int startIndex, int length)
    {
      Marshal.Copy(source, startIndex, destination, length);
    }
  }
}
