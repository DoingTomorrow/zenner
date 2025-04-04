// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.NonActionAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class NonActionAttribute : ActionMethodSelectorAttribute
  {
    public override bool IsValidForRequest(
      ControllerContext controllerContext,
      MethodInfo methodInfo)
    {
      return false;
    }
  }
}
