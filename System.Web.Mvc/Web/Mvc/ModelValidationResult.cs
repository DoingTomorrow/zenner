// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelValidationResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ModelValidationResult
  {
    private string _memberName;
    private string _message;

    public string MemberName
    {
      get => this._memberName ?? string.Empty;
      set => this._memberName = value;
    }

    public string Message
    {
      get => this._message ?? string.Empty;
      set => this._message = value;
    }
  }
}
