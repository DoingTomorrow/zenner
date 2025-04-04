// Decompiled with JetBrains decompiler
// Type: Utils.Pointers
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace Utils
{
  internal static class Pointers
  {
    internal static IntPtr Add(IntPtr x, int y) => new IntPtr(checked (x.ToInt64() + (long) y));

    private static IntPtr Add(IntPtr x, IntPtr y)
    {
      return new IntPtr(checked (x.ToInt64() + y.ToInt64()));
    }
  }
}
