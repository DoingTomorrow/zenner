// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.ExtensionMethods
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal static class ExtensionMethods
  {
    private static readonly string DataSqlServerCeEntityAssembly = "System.Data.SqlServerCe.Entity";
    private static readonly string DataEntityAssembly = "System.Data.Entity";
    private static string SystemDataCommonDbProviderServices_TypeName = Assembly.CreateQualifiedName(ExtensionMethods.GetFullAssemblyName(ExtensionMethods.DataEntityAssembly), "System.Data.Common.DbProviderServices");
    internal static Type SystemDataCommonDbProviderServices_Type = Type.GetType(ExtensionMethods.SystemDataCommonDbProviderServices_TypeName, false);
    private static string SqlCeProviderServices_TypeName = "System.Data.SqlServerCe.SqlCeProviderServices";
    private static FieldInfo SqlCeProviderServices_Instance_FieldInfo;

    internal static object SystemDataSqlServerCeSqlCeProviderServices_Instance()
    {
      if (ExtensionMethods.SqlCeProviderServices_Instance_FieldInfo == null)
      {
        string str = ExtensionMethods.ConstructFullAssemblyName(ExtensionMethods.DataSqlServerCeEntityAssembly);
        string qualifiedName = Assembly.CreateQualifiedName(str, ExtensionMethods.SqlCeProviderServices_TypeName);
        try
        {
          Assembly.Load(str);
        }
        catch (FileNotFoundException ex)
        {
          throw new FileNotFoundException(Res.GetString("SQLCE_CantLoadEntityDll"), (Exception) ex);
        }
        catch (FileLoadException ex)
        {
          throw new FileLoadException(Res.GetString("SQLCE_CantLoadEntityDll"), (Exception) ex);
        }
        catch (BadImageFormatException ex)
        {
          throw new BadImageFormatException(Res.GetString("SQLCE_CantLoadEntityDll"), (Exception) ex);
        }
        Type type = Type.GetType(qualifiedName, false);
        if (type != null)
          ExtensionMethods.SqlCeProviderServices_Instance_FieldInfo = type.GetField("Instance", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
      }
      return ExtensionMethods.SystemDataSqlServerCeSqlCeProviderServices_Instance_GetValue();
    }

    [SuppressMessage("Microsoft.Security", "CA2106:SecureAsserts")]
    [ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
    private static object SystemDataSqlServerCeSqlCeProviderServices_Instance_GetValue()
    {
      object obj = (object) null;
      if (ExtensionMethods.SqlCeProviderServices_Instance_FieldInfo != null)
        obj = ExtensionMethods.SqlCeProviderServices_Instance_FieldInfo.GetValue((object) null);
      return obj;
    }

    private static string ConstructFullAssemblyName(string assemblyName)
    {
      if (assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        assemblyName = Path.GetFileNameWithoutExtension(assemblyName);
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      return executingAssembly.FullName.Replace(executingAssembly.GetName().Name, assemblyName);
    }

    private static string GetFullAssemblyName(string assemblyName)
    {
      if (assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        assemblyName = Path.GetFileNameWithoutExtension(assemblyName);
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        AssemblyName assemblyName1 = new AssemblyName(assembly.FullName);
        if (string.Compare(assemblyName1.Name, assemblyName) == 0)
          return assemblyName1.FullName;
      }
      throw new ArgumentException(assemblyName);
    }
  }
}
