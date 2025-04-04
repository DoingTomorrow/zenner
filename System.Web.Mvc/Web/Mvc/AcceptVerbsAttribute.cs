// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AcceptVerbsAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class AcceptVerbsAttribute : ActionMethodSelectorAttribute
  {
    public AcceptVerbsAttribute(HttpVerbs verbs)
      : this(AcceptVerbsAttribute.EnumToArray(verbs))
    {
    }

    public AcceptVerbsAttribute(params string[] verbs)
    {
      this.Verbs = verbs != null && verbs.Length != 0 ? (ICollection<string>) new ReadOnlyCollection<string>((IList<string>) verbs) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (verbs));
    }

    public ICollection<string> Verbs { get; private set; }

    private static void AddEntryToList(
      HttpVerbs verbs,
      HttpVerbs match,
      List<string> verbList,
      string entryText)
    {
      if ((verbs & match) == (HttpVerbs) 0)
        return;
      verbList.Add(entryText);
    }

    internal static string[] EnumToArray(HttpVerbs verbs)
    {
      List<string> verbList = new List<string>();
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Get, verbList, "GET");
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Post, verbList, "POST");
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Put, verbList, "PUT");
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Delete, verbList, "DELETE");
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Head, verbList, "HEAD");
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Patch, verbList, "PATCH");
      AcceptVerbsAttribute.AddEntryToList(verbs, HttpVerbs.Options, verbList, "OPTIONS");
      return verbList.ToArray();
    }

    public override bool IsValidForRequest(
      ControllerContext controllerContext,
      MethodInfo methodInfo)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      return this.Verbs.Contains<string>(controllerContext.HttpContext.Request.GetHttpMethodOverride(), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }
  }
}
