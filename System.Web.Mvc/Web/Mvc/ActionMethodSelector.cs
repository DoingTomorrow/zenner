// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionMethodSelector
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class ActionMethodSelector
  {
    public ActionMethodSelector(Type controllerType)
    {
      this.ControllerType = controllerType;
      this.PopulateLookupTables();
    }

    public Type ControllerType { get; private set; }

    public MethodInfo[] AliasedMethods { get; private set; }

    public ILookup<string, MethodInfo> NonAliasedMethods { get; private set; }

    private AmbiguousMatchException CreateAmbiguousMatchException(
      List<MethodInfo> ambiguousMethods,
      string actionName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (MethodInfo ambiguousMethod in ambiguousMethods)
      {
        string str = Convert.ToString((object) ambiguousMethod, (IFormatProvider) CultureInfo.CurrentCulture);
        string fullName = ambiguousMethod.DeclaringType.FullName;
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ActionMethodSelector_AmbiguousMatchType, new object[2]
        {
          (object) str,
          (object) fullName
        });
      }
      return new AmbiguousMatchException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ActionMethodSelector_AmbiguousMatch, new object[3]
      {
        (object) actionName,
        (object) this.ControllerType.Name,
        (object) stringBuilder
      }));
    }

    public MethodInfo FindActionMethod(ControllerContext controllerContext, string actionName)
    {
      List<MethodInfo> matchingAliasedMethods = this.GetMatchingAliasedMethods(controllerContext, actionName);
      matchingAliasedMethods.AddRange(this.NonAliasedMethods[actionName]);
      List<MethodInfo> ambiguousMethods = ActionMethodSelector.RunSelectionFilters(controllerContext, matchingAliasedMethods);
      switch (ambiguousMethods.Count)
      {
        case 0:
          return (MethodInfo) null;
        case 1:
          return ambiguousMethods[0];
        default:
          throw this.CreateAmbiguousMatchException(ambiguousMethods, actionName);
      }
    }

    internal List<MethodInfo> GetMatchingAliasedMethods(
      ControllerContext controllerContext,
      string actionName)
    {
      return ((IEnumerable<MethodInfo>) this.AliasedMethods).Select(methodInfo => new
      {
        methodInfo = methodInfo,
        attrs = ReflectedAttributeCache.GetActionNameSelectorAttributes(methodInfo)
      }).Where(_param1 => _param1.attrs.All<ActionNameSelectorAttribute>((Func<ActionNameSelectorAttribute, bool>) (attr => attr.IsValidName(controllerContext, actionName, _param1.methodInfo)))).Select(_param0 => _param0.methodInfo).ToList<MethodInfo>();
    }

    private static bool IsMethodDecoratedWithAliasingAttribute(MethodInfo methodInfo)
    {
      return methodInfo.IsDefined(typeof (ActionNameSelectorAttribute), true);
    }

    private static bool IsValidActionMethod(MethodInfo methodInfo)
    {
      return !methodInfo.IsSpecialName && !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof (Controller));
    }

    private void PopulateLookupTables()
    {
      MethodInfo[] all = Array.FindAll<MethodInfo>(this.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod), new Predicate<MethodInfo>(ActionMethodSelector.IsValidActionMethod));
      this.AliasedMethods = Array.FindAll<MethodInfo>(all, new Predicate<MethodInfo>(ActionMethodSelector.IsMethodDecoratedWithAliasingAttribute));
      this.NonAliasedMethods = ((IEnumerable<MethodInfo>) all).Except<MethodInfo>((IEnumerable<MethodInfo>) this.AliasedMethods).ToLookup<MethodInfo, string>((Func<MethodInfo, string>) (method => method.Name), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private static List<MethodInfo> RunSelectionFilters(
      ControllerContext controllerContext,
      List<MethodInfo> methodInfos)
    {
      List<MethodInfo> methodInfoList1 = new List<MethodInfo>();
      List<MethodInfo> methodInfoList2 = new List<MethodInfo>();
      foreach (MethodInfo methodInfo1 in methodInfos)
      {
        MethodInfo methodInfo = methodInfo1;
        ICollection<ActionMethodSelectorAttribute> selectorAttributes = ReflectedAttributeCache.GetActionMethodSelectorAttributes(methodInfo);
        if (selectorAttributes.Count == 0)
          methodInfoList2.Add(methodInfo);
        else if (selectorAttributes.All<ActionMethodSelectorAttribute>((Func<ActionMethodSelectorAttribute, bool>) (attr => attr.IsValidForRequest(controllerContext, methodInfo))))
          methodInfoList1.Add(methodInfo);
      }
      return methodInfoList1.Count <= 0 ? methodInfoList2 : methodInfoList1;
    }
  }
}
