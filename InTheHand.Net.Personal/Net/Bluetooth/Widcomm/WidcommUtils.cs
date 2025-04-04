// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommUtils
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal static class WidcommUtils
  {
    internal static readonly byte[] SetSecurityLevel_Client_ServiceName = new byte[8]
    {
      (byte) 102,
      (byte) 97,
      (byte) 107,
      (byte) 101,
      (byte) 51,
      (byte) 50,
      (byte) 102,
      (byte) 0
    };

    internal static BTM_SEC ToBTM_SEC(bool isServer, bool authenticate, bool encrypt)
    {
      BTM_SEC btmSec = BTM_SEC.NONE;
      if (!isServer)
      {
        if (encrypt)
          btmSec = btmSec | BTM_SEC.OUT_ENCRYPT | BTM_SEC.OUT_AUTHENTICATE;
        if (authenticate)
          btmSec |= BTM_SEC.OUT_AUTHENTICATE;
      }
      else
      {
        if (encrypt)
          btmSec = btmSec | BTM_SEC.IN_ENCRYPT | BTM_SEC.IN_AUTHENTICATE;
        if (authenticate)
          btmSec |= BTM_SEC.IN_AUTHENTICATE;
      }
      MiscUtils.Trace_WriteLine("{0}; {1},{2}-> {3}", (object) isServer, (object) authenticate, (object) encrypt, (object) btmSec);
      return btmSec;
    }

    internal static BluetoothAddress ToBluetoothAddress(byte[] address_)
    {
      byte[] address = (byte[]) address_.Clone();
      Array.Reverse((Array) address);
      return new BluetoothAddress(address);
    }

    internal static byte[] FromBluetoothAddress(BluetoothAddress address)
    {
      byte[] byteArray = address.ToByteArray();
      byte[] destinationArray = new byte[6];
      Array.Copy((Array) byteArray, (Array) destinationArray, destinationArray.Length);
      Array.Reverse((Array) destinationArray);
      return destinationArray;
    }

    internal static string BdNameToString(byte[] deviceName)
    {
      int deviceNameLength = Array.IndexOf<byte>(deviceName, (byte) 0);
      if (deviceNameLength == -1)
        deviceNameLength = deviceName.Length;
      return WidcommUtils.BdNameToString_(deviceName, deviceNameLength);
    }

    internal static string BdNameToString_(byte[] deviceName, int deviceNameLength)
    {
      if (deviceName == null || deviceName.Length == 0)
        return (string) null;
      if (deviceNameLength < 0 || deviceNameLength > deviceName.Length)
        throw new ArgumentOutOfRangeException(nameof (deviceNameLength));
      return Encoding.UTF8.GetString(deviceName, 0, deviceNameLength);
    }

    internal static ClassOfDevice ToClassOfDevice(byte[] devClass)
    {
      return new ClassOfDevice((uint) WidcommUtils.ToInt32FromBigEndianUInt24(devClass));
    }

    private static int ToInt32FromBigEndianUInt24(byte[] devClass)
    {
      byte[] numArray = new byte[4];
      devClass.CopyTo((Array) numArray, 1);
      return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(numArray, 0));
    }

    internal static string GetTime4Log() => DateTime.Now.TimeOfDay.ToString();

    internal static byte[] GetByteArray(IntPtr p, int count)
    {
      byte[] byteArray = new byte[count];
      for (int ofs = 0; ofs < count; ++ofs)
        byteArray[ofs] = Marshal.ReadByte(p, ofs);
      return byteArray;
    }

    internal static byte[] GetByteArrayNullTerminated(IntPtr p, int maxCount)
    {
      int length = 0;
      for (int ofs = 0; ofs < maxCount; ++ofs)
      {
        if (Marshal.ReadByte(p, ofs) == (byte) 0)
        {
          length = ofs;
          break;
        }
      }
      byte[] arrayNullTerminated = length != maxCount ? new byte[length] : throw new ArgumentException("String too long or not there at all?!");
      for (int ofs = 0; ofs < length; ++ofs)
        arrayNullTerminated[ofs] = Marshal.ReadByte(p, ofs);
      return arrayNullTerminated;
    }

    internal static void GetBluetoothCallbackValues(
      IntPtr bdAddr,
      IntPtr devClass,
      IntPtr deviceName,
      out byte[] bdAddrArr,
      out byte[] devClassArr,
      out byte[] deviceNameArr)
    {
      bdAddrArr = WidcommUtils.GetByteArray(bdAddr, 6);
      devClassArr = WidcommUtils.GetByteArray(devClass, 3);
      deviceNameArr = WidcommUtils.GetByteArrayNullTerminated(deviceName, 249);
    }
  }
}
