// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.PlatformVerification
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net
{
  internal static class PlatformVerification
  {
    private static bool? s_isMonoRuntime;

    public static void ThrowException()
    {
    }

    public static bool IsMonoRuntime
    {
      get
      {
        if (!PlatformVerification.s_isMonoRuntime.HasValue)
          PlatformVerification.s_isMonoRuntime = new bool?(Type.GetType("Mono.Runtime") != null);
        return PlatformVerification.s_isMonoRuntime.Value;
      }
    }
  }
}
