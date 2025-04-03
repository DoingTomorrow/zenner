// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.AppSettings
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System.ComponentModel;
using System.Configuration;

#nullable disable
namespace MSS_Client.Utils
{
  public static class AppSettings
  {
    public static T Get<T>(string key)
    {
      string appSetting = ConfigurationManager.AppSettings[key];
      return !string.IsNullOrWhiteSpace(appSetting) ? (T) TypeDescriptor.GetConverter(typeof (T)).ConvertFromInvariantString(appSetting) : throw new InvalidEnumArgumentException(key);
    }
  }
}
