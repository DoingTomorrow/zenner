// Decompiled with JetBrains decompiler
// Type: Excel.Core.ReferenceHelper
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Excel.Core
{
  public static class ReferenceHelper
  {
    public static int[] ReferenceToColumnAndRow(string reference)
    {
      Regex regex = new Regex("([a-zA-Z]*)([0-9]*)");
      string upper = regex.Match(reference).Groups[1].Value.ToUpper();
      string s = regex.Match(reference).Groups[2].Value;
      int num1 = 0;
      int num2 = 1;
      for (int index = upper.Length - 1; index >= 0; --index)
      {
        int num3 = (int) upper[index] - 65 + 1;
        num1 += num2 * num3;
        num2 *= 26;
      }
      return new int[2]{ int.Parse(s), num1 };
    }
  }
}
