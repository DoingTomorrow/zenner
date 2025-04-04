// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AuthorizeAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
  public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
  {
    private readonly object _typeId = new object();
    private string _roles;
    private string[] _rolesSplit = new string[0];
    private string _users;
    private string[] _usersSplit = new string[0];

    public string Roles
    {
      get => this._roles ?? string.Empty;
      set
      {
        this._roles = value;
        this._rolesSplit = AuthorizeAttribute.SplitString(value);
      }
    }

    public override object TypeId => this._typeId;

    public string Users
    {
      get => this._users ?? string.Empty;
      set
      {
        this._users = value;
        this._usersSplit = AuthorizeAttribute.SplitString(value);
      }
    }

    protected virtual bool AuthorizeCore(HttpContextBase httpContext)
    {
      IPrincipal principal = httpContext != null ? httpContext.User : throw new ArgumentNullException(nameof (httpContext));
      return principal.Identity.IsAuthenticated && (this._usersSplit.Length <= 0 || ((IEnumerable<string>) this._usersSplit).Contains<string>(principal.Identity.Name, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)) && (this._rolesSplit.Length <= 0 || ((IEnumerable<string>) this._rolesSplit).Any<string>(new Func<string, bool>(principal.IsInRole)));
    }

    private void CacheValidateHandler(
      HttpContext context,
      object data,
      ref HttpValidationStatus validationStatus)
    {
      validationStatus = this.OnCacheAuthorization((HttpContextBase) new HttpContextWrapper(context));
    }

    public virtual void OnAuthorization(AuthorizationContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (OutputCacheAttribute.IsChildActionCacheActive((ControllerContext) filterContext))
        throw new InvalidOperationException(MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache);
      if (filterContext.ActionDescriptor.IsDefined(typeof (AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute), true))
        return;
      if (this.AuthorizeCore(filterContext.HttpContext))
      {
        HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
        cache.SetProxyMaxAge(new TimeSpan(0L));
        cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), (object) null);
      }
      else
        this.HandleUnauthorizedRequest(filterContext);
    }

    protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
      filterContext.Result = (ActionResult) new HttpUnauthorizedResult();
    }

    protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
    {
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      return !this.AuthorizeCore(httpContext) ? HttpValidationStatus.IgnoreThisRequest : HttpValidationStatus.Valid;
    }

    internal static string[] SplitString(string original)
    {
      if (string.IsNullOrEmpty(original))
        return new string[0];
      return ((IEnumerable<string>) original.Split(',')).Select(piece => new
      {
        piece = piece,
        trimmed = piece.Trim()
      }).Where(_param0 => !string.IsNullOrEmpty(_param0.trimmed)).Select(_param0 => _param0.trimmed).ToArray<string>();
    }
  }
}
