// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Utils.SqRefUtility
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.Utils
{
  public static class SqRefUtility
  {
    public static string ToSqRefAddress(string address)
    {
      Require.Argument<string>(address).IsNotNullOrEmpty(address);
      address = address.Replace(",", " ");
      address = new Regex("[ ]+").Replace(address, " ");
      return address;
    }

    public static string FromSqRefAddress(string address)
    {
      Require.Argument<string>(address).IsNotNullOrEmpty(address);
      address = address.Replace(" ", ",");
      return address;
    }
  }
}
