// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.API_Access
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public class API_Access
  {
    private static int METHOD_BUFFERED = 0;
    private static int FILE_ANY_ACCESS = 0;
    private static int FILE_DEVICE_HAL = 257;
    private const int ERROR_NOT_SUPPORTED = 50;
    private const int ERROR_INSUFFICIENT_BUFFER = 122;
    private static int IOCTL_HAL_GET_DEVICEID = API_Access.FILE_DEVICE_HAL << 16 | API_Access.FILE_ANY_ACCESS << 14 | 84 | API_Access.METHOD_BUFFERED;

    [DllImport("coredll.dll", SetLastError = true)]
    private static extern bool KernelIoControl(
      int dwIoControlCode,
      IntPtr lpInBuf,
      int nInBufSize,
      byte[] lpOutBuf,
      int nOutBufSize,
      ref int lpBytesReturned);

    public static string GetDeviceID()
    {
      byte[] lpOutBuf = new byte[256];
      bool flag = false;
      int nOutBufSize = lpOutBuf.Length;
      BitConverter.GetBytes(nOutBufSize).CopyTo((Array) lpOutBuf, 0);
      int lpBytesReturned = 0;
      while (!flag)
      {
        if (API_Access.KernelIoControl(API_Access.IOCTL_HAL_GET_DEVICEID, IntPtr.Zero, 0, lpOutBuf, nOutBufSize, ref lpBytesReturned))
        {
          flag = true;
        }
        else
        {
          switch (Marshal.GetLastWin32Error())
          {
            case 50:
              throw new NotSupportedException("IOCTL_HAL_GET_DEVICEID nicht unsterstützt");
            case 122:
              nOutBufSize = BitConverter.ToInt32(lpOutBuf, 0);
              lpOutBuf = new byte[nOutBufSize];
              BitConverter.GetBytes(nOutBufSize).CopyTo((Array) lpOutBuf, 0);
              break;
            default:
              throw new SystemException("Fehler beim Auslesen der DeviceID");
          }
        }
      }
      int int32_1 = BitConverter.ToInt32(lpOutBuf, 4);
      int int32_2 = BitConverter.ToInt32(lpOutBuf, 12);
      int int32_3 = BitConverter.ToInt32(lpOutBuf, 16);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("{0:X8}-{1:X4}-{2:X4}-{3:X4}-", (object) BitConverter.ToInt32(lpOutBuf, int32_1), (object) BitConverter.ToInt16(lpOutBuf, int32_1 + 4), (object) BitConverter.ToInt16(lpOutBuf, int32_1 + 6), (object) BitConverter.ToInt16(lpOutBuf, int32_1 + 8)));
      for (int index = int32_2; index < int32_2 + int32_3; ++index)
        stringBuilder.Append(string.Format("{0:X2}", (object) lpOutBuf[index]));
      return stringBuilder.ToString();
    }
  }
}
