// Decompiled with JetBrains decompiler
// Type: MBusLib.Utility.ArrayExtension
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;

#nullable disable
namespace MBusLib.Utility
{
  public static class ArrayExtension
  {
    public static T[] SubArray<T>(this T[] data, int index)
    {
      int length = data.Length - index;
      return data.SubArray<T>(index, length);
    }

    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
      if (index == 0 && data.Length == length)
        return data;
      if (index + length > data.Length)
        throw new Exception(string.Format("Failed to get sub array! Buffer length: {0}, Index: {1}, Length: {2}", (object) data.Length, (object) index, (object) length));
      T[] destinationArray = new T[length];
      Array.Copy((Array) data, index, (Array) destinationArray, 0, length);
      return destinationArray;
    }

    public static T[] Concat<T>(this T[] a, T[] b)
    {
      T[] destinationArray = new T[a.Length + b.Length];
      Array.Copy((Array) a, 0, (Array) destinationArray, 0, a.Length);
      Array.Copy((Array) b, 0, (Array) destinationArray, a.Length, b.Length);
      return destinationArray;
    }
  }
}
