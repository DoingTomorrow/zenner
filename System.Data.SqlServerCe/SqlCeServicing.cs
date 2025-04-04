// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeServicing
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal sealed class SqlCeServicing
  {
    internal static void DoBreadcrumbServicing(string modulePath)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        if (string.IsNullOrEmpty(folderPath))
          return;
        string path = Path.Combine(folderPath, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Microsoft.SqlServer.Compact.351.{0}.bc", 4 == IntPtr.Size ? (object) "32" : (object) "64"));
        stringBuilder.Append("<Breadcrumb>");
        stringBuilder.Append(modulePath).Append(", ");
        stringBuilder.Append("3.5.8080.0").Append(", ");
        stringBuilder.Append(Process.GetCurrentProcess().MainModule.ModuleName);
        stringBuilder.Append("</Breadcrumb>");
        string strB = stringBuilder.ToString();
        if (!File.Exists(path))
        {
          using (File.Create(path))
            ;
        }
        string[] strArray = File.ReadAllLines(path, Encoding.UTF8);
        for (int index = strArray.Length - 1; index >= 0; --index)
        {
          if (!string.IsNullOrEmpty(strArray[index]) && string.Compare(strArray[index], strB, true, CultureInfo.InvariantCulture) == 0)
            return;
        }
        string[] contents = new string[strArray.Length >= 32 ? 32 : strArray.Length + 1];
        int index1 = 0;
        for (int index2 = strArray.Length < 32 ? 0 : 1; index2 < strArray.Length && index1 < contents.Length; ++index1)
        {
          contents[index1] = strArray[index2];
          ++index2;
        }
        contents[contents.Length - 1] = strB;
        File.WriteAllLines(path, contents, Encoding.UTF8);
      }
      catch
      {
      }
    }
  }
}
