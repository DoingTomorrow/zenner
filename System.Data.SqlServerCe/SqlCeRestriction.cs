// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeRestriction
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Globalization;
using System.Reflection;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class SqlCeRestriction
  {
    private static bool IsWebHosted()
    {
      flag = false;
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (assembly.GetName().Name == "System.Web" && assembly.GlobalAssemblyCache)
        {
          Type type = assembly.GetType("System.Web.Hosting.HostingEnvironment");
          if (type != null)
          {
            PropertyInfo property = type.GetProperty("IsHosted", typeof (bool));
            if (property != null)
            {
              object obj = property.GetValue((object) type, (object[]) null);
              if (obj == null || !(obj is bool flag))
                break;
              break;
            }
            break;
          }
          break;
        }
      }
      return flag;
    }

    private static bool IsExplicitlyEnabled()
    {
      flag = false;
      object data = AppDomain.CurrentDomain.GetData("SQLServerCompactEditionUnderWebHosting");
      if (data == null || !(data is bool flag))
        ;
      return flag;
    }

    public static void CheckExplicitWebHosting()
    {
      if (!SqlCeRestriction.IsExplicitlyEnabled() && SqlCeRestriction.IsWebHosted())
        throw new NotSupportedException(Res.GetString(CultureInfo.CurrentCulture, "SQLCE_WebHostingRestriction"));
    }
  }
}
