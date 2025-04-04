// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.ArrayExtension
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using System;

#nullable disable
namespace PDCL2_Handler
{
  public static class ArrayExtension
  {
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
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
