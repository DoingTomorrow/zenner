// Decompiled with JetBrains decompiler
// Type: StartupLib.UserInfo
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using PlugInLib;
using System.Collections.Generic;

#nullable disable
namespace StartupLib
{
  public class UserInfo
  {
    public bool isAdministrator;
    public bool isEnableExtendedRights;

    public int UserId { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public int PersonalNumber { get; set; }

    public UserInfo.PN_Source PNSource { get; set; }

    public string LanguageSetting { get; set; }

    public string ControlKey { get; set; }

    public int RoleUserId { get; set; }

    public string UserRole { get; set; }

    public bool OnlyFinterprintLogin { get; set; }

    public string PhoneNumber { get; set; }

    public string EmailAddress { get; set; }

    public string UserExtendedInfo { get; set; }

    public List<PermissionInfo> Permissions { get; internal set; }

    public string ErrorMessageText { get; set; }

    public UserInfo()
    {
      this.isAdministrator = false;
      this.isEnableExtendedRights = false;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", (object) this.UserId, this.Name != null ? (object) this.Name : (object) string.Empty).TrimEnd();
    }

    public enum PN_Source
    {
      None,
      ZRI,
      ZSH,
      ZFZ,
      Mulda,
      SB,
      ZBA,
    }
  }
}
