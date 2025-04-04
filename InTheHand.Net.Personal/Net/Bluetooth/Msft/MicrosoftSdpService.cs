// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.MicrosoftSdpService
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  public static class MicrosoftSdpService
  {
    public static void RemoveService(IntPtr handle, byte[] sdpRecord)
    {
      BTHNS_SETBLOB bthnsSetblob = new BTHNS_SETBLOB(sdpRecord);
      bthnsSetblob.Handle = handle;
      WSAQUERYSET lpqsRegInfo = new WSAQUERYSET();
      lpqsRegInfo.dwSize = WqsOffset.StructLength_60;
      lpqsRegInfo.dwNameSpace = 16;
      GCHandle handle1 = GCHandle.Alloc((object) bthnsSetblob.ToByteArray(), GCHandleType.Pinned);
      GCHandle gcHandle = GCHandle.Alloc((object) new BLOB(bthnsSetblob.Length, GCHandleHelper.AddrOfPinnedObject(handle1)), GCHandleType.Pinned);
      lpqsRegInfo.lpBlob = gcHandle.AddrOfPinnedObject();
      try
      {
        SocketBluetoothClient.ThrowSocketExceptionForHR(NativeMethods.WSASetService(ref lpqsRegInfo, WSAESETSERVICEOP.RNRSERVICE_DELETE, 0));
      }
      finally
      {
        gcHandle.Free();
        handle1.Free();
        bthnsSetblob.Dispose();
      }
    }

    public static IntPtr SetService(byte[] sdpRecord, ServiceClass cod)
    {
      BTHNS_SETBLOB bthnsSetblob = new BTHNS_SETBLOB(sdpRecord);
      bthnsSetblob.CodService = (uint) cod;
      WSAQUERYSET lpqsRegInfo = new WSAQUERYSET();
      lpqsRegInfo.dwSize = WqsOffset.StructLength_60;
      lpqsRegInfo.dwNameSpace = 16;
      GCHandle handle1 = GCHandle.Alloc((object) bthnsSetblob.ToByteArray(), GCHandleType.Pinned);
      GCHandle gcHandle = GCHandle.Alloc((object) new BLOB(bthnsSetblob.Length, GCHandleHelper.AddrOfPinnedObject(handle1)), GCHandleType.Pinned);
      lpqsRegInfo.lpBlob = gcHandle.AddrOfPinnedObject();
      try
      {
        SocketBluetoothClient.ThrowSocketExceptionForHR(NativeMethods.WSASetService(ref lpqsRegInfo, WSAESETSERVICEOP.RNRSERVICE_REGISTER, 0));
      }
      finally
      {
        gcHandle.Free();
        handle1.Free();
      }
      IntPtr handle2 = bthnsSetblob.Handle;
      bthnsSetblob.Dispose();
      return handle2;
    }
  }
}
