// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.ZlibConstants
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zlib
{
  public static class ZlibConstants
  {
    public const int WindowBitsMax = 15;
    public const int WindowBitsDefault = 15;
    public const int Z_OK = 0;
    public const int Z_STREAM_END = 1;
    public const int Z_NEED_DICT = 2;
    public const int Z_STREAM_ERROR = -2;
    public const int Z_DATA_ERROR = -3;
    public const int Z_BUF_ERROR = -5;
    public const int WorkingBufferSizeDefault = 16384;
    public const int WorkingBufferSizeMin = 1024;
  }
}
