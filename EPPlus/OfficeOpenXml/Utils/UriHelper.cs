// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Utils.UriHelper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Utils
{
  internal class UriHelper
  {
    internal static Uri ResolvePartUri(Uri sourceUri, Uri targetUri)
    {
      if (targetUri.OriginalString.StartsWith("/"))
        return targetUri;
      string[] strArray1 = sourceUri.OriginalString.Split('/');
      string[] strArray2 = targetUri.OriginalString.Split('/');
      int num1 = strArray2.Length - 1;
      int num2 = !sourceUri.OriginalString.EndsWith("/") ? strArray1.Length - 2 : strArray1.Length - 1;
      string[] strArray3 = strArray2;
      int index1 = num1;
      int index2 = index1 - 1;
      string uriString = strArray3[index1];
      while (index2 >= 0 && !(strArray2[index2] == "."))
      {
        if (strArray2[index2] == "..")
        {
          --num2;
          --index2;
        }
        else
          uriString = strArray2[index2--] + "/" + uriString;
      }
      if (num2 >= 0)
      {
        for (int index3 = num2; index3 >= 0; --index3)
          uriString = strArray1[index3] + "/" + uriString;
      }
      return new Uri(uriString, UriKind.RelativeOrAbsolute);
    }

    internal static Uri GetRelativeUri(Uri WorksheetUri, Uri uri)
    {
      string[] strArray1 = WorksheetUri.OriginalString.Split('/');
      string[] strArray2 = uri.OriginalString.Split('/');
      int num = !WorksheetUri.OriginalString.EndsWith("/") ? strArray1.Length - 1 : strArray1.Length;
      int index1 = 0;
      while (index1 < num && index1 < strArray2.Length && strArray1[index1] == strArray2[index1])
        ++index1;
      string str1 = "";
      for (int index2 = index1; index2 < num; ++index2)
        str1 += "../";
      string str2 = "";
      for (int index3 = index1; index3 < strArray2.Length; ++index3)
        str2 = str2 + (str2 == "" ? "" : "/") + strArray2[index3];
      return new Uri(str1 + str2, UriKind.Relative);
    }
  }
}
