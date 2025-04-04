// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValidateAntiForgeryTokenAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.ComponentModel;
using System.Web.Helpers;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
  {
    private string _salt;

    public ValidateAntiForgeryTokenAttribute()
      : this(new Action(AntiForgery.Validate))
    {
    }

    internal ValidateAntiForgeryTokenAttribute(Action validateAction)
    {
      this.ValidateAction = validateAction;
    }

    [Obsolete("The 'Salt' property is deprecated. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string Salt
    {
      get => this._salt;
      set
      {
        this._salt = string.IsNullOrEmpty(value) ? value : throw new NotSupportedException("The 'Salt' property is deprecated. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.");
      }
    }

    internal Action ValidateAction { get; private set; }

    public void OnAuthorization(AuthorizationContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      this.ValidateAction();
    }
  }
}
