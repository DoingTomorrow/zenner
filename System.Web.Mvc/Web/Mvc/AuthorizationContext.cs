// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AuthorizationContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class AuthorizationContext : ControllerContext
  {
    public AuthorizationContext()
    {
    }

    [Obsolete("The recommended alternative is the constructor AuthorizationContext(ControllerContext controllerContext, ActionDescriptor actionDescriptor).")]
    public AuthorizationContext(ControllerContext controllerContext)
      : base(controllerContext)
    {
    }

    public AuthorizationContext(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
      : base(controllerContext)
    {
      this.ActionDescriptor = actionDescriptor != null ? actionDescriptor : throw new ArgumentNullException(nameof (actionDescriptor));
    }

    public virtual ActionDescriptor ActionDescriptor { get; set; }

    public ActionResult Result { get; set; }
  }
}
