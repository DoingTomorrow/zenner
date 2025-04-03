// Decompiled with JetBrains decompiler
// Type: AForge.Video.ByteArrayUtils
// Assembly: AForge.Video, Version=2.2.5.0, Culture=neutral, PublicKeyToken=cbfb6e07d173c401
// MVID: 869827A8-29D1-478E-B314-48676C61CBC2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.dll

using System;

#nullable disable
namespace AForge.Video
{
  internal static class ByteArrayUtils
  {
    public static bool Compare(byte[] array, byte[] needle, int startIndex)
    {
      int length = needle.Length;
      int index1 = 0;
      int index2 = startIndex;
      while (index1 < length)
      {
        if ((int) array[index2] != (int) needle[index1])
          return false;
        ++index1;
        ++index2;
      }
      return true;
    }

    public static int Find(byte[] array, byte[] needle, int startIndex, int sourceLength)
    {
      int length = needle.Length;
      while (sourceLength >= length)
      {
        int num = Array.IndexOf<byte>(array, needle[0], startIndex, sourceLength - length + 1);
        if (num == -1)
          return -1;
        int index1 = 0;
        for (int index2 = num; index1 < length && (int) array[index2] == (int) needle[index1]; ++index2)
          ++index1;
        if (index1 == length)
          return num;
        sourceLength -= num - startIndex + 1;
        startIndex = num + 1;
      }
      return -1;
    }
  }
}
