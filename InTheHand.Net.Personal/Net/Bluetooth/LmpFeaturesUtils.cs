// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.LmpFeaturesUtils
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal static class LmpFeaturesUtils
  {
    internal static void FindUndefinedValues(LmpFeatures lmpFeatures)
    {
      LmpFeaturesUtils.ForEachBit((Action<LmpFeatures>) (bitMask =>
      {
        LmpFeatures lmpFeatures1 = lmpFeatures & bitMask;
        if (lmpFeatures1 == LmpFeatures.None || Enum.GetName(typeof (LmpFeatures), (object) lmpFeatures1) != null)
          return;
        string str = "Not defined: 0x" + ((ulong) lmpFeatures1).ToString("X16");
      }));
    }

    internal static void FindUnsetValues(LmpFeatures lmpFeatures)
    {
      LmpFeaturesUtils.ForEachBit((Action<LmpFeatures>) (bitMask =>
      {
        if ((lmpFeatures & bitMask) != LmpFeatures.None || Enum.GetName(typeof (LmpFeatures), (object) bitMask) == null)
          return;
        "Not set: '" + (object) bitMask + "' 0x" + ((ulong) bitMask).ToString("X16");
      }));
    }

    private static void ForEachBit(Action<LmpFeatures> action)
    {
      ulong num = 1;
      while (true)
      {
        action((LmpFeatures) num);
        try
        {
          checked { num *= 2UL; }
        }
        catch (OverflowException ex)
        {
          break;
        }
      }
    }
  }
}
