// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionNameAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class ActionNameAttribute : ActionNameSelectorAttribute
  {
    public ActionNameAttribute(string name)
    {
      this.Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (name));
    }

    public string Name { get; private set; }

    public override bool IsValidName(
      ControllerContext controllerContext,
      string actionName,
      MethodInfo methodInfo)
    {
      return string.Equals(actionName, this.Name, StringComparison.OrdinalIgnoreCase);
    }
  }
}
