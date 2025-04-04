// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.TestUtilities
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Diagnostics;

#nullable disable
namespace InTheHand.Net
{
  internal static class TestUtilities
  {
    private static bool? s_isNunitHarness;

    public static bool IsUnderTestHarness()
    {
      if (!TestUtilities.s_isNunitHarness.HasValue)
        TestUtilities.s_isNunitHarness = new bool?(TestUtilities.IsRunningUnderNUnit());
      return TestUtilities.s_isNunitHarness.Value;
    }

    private static bool IsRunningUnderNUnit()
    {
      StackTrace stackTrace = new StackTrace();
      for (int index = 0; index < stackTrace.FrameCount; ++index)
      {
        foreach (object customAttribute in stackTrace.GetFrame(index).GetMethod().GetCustomAttributes(false))
        {
          if (customAttribute.GetType().Name == "TestAttribute" || customAttribute.GetType().Name == "TestFixtureSetUpAttribute")
            return true;
        }
      }
      return false;
    }
  }
}
