// Decompiled with JetBrains decompiler
// Type: SQLitePCL.Platform
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace SQLitePCL
{
  internal static class Platform
  {
    private static IPlatform current;
    private static string platformAssemblyName = "SQLitePCL.Ext";
    private static string platformTypeFullName = "SQLitePCL.CurrentPlatform";

    public static IPlatform Instance
    {
      get
      {
        if (Platform.current == null)
        {
          Type type = Type.GetType(Platform.platformTypeFullName + ", " + new AssemblyName(typeof (IPlatform).GetTypeInfo().Assembly.FullName)
          {
            Name = Platform.platformAssemblyName
          }.FullName, false);
          if ((object) type != null)
            Platform.current = (IPlatform) Activator.CreateInstance(type);
          else
            Platform.ThrowForMissingPlatformAssembly();
        }
        return Platform.current;
      }
      set => Platform.current = value;
    }

    private static void ThrowForMissingPlatformAssembly()
    {
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, Resources.Platform_AssemblyNotFound, new object[2]
      {
        (object) new AssemblyName(typeof (Platform).GetTypeInfo().Assembly.FullName).Name,
        (object) Platform.platformAssemblyName
      }));
    }
  }
}
