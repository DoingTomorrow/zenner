// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMM_User
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class GMM_User
  {
    public string UserName;
    public int UserPersonalNumber;
    public string UserRights;
    public string UserKey;
    public string ChangedUserRights;
    public string ChangedUserKey;

    public override string ToString() => this.UserName;
  }
}
