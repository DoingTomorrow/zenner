// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MetrologicalCore
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class MetrologicalCore
  {
    public static string Get() => MetrologicalCore.GetFileChecksum(string.Empty);

    public static string GetFileChecksum(string filename)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 35678;
      stringBuilder.Append(num.ToString("X04"));
      for (int index = 0; index < 7; ++index)
      {
        stringBuilder.Append('-');
        num *= 3;
        num += 12624;
        num &= (int) ushort.MaxValue;
        stringBuilder.Append(num.ToString("X04"));
      }
      return stringBuilder.ToString();
    }
  }
}
