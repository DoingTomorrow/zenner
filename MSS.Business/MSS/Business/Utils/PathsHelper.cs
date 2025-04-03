// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.PathsHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Configuration;
using System.IO;

#nullable disable
namespace MSS.Business.Utils
{
  public static class PathsHelper
  {
    public static string GetTempFolderPath()
    {
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ConfigurationManager.AppSettings["CommonAppDataTempPath"]);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path + "\\";
    }
  }
}
