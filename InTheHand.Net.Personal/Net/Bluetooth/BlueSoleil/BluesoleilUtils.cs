// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BluesoleilUtils
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal static class BluesoleilUtils
  {
    [DebuggerNonUserCode]
    internal static void CheckAndThrow(BtSdkError ret, string descr)
    {
      if (ret != BtSdkError.OK)
      {
        SocketError? nullable = new SocketError?();
        switch (ret)
        {
          case BtSdkError.NO_SERVICE:
            nullable = new SocketError?(SocketError.ConnectionRefused);
            break;
          case BtSdkError.SDK_UNINIT:
            nullable = new SocketError?(SocketError.NotInitialized);
            break;
          case BtSdkError.PAGE_TIMEOUT:
            nullable = new SocketError?(SocketError.TimedOut);
            break;
        }
        nullable = !nullable.HasValue ? new SocketError?(SocketError.NotSocket) : throw new BlueSoleilSocketException(ret, nullable.Value);
      }
    }

    internal static Exception ErrorConnectIsNonRfcomm()
    {
      return (Exception) new BlueSoleilSocketException(BtSdkError.OK, SocketError.MessageSize);
    }

    internal static void Assert(BtSdkError ret, string descr)
    {
    }

    public static string BtSdkErrorToString(BtSdkError ret)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}=0x{1:X4}", (object) ret, (object) (int) ret);
    }

    internal static BluetoothAddress ToBluetoothAddress(byte[] address_)
    {
      return new BluetoothAddress((byte[]) address_.Clone());
    }

    internal static byte[] FromBluetoothAddress(BluetoothAddress address)
    {
      byte[] byteArray = address.ToByteArray();
      byte[] destinationArray = new byte[6];
      Array.Copy((Array) byteArray, (Array) destinationArray, destinationArray.Length);
      return destinationArray;
    }

    internal static string FromNameString(byte[] arr, ushort? bufferLen)
    {
      int num = Array.IndexOf<byte>(arr, (byte) 0);
      int count = num != -1 ? num : arr.Length;
      if (bufferLen.HasValue && (int) bufferLen.Value < count)
        count = (int) bufferLen.Value;
      return Encoding.UTF8.GetString(arr, 0, count);
    }

    internal static string FromNameString(byte[] arr)
    {
      return BluesoleilUtils.FromNameString(arr, new ushort?());
    }

    internal static Manufacturer FromManufName(ushort mn) => (Manufacturer) mn;

    internal static string FixedLengthArrayToStringUtf8(byte[] arr)
    {
      int count = Array.IndexOf<byte>(arr, (byte) 0);
      if (count == -1)
        count = arr.Length;
      return Encoding.UTF8.GetString(arr, 0, count);
    }
  }
}
