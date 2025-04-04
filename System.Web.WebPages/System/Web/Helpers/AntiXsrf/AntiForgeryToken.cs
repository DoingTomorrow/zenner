// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.AntiForgeryToken
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class AntiForgeryToken
  {
    internal const int SecurityTokenBitLength = 128;
    internal const int ClaimUidBitLength = 256;
    private string _additionalData;
    private BinaryBlob _securityToken;
    private string _username;

    public string AdditionalData
    {
      get => this._additionalData ?? string.Empty;
      set => this._additionalData = value;
    }

    public BinaryBlob ClaimUid { get; set; }

    public bool IsSessionToken { get; set; }

    public BinaryBlob SecurityToken
    {
      get
      {
        if (this._securityToken == null)
          this._securityToken = new BinaryBlob(128);
        return this._securityToken;
      }
      set => this._securityToken = value;
    }

    public string Username
    {
      get => this._username ?? string.Empty;
      set => this._username = value;
    }
  }
}
