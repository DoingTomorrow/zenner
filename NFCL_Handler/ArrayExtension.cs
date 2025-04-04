// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.ArrayExtension
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using System;

#nullable disable
namespace NFCL_Handler
{
  public static class ArrayExtension
  {
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
      if (index == 0 && data.Length == length)
        return data;
      T[] destinationArray = new T[length];
      Array.Copy((Array) data, index, (Array) destinationArray, 0, length);
      return destinationArray;
    }
  }
}
