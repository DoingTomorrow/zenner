// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.WqsOffset
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal static class WqsOffset
  {
    public const int NsBth_16 = 16;
    public static readonly int dwSize_0 = 0;
    public static readonly int dwNameSpace_20 = 5 * IntPtr.Size;
    public static readonly int lpcsaBuffer_48 = 12 * IntPtr.Size;
    public static readonly int dwOutputFlags_52 = 13 * IntPtr.Size;
    public static readonly int lpBlob_56 = 14 * IntPtr.Size;
    public static readonly int StructLength_60 = 15 * IntPtr.Size;
    private static bool s_doneAssert;

    [Conditional("DEBUG")]
    public static void AssertCheckLayout()
    {
      if (WqsOffset.s_doneAssert)
        return;
      WqsOffset.s_doneAssert = true;
    }
  }
}
