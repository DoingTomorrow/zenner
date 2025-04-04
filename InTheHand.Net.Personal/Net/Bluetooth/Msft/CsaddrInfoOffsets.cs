// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.CsaddrInfoOffsets
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal static class CsaddrInfoOffsets
  {
    public static readonly int OffsetRemoteAddr_lpSockaddr_8 = 2 * IntPtr.Size;
    public static readonly int OffsetRemoteAddr_iSockaddrLength_12 = 3 * IntPtr.Size;
    private static bool s_doneAssert;

    [Conditional("DEBUG")]
    public static void AssertCheckLayout()
    {
      if (CsaddrInfoOffsets.s_doneAssert)
        return;
      CsaddrInfoOffsets.s_doneAssert = true;
    }
  }
}
